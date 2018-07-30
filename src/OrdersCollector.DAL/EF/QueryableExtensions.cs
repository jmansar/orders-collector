using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace OrdersCollector.DAL.EF
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> EagerFetch<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path) where T : class
        {
            return source.Include(path);
        }
    }
}
