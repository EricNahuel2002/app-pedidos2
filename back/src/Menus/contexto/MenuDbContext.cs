using Microsoft.EntityFrameworkCore;
using Menus.entidad;
namespace Menus.Context
{

        public class MenuDbContext : DbContext
        {
            public MenuDbContext(DbContextOptions<MenuDbContext> options)
            : base(options)
            {
            }
            public DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
        }
    }
    
}
