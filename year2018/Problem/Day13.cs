using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day13 : BaseDay
	{
		public override string Example1 =>
@"/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ";

		public override string Example2 =>
@"/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/";

		[Flags]
		enum Dir
		{
			Down = 0,
			Left = 1,
			Up = 2,
			Right = 3,
			None = 4,
		}

		class Train
		{
			public int x;
			public int y;
			public Dir dir;
			public int turnState;
			public int idx;
		}

		static Dictionary<int, Func<Dir, Dir>> turns = new Dictionary<int, Func<Dir, Dir>>()
		{
			{ 0, d => (Dir)(((int)d + 3) % 4) },
			{ 1, d => d },
			{ 2, d => (Dir)(((int)d + 1) % 4) },
		};

		Dictionary<char, Func<Dir, Dir>> corner = new Dictionary<char, Func<Dir, Dir>>()
		{
			{ '/', d => d == Dir.Right || d == Dir.Left ? turns[0](d) : turns[2](d) },
			{ '\\', d => d == Dir.Right || d == Dir.Left ? turns[2](d) : turns[0](d) },
		};

		class Input
		{
			public string grid;
			public int w;
			public int h;
			public List<Train> trains = new List<Train>();
		}

		Input Transform(string raw)
		{
			var split = raw
				.Replace("\r", "")
				.Split('\n');

			var input = new Input()
			{
				w = split[0].Length,
				h = split.Length,
				grid = raw
					.Replace("\r", "")
					.Replace("\n", "")
					.Replace("v", "|")
					.Replace("^", "|")
					.Replace(">", "-")
					.Replace("<", "-")
			};

			for (int i = 0; i < input.h; i++)
			{
				for (int j = 0; j < input.w; j++)
				{
					var t = new Train()
					{
						x = j,
						y = i,
						idx = i * input.w + j,
						dir = Dir.None,
						turnState = 0
					};

					switch (split[i][j])
					{
						case 'v':
							t.dir = Dir.Down;
							break;
						case '^':
							t.dir = Dir.Up;
							break;
						case '>':
							t.dir = Dir.Right;
							break;
						case '<':
							t.dir = Dir.Left;
							break;
					}

					if(t.dir != Dir.None)
						input.trains.Add(t);
				}
			}

			return input;
		}


		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			var move = new Dictionary<Dir, int>()
			{
				{ Dir.Left, -1 },
				{ Dir.Right, 1 },
				{ Dir.Up, -input.w },
				{ Dir.Down, input.w },
			};

			for (int step = 0; step < 100000; step++)
			{
				input.trains.Sort((a, b) => a.idx - b.idx);
				
				for (int i = 0; i < input.trains.Count; i++)
				{
					var t = input.trains[i];

					var xy = (t.idx % input.w) + "," + (t.idx / input.w);

					t.idx += move[t.dir];

					var xy2 = (t.idx % input.w) + "," + (t.idx / input.w);



					if (input.grid[t.idx] == '+')
					{
						t.dir = turns[t.turnState](t.dir);
						t.turnState = (t.turnState + 1) % 3;
					}
					else if (input.grid[t.idx] == '\\' || input.grid[t.idx] == '/')
					{
						t.dir = corner[input.grid[t.idx]](t.dir);
					}

					bool crash = false;
					for (int j = 0; j < input.trains.Count; j++)
					{
						if (i == j)
							continue;

						if (input.trains[j].idx == t.idx)
						{
							crash = true;
							break;
						}
					}

					//Console.WriteLine($"train {i}  | \t{xy} -> {xy2}     {t.dir}");

					if (crash)
					{
						return (t.idx % input.w) + "," + (t.idx / input.w);
					}
				}
			}

			return -1;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			var move = new Dictionary<Dir, int>()
			{
				{ Dir.Left, -1 },
				{ Dir.Right, 1 },
				{ Dir.Up, -input.w },
				{ Dir.Down, input.w },
			};

			for (int step = 0; step < 100000; step++)
			{
				input.trains.Sort((a, b) => a.idx - b.idx);

				for (int i = 0; i < input.trains.Count; i++)
				{
					var t = input.trains[i];

					var xy = (t.idx % input.w) + "," + (t.idx / input.w);

					t.idx += move[t.dir];

					var xy2 = (t.idx % input.w) + "," + (t.idx / input.w);



					if (input.grid[t.idx] == '+')
					{
						t.dir = turns[t.turnState](t.dir);
						t.turnState = (t.turnState + 1) % 3;
					}
					else if (input.grid[t.idx] == '\\' || input.grid[t.idx] == '/')
					{
						t.dir = corner[input.grid[t.idx]](t.dir);
					}

					bool crash = false;
					for (int j = 0; j < input.trains.Count; j++)
					{
						if (i == j)
							continue;

						if (input.trains[j].idx == t.idx)
						{
							input.trains.RemoveAt(j);
							crash = true;
							break;
						}
					}

					//Console.WriteLine($"train {i}  | \t{xy} -> {xy2}     {t.dir}");

					if (crash)
					{
						i = input.trains.IndexOf(t);
						input.trains.RemoveAt(i);
						i--;
					}
				}


				if (input.trains.Count == 1)
				{
					var t = input.trains[0];
					return (t.idx % input.w) + "," + (t.idx / input.w);
				}
			}

			return -1;
		}
	}
}