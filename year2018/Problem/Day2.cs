using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day2 : BaseDay
	{
		public override string Example1 =>
@"abcdef
bababc
abbcde
abcccd
aabcdd
abcdee
ababab";

		public override string Example2 =>
@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz";
		
		static List<string> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Split('\n')
				.ToList();
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);

			var two = input
				.Where(s => s
					.ToList()
					.GroupBy(c => c)
					.Any(g => g.Count() == 2))
				.Count();

			var three = input
				.Where(s => s
					.ToList()
					.GroupBy(c => c)
					.Any(g => g.Count() == 3))
				.Count();

			return two * three;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);

			foreach (var item in input)
			{
				var other = input.FirstOrDefault(i => GoodId(item, i));
				if (other != null)
				{
					return Clean(item, other);
				}
			}

			return string.Empty;
		}

		static bool GoodId(string a, string b)
		{
			int errors = 0;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					errors++;
					if (errors > 1)
					{
						break;
					}
				}
			}

			return errors == 1;
		}

		static string Clean(string a, string b)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return a.Remove(i, 1);
				}
			}

			return string.Empty;
		}
	}
}
