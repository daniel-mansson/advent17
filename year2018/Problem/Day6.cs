using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Vec2
	{
		public int x;
		public int y;

		public static Vec2 Parse(string raw, string sep)
		{
			var s = raw.Split(new string[] { sep }, StringSplitOptions.None);
			return new Vec2()
			{
				x = int.Parse(s[0]),
				y = int.Parse(s[1])
			};
		}

		public static int Mahattan(Vec2 a, Vec2 b)
		{
			return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
		}
	}

	public class Day6 : BaseDay
	{
		public override string Example1 => 
@"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9";

		List<Vec2> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Split('\n')
				.Select(i => Vec2.Parse(i, ", "))
				.ToList();
		}

		class Entry
		{
			public int id;
			public Vec2 pos = new Vec2();
		}

		class Result
		{
			public int[] grid;
			public Vec2 size;
			public KeyValuePair<int,int> best;
		}

		Result Wrap1(List<Vec2> input)
		{
			Vec2 min = new Vec2();
			min.x = input.Min(i => i.x);
			min.y = input.Min(i => i.y);
			Vec2 max = new Vec2();
			max.x = input.Max(i => i.x);
			max.y = input.Max(i => i.y);
			Vec2 size = new Vec2();
			size.x = max.x - min.x + 1;
			size.y = max.y - min.y + 1;

			foreach (var item in input)
			{
				item.x -= min.x;
				item.y -= min.y;
			}

			var grid = new int[size.x * size.y];
			var queue = new List<Entry>();
			for (int i = 0; i < input.Count; i++)
			{
				queue.Add(new Entry()
				{
					id = i + 1,
					pos = input[i]
				});
			}

			var n = new Vec2[]
			{
				new Vec2(){ x = 1, y = 0 },
				new Vec2(){ x = -1, y = 0 },
				new Vec2(){ x = 0, y = 1 },
				new Vec2(){ x = 0, y = -1 },
			};

			var work = new List<Entry>();

			while (queue.Count > 0)
			{
				work.Clear();

				foreach (var entry in queue)
				{
					var idx = entry.pos.y * size.x + entry.pos.x;

					if (grid[idx] == 0)
					{
						grid[idx] = entry.id;
						work.Add(entry);
					}
					else if (grid[idx] != entry.id)
					{
						if (grid[idx] != -1)
						{
							grid[idx] = -1;
							work.Add(entry);
						}
					}
				}
				queue.Clear();

				foreach (var entry in work)
				{
					for (int i = 0; i < n.Length; i++)
					{
						var p = new Vec2()
						{
							x = entry.pos.x + n[i].x,
							y = entry.pos.y + n[i].y
						};

						if (p.x >= 0 && p.x < size.x && p.y >= 0 && p.y < size.y)
						{
							var nidx = p.y * size.x + p.x;
							if (grid[nidx] == 0)
							{
								queue.Add(new Entry()
								{
									id = entry.id,
									pos = p
								});
							}
						}
					}
				}
			}

			var dict = new Dictionary<int, int>();
			for (int i = 0; i < input.Count; i++)
			{
				dict[i + 1] = 0;
			}
			dict[-1] = 0;
			dict[0] = 0;

			var edge = new Dictionary<int, int>();
			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					var idx = y * size.x + x;

					if (x == 0 || y == 0 || x == size.x - 1 || y == size.y - 1)
					{
						edge[grid[idx]] = grid[idx];
					}

					dict[grid[idx]]++;
				}
			}

			var valid = dict.Where(kvp => !edge.ContainsKey(kvp.Key)).ToList();
			var best = valid.First(kvp => kvp.Value == valid.Select(v => v.Value).Max());

			return new Result()
			{
				best = best,
				grid = grid,
				size = size
			};
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			return Wrap1(input).best.Value;
		}

		void DebugPrint(int[] grid, Vec2 size)
		{
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.x; j++)
				{
					var v = grid[i * size.x + j];
					Console.Write((char)(v + 'a' - 1));
				}
				Console.WriteLine();
			}

			Console.WriteLine();
			Console.WriteLine();
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);
			var res = Wrap1(input);

			int target = 10000;
			if (input.Count < 10)
				target = 32;

			int count = 0;

			for (int i = 0; i < res.size.y; i++)
			{
				for (int j = 0; j < res.size.x; j++)
				{
					var sum = input
						.Select(p => Vec2.Mahattan(p, new Vec2 { x = j, y = i }))
						.Sum();

					if (sum < target)
					{
						++count;
					}
				}
			}

			return count;
		}
	}
}