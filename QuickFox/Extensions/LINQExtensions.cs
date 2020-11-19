using System;
using System.Collections.Generic;

namespace QuickFox.Extensions
{
    public static class LINQExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var element in sequence)
            {
                action.Invoke(element);
            }
        }
    }
}
