using API_GestionEconomia.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_GestionEconomia.DataBase
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) :base(options)
        {
            
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Organizador> Organizadores { get; set; }
        public DbSet<OrganizacionEconomica> OrganizacionesEconomicas { get; set; }
        public DbSet<Economia> Economias { get; set; }
    }
}
