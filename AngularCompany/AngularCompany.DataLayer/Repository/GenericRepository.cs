using AngularCompany.DataLayer.Context;
using AngularCompany.DataLayer.Entitys.Common;
using Microsoft.EntityFrameworkCore;

namespace AngularCompany.DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region Constractor
        private DbSet<TEntity> dbset;
        private AngularCompanyDbContext Context;
        public GenericRepository(AngularCompanyDbContext context)
        {
            this.Context = context;
            this.dbset = this.Context.Set<TEntity>();  
        }
        #endregion
        
        public IQueryable<TEntity> GetEntityQuerys()
        {
            return this.dbset.AsQueryable();
        }
        public async Task<TEntity> GetEntityById(long Id)
        {
            return await this.dbset.SingleOrDefaultAsync(t => t.Id == Id);
        }
        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            await dbset.AddAsync(entity);
        }
       
        public void UpdateEntity(TEntity entity)
        {
            this.dbset.Update(entity); 
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.IsDeleted= true; 
            this.UpdateEntity(entity);
        }

        public async Task RemoveEntity(long Id)
        {
            var Entity = await this.GetEntityById(Id);
            Entity.IsDeleted = true;
            this.RemoveEntity(Entity);
        }          
        public async Task SaveChenges()
        {
            this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Context!.Dispose();
        }
    }
}
