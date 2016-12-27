using Okiroya.Campione.SystemUtility;
using Okiroya.Campione.SystemUtility.DI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.EntityFrameworkCore
{
    public abstract class EntityFrameworkDataService<TContext, TOptionBuilder> : BaseDataService
        where TContext : BaseDbContext<TOptionBuilder>
        where TOptionBuilder : class
    {
        private readonly TContext _context;

        protected TContext Context => _context;

        public EntityFrameworkDataService(TContext context)
        {
            Guard.ArgumentNotNull(context);

            _context = context;
        }

        public override void BulkInsert<T>(string destination, TableValueParameter<T> table)
        {
            throw new NotImplementedException();
        }

        public override Task BulkInsertAsync<T>(string destination, TableValueParameter<T> table, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override DataQueryResult ExecuteCommand(string commandName, IDictionary<string, object> parameters)
        {
            return RegisterDependencyContainer<IDbContextCommand<TContext>>.Resolve(commandName).Execute(_context, parameters);
        }

        public override async Task<DataQueryResult> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await RegisterDependencyContainer<IDbContextCommand<TContext>>.Resolve(commandName).ExecuteAsync(_context, parameters, cancellationToken);
        }

        public override DataQueryResult ExecuteQuery(string commandName, IDictionary<string, object> parameters)
        {
            return RegisterDependencyContainer<IDbContextCommand<TContext>>.Resolve(commandName).Execute(_context, parameters);
        }

        public override async Task<DataQueryResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await RegisterDependencyContainer<IDbContextCommand<TContext>>.Resolve(commandName).ExecuteAsync(_context, parameters, cancellationToken);
        }

        protected override string ResolveConnectionString(DataServiceCommandType commandType)
        {
            return _context.ConnectionString;
        }
    }
}
