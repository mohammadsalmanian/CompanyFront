

using AngularCompany.DataLayer.Entitys.Access;
using AngularCompany.DataLayer.Entitys.Account;
using AngularCompany.DataLayer.Entitys.Site;
using Microsoft.EntityFrameworkCore;

namespace AngularCompany.DataLayer.Context
{
    public class AngularCompanyDbContext : DbContext
    {
        public AngularCompanyDbContext(DbContextOptions<AngularCompanyDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder option)
        //{
        //    option.UseSqlServer("Server = . ; Initial Catalog = AngularCompanyDb; Integrated Security = True;TrustServerCertificate=True");
        //    base.OnConfiguring(option);
        //}

        #region dbset
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Slider> Sliders { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascades = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
