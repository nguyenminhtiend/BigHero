using System.Collections.Generic;

namespace HvN.BigHero.DAL.Entities
{
    public class Table : BaseEntity
    {
        public string Name { get; set; }
        public bool IsIdentity { get; set; }
        public ICollection<Column> Columns { get; set; }
    }
}
