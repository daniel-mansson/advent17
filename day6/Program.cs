using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day6
{
	class Program
	{
		static string s_example = "0 2 7 0";
		static string s_input1 = @"0	5	10	0	11	14	13	4	11	8	8	7	1	4	12	11";

		static void Main(string[] args)
		{
			Console.WriteLine(Run(s_example));

			var input = ProblemInput.FetchBlocking(2017, 6);
			Console.WriteLine(Run(input));

			Console.ReadKey();
		}

		static int Run(string indata)
		{
			var data = Transform(indata);
			return Solve2(data);
		}

		static List<int> Transform(string indata)
		{
			return indata.Split(' ', '\t').Select(n => int.Parse(n)).ToList();
		}

		static int Solve(List<int> data)
		{
			var memory = new List<List<int>>();

			while (!memory.Any(d => d.SequenceEqual(data)))
			{
				memory.Add(data.ToList());
				Move(data);
			}

			return memory.Count;
		}

		static int Solve2(List<int> data)
		{
			var memory = new List<List<int>>();

			while (!memory.Any(d => d.SequenceEqual(data)))
			{
				memory.Add(data.ToList());
				Move(data);
			}

			int cycleStartIndex = memory.FindIndex(d => d.SequenceEqual(data));

			return memory.Count - cycleStartIndex;
		}

		static void Move(List<int> data)
		{
			int max = int.MinValue;
			int maxIdx = 0;

			for (int i = 0; i < data.Count; i++)
			{
				if (data[i] > max)
				{
					max = data[i];
					maxIdx = i;
				}
			}

			int steps = data[maxIdx];
			data[maxIdx] = 0;

			int start = maxIdx + 1;
			for (int i = 0; i < steps; i++)
			{
				int idx = (start + i) % data.Count;

				data[idx]++;
			}
		}
	}
}
