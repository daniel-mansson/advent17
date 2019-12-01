using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2019.Problem
{
	public class Day1 : BaseDay
	{
		public override string Example1 =>
@"12
14
1969
100756";

		public override string Example2 =>
@"100756";

		List<long> Transform(string raw)
		{
			return raw
				.Split('\n')
				.Select(v => long.Parse(v))
				.ToList();
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			var result = input
				.Select(v => v / 3 - 2)
				.Sum();

			return result;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			var fuel = input
				.Select(v => GetReqFuel(v))
				.Sum();

			return fuel;
		}

		long GetReqFuel(long v)
		{
			var fuel = v / 3 - 2;

			if (fuel > 0)
			{
				return GetReqFuel(fuel) + fuel;
			}
			else
			{
				return 0;
			}
		}
	}
}