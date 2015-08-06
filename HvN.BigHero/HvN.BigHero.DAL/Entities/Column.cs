using System.ComponentModel.DataAnnotations.Schema;
using HvN.BigHero.DAL.Utility;

namespace HvN.BigHero.DAL.Entities
{
    public class Column : BaseEntity
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public int Order { get; set; }
        public ColumnType DataType{get; set; }
        public int? Size { get; set; }
        public bool IsPrimarykey { get; set; }
        public bool Nullable { get; set; }
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        public virtual Table Table { get; set; }
    }
}
