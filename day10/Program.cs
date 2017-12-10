using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day10
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 10;
		static string s_example =
@"3,4,1,5";
		static string s_example1 =
@"";
		static string s_example2 =
@"AoC 2017";
		static string s_example3 =
@"1,2,3";
		static string s_example4 =
@"1,2,4";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example1));
			Console.WriteLine("Example 2: " + Solve2(s_example2));
			Console.WriteLine("Example 2: " + Solve2(s_example3));
			Console.WriteLine("Example 2: " + Solve2(s_example4));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		static List<int> Transform(string raw)
		{
			return raw.Split(',').Select(s => int.Parse(s)).ToList();
		}

		static List<int> Transform2(string raw)
		{
			return raw.Select(s => (int)(char)s).ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			int size = 256;
			var list = new List<int>();
			for (int i = 0; i < size; i++)
				list.Add(i);

			int current = 0;
			int skipSize = 0;

			foreach (var op in input)
			{
				Reverse(list, current, op);
				current += (op + skipSize) % list.Count;
				skipSize++;
			}

			var result = list[0] * list[1];
			return result;
		}

		static void Reverse(List<int> list, int start, int length)
		{
			for (int i = 0; i < length/2; i++)
			{
				int a = (start + i) % list.Count;
				int b = (start + length - 1 - i + list.Count) % list.Count;

				int tmp = list[a];
				list[a] = list[b];
				list[b] = tmp;
			}
		}

		static string Solve2(string raw)
		{
			var input = Transform2(raw);
			input.AddRange(new List<int>{ 17, 31, 73, 47, 23 });

			int size = 256;
			var list = new List<int>();
			for (int i = 0; i < size; i++)
				list.Add(i);

			int current = 0;
			int skipSize = 0;

			for (int round = 0; round < 64; round++)
			{
				foreach (var op in input)
				{
					Reverse(list, current, op);
					current += (op + skipSize) % list.Count;
					skipSize++;
				}
			}

			var dense = new List<int>();
			for (int i = 0; i < 16; i++)
			{
				int xored = 0;
				for (int j = 0; j < 16; j++)
					xored ^= list[i * 16 + j];

				dense.Add(xored);
			}

			var result = string.Join("", dense.Select(i => i.ToString("X2")));
			return result.ToLower();
		}
	}
}