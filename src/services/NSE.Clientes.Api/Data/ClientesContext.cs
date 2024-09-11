using Microsoft.EntityFrameworkCore;
using NSE.Clientes.Api.Models;
using NSE.Core.Data;
using NSE.Core.Mediator;
using NSE.Core.Messages;
using NSE.Core.ModelObjects;

namespace NSE.Clientes.Api.Data
{
    public class ClientesContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesContext(DbContextOptions<ClientesContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediatorHandler = mediatorHandler;
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesContext).Assembly);
        }

        public async Task<bool> CommitAsync()
        {
            var sucesso = await base.SaveChangesAsync() > 0;
            if (sucesso) await _mediatorHandler.PublicarEventos(this);
            return sucesso;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T context) where T : DbContext
        {
            var domainEntities = context.ChangeTracker.Entries<Entity>().Where(e => e.Entity.Notificacoes != null && e.Entity.Notificacoes.Any());
            var domainEvents = domainEntities.SelectMany(e => e.Entity.Notificacoes).ToList();
            domainEntities.ToList().ForEach(e => e.Entity.LimparEventos());
            var tasks = domainEvents.Select(async evento => await mediator.PublicarEvento(evento));
            await Task.WhenAll(tasks);
        }
    }
}
