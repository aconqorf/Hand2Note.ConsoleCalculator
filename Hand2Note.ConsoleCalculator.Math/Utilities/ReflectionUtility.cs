using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Hand2Note.ConsoleCalculator.Math.Utilities
{
    public static class ReflectionUtility
    {
        public static IEnumerable<T> GetInstancesOfInterface<T>() where T : class
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(item => typeof(T).IsAssignableFrom(item) && !item.IsInterface).Select(item => (T) Activator.CreateInstance(item));
        }
    }
}
