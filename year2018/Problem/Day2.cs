using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day2
	{
		static string s_example =
@"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab";

		static string s_example2 =
@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";

		static void Run(int year, int day)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

			Console.WriteLine("Example 2: " + Solve2(s_example2));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

			Console.ReadKey();
		}
		
		static List<string> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Split('\n')
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			var two = input
				.Where(s => s
					.ToList()
					.GroupBy(c => c)
					.Any(g => g.Count() == 2))
				.Count();

			var three = input
				.Where(s => s
					.ToList()
					.GroupBy(c => c)
					.Any(g => g.Count() == 3))
				.Count();

			return two * three;
		}

		static string Solve2(string raw)
		{
			var input = Transform(raw);

			foreach (var item in input)
			{
				var other = input.FirstOrDefault(i => GoodId(item, i));
				if (other != null)
				{
					return Clean(item, other);
				}
			}

			return string.Empty;
		}

		static bool GoodId(string a, string b)
		{
			int errors = 0;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					errors++;
					if (errors > 1)
					{
						break;
					}
				}
			}

			return errors == 1;
		}

		static string Clean(string a, string b)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return a.Remove(i, 1);
				}
			}

			return string.Empty;
		}
	}
}
