using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore
{
    public interface IDbContextCommand<TContext>
        where TContext : DbContext
    {
        string CommandName { get; }

        DataQueryResult Execute(TContext context, IDictionary<string, object> parameters);

        Task<DataQueryResult> ExecuteAsync(TContext context, IDictionary<string, object> parameters, CancellationToken token);
    }
}
