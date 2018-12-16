using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day15 : BaseDay
	{
		//		public override string Example1 =>
		//@"#########
		//#G......#
		//#.E.#...#
		//#..##..G#
		//#...##..#
		//#...#...#
		//#.G...G.#
		//#.....G.#
		//#########";

		//		public override string Example1 =>
		//@"#######
		//#.E...#
		//#.#..G#
		//#.###.#
		//#E#G#G#
		//#...#G#
		//#######";

		//		public override string Example1 =>
		//@"#######
		//#E.G#.#
		//#.#G..#
		//#G.#.G#
		//#G..#.#
		//#...E.#
		//#######";

		public override string Example1 =>
@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";


		class Unit
		{
			public int idx;
			public char team;
			public int hp = 200;
			public int attack = 3;
		}

		class Input
		{
			public List<char> grid;
			public int w;
			public List<Unit> units = new List<Unit>();
			public List<int> neighbors;
			internal List<int> neighbors2;
		}

		Input Transform(string raw)
		{
			var input = new Input();

			input.grid = raw
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("G", ".")
				.Replace("E", ".")
				.ToList();

			var s = raw
				.Replace("\r", "")
				.Replace("\n", "");

			input.w = raw
				.Replace("\r", "")
				.IndexOf("\n");

			input.neighbors = new List<int>()
			{
				-input.w,
				-1,
				1,
				input.w
			};

			input.neighbors2 = new List<int>()
			{
				-input.w,
				input.w,
				-1,
				1,
			};

			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == 'G' || s[i] == 'E')
				{
					input.units.Add(new Unit()
					{
						idx = i,
						team = s[i]
					});
				}
			}

			return input;
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			for (int step = 0; step < 100000; step++)
			{
				//Print(input);
				input.units.Sort((a, b) => a.idx - b.idx);

				foreach (var unit in input.units)
				{
					if(unit.hp > 0)
					{

						bool noElves = !input.units.Any(u => u.hp > 0 && u.team == 'E');
						bool noGoblins = !input.units.Any(u => u.hp > 0 && u.team == 'G');

						if (noElves || noGoblins)
						{
							//Ended
							var hpSum = input.units
								.Where(u => u.hp > 0)
								.Sum(u => u.hp);

							Print(input);

							Console.WriteLine(step + " * " + hpSum);

							return step * hpSum;
						}

						//Move
						var nextIdx = FindStep(unit, input);
						if (nextIdx >= 0)
						{
							unit.idx = nextIdx;
						}

						//Attack
						var other = AdjEnemy(unit, input);
						if (other != null)
						{
							other.hp -= unit.attack;
						}
					}
				}
			}

			return -1;
		}

		void Print(Input input)
		{
			for (int i = 0; i < input.grid.Count; i++)
			{
				char c = input.grid[i];
				var unit = input.units.FirstOrDefault(u => u.idx == i && u.hp > 0);
				if (unit != null)
				{
					c = unit.team;
				}

				Console.Write(c);
				if ((i + 1) % input.w == 0)
				{
					Console.WriteLine();
				}
			}

			foreach (var unit in input.units)
			{
				Console.WriteLine(unit.team + ": " + unit.hp);
			}
			Console.WriteLine();

		}

		int FindStep(Unit unit, Input input)
		{
			var queue = new Queue<int>();
			queue.Enqueue(unit.idx);

			var visited = new List<int>();
			visited.Add(unit.idx);

			var parents = new Dictionary<int, int>();
			int best = -1;

			while (queue.Count > 0)
			{
				int idx = queue.Dequeue();

				if (AdjToEnemy(idx, unit.team, input) > 0)
				{
					best = idx;
					break;
				}

				for (int i = 0; i < input.neighbors.Count; i++)
				{
					var nidx = idx + input.neighbors[i];

					if (input.grid[nidx] == '.' 
						&& !input.units.Any(u => u.idx == nidx && u.hp > 0) 
						&& !visited.Contains(nidx))
					{
						queue.Enqueue(nidx);
						visited.Add(nidx);
						parents.Add(nidx, idx);
					}
				}
			}

			if (best < 0)
			{
				return -1;
			}
			else
			{
				if (parents.Count == 0)
				{
					return unit.idx;
				}

				while (true)
				{
					if (parents.TryGetValue(best, out int next))
					{
						if (next == unit.idx)
						{
							return best;
						}
						else
						{
							best = next;
						}
					}
					else
					{
						throw new Exception("boom " + best);
					}
				}
			}

		}

		private int AdjToEnemy(int idx, char team, Input input)
		{
			for (int i = 0; i < input.neighbors.Count; i++)
			{
				var nidx = idx + input.neighbors[i];
				var unit = input.units.FirstOrDefault(u => u.team != team && u.idx == nidx && u.hp > 0);
				if (unit != null)
				{
					return unit.idx;
				}
			}

			return -1;
		}

		private Unit AdjEnemy(Unit self, Input input)
		{
			var enemies = new List<Unit>();

			for (int i = 0; i < input.neighbors2.Count; i++)
			{
				var nidx = self.idx + input.neighbors2[i];
				var unit = input.units.FirstOrDefault(u => u.team != self.team && u.idx == nidx && u.hp > 0);
				if (unit != null)
				{
					enemies.Add(unit);
				}
			}

			enemies.Sort((a, b) => a.hp - b.hp);
			return enemies.FirstOrDefault();
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			for (int attack = 3; attack < 100; attack++)
			{
				var current = new Input()
				{
					grid = input.grid,
					w = input.w,
					neighbors = input.neighbors,
					neighbors2 = input.neighbors2,
					units = input.units.Select(u =>
					{
						return new Unit()
						{
							attack = u.team == 'E' ? attack : 3,
							idx = u.idx,
							team = u.team,
							hp = u.hp
						};
					}).ToList()
				};

				var result = LosslessCombat(current);
				if (result >= 0)
				{
					return result;
				}
			}

			return -1;
		}

		int LosslessCombat(Input input)
		{
			for (int step = 0; step < 100000; step++)
			{
				//Print(input);
				input.units.Sort((a, b) => a.idx - b.idx);

				foreach (var unit in input.units)
				{
					if (unit.hp > 0)
					{
						if (input.units.Any(u => u.hp <= 0 && u.team == 'E'))
						{
							return -1;
						}

						bool noGoblins = !input.units.Any(u => u.hp > 0 && u.team == 'G');

						if (noGoblins)
						{
							//Ended
							var hpSum = input.units
								.Where(u => u.hp > 0)
								.Sum(u => u.hp);

							Print(input);

							Console.WriteLine(step + " * " + hpSum);

							return step * hpSum;
						}

						//Move
						var nextIdx = FindStep(unit, input);
						if (nextIdx >= 0)
						{
							unit.idx = nextIdx;
						}

						//Attack
						var other = AdjEnemy(unit, input);
						if (other != null)
						{
							other.hp -= unit.attack;
						}
					}
				}
			}

			return -1;
		}
	}
}