using ApiOAuthExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAuthExamen.Data
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext>
            options) : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<UsuarioCubo> Usuarios { get; set; }
        public DbSet<CompraCubo> Pedidos { get; set; }
    }
}
