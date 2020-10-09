using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Blogger.Context
{

    public interface IDbContext : IDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable
    {

        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }
        IModel Model { get; }
        DbContextId ContextId { get; }

        EntityEntry Add(object entity);
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        void AddRange(params object[] entities);
        void AddRange(IEnumerable<object> entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
        Task AddRangeAsync(params object[] entities);
        EntityEntry Attach(object entity);
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
        void AttachRange(IEnumerable<object> entities);
        void AttachRange(params object[] entities);
        void Dispose();
        ValueTask DisposeAsync();
        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        bool Equals(object obj);
        TEntity Find<TEntity>( params object[] keyValues) where TEntity : class;
        object Find(Type entityType,  params object[] keyValues);
        ValueTask<object> FindAsync(Type entityType,  object[] keyValues, CancellationToken cancellationToken);
        ValueTask<object> FindAsync(Type entityType,  params object[] keyValues);
        ValueTask<TEntity> FindAsync<TEntity>( object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
        ValueTask<TEntity> FindAsync<TEntity>( params object[] keyValues) where TEntity : class;
        int GetHashCode();
        DbQuery<TQuery> Query<TQuery>() where TQuery : class;
        EntityEntry Remove(object entity);
        EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange(IEnumerable<object> entities);
        void RemoveRange(params object[] entities);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();
        EntityEntry Update(object entity);
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange(IEnumerable<object> entities);
        void UpdateRange(params object[] entities);
    }
}
