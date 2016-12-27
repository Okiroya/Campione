using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore.Providers.InMemory
{
    public abstract class InMemoryDbContext : BaseDbContext<InMemoryDbContextOptionsBuilder>
    {
        public InMemoryDbContext(string connectionString, Action<InMemoryDbContextOptionsBuilder> optionsBuilderConfiguration)
            : base(connectionString, optionsBuilderConfiguration)
        { }

        protected override void ConfigureBaseDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString, Action<InMemoryDbContextOptionsBuilder> optionsBuilderConfiguration)
        {
            optionsBuilder.UseInMemoryDatabase(connectionString, optionsBuilderConfiguration);
        }
    }
}
