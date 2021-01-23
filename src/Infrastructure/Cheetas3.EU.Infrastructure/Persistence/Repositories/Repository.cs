using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities.Base;

namespace Cheetas3.EU.Infrastructure.Persistance
{

    public class Repository : IRepository
    {
        private readonly DbContext context;
        public DbContext Context
        {
            get { return context; }
        }

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TModel> FromSqlRaw<TModel>(string sql) where TModel : Entity
        {
            var items = context.Set<TModel>().FromSqlRaw(sql);
            return items;
        }

        public IEnumerable<TModel> ExecWithStoredProcedure<TModel>(string spName, params object[] parameters) where TModel : Entity
        {
            return context.Set<TModel>().FromSqlRaw(spName, parameters);
        }

        public void Add<TModel>(TModel entity) where TModel : Entity
        {
            context.Set<TModel>().Add(entity);
            SaveChanges();

            //try
            //{
            //    context.SaveChanges();
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    //foreach (var error in ex.EntityValidationErrors)
            //    //{
            //    //    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", error.Entry.Entity.GetType().Name, error.Entry.State);
            //    //    foreach (var item in error.ValidationErrors)
            //    //        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", item.PropertyName, item.ErrorMessage);
            //    //}
            //    throw;
            //}
        }

        public void Add<TModel>(IEnumerable<TModel> entities) where TModel : Entity
        {
            foreach (var entity in entities)
                Add(entity);
        }

        public IQueryable<TModel> FindAll<TModel>(params Expression<Func<TModel, object>>[] navigationProperties) where TModel : Entity
        {
            return Query<TModel>(x => true, navigationProperties);
        }

        public IQueryable<TModel> Query<TModel>(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : Entity
        {
            var items = GetSetWithNavigationProperties<TModel>(navigationProperties);
            return predicate != null ? items.Where(predicate) : items;
        }

        public void Delete<TModel>(object key) where TModel : Entity
        {
            var entity = Find<TModel>(key);
            Delete(entity);
        }

        public void Delete<TModel>(TModel entity) where TModel : Entity
        {
            if (entity == null) return;
            context.Set<TModel>().Remove(entity);
            SaveChanges();
        }

        public void Delete<TModel>(Expression<Func<TModel, bool>> predicate) where TModel : Entity
        {
            var entity = Find(predicate);
            Delete(entity);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public TModel SaveChanges<TModel>(object key) where TModel : Entity
        {
            var entity = context.Set<TModel>().Find(key);
            context.SaveChanges();
            return entity;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public TModel Find<TModel>(object key) where TModel : Entity
        {
            return context.Set<TModel>().Find(key);
        }

        public TModel Find<TModel>(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] navigationProperties) where TModel : Entity
        {
            var entity = GetSetWithNavigationProperties<TModel>(navigationProperties).SingleOrDefault(predicate);
            return entity;
        }

        private IQueryable<TModel> GetSetWithNavigationProperties<TModel>(params Expression<Func<TModel, object>>[] navigationProperties) where TModel : Entity
        {
            IQueryable<TModel> items = context.Set<TModel>();

            if (!navigationProperties.Any())
                return items;
            else
            {
                //return items.SimpleInclude<TModel>();

                //When multiple descending navigationProperties are required we'll have to implment 'ThenInclude'
                foreach (Expression<Func<TModel, object>> navigationProperty in navigationProperties)
                    items = items.Include(navigationProperty);

                return items;

            }
        }
    }
}