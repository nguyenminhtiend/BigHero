using System.Data.Entity;
using HvN.BigHero.DAL.Entities;

namespace HvN.BigHero.DAL.DataContext
{
    public class BigHeroContext : DbContext
    {
        public BigHeroContext()
            : base("name=BigHeroConnnectionString")
        {
            
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            //Database.SetInitializer(new DropCreateDatabaseAlways<BigHeroContext>());
        }

        public IDbSet<Table> Tables { get; set; }
        public IDbSet<Column> Columns { get; set; }
    }
}
