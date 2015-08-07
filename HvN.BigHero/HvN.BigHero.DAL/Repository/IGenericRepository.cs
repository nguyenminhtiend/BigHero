﻿using System;
using System.Linq;
using System.Linq.Expressions;
using HvN.BigHero.DAL.Entities;

namespace HvN.BigHero.DAL.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll();
        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetItemsWithNavigation(Expression<Func<TEntity, bool>> predicate, params string[] navigationProperties);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);

    }
}
