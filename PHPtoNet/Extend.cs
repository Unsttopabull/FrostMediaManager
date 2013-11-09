using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Frost.PHPtoNET {
    static class Extend {
        public static T CastAs<T>(this object ex){
            return (T) ex;
        }

        static readonly Dictionary<string, Func<object>> List = new Dictionary<string, Func<object>>();

        public static T New<T>() where T : class {
            return New(typeof(T)) as T;
        }

        public static object New(Type type){
            if (List.ContainsKey(type.Name)) {
                return List[type.Name];
            }

            Func<object> method = Expression.Lambda<Func<object>>(Expression.Block(type, new Expression[] { Expression.New(type) })).Compile();
            List.Add(type.Name, method);
            return method();
        }

    }
}
