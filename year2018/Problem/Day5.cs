using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day5
	{
		static string s_example =
@"";

		static void Run(int year, int day)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

			Console.ReadKey();
		}

		static string Transform(string raw)
		{
			return raw;
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			return -1;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}