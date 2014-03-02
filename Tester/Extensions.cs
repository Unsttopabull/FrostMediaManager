using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Frost.Tester {

    public static class Extensions {
        //public static T LocalOrDatabase<T>(this DbContext context, Func<T, bool> expression) where T : class {
        //    T localOrDatabase = context.Set<T>().Local.Where(expression).FirstOrDefault();
        //    if (localOrDatabase == null) {
        //        return context.Set<T>().FirstOrDefault(t => Predicate(t, expression));
        //    }
        //    return localOrDatabase;
        //}

        public static T LocalOrDatabase<T>(this DbContext context, Expression<Func<T, Boolean>> expression) where T : class {
            IEnumerable<T> localResults = context.Set<T>().Local.Where(expression.Compile());

            T item = localResults.FirstOrDefault();
            if (item != null) {
                return item;
            }

            return context.Set<T>().FirstOrDefault(expression);
        }
    }

}