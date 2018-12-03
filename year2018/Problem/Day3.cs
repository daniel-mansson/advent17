using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day3
	{
		static string s_example =
@"#1 @ 1,3: 4x4
#2 @ 3,1: 4x4
#3 @ 5,5: 2x2";

		static void Run(int year, int day)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

			Console.ReadKey();
		}

		class Entry
		{
			public int id;
			public int x;
			public int y;
			public int w;
			public int h;
			public int overlaps;
		}

		static List<Entry> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Replace("#", "")
				.Replace(" @ ", ";")
				.Replace(",", ";")
				.Replace(": ", ";")
				.Replace("x", ";")
				.Split('\n')
				.Select(s => 
				{
					var split = s.Split(';');
					return new Entry()
					{
						id = int.Parse(split[0]),
						x = int.Parse(split[1]),
						y = int.Parse(split[2]),
						w = int.Parse(split[3]),
						h = int.Parse(split[4]),
					};
				})
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			int maxW = input
				.Select(i => i.x + i.w)
				.Max();

			var dict = new Dictionary<int, int>();
			foreach (var item in input)
			{
				for (int i = 0; i < item.h; i++)
				{
					for (int j = 0; j < item.w; j++)
					{
						int hash = (i + item.y) * maxW + item.x + j;
						dict.TryGetValue(hash, out var value);
						dict[hash] = value + 1;
					}
				}
			}

			return dict.Count(kvp => kvp.Value > 1);
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			int maxW = input
				.Select(i => i.x + i.w)
				.Max();

			var dict = new Dictionary<int, Entry>();
			foreach (var item in input)
			{
				for (int i = 0; i < item.h; i++)
				{
					for (int j = 0; j < item.w; j++)
					{
						int hash = (i + item.y) * maxW + item.x + j;
						if (dict.TryGetValue(hash, out var entry))
						{
							item.overlaps++;
							entry.overlaps++;
						}
						else
						{
							dict[hash] = item;
						}
					}
				}
			}

			return input.First(i => i.overlaps == 0).id;

		}
	}
}