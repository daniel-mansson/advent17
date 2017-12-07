using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year16day5
{
	class Program
	{
		static int s_year = 2016;
		static int s_day = 5;
		static string s_example =
@"";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

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