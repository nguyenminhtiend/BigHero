using System;
using System.Collections.Generic;
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
                            IsPrimarykey = true
                        },
                        new Column
                        {
                            Name = "Name",
                            Display = "Name",
                            DataType = ColumnType.VarChar,
                            Size = 50
                        },
                        new Column
                        {
                            Name = "DateOfBird",
                            Display = "Date of Bird",
                            DataType = ColumnType.DateTime,
                            Nullable = true
                        }
                    }
                };
                var tableScript = SqlHelper.CreateTable(table);
                db.Tables.Add(table);
                db.Database.ExecuteSqlCommand(tableScript);
                db.SaveChanges();

              
               var data = new Dictionary<string, object>();
                data.Add("Id", 1);
                data.Add("Name", "Messi");
                data.Add("DateOfBird", DateTime.Now);
                var insert = SqlHelper.InsertData(table, data);
                db.Database.ExecuteSqlCommand(insert);
                db.SaveChanges();
                var updateData = new Dictionary<string, object>();
                updateData.Add("Id", 1);
                updateData.Add("Name", "Messi11");
                updateData.Add("DateOfBird", DateTime.Now);
                var update = SqlHelper.UpdatetData(table, updateData);
                db.Database.ExecuteSqlCommand(update);
                db.SaveChanges();
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
