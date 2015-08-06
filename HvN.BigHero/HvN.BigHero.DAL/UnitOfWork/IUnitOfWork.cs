using System.Data.Entity;

namespace HvN.BigHero.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        bool Commit();
        DbContext Context { get; }
    }
}
