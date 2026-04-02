using System.Collections.Generic;
using System;

namespace RogueCard.Utility
{
    public static class Extensions
    {
        /// <summary>Shuffle a list in place using Fisher-Yates.</summary>
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>Pick a random element from a list.</summary>
        public static T PickRandom<T>(this IList<T> list, Random rng)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("List is empty.");
            return list[rng.Next(list.Count)];
        }

        /// <summary>Returns true if the list is null or empty.</summary>
        public static bool IsNullOrEmpty<T>(this IList<T> list) =>
            list == null || list.Count == 0;
    }
}
