using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day17
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 17;
		static string s_example =
@"3";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		static int Transform(string raw)
		{
			return int.Parse(raw);
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			var list = new List<int>();
			int current = 0;
			int nextValue = 1;

			list.Add(0);
			for (; nextValue <= 2017; nextValue++)
			{
				current = (current + input) % list.Count + 1;
				list.Insert(current, nextValue);
			}

			return list[(current + 1) % list.Count];
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			int listValue = 0;
			int listCount = 1;
			int current = 0;
			int nextValue = 1;

			for (; nextValue <= 50000000; nextValue++)
			{
				current = (current + input) % listCount + 1;

				++listCount;
				if (current == 1)
					listValue = nextValue;
			}

			return listValue;
		}
	}
}