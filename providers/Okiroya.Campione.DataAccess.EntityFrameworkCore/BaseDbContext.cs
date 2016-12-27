using Microsoft.EntityFrameworkCore;
using Okiroya.Campione.SystemUtility;
using System;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore
{
    public abstract class BaseDbContext<T> : DbContext where T : class
    {
        private readonly string _connectionString;
        private readonly Action<T> _optionsBuilderConfiguration;

        public string ConnectionString => _connectionString;

        public BaseDbContext(string connectionString, Action<T> optionsBuilderConfiguration)
        {
            Guard.ArgumentNotEmpty(connectionString);
            Guard.ArgumentNotNull(optionsBuilderConfiguration);

            _connectionString = connectionString;
            _optionsBuilderConfiguration = optionsBuilderConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigureBaseDbContext(optionsBuilder, _connectionString, _optionsBuilderConfiguration);
            }
        }

        protected abstract void ConfigureBaseDbContext(DbContextOptionsBuilder optionsBuilder, string connectionString, Action<T> optionsBuilderConfiguration);
    }
}
