namespace Corona.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AssemblyExtensions
    {
        public static List<Type> GetTypesAssignableFrom<T>(this AppDomain appDomain)
        {
            var assignableType = typeof(T);
            return appDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => 
                    assignableType.IsAssignableFrom(x) 
                    && x.IsClass 
                    && !x.IsAbstract 
                    && assignableType != x
                )
                .ToList();
        }
    }
}
