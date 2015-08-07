using System.Collections.Generic;
using HvN.BigHero.DAL.Entities;

namespace HvN.BigHero.DAL.Model
{
    public class RowDetailViewModel
    {
        public string TableName { get; set; }
        public ColumnViewModel PrimaryColumn { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
    }
}
