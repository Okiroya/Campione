using Okiroya.Campione.DataAccess.EntityFrameworkCore.Providers.InMemory;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Okiroya.Campione.Tests.Internal.DataAccess
{
    public class TestInMemoryDbContext : InMemoryDbContext
    {
        public DbSet<TestRepositoryEntity> Entities { get; set; }

        public TestInMemoryDbContext(string connectionString, Action<InMemoryDbContextOptionsBuilder> optionsBuilderConfiguration)
            : base(connectionString, optionsBuilderConfiguration)
        { }
    }
}
