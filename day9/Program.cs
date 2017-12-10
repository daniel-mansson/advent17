using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day9
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 9;
		static string s_example =
"<{o\"i!a,<{i<a><><random characters>";

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

			string noEscapes = Regex.Replace(input, "!.", "");
			string noGarbage = Regex.Replace(noEscapes, "<.*?>", "");
			string noCommas = noGarbage.Replace(",", "");

			int depth = 0;
			int sum = 0;
			foreach (var c in noCommas)
			{
				switch (c)
				{
					case '{':
						depth++;
						sum += depth;
						break;
					case '}':
						depth--;
						break;
					default:
						throw new Exception("Unexpected: " + c);
				}
			}

			return sum;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			string noEscapes = Regex.Replace(input, "!.", "");

			var garbageMatch = Regex.Matches(noEscapes, "<(.*?)>");
			var sum = garbageMatch
				.Cast<Match>()
				.Sum(m => m.Groups.Count == 2 ? m.Groups[1].Length : 0);

			return sum;
		}
	}
}