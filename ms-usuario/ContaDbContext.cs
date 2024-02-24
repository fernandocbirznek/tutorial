using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;
using System.Data.Common;

namespace ms_usuario
{
    public class ContaDbContext : DbContext, IDbContext
    {
        public ContaDbContext(DbContextOptions<ContaDbContext> options) : base(options) { }
        public DbSet<Conta> Conta { get; set; }

        public DbConnection Connection => base.Database.GetDbConnection();
    }
}
