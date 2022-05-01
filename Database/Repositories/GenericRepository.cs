using Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
       where TEntity : class, IEntity, new()
    {
        #region Declarations

        protected DataContext database;
        protected DbSet<TEntity> entities;
        protected object lockObject;

        #endregion

        #region Constructor 

        public GenericRepository(object lockObject, DataContext database)
        {
            this.lockObject = lockObject;
            this.database = database;
            entities = database.Set<TEntity>();
        }

        #endregion

        #region Methods

        public int Insert(TEntity entity)
        {
            lock (lockObject)
            {
                if (entity != null)
                {
                    entity.Id = null;
                    entities.Add(entity);
                }
                database.SaveChanges();
                return entity.Id.Value;
            }

        }
        public void Update(TEntity entity)
        {
            lock (lockObject)
            {
                if (entity != null)
                {
                    entities.Update(entity);
                }
                database.SaveChanges();
            }
        }
        public void Delete(TEntity entity)
        {
            lock (lockObject)
            {
                if (entity != null)
                {
                    entities.Remove(entity);
                }
                database.SaveChanges();
            }
        }

        public async Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> where = null)
        {
            TEntity selectedEntity = new TEntity();

            lock (lockObject)
            {
                if (where != null)
                {
                    selectedEntity = entities.Where(where).FirstOrDefault();
                }
            }
            return await Task.Run(() => { return selectedEntity; });
        }

        public async Task<IEnumerable<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> where = null)
        {
            List<TEntity> selectedEntities = new List<TEntity>();

            lock (lockObject)
            {
                if (where != null)
                {
                    selectedEntities = entities.Where(where).ToList();
                }
                else
                {
                    selectedEntities = entities.ToList();
                }
            }
            return await Task.Run(() => { return selectedEntities; });
        }

        public void Dispose()
        {
            database.Dispose();
        }

        #endregion
    }
}
