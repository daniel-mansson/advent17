using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year16day4
{
	class Program
	{
		static int s_year = 2016;
		static int s_day = 4;
		static string s_example =
@"aaaaa-bbb-z-y-x-123[abxyz]
a-b-c-d-e-f-g-h-987[abcde]
not-a-real-room-404[oarel]
totally-real-room-200[decoy]
qzmt-zixmtkozy-ivhz-343[aaaaa]";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Entry
		{
			public List<string> words;
			public int sector;
			public string checksum;

			public bool IsValid()
			{
				var dict = new Dictionary<char, int>();

				foreach (var word in words)
				{
					foreach (var c in word)
					{
						if (!dict.ContainsKey(c))
							dict[c] = 0;

						dict[c]++;
					}
				}

				var generated = string.Join("", dict.OrderByDescending(kvp => kvp.Value * 10000 + 'z' - kvp.Key).Take(5).Select(kvp => kvp.Key));
				return generated == checksum;
			}
		}

		static List<Entry> Transform(string raw)
		{
			return raw.Split('\n')
				.Select(l => 
				{
					var s = l
						.Replace("[", ",")
						.Replace("]", "")
						.Replace("\r", "")
						.Replace("-", ",")
						.Split(',')
						.ToList();

					return new Entry()
					{
						words = s.Take(s.Count - 2).ToList(),
						checksum = s.Last(),
						sector = int.Parse(s[s.Count - 2])
					};
				}).ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			return input.Sum(i => i.IsValid() ? i.sector : 0);
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			var decrypted = input
				.Where(i => i.IsValid())
				.Select(i =>
				{
					return new KeyValuePair<string, int>(string.Join(" ",
						i.words
						.Select(w => string.Join("", w.Select(c => (char)((c - 'a' + i.sector) % ('z' - 'a' + 1) + 'a'))))),
						i.sector);
				})
				.ToList();

			var northpoles = decrypted.Where(kvp => kvp.Key.Contains("north")).ToList();

			if (northpoles.Count != 1)
				throw new Exception();

			return northpoles[0].Value;
		}
	}
}