using Microsoft.EntityFrameworkCore.Infrastructure;
using Okiroya.Campione.DataAccess.EntityFrameworkCore;
using System;

namespace Okiroya.Campione.Tests.Internal.DataAccess
{
    public sealed class TestInMemoryEntityFrameworkDataService : EntityFrameworkDataService<TestInMemoryDbContext, InMemoryDbContextOptionsBuilder>
    {
        public TestInMemoryEntityFrameworkDataService(TestInMemoryDbContext context)
            : base(context)
        { }
    }
}
