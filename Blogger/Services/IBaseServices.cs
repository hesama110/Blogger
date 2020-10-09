using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBaseServices<TEntity>:IDisposable
    {
        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        List<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Get Async all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        /// <summary>
        /// Get query for entity page and page size
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? page = null,
        int? pageSize = null);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// Get Async single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get Async first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);


        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert Async entity to db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<(int ResultAction, TEntity AddedEntity)> InsertAsync(TEntity entity);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity);

        /// <summary>
        /// ...Update(Model, d=>d.Name);
        ///or
        /// ...Update(Model, d=>d.Name, d=>d.SecondProperty, d=>d.AndSoOn);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties);

        /// <summary>
        /// ...Update(Model, d=>d.Name);
        ///or
        /// ...Update(Model, d=>d.Name, d=>d.SecondProperty, d=>d.AndSoOn);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="modifiedProperties"></param>
        /// <returns></returns>
        Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] modifiedProperties);

        /// <summary>
        /// update with list of fields
        /// ...Update(Model, d=>d.Name);
        ///or
        /// ...Update(Model, d=>d.Name, List<string>);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="modifiedProperties"></param>
        void Update(TEntity entity,  List<string> modifiedProperties);

        /// <summary>
        /// update with list of fields
        /// ...Update(Model, d=>d.Name);
        ///or
        /// ...Update(Model, d=>d.Name,  List<string>);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="modifiedProperties"></param>
        /// <returns></returns>
        Task<(int ResultAction, TEntity UpdatedEntity)> UpdateAsync(TEntity entity, List<string> modifiedProperties);

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        ///  Delete Async entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(int ResultAction, TEntity DeletedEntity)> DeleteAsync(object id);

        /// <summary>
        /// Count Async all rows
        /// </summary>
        /// <returns>number of rows</returns>
        int Count();

        /// <summary>
        /// Count all rows
        /// </summary>
        /// <returns>number of rows</returns>
        Task<int> CountAsync();

    }
}
