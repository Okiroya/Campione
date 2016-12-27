using System;
using Okiroya.Campione.Domain;

namespace Okiroya.Campione.Service
{
    internal class DefaultEntityService<TResult, TKey> : BaseEntityService<TResult, TKey>
        where TResult : class, IEntityObject<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    { }
}
