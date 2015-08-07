using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Sql;
using HvN.BigHero.DAL.Utility;

namespace HvN.BigHero.DAL.DataContext
{
    public class BigHeroContextInititalizer : DropCreateDatabaseIfModelChanges<BigHeroContext>
    {
        protected override void Seed(BigHeroContext context)
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
                            Name = "DateOfBirth",
                            Display = "Date of Birth",
                            DataType = ColumnType.DateTime,
                            Nullable = true
                        },
                        new Column
                        {
                            Name = "Phone",
                            Display = "Phone",
                            DataType = ColumnType.VarChar,
                            Size = 15,
                            Nullable = true
                        },
                         new Column
                        {
                            Name = "Email",
                            Display = "Email",
                            DataType = ColumnType.VarChar,
                            Size = 50,
                            Nullable = true
                        }
                    }
                };
                var tableScript = SqlHelper.GetCreateTableStatement(table);
                context.Tables.Add(table);
                context.Database.ExecuteSqlCommand(tableScript);
                context.SaveChanges();

                var insert = SqlHelper.GetInsertStatement(table);
                context.Database.ExecuteSqlCommand(insert,
                          new SqlParameter("@Name", "Lionel Messi"),
                          new SqlParameter("@DateOfBirth", DateTime.Now),
                          new SqlParameter("@Phone", "0966638355"),
                          new SqlParameter("@Email", "nguyenminhtiend@gmail.com"));
                context.SaveChanges();
                base.Seed(context);
        }
    }
}
