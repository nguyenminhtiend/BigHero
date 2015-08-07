using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Model;
using HvN.BigHero.DAL.Repository;
using HvN.BigHero.DAL.Sql;
using HvN.BigHero.DAL.UnitOfWork;

namespace HvN.BigHero.DAL.Service
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<Table> tableRepository;
        private readonly IGenericRepository<Column> columnRepository;

        public TableService(IUnitOfWork unitOfWork, IGenericRepository<Table> tableRepository, IGenericRepository<Column> columnRepository)
        {
            this.unitOfWork = unitOfWork;
            this.tableRepository = tableRepository;
            this.columnRepository = columnRepository;
        }

        public TableDetailViewModel GeTableDetailViewModel(string tableName)
        {
            var tableDetailViewModel = new TableDetailViewModel();
            var listColumns = (from table in tableRepository.GetAll()
                                            join column in columnRepository.GetAll() on table.Id equals column.TableId
                                            where table.Name.Equals(tableName)
                                            orderby column.Order
                                            select new ColumnViewModel
                                            {
                                                Display = column.Display,
                                                Name = column.Name,
                                                IsPrimaryKey = column.IsPrimarykey
                                            }).ToList();
            tableDetailViewModel.Columns = listColumns.Where(x =>!x.IsPrimaryKey).ToList();
            var primaryColumn = listColumns.FirstOrDefault(x => x.IsPrimaryKey);
            if (primaryColumn != null)
            {
                tableDetailViewModel.PrimaryColumn = primaryColumn.Name;
            }
            tableDetailViewModel.TableName = tableName;
            var seleteStatement = SqlHelper.GetSeleteStatement(listColumns, tableName);
            var dataTable = new DataTable();
            using (var connection = unitOfWork.Context.Database.Connection)
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = seleteStatement;
                    cmd.CommandType = CommandType.Text;
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            var data = new List<Dictionary<string, object>>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var row = new Dictionary<string, object>();
                foreach (var columnViewModel in listColumns)
                {
                    row.Add(columnViewModel.Name, dataRow[columnViewModel.Name]);
                }
                data.Add(row);
            }
            tableDetailViewModel.Data = data;
            return tableDetailViewModel;
        }

        public RowDetailViewModel GetListColumnForAddNew(string tableName)
        {
            var listColumns = (from table in tableRepository.GetAll()
                               join column in columnRepository.GetAll() on table.Id equals column.TableId
                               where table.Name.Equals(tableName)
                               orderby column.Order
                               select new ColumnViewModel
                               {
                                   Name = column.Name,
                                   Display = column.Display,
                                   IsPrimaryKey = column.IsPrimarykey,
                                   NullAble = column.Nullable
                               }).ToList();
            return new RowDetailViewModel
            {
                TableName = tableName,
                PrimaryColumn = listColumns.FirstOrDefault(x => x.IsPrimaryKey),
                Columns = listColumns.Where(x => !x.IsPrimaryKey).ToList()
            };
        }

        public void AddNewTable(Table table)
        {
            var tableScript = SqlHelper.GetCreateTableStatement(table);
            tableRepository.Add(table);
            unitOfWork.Context.Database.ExecuteSqlCommand(tableScript);
            unitOfWork.Commit();
        }
        public void InsertData(Table table, Dictionary<string, object> data)
        {
            ExecuteNonQuery(data, SqlHelper.GetInsertStatement(table));
        }
        public void UpdateData(Table table, Dictionary<string, object> data)
        {
            ExecuteNonQuery(data, SqlHelper.GetUpdateStatement(table));
        }

        private void ExecuteNonQuery(Dictionary<string, object> data, string sqlStatement)
        {
            var param = new object[data.Count];
            int i = 0;
            foreach (var item in data)
            {
                param[i] = new SqlParameter(string.Format("@{0}", item.Key), item.Value);
                i++;
            }
            unitOfWork.Context.Database.ExecuteSqlCommand(sqlStatement, param);
            unitOfWork.Commit();
        }

    }
}
