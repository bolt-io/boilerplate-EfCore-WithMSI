using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.SharedKernel.Abstractions;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Loads the navigation properties of an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static async Task<T?> LoadNavigationPropertiesAsync<T>(this T? entity, DbContext dbContext) where T : IAggregateRoot
        {
            if (entity == null) return default;

            foreach (var navigation in dbContext.Entry(entity).Navigations)
            {
                await navigation.LoadAsync();
            }

            return entity;
        }

        /// <summary>
        /// Includes the navigation properties of an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IQueryable<T?> IncludeNavigationPropertiesAsync<T>(this IQueryable<T?> entity, DbContext dbContext) where T : class, IAggregateRoot
        {
            if (entity == null) return default;

            var matchedEntityType = dbContext.Model.GetEntityTypes(typeof(T)).FirstOrDefault();
            if (matchedEntityType == null) return entity;
            
            foreach (var navigation in matchedEntityType.GetNavigations())
            {
                entity = entity.Include(navigation.Name); // this only goes down one level. We may need to do some ThenInclude() processing if objects become further nested.
            }

            return entity;
        }

        /// <summary>
        /// A safe method to call ToListAsync.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        { // https://expertcodeblog.wordpress.com/2018/02/19/net-core-2-0-resolve-error-the-source-iqueryable-doesnt-implement-iasyncenumerable/
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }


        internal static Task<Storage.IDbContextTransaction> StartTransaction(this DbContext dbContext)
        {
            return dbContext.Database.BeginTransactionAsync();
        }
    }
}