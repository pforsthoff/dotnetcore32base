using Cheetas3.EU.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cheetas3.EU.Application.Interfaces
{
    public interface IRepository : IDisposable
    {
        #region Public Methods and Operators

        IEnumerable<TEntity> FromSqlRaw<TEntity>(string sql) where TEntity : Entity;

        IEnumerable<TEntity> ExecWithStoredProcedure<TEntity>(string spName, params object[] parameters) where TEntity : Entity;

        void Add<TEntity>(TEntity entity) where TEntity : Entity;

        void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;

        IQueryable<TEntity> FindAll<TEntity>(params Expression<Func<TEntity, object>>[] navigationProperties) where TEntity : Entity;

        void Delete<TEntity>(object key) where TEntity : Entity;

        void Delete<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity;

        IQueryable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] navigationProperties)
            where TEntity : Entity;

        TEntity Find<TEntity>(object key) where TEntity : Entity;

        TEntity Find<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] navigationProperties)
            where TEntity : Entity;

        TEntity SaveChanges<TEntity>(object key) where TEntity : Entity;

        void SaveChanges();

        #endregion
    }
}
