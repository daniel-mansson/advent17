using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day3
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(GetDist(1));
			Console.WriteLine(GetDist(12));
			Console.WriteLine(GetDist(23));
			Console.WriteLine(GetDist(1024));
			Console.WriteLine(GetDist(312051));

			SolvePart2();

			Console.ReadKey();
		}

		static int GetDist(int idx)
		{
			if (idx == 1)
				return 0;

			//find corner
			int c = 1;
			for (c = 1; c < SIZE; c += 2)
			{
				if (idx <= c * c)
				{
					break;
				}
			}

			int distFromCorner = c * c - idx;
			int v = c / 2;

			int a = distFromCorner % v;
			if ((distFromCorner / v) % 2 == 0)
				a = v - a;

			return a + v;
		}

		const int SIZE = 100;
		static int SolvePart2()
		{
			var grid = new int[SIZE * SIZE];

			int idx = SIZE * (SIZE/2) + SIZE/2;
			grid[idx] = 1;

			for (int c = 1; c < 1000; c += 2)
			{
				int steps = c - 1;
				//up
				for (int i = 0; i < steps - 1; i++)
				{
					idx += SIZE;
					Fill(grid, idx);
				}
				//left
				for (int i = 0; i < steps; i++)
				{
					idx -= 1;
					Fill(grid, idx);
				}
				//down
				for (int i = 0; i < steps; i++)
				{
					idx -= SIZE;
					Fill(grid, idx);
				}
				//right
				for (int i = 0; i < steps + 1; i++)
				{
					idx += 1;
					Fill(grid, idx);
				}
			}

			return 0;
		}

		static void Fill(int[] grid, int idx)
		{
			grid[idx] =
				grid[idx + 1] +
				grid[idx - 1] +
				grid[idx + SIZE] +
				grid[idx - SIZE] +
				grid[idx + 1 - SIZE] +
				grid[idx - 1 - SIZE] +
				grid[idx + 1 + SIZE] +
				grid[idx - 1 + SIZE];

			Console.WriteLine(grid[idx]);

			if (grid[idx] > 312051)
			{
				Console.WriteLine(grid[idx]);
				throw new Exception("Whatever, time to bail, haha: " + grid[idx]);
			}
		}
	}
}
