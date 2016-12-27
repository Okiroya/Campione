using Okiroya.Campione.DataAccess;
using Okiroya.Campione.DataAccess.EntityFrameworkCore;
using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Tests.Internal.DataAccess
{
    public class SelectDataServiceCommand : IDbContextCommand<TestInMemoryDbContext>
    {
        public const string Name = "SelectDataServiceCommand";

        public string CommandName => Name;

        public DataQueryResult Execute(TestInMemoryDbContext context, IDictionary<string, object> parameters)
        {
            Guard.ArgumentNotNull(context);
            Guard.ArgumentNotNull(parameters);

            var data = new List<DataItemElement>();

            var query = context.Entities.Where(p => p.Name == parameters["Name"].ToString());
            foreach (var item in query)
            {
                data.Add(new DataItemElement("Id", typeof(int), item.Id));
                data.Add(new DataItemElement("Name", typeof(int), item.Name));
            }

            return new DataQueryResult(new[] { new DataItem { Index = 0, Items = data } }, new Dictionary<string, object>(), null);
        }

        public Task<DataQueryResult> ExecuteAsync(TestInMemoryDbContext context, IDictionary<string, object> parameters, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}