using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Model;
using HvN.BigHero.DAL.MyException;
using HvN.BigHero.DAL.Repository;
using HvN.BigHero.DAL.Sql;
using HvN.BigHero.DAL.UnitOfWork;
using HvN.BigHero.DAL.Utility;

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
                                   IsPrimaryKey = column.IsPrimarykey,
                                   DataType = column.DataType
                               }).ToList();
            if (!listColumns.Any())
            {
                throw new NotFoundException();
            }
            tableDetailViewModel.Columns = listColumns.Where(x => !x.IsPrimaryKey).ToList();
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
                    if (columnViewModel.DataType == ColumnType.DateTime)
                    {
                        var datetime = (DateTime)dataRow[columnViewModel.Name];
                        row.Add(columnViewModel.Name, datetime.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        row.Add(columnViewModel.Name, dataRow[columnViewModel.Name]);
                    }

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
                                   NullAble = column.Nullable,
                                   DataType = column.DataType,
                                   Size = column.Size
                               }).ToList();
            if (!listColumns.Any())
            {
                throw new NotFoundException();
            }
            return new RowDetailViewModel
            {
                TableName = tableName,
                PrimaryColumn = listColumns.FirstOrDefault(x => x.IsPrimaryKey),
                Columns = listColumns.Where(x => !x.IsPrimaryKey).ToList()
            };
        }

        public RowDetailViewModel GetListColumnForEdit(string tableName, int rowId)
        {
            var table = tableRepository.GetItemsWithNavigation(x => x.Name.Equals(tableName), "Columns").FirstOrDefault();
            if (table == null)
            {
                throw new NotFoundException();
            }
            var primaryColumn = table.Columns.FirstOrDefault(x => x.IsPrimarykey);
            if (primaryColumn == null)
            {
                throw new InternalServerException();
            }
            var dataTable = new DataTable();
            using (var connection = unitOfWork.Context.Database.Connection)
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = SqlHelper.GetSelectTopOneStatement(table);
                    cmd.CommandType = CommandType.Text;
                    var parameter = new SqlParameter
                    {
                        ParameterName = string.Format("@{0}", primaryColumn.Name),
                        Value = rowId
                    };
                    cmd.Parameters.Add(parameter);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            if (dataTable.Rows.Count == 0)
            {
                throw new NotFoundException();
            }
            var dataRow = dataTable.Rows[0];

            var listColumns = new List<ColumnViewModel>();
            foreach (var column in table.Columns.Where(x => !x.IsPrimarykey))
            {
                listColumns.Add(new ColumnViewModel
                {
                    Display = column.Display,
                    IsPrimaryKey = column.IsPrimarykey,
                    Name = column.Name,
                    NullAble = column.Nullable,
                    DataType = column.DataType,
                    Size = column.Size,
                    Value = dataRow[column.Name]
                });
            }

            return new RowDetailViewModel
            {
                Columns = listColumns,
                TableName = tableName,
                PrimaryColumn = new ColumnViewModel
                {
                    Name = primaryColumn.Name,
                    Value = rowId
                }
            };
        }

        public void AddNewTable(Table table)
        {
            var tableScript = SqlHelper.GetCreateTableStatement(table);
            tableRepository.Add(table);
            unitOfWork.Context.Database.ExecuteSqlCommand(tableScript);
            unitOfWork.Commit();
        }
        public void InsertData(string tableName, Dictionary<string, object> data)
        {
            var table = tableRepository.GetItemsWithNavigation(x => x.Name.Equals(tableName), "Columns").FirstOrDefault();
            ExecuteNonQuery(data, SqlHelper.GetInsertStatement(table));
        }
        public void UpdateData(string tableName, Dictionary<string, object> data)
        {
            var table = tableRepository.GetItemsWithNavigation(x => x.Name.Equals(tableName), "Columns").FirstOrDefault();
            ExecuteNonQuery(data, SqlHelper.GetUpdateStatement(table));
        }

        public void DeleteData(string tableName, string primaryColumn, string rowId)
        {
            var deleteStatement = SqlHelper.GetDeleteStatement(tableName, primaryColumn);
            unitOfWork.Context.Database.ExecuteSqlCommand(deleteStatement, new SqlParameter(string.Format("@{0}", primaryColumn), rowId));
            unitOfWork.Commit();
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
