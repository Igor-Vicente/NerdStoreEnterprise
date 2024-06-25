using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Api.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.Api.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        public async Task<bool> CommitAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        }
    }
}
