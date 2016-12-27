using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore.Providers.PostgreSql
{
    public abstract class PostgreSqlDbContext : BaseDbContext<NpgsqlDbContextOptionsBuilder>
    {
        public PostgreSqlDbContext(string connectionString, Action<NpgsqlDbContextOptionsBuilder> optionsBuilderConfiguration)
            : base(connectionString, optionsBuilderConfiguration)
        { }

        protected override void ConfigureBaseDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString, Action<NpgsqlDbContextOptionsBuilder> optionsBuilderConfiguration)
        {
            optionsBuilder.UseNpgsql(connectionString, optionsBuilderConfiguration);
        }
    }
}
