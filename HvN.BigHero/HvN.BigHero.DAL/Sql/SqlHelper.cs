using System.Collections.Generic;
using System.Linq;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Model;
using HvN.BigHero.DAL.Utility;

namespace HvN.BigHero.DAL.Sql
{
    public class SqlHelper
    {
        private const string Table = "CREATE TABLE {0}({1})";
        private const string Column = "{0} {1} {2} {3},";
        public const string Nvarchar = "nvarchar({0})";
        public const string InsertStatement = "INSERT INTO {0}({1}) VALUES({2})";
        public const string UpdateStatement = "UPDATE {0} SET {1} WHERE {2}";
        public const string SelectStatement = "SELECT{0} FROM {1}";
        public const string SelectTopOneStatement = "SELECT TOP 1 {0} FROM {1} WHERE {2}";
        public const string DeleteStatement = "DELETE FROM {0} WHERE {1}=@{1}";

        public static string GetCreateTableStatement(Table table)
        {
            string columns = string.Empty;
            foreach (var column in table.Columns)
            {
                string columnTemp = Column;
                string dataType;
                if (column.DataType == ColumnType.VarChar)
                {
                    dataType = column.Size != null ? string.Format(Nvarchar, column.Size) : string.Format(Nvarchar, "max");
                }
                else
                {
                    dataType = column.DataType.ToString();
                }
                columnTemp = columnTemp.Replace("{0}", column.Name);
                columnTemp = columnTemp.Replace("{1}", dataType);
                columnTemp = columnTemp.Replace("{2}", !column.Nullable ? "NOT NULL" : "");
                columnTemp = column.IsPrimarykey ? columnTemp.Replace("{3}", table.IsIdentity ? "IDENTITY(1,1) PRIMARY KEY" : "PRIMARY KEY") : columnTemp.Replace("{3}", "");

                columns += columnTemp;
            }

            return string.Format(Table, table.Name, columns.Substring(0, columns.Length - 1));
        }

        public static string GetSelectTopOneStatement(Table table)
        {
            string columns = string.Empty;
            string conditions = string.Empty;
            foreach (var column in table.Columns)
            {
                columns += string.Format("{0},", column.Name);
            }
            var primaryColumns = table.Columns.Where(x => x.IsPrimarykey).ToList();

            foreach (var column in primaryColumns)
            {
                conditions += string.Format("{0}=@{0},", column.Name);
            }
            return string.Format(SelectTopOneStatement, columns.Substring(0, columns.Length - 1), table.Name, conditions.Substring(0, conditions.Length - 1));
        }
        public static string GetInsertStatement(Table table)
        {
            string columns = string.Empty;
            string values = string.Empty;

            var listColums = table.IsIdentity ? table.Columns.Where(x => !x.IsPrimarykey).ToList() : table.Columns;

            foreach (var column in listColums)
            {
                columns += column.Name + ",";
                values += string.Format("@{0},", column.Name);
            }
            return string.Format(InsertStatement, table.Name, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));
        }

        public static string GetUpdateStatement(Table table)
        {
            string columns = string.Empty;
            string conditions = string.Empty;
            var listColums = table.IsIdentity ? table.Columns.Where(x => !x.IsPrimarykey).ToList() : table.Columns;

            foreach (var column in listColums)
            {
                columns += string.Format("{0}=@{0},", column.Name);
            }
            var primaryColumns = table.Columns.Where(x => x.IsPrimarykey).ToList();

            foreach (var column in primaryColumns)
            {
                conditions += string.Format("{0}=@{0},", column.Name);
            }

            return string.Format(UpdateStatement, table.Name, columns.Substring(0, columns.Length - 1), conditions.Substring(0, conditions.Length - 1));
        }

        public static string GetDeleteStatement(string tableName, string primaryColumn)
        {
            return string.Format(DeleteStatement, tableName, primaryColumn);
        }
        public static string GetSeleteStatement(List<ColumnViewModel> columnViewModels, string tableName)
        {
            string columns = string.Empty;
            foreach (var columnViewModel in columnViewModels)
            {
                columns += string.Format(" {0},", columnViewModel.Name);
            }
            return string.Format(SelectStatement, columns.Substring(0, columns.Length - 1), tableName);
        }
    }
}
