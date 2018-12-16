using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day14 : BaseDay
	{
		public override string Example1 =>
@"2018";
		public override string Example2 =>
@"59414";

		int Transform(string raw)
		{
			return int.Parse(raw);
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);
			var recipes = new List<int>() { 3, 7 };
			var elves = new List<int>() { 0, 1 };

			while (recipes.Count < input + 10)
			{
				long sum = elves.Select(e => recipes[e]).Sum();
				recipes.AddRange(sum.ToString().Select(c => int.Parse(c.ToString())));

				for (int i = 0; i < elves.Count; i++)
				{
					elves[i] = (elves[i] + recipes[elves[i]] + 1) % recipes.Count;
				}

				//foreach (var r in recipes)
				//{
				//	Console.Write(r + " ");
				//}
				//Console.WriteLine();
			}

			var s = "";
			for (int i = 0; i < 10; i++)
			{
				s += recipes[input + i];
			}

			return s;
		}

		public override object Solve2(string raw)
		{
			var recipes = new List<int>() { 3, 7 };
			var res = "37";
			var elves = new List<int>() { 0, 1 };

			int count = 0;
			while (recipes.Count < 100000000)
			{
				long sum = elves.Select(e => recipes[e]).Sum();
				recipes.AddRange(sum.ToString().Select(c => int.Parse(c.ToString())));
				res += sum.ToString();

				for (int i = 0; i < elves.Count; i++)
				{
					elves[i] = (elves[i] + recipes[elves[i]] + 1) % recipes.Count;
				}

				if (res.Contains(raw))
				{
					return count + res.IndexOf(raw);
				}

				if (res.Length > 6)
				{
					count += 3;
					res = res.Substring(3);
				}
				
				if (recipes.Count % 1000000 == 0)
					Console.WriteLine(recipes.Count);
			}

			
			return -1;
		}
	}
}