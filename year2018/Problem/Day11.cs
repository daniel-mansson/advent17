using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day11 : BaseDay
	{
		public override string Example1 =>
@"42";

		int Transform(string raw)
		{
			return int.Parse(raw);
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			var grid = new int[300 * 300];

			for (int i = 0; i < 300; i++)
			{
				for (int j = 0; j < 300; j++)
				{
					long rackId = j + 1 + 10;
					long beginPower = rackId * (i + 1);
					long power = beginPower + input;
					power *= rackId;
					int hundred = (int)((power / 100) % 10);
					int realPower = hundred - 5;

					grid[Idx(j, i)] = realPower;
				}
			}

			int maxV = 0;
			int mx = 0;
			int my = 0;
			for (int i = 0; i < 300 - 2; i++)
			{
				for (int j = 0; j < 300 - 2; j++)
				{
					int sum = 0;

					for (int y = 0; y < 3; y++)
					{
						for (int x = 0; x < 3; x++)
						{
							sum += grid[Idx(j + x, i + y)];
						}
					}

					if (sum > maxV)
					{
						maxV = sum;
						mx = j;
						my = i;
					}
				}
			}
			Console.WriteLine($"x:{mx+ 1} y:{my+1}");

			return maxV;
		}

		int Idx(int x, int y)
		{
			return y * 300 + x;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			var grid = new int[300 * 300];

			for (int i = 0; i < 300; i++)
			{
				for (int j = 0; j < 300; j++)
				{
					long rackId = j + 1 + 10;
					long beginPower = rackId * (i + 1);
					long power = beginPower + input;
					power *= rackId;
					int hundred = (int)((power / 100) % 10);
					int realPower = hundred - 5;

					grid[Idx(j, i)] = realPower;
				}
			}

			int maxV = 0;
			int mx = 0;
			int my = 0;
			int ms = 0;
			for (int s = 1; s <= 300; s++)
			{
				Console.WriteLine(s);

				for (int i = 0; i < 300 - s; i++)
				{
					for (int j = 0; j < 300 - s; j++)
					{
						int sum = 0;

						for (int y = 0; y < s; y++)
						{
							for (int x = 0; x < s; x++)
							{
								sum += grid[Idx(j + x, i + y)];
							}
						}

						if (sum > maxV)
						{
							maxV = sum;
							mx = j;
							my = i;
							ms = s;
							Console.WriteLine($"x:{mx + 1} y:{my + 1} s:{ms} sum:{maxV}");
						}
					}
				}
			}

			Console.WriteLine($"x:{mx + 1} y:{my + 1} s:{ms} sum:{maxV}");

			return maxV;
		}
	}
}