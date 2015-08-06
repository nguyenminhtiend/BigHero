using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HvN.BigHero.DAL.DataContext;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Sql;
using HvN.BigHero.DAL.Utility;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BigHeroContext())
            {
                var table = new Table
                {
                    Name = "Student",
                    IsIdentity = true,
                    Columns = new List<Column>
                    {
                        new Column
                        {
                            Name = "Id",
                            Display = "Id",
                            DataType = ColumnType.Int,
                            Order = 0,
                            IsPrimarykey = true
                        },
                        new Column
                        {
                            Name = "Name",
                            Display = "Name",
                            DataType = ColumnType.VarChar,
                            Size = 50,
                            Order = 1
                        },
                        new Column
                        {
                            Name = "DateOfBird",
                            Display = "Date of Bird",
                            DataType = ColumnType.DateTime,
                            Nullable = true,
                            Order = 2
                        }
                    }
                };
                var tableScript = SqlHelper.GetCreateTableStatement(table);
                db.Tables.Add(table);
                db.Database.ExecuteSqlCommand(tableScript);
                db.SaveChanges();


                var insert = SqlHelper.GetInsertStatement(table);
                db.Database.ExecuteSqlCommand(insert, 
                    new SqlParameter("@Name", "messi-zip"),
                    new SqlParameter("@DateOfBird", DateTime.Now));
                db.Database.ExecuteSqlCommand(insert,
                    new SqlParameter("@Name", "messi-zip11"),
                    new SqlParameter("@DateOfBird", DateTime.Now));
                db.SaveChanges();

                //var update = SqlHelper.GetUpdateStatement(table);
                //object[] param = new object[3];
                // db.Database.ExecuteSqlCommand(update,
                //    new SqlParameter("@Id", 1),
                //    new SqlParameter("@Name", "messi-zip-11"),
                //    new SqlParameter("@DateOfBird", DateTime.Now));
                //db.SaveChanges();
                Console.WriteLine("");

                //var sql = @"Update [User] SET FirstName = @FirstName WHERE Id = @Id";

                //ctx.Database.ExecuteSqlCommand(
                //    sql,
                //    new SqlParameter("@FirstName", firstname),
                //    new SqlParameter("@Id", id));
            }
        }
    }
}
