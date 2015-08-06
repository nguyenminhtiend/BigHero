using System.Collections.Generic;
using System.Linq;
using HvN.BigHero.DAL.Entities;
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

        public static string CreateTable(Table table)
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

        public static string InsertData(Table table, Dictionary<string, object> data)
        {
            string columns = string.Empty;
            string values = string.Empty;

            var listColums = table.IsIdentity ? table.Columns.Where(x => !x.IsPrimarykey).ToList() : table.Columns;

            foreach (var column in listColums)
            {
                columns += column.Name + ",";
                if (column.DataType == ColumnType.VarChar || column.DataType == ColumnType.DateTime)
                {
                    values += string.Format("'{0}',", data[column.Name]);
                }
                else
                {
                    values += string.Format("{0},", data[column.Name]);
                }

            }
            return string.Format(InsertStatement, table.Name, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));
        }

        public static string UpdatetData(Table table, Dictionary<string, object> data)
        {
            string columns = string.Empty;
            string coditions = string.Empty;
            var listColums = table.IsIdentity ? table.Columns.Where(x => !x.IsPrimarykey).ToList() : table.Columns;

            foreach (var column in listColums)
            {
                if (column.DataType == ColumnType.VarChar || column.DataType == ColumnType.DateTime)
                {
                    columns += string.Format("{0}='{1}',", column.Name, data[column.Name]);
                }
                else
                {
                    columns += string.Format("{0}={1},", column.Name, data[column.Name]);
                }

            }
            var primaryColumns = table.Columns.Where(x => x.IsPrimarykey).ToList();

            foreach (var column in primaryColumns)
            {
                coditions += string.Format("{0}={1},", column.Name, data[column.Name]);
            }

            return string.Format(UpdateStatement, table.Name, columns.Substring(0, columns.Length - 1), coditions.Substring(0, coditions.Length - 1));
        }

    }
}
