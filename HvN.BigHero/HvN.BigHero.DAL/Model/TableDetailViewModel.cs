using System.Collections.Generic;

namespace HvN.BigHero.DAL.Model
{
    public class TableDetailViewModel
    {
        public string TableName { get; set; }
        public string PrimaryColumn { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
        public List<Dictionary<string, object>> Data { get; set; } 
    }
}
