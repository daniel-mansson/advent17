using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day6
	{
		static string s_example =
@"";

		static void Run(int year, int day)
		{
			try
			{
				Console.WriteLine("Example: " + Solve(s_example));
				Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

				Console.WriteLine("Example 2: " + Solve2(s_example));
				Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

				Console.ReadKey();
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.WriteLine();

				var lines = e.StackTrace.Split('\n');
				foreach (var line in lines)
				{
					var s = line.Split(new string[] { "line" }, StringSplitOptions.None);

					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(s[0]);

					if (s.Length > 1)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write("line" + s[1]);
					}
				}

				Console.ReadKey();
			}
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