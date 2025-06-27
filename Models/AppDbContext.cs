using Microsoft.EntityFrameworkCore;

namespace PagamentosApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}
