using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day12 : BaseDay
	{
		public override string Example1 =>
@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";

		class Init
		{
			public Dictionary<int, int> state = new Dictionary<int, int>();
			public Dictionary<int, bool> alive = new Dictionary<int, bool>();
		}

		Init Transform(string raw)
		{
			var res = new Init();

			var lines = raw
				.Replace("initial state: ", "")
				.Replace("\r", "")
				.Split('\n');

			for (int i = 0; i < lines[0].Length; i++)
			{
				if (lines[0][i] == '#')
				{
					res.state.Add(i, i);
				}
			}

			for (int i = 2; i < lines.Length; i++)
			{
				var str = lines[i];

				int value = 0;
				for (int j = 0; j < 5; j++)
				{
					if (str[j] == '#')
					{
						value |= 1 << j;
					}
				}

				res.alive.Add(value, str[9] == '#');
			}

			return res;
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			Dictionary<int, int> current = input.state;
			//Print(current);

			for (int gen = 0; gen < 20; gen++)
			{
				Dictionary<int, int> nextGen = current.ToDictionary(e => e.Key, e => e.Value);

				int minIdx = current.Keys.Min();
				int maxIdx = current.Keys.Max();

				for (int i = minIdx - 5; i <= maxIdx + 5; i++)
				{
					int value = 0;
					for (int n = -2; n <= 2; n++)
					{
						if (current.ContainsKey(i + n))
						{
							value |= 1 << (n + 2);
						}
					}

					if (input.alive.TryGetValue(value, out bool alive))
					{
						if (alive)
						{
							nextGen[i] = i;
						}
						else
						{
							nextGen.Remove(i);
						}
					}
					else
					{
						nextGen.Remove(i);
					}
				}

				current = nextGen;
				//Print(current);
			}

			return current.Values.Sum();
		}

		void Print(Dictionary<int, int> state)
		{
			int minIdx = state.Keys.Min();
			int maxIdx = state.Keys.Max();

			minIdx = -3;
			maxIdx = 35;
			//Console.WriteLine(minIdx + " - " + maxIdx);
			for (int i = minIdx; i <= maxIdx; i++)
			{
				Console.Write(state.ContainsKey(i) ? "#" : ".");
			}
			Console.WriteLine();
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			Dictionary<int, int> current = input.state;

			int prevdelta = current.Values.Sum();
			int prevsum = current.Values.Sum();
			int deltacount = 0;

			for (int gen = 0; gen < 200; gen++)
			{
				Dictionary<int, int> nextGen = current.ToDictionary(e => e.Key, e => e.Value);

				int minIdx = current.Keys.Min();
				int maxIdx = current.Keys.Max();

				for (int i = minIdx - 5; i <= maxIdx + 5; i++)
				{
					int value = 0;
					for (int n = -2; n <= 2; n++)
					{
						if (current.ContainsKey(i + n))
						{
							value |= 1 << (n + 2);
						}
					}

					if (input.alive.TryGetValue(value, out bool alive))
					{
						if (alive)
						{
							nextGen[i] = i;
						}
						else
						{
							nextGen.Remove(i);
						}
					}
					else
					{
						nextGen.Remove(i);
					}
				}

				current = nextGen;

				var sum = current.Values.Sum();
				var delta = sum - prevsum;

				if (delta == prevdelta)
				{
					deltacount++;
					if (deltacount > 10)
					{
						return (long)sum + (long)delta * (50000000000L - (long)gen - 1L);
					}
				}
				else
				{
					deltacount = 0;
				}
				//Console.WriteLine(delta + "\t" + prevsum);
				prevdelta = delta;
				prevsum = sum;
			}

			return -1;
		}
	}
}