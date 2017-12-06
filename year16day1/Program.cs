using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year16day1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Run("R2, L3"));
			Console.WriteLine(Run("R2, R2, R2"));
			Console.WriteLine(Run("R5, L5, R5, R3"));
			Console.WriteLine(Run(ProblemInput.FetchBlocking(2016, 1)));
		//	Console.WriteLine(Run2("R8, R4, R4, R8"));
			Console.WriteLine(Run2(ProblemInput.FetchBlocking(2016, 1)));

			Console.ReadKey();
		}

		static int Run(string raw)
		{
			var input = Transform(raw);
			return Solve(input);
		}

		static int Run2(string raw)
		{
			var input = Transform(raw);
			return Solve2(input);
		}

		class Op
		{
			public int Turn;
			public int Steps;
		}

		static List<Op> Transform(string raw)
		{
			return raw
				.Replace(", ", ",")
				.Split(',')
				.Select(i => new Op
				{
					Turn = i[0] == 'R' ? 1 : 3,
					Steps = int.Parse(i.Substring(1))
				})
				.ToList();
		}

		static int Solve(List<Op> input)
		{
			int dir = 0;
			var delta = new KeyValuePair<int, int>[]
			{
				new KeyValuePair<int, int>(0, 1),
				new KeyValuePair<int, int>(1, 0),
				new KeyValuePair<int, int>(0, -1),
				new KeyValuePair<int, int>(-1, 0),
			};

			int x = 0;
			int y = 0;
			foreach (var op in input)
			{
				dir = (dir + op.Turn) % 4;
				x += delta[dir].Key * op.Steps;
				y += delta[dir].Value * op.Steps;
			}

			return Math.Abs(x) + Math.Abs(y);
		}

		static int Solve2(List<Op> input)
		{
			int dir = 0;
			var delta = new KeyValuePair<int, int>[]
			{
				new KeyValuePair<int, int>(0, 1),
				new KeyValuePair<int, int>(1, 0),
				new KeyValuePair<int, int>(0, -1),
				new KeyValuePair<int, int>(-1, 0),
			};

			var memory = new List<int>();
			int x = 0;
			int y = 0;
			memory.Add(0);
			foreach (var op in input)
			{
				dir = (dir + op.Turn) % 4;

				for (int i = 0; i < op.Steps; i++)
				{
					x += delta[dir].Key;
					y += delta[dir].Value;

					Console.WriteLine($"{x}\t{y}");

					int hash = y * 20000 + x;
					if (memory.Contains(hash))
						return Math.Abs(x) + Math.Abs(y);

					memory.Add(hash);
				}

			}

			return -99999999;
		}
	}
}
