using System.Collections.Generic;
using System.Linq;

namespace HvN.BigHero.DAL.Model
{
    public class RowDetailViewModel
    {
        public string TableName { get; set; }
        public ColumnViewModel PrimaryColumn { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
        public string StringColumns
        {
            get
            {
                return string.Join(",", Columns.Select(x => x.Name));
            }
        }
    }
}
