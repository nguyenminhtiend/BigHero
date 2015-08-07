using System.Collections.Generic;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Model;

namespace HvN.BigHero.DAL.Service
{
    public interface ITableService
    {
        TableDetailViewModel GeTableDetailViewModel(string tableName);
        RowDetailViewModel GetListColumnForAddNew(string tableName);
        void AddNewTable(Table table);
        void InsertData(string table, Dictionary<string, object> data);
        void UpdateData(Table table, Dictionary<string, object> data);
    }
}
