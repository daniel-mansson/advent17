using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day1
	{
		static string s_example =
@"+3
+3
+4
-2
-4";

		static void Run(int year, int day)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

			Console.ReadKey();
		}

		static List<int> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Replace("+", "")
				.Split('\n')
				.Select(s => int.Parse(s))
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);
			return input.Sum();
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			var state = 0;
			var memory = new List<int>() { 0 };

			for (int i = 0; i < 10000; i++)
			{
				foreach (var item in input)
				{
					state += item;

					if (memory.Contains(state))
					{
						return state;
					}
					else
					{
						memory.Add(state);
					}
				}
			}


			return -1;
		}
	}
}
