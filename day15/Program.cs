using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day15
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 15;
		static string s_example =
@"hello 65
hello 8921";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Generator
		{
			public long state;
			public long multiplier;

			public Generator(long start, long mul)
			{
				state = start;
				multiplier = mul;
			}

			public void Step()
			{
				state *= multiplier;
				state %= 2147483647L;
			}

			public void StepUntil(long multiple)
			{
				do
				{
					Step();
				} while (state % multiple != 0);
			}

			public long GetLowest16()
			{
				return state & 0xffff;
			}
		}

		static List<long> Transform(string raw)
		{
			return raw
				.Split('\n')
				.Select(l => long.Parse(l.Split(' ').Last()))
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			var a = new Generator(input[0], 16807);
			var b = new Generator(input[1], 48271);
			int count = 0;

			for (int i = 0; i < 40000000; i++)
			{
				a.Step();
				b.Step();

				if (a.GetLowest16() == b.GetLowest16())
					count++;
			}
			return count;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			var a = new Generator(input[0], 16807);
			var b = new Generator(input[1], 48271);
			int count = 0;

			for (int i = 0; i < 5000000; i++)
			{
				a.StepUntil(4);
				b.StepUntil(8);

				if (a.GetLowest16() == b.GetLowest16())
					count++;
			}
			return count;
		}
	}
}