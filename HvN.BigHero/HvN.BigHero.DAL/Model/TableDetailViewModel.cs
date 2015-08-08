using System.Collections.Generic;
using System.Linq;

namespace HvN.BigHero.DAL.Model
{
    public class TableDetailViewModel
    {
        public string TableName { get; set; }
        public string PrimaryColumn { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }

        public bool ContainRecord
        {
            get { return Data.Any(); }
        }

        public int TotalRecord
        {
            get { return Data.Count; }
        }
        public int TotalColumn
        {
            get { return Columns.Count + 1; }
        }
    }
}
