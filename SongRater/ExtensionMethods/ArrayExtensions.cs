using System;
using System.Collections.Generic;

namespace SongRater.ExtensionMethods
{
	public static class ArrayExtensions
	{
		private static readonly Random rng = new Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			Shuffle(list, rng);
		}

		public static void Shuffle<T>(this IList<T> list, int seed)
		{
			Shuffle(list, new Random(seed));
		}

		public static void Shuffle<T>(this IList<T> list, Random random)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}