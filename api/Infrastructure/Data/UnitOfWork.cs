using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string,object> _repositories = [];

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
            {
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(repositoryType, context)
                    ?? throw new InvalidOperationException($"Création de l'instance impossible pour {t}");
            });
        }

        public async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() > 0;
        }
        public void Dispose()
        {
            context.Dispose();
            
        }
    }
}
