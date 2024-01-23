
using AngularCompany.DataLayer.Entitys.Common;

namespace AngularCompany.DataLayer.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetEntityQuerys();
        Task<TEntity> GetEntityById(long Id);
        Task AddEntity(TEntity entity);
        void UpdateEntity(TEntity entity);
        void RemoveEntity(TEntity entity);
        Task RemoveEntity(long Id);
        Task SaveChenges();
    }
}
