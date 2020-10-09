
using Blogger.Context;
using Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class
    {
        private readonly IDbContext _context = null;
        private DbSet<TEntity> dbSet = null;
        /*public BaseServices()
        {
            this.context = new ApplicationDbContext();
            this.dbSet = _context.Set<TEntity>();
        }*/

        public BaseServices(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = context.Set<TEntity>();
        }
        public string GetConnectionString => _context.Database.GetDbConnection()?.ConnectionString;
        /// <summary>
        /// myservice.Get(x => x.FirstName = "Bob",q => q.OrderBy(s => s.LastName));
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;
            query = Query(filter, orderBy, includes);

            /*foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);*/

            return query.ToList();
        }

        /// <summary>
        /// myservice.Get(x => x.FirstName = "Bob",q => q.OrderBy(s => s.LastName));
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            query = Query(filter, orderBy, includes);
            /*foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);*/

            return await query.ToListAsync().ConfigureAwait(false);
        }
        /// <summary>
        ///         List<string> result = p.Get(s => s.StartsWith("A"), orderBy: q => q.OrderBy(d => d.Length)).ToList();
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? page = null,
        int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return query;
        }
        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id).ConfigureAwait(false);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="filter">where values</param>
        /// <param name="includes">include table</param>
        /// <returns></returns>
        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            query = Query(filter, null, includes);

            /*foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);
*/
            return query.FirstOrDefault(filter);
        }
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;
            query = Query(filter, null, includes);

            /*foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);
*/
            return await query.FirstOrDefaultAsync(filter).ConfigureAwait(false);
        }

        public virtual void Insert(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
            // dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual async Task<(int ResultAction, TEntity AddedEntity)> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);
            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return (result, entity);
        }
        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public virtual async Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return (result, entity);
        }
        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties)
        {
            dbSet.Attach(entity);

            foreach (var property in modifiedProperties)
            {
                // string propertyName = GeneralExtensions.GetPropertyName<TEntity, object>(property);
                string propertyName = Extensions.PropertyExtensions.GetPropertyName<TEntity, object>(property);
                _context.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            }

            _context.SaveChanges();
        }

        public virtual async Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties)
        {
            dbSet.Attach(entity);

            foreach (var property in modifiedProperties)
            {
                // string propertyName = GeneralExtensions.GetPropertyName<TEntity, object>(property);
                string propertyName = Extensions.PropertyExtensions.GetPropertyName<TEntity, object>(property);
                _context.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            }

            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return (result, entity);
        }

        public virtual void Update(TEntity entity, List<string> modifiedProperties)
        {
            dbSet.Attach(entity);

            foreach (var property in modifiedProperties)
            {
                // string propertyName = GeneralExtensions.GetPropertyName<TEntity, object>(property);
                string propertyName = property;// PropertyHelper.GetPropertyName<TEntity, object>(property);
                _context.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            }

            _context.SaveChanges();
        }

        public virtual async Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity, List<string> modifiedProperties)
        {
            
            dbSet.Attach(entity);

            foreach (var property in modifiedProperties)
            {
                // string propertyName = GeneralExtensions.GetPropertyName<TEntity, object>(property);
                string propertyName = property;// PropertyHelper.GetPropertyName<TEntity, object>(property);
                _context.Entry<TEntity>(entity).Property(propertyName).IsModified = true;
            }

            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return (result, entity);
        }

        /*  public void Update(TEntity entity, Expression<Func<TEntity, object>>[] properties)
          {
              _context.Entry(entity).State = EntityState.Unchanged;
              foreach (var property in properties)
              {
                  var propertyName = ExpressionHelper.GetExpressionText(property);
                  _context.Entry(entity).Property(propertyName).IsModified = true;
              }
              //return _context.SaveChangesWithoutValidation();
          }*/

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }
        public virtual async Task<(int ResultAction, TEntity DeletedEntity)> DeleteAsync(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            var result = await _context.SaveChangesAsync().ConfigureAwait(false);
            return (result, entityToDelete);
        }
        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync().ConfigureAwait(false);
        }

        private IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, object>>> includes)
        {
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }


    }
}
