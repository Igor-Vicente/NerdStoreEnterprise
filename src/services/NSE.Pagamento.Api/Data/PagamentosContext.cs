﻿using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Core.Messages;
using NSE.Pagamentos.Api.Models;

namespace NSE.Pagamentos.Api.Data
{
    public class PagamentosContext : DbContext, IUnitOfWork
    {
        public PagamentosContext(DbContextOptions<PagamentosContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosContext).Assembly);
        }

        public async Task<bool> CommitAsync()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}
