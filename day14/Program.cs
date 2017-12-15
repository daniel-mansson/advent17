using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day14
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 14;
		static string s_example =
@"flqrgnkx";

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
			var l = new List<List<int>>();

			for (int i = 0; i < 128; i++)
			{
				l.Add(Knothash(raw + "-" + i));
			}

			return l;
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);
			int count = 0;

			foreach (var row in input)
			{
				foreach (var part in row)
				{
					for (int i = 0; i < 32; i++)
					{
						if ((part & (1 << i)) != 0)
							count++;
					}
				}
			}

			return count;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);
			int[] grid = new int[128 * 128];

			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 8; k++)
					{
						grid[128 * i + j * 8 + 7 - k] = ((input[i][j] & (1 << k)) != 0) ? 0 : -1;
					}
				}
			}

			int nextId = 1;
			for (int i = 0; i < 128 * 128; i++)
			{
				int c = Floodfill(grid, i, nextId);
				if (c != 0)
					nextId++;
			}

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					int vv = grid[128 * i + j];
					Console.Write(vv == -1 ? "." : "#");
				}
				Console.WriteLine();
			}

			return nextId - 1;
		}

		static int Floodfill(int[] grid, int pos, int v)
		{
			List<int> queue = new List<int>();
			queue.Add(pos);
			int count = 0;

			while (queue.Count > 0)
			{
				int p = queue[0];
				queue.RemoveAt(0);

				if (grid[p] == 0)
				{
					count++;
					grid[p] = v;

					int x = p % 128;
					int y = p / 128;

					if (x > 0)
						queue.Add(p - 1);
					if (x < 127)
						queue.Add(p + 1);
					if (y > 0)
						queue.Add(p - 128);
					if (y < 127)
						queue.Add(p + 128);
				}
			}

			return count;
		}
		
		static List<int> TransformKnotInput(string raw)
		{
			return raw.Select(s => (int)(char)s).ToList();
		}

		static void Reverse(List<int> list, int start, int length)
		{
			for (int i = 0; i < length / 2; i++)
			{
				int a = (start + i) % list.Count;
				int b = (start + length - 1 - i + list.Count) % list.Count;

				int tmp = list[a];
				list[a] = list[b];
				list[b] = tmp;
			}
		}

		static List<int> Knothash(string raw)
		{
			var input = TransformKnotInput(raw);
			input.AddRange(new List<int> { 17, 31, 73, 47, 23 });

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

			return dense;
		}
	}
}