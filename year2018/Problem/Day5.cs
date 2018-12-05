using System;
using System.Collections.Generic;

namespace year2018.Problem
{
	class Day5
	{
		static string s_example =
@"dabAcCaCBAcCcaDA";

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

			for (int i = input.Length - 2; i >= 0; --i)
			{
				if (input[i] != input[i + 1] && input[i].ToString().ToUpper() == input[i + 1].ToString().ToUpper())
				{
					input = input.Remove(i, 2);
					if (i == input.Length)
					{
						--i;
					}
				}
			}

			return input.Length;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			int minLength = int.MaxValue;
			char minType = '%';

			for (char type = 'A'; type <= 'Z'; type++)
			{
				var length = Solve(input
					.Replace(type.ToString(), "")
					.Replace(type.ToString().ToLower(), ""));

				if (length < minLength)
				{
					minType = type;
					minLength = length;
				}
			}

			return minLength;
		}
	}
}