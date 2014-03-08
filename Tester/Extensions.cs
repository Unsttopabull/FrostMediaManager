using System;
using System.Data.Entity;
using System.Diagnostics;
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
            T item = context.Set<T>().Local.Where(expression.Compile()).FirstOrDefault();
            if (item != null) {
                Debug.WriteLine("Found in EF Cache");
                Debug.Unindent();

                return item;
            }

            item = context.Set<T>().FirstOrDefault(expression);

            if (item != null) {
                Debug.WriteLine("Found in DB");
                Debug.Unindent();

                return item;
            }
            return null;
        }
    }

}