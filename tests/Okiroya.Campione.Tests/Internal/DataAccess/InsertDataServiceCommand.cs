using Okiroya.Campione.DataAccess;
using Okiroya.Campione.DataAccess.EntityFrameworkCore;
using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Tests.Internal.DataAccess
{
    public class InsertDataServiceCommand : IDbContextCommand<TestInMemoryDbContext>
    {
        public const string Name = "InsertDataServiceCommand";

        public string CommandName => Name;

        public DataQueryResult Execute(TestInMemoryDbContext context, IDictionary<string, object> parameters)
        {
            Guard.ArgumentNotNull(context);
            Guard.ArgumentNotNull(parameters);

            context.Add(
                new TestRepositoryEntity
                {
                    Id = (int)parameters["Id"],
                    Name = parameters["Name"].ToString()
                });

            context.SaveChanges(true);

            return new DataQueryResult(new[] { new DataItem { Index = 0, Items = new DataItemElement[0] } }, new Dictionary<string, object>(), null);
        }

        public Task<DataQueryResult> ExecuteAsync(TestInMemoryDbContext context, IDictionary<string, object> parameters, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
