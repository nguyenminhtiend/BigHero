using System.Collections.Generic;
using HvN.BigHero.DAL.Entities;
using HvN.BigHero.DAL.Model;

namespace HvN.BigHero.DAL.Service
{
    public interface ITableService
    {
        TableDetailViewModel GeTableDetailViewModel(string tableName);
        RowDetailViewModel GetListColumnForAddNew(string tableName);
        RowDetailViewModel GetListColumnForEdit(string tableName, int rowId);
        void AddNewTable(Table table);
        void InsertData(string tableName, Dictionary<string, object> data);
        void UpdateData(string tableName, Dictionary<string, object> data);

        void DeleteData(string tableName, string primaryColumn, string rowId);
    }
}
