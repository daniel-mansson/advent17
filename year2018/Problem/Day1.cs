using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day1 : BaseDay
	{
		public override string Example1 =>
@"+3
+3
+4
-2
-4";

		static List<int> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Replace("+", "")
				.Split('\n')
				.Select(s => int.Parse(s))
				.ToList();
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);
			return input.Sum();
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			var state = 0;
			var memory = new List<int>() { 0 };

			for (int i = 0; i < 10000; i++)
			{
				foreach (var item in input)
				{
					state += item;

					if (memory.Contains(state))
					{
						return state;
					}
					else
					{
						memory.Add(state);
					}
				}
			}

			return -1;
		}
	}
}
