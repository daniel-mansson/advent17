using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day22
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 22;
		static string s_example =
@"..#
#..
...";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		public struct Vec2
		{
			public int x;
			public int y;
		}

		public class Carrier
		{
			public Vec2 pos;
			public int dir;

			public bool behaviourPart2 = false;

			public void TurnLeft()
			{
				dir = (dir + 3) % 4;
			}

			public void TurnRight()
			{
				dir = (dir + 1) % 4;
			}

			public void TurnBack()
			{
				dir = (dir + 2) % 4;
			}

			public void StepForward()
			{
				var d = lookup[dir];
				pos.x += d.x;
				pos.y += d.y;
			}

			public void Step(Map map)
			{
				var state = map.Get(pos.x, pos.y);

				if (state == Map.NodeState.Infected)
				{
					TurnRight();
					map.Put(pos.x, pos.y, behaviourPart2 ? Map.NodeState.Flagged : Map.NodeState.Clean);
				}
				else if (state == Map.NodeState.Clean)
				{
					TurnLeft();
					map.Put(pos.x, pos.y, behaviourPart2 ? Map.NodeState.Weakened : Map.NodeState.Infected);
				}
				else if (state == Map.NodeState.Flagged)
				{
					TurnBack();
					map.Put(pos.x, pos.y, Map.NodeState.Clean);
				}
				else if (state == Map.NodeState.Weakened)
				{
					map.Put(pos.x, pos.y, Map.NodeState.Infected);
				}

				StepForward();
			}
		}

		public static List<Vec2> lookup = new List<Vec2>()
		{
			new Vec2() { x = 0, y = -1 },
			new Vec2() { x = 1, y = 0 },
			new Vec2() { x = 0, y = 1 },
			new Vec2() { x = -1, y = 0 }
		};

		public class Map
		{
			public Dictionary<int, NodeState> map = new Dictionary<int, NodeState>();
			public int infectionCount = 0;
			public Vec2 start;

			public enum NodeState
			{
				Clean,
				Weakened,
				Infected,
				Flagged
			}

			public Map(string raw)
			{
				var l = raw
					.Replace("\r", "")
					.Split('\n')
					.ToList();

				for (int y = 0; y < l.Count; y++)
				{
					for (int x = 0; x < l[y].Length; x++)
					{
						if (l[y][x] == '#')
						{
							Flip(x, y);
						}
					}
				}

				start = new Vec2()
				{
					x = l.Count / 2,
					y = l[0].Length / 2
				};
			}

			public NodeState Get(int x, int y)
			{
				NodeState state;
				if (map.TryGetValue(y * 1000000 + x, out state))
				{
					return state;
				}
				else
				{
					return NodeState.Clean;
				}
			}

			public void Flip(int x, int y)
			{
				int i = y * 1000000 + x;
				if (map.ContainsKey(i))
				{
					map.Remove(i);
				}
				else
				{
					map.Add(i, NodeState.Infected);
				}
			}

			public void Draw(int size)
			{
				for (int y = -3; y < size; y++)
				{
					for (int x = -3; x < size; x++)
					{
						var state = Get(x, y);
						switch (state)
						{
							case NodeState.Clean:
								Console.Write(".");
								break;
							case NodeState.Weakened:
								Console.Write("W");
								break;
							case NodeState.Infected:
								Console.Write("#");
								break;
							case NodeState.Flagged:
								Console.Write("F");
								break;
							default:
								break;
						}
					}
					Console.WriteLine();
				}
			}

			public void Put(int x, int y, NodeState state)
			{
				if (state == NodeState.Clean)
				{
					map.Remove(y * 1000000 + x);
				}
				else
				{
					map[y * 1000000 + x] = state;

					if (state == NodeState.Infected)
						infectionCount++;
				}
			}
		}

		static Map Transform(string raw)
		{
			return new Map(raw);
		}

		static int Solve(string raw)
		{
			var map = Transform(raw);
			var carrier = new Carrier();
			carrier.pos = map.start;

			for (int i = 0; i < 10000; i++)
			{
				carrier.Step(map);
			}

			map.Draw(5);

			return map.infectionCount;
		}

		static int Solve2(string raw)
		{
			var map = Transform(raw);
			var carrier = new Carrier();
			carrier.pos = map.start;
			carrier.behaviourPart2 = true;

			for (int i = 0; i < 10000000; i++)
			{
				carrier.Step(map);
			}

			map.Draw(10);

			return map.infectionCount;
		}
	}
}