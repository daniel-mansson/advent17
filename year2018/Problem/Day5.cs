using System;
using System.Collections.Generic;

namespace year2018.Problem
{
	public class Day5 : BaseDay
	{
		public override string Example1 =>
@"dabAcCaCBAcCcaDA";

		string Transform(string raw)
		{
			return raw;
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			for (int i = input.Length - 2; i >= 0; --i)
			{
				if (input[i] != input[i + 1] && input[i].ToString().ToUpper() == input[i + 1].ToString().ToUpper())
				{
					input = input.Remove(i, 2);
					if (i == input.Length)
					{
						--i;
					}
				}
			}

			return input.Length;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			int minLength = int.MaxValue;
			char minType = '%';

			for (char type = 'A'; type <= 'Z'; type++)
			{
				var length = (int)Solve1(input
					.Replace(type.ToString(), "")
					.Replace(type.ToString().ToLower(), ""));

				if (length < minLength)
				{
					minType = type;
					minLength = length;
				}
			}

			return minLength;
		}
	}
}