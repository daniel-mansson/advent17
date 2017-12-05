using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day5
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Run(s_example1));
			Console.WriteLine(Run(File.ReadAllText("input.txt")));

			Console.WriteLine();
			Console.WriteLine(Run2(s_example1));
			Console.WriteLine(Run2(File.ReadAllText("input.txt")));

			Console.ReadKey();
		}

		static int Run(string indata)
		{
			var data = Transform(indata);
			return Solve(data);
		}

		static int Run2(string indata)
		{
			var data = Transform(indata);
			return Solve2(data);
		}

		static List<int> Transform(string indata)
		{
			return indata.Split('\n').Select(n => int.Parse(n)).ToList();
		}

		static int Solve(List<int> indata)
		{
			int steps = 0;
			int pos = 0;

			while (pos < indata.Count)
			{
				pos += indata[pos]++;
				steps++;
			}

			return steps;
		}

		static int Solve2(List<int> indata)
		{
			int steps = 0;
			int pos = 0;

			while (pos < indata.Count)
			{
				int offset = indata[pos];
				if (offset >= 3)
					indata[pos]--;
				else
					indata[pos]++;

				pos += offset;
				steps++;
			}

			return steps;
		}

		static string s_example1 =
@"0
3
0
1
-3";
	}
}
