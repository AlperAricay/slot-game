using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Utility
{
    public static class EnumerableExtensions
    {
        private static readonly System.Random Random = new();
         
        public static T GetRandomElement<T>(this IEnumerable<T> list)
        {
            // If there are no elements in the collection, return the default value of T
            var enumerable = list as T[] ?? list.ToArray();
            return !enumerable.Any() ? default : enumerable.ElementAt(Random.Next(enumerable.Length));
        }
    }
}
