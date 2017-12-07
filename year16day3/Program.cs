using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year16day3
{
	class Program
	{
		static int s_year = 2016;
		static int s_day = 3;
		static string s_example =
@"5 10 25
5 10 25
5 10 25";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		static List<List<int>> Transform(string raw)
		{
			return raw
				.Split('\n')
				.Select(line =>
				{
					return line
					.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(s => int.Parse(s))
					.ToList();
				})
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);
			int count = 0;

			foreach (var t in input)
			{
				t.Sort();
				if (t[0] + t[1] > t[2])
					count++;
			}

			return count;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);
			int count = 0;

			for (int i = 0; i < input.Count; i += 3)
			{
				for (int j = 0; j < 3; j++)
				{
					var t = new int[]
					{
						input[i + 0][j],
						input[i + 1][j],
						input[i + 2][j],
					}.ToList();

					t.Sort();

					if (t[0] + t[1] > t[2])
						count++;
				}
			}

			return count;
		}
	}
}