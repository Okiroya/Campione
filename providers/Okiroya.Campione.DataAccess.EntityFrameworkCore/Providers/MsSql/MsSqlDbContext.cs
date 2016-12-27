using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore.Providers.MsSql
{
    public abstract class MsSqlDbContext : BaseDbContext<SqlServerDbContextOptionsBuilder>
    {
        public MsSqlDbContext(string connectionString, Action<SqlServerDbContextOptionsBuilder> optionsBuilderConfiguration)
            : base(connectionString, optionsBuilderConfiguration)
        { }

        protected override void ConfigureBaseDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString, Action<SqlServerDbContextOptionsBuilder> optionsBuilderConfiguration)
        {
            optionsBuilder.UseSqlServer(connectionString, optionsBuilderConfiguration);
        }
    }
}
