using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day21
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 21;
		static string s_example =
@"../.# => ##./#../...
.#./#../### => #..#/..../..../#..#";
//.#./..#/### => #..#/..../..../#..#";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example, 2));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day), 5));

			Console.WriteLine("Real 2: " + Solve(ProblemInput.FetchBlocking(s_year, s_day), 18));

			Console.ReadKey();
		}

		class Rule
		{
			public int[] hashes;
			public List<List<char>> chars;
			public List<List<char>> result;
			public int size;

			public Rule(string raw)
			{
				var split = raw
					.Replace(" => ", ",")
					.Split(',');

				chars = split[0]
					.Split('/')
					.Select(l => l.ToList())
					.ToList();

				result = split[1]
					.Split('/')
					.Select(l => l.ToList())
					.ToList();

				size = chars.Count;

				hashes = new int[8];

				for (int k = 0; k < 2; k++)
				{
					for (int i = 0; i < size; i++)
					{
						for (int j = 0; j < size; j++)
						{
							if (chars[i][j] == '#')
								hashes[k * 4 + 0] |= 1 << (i * size + j);

							if (chars[j][size - 1 - i] == '#')
								hashes[k * 4 + 1] |= 1 << (i * size + j);

							if (chars[size - 1 - i][size - 1 - j] == '#')
								hashes[k * 4 + 2] |= 1 << (i * size + j);

							if (chars[size - 1 - j][i] == '#')
								hashes[k * 4 + 3] |= 1 << (i * size + j);
						}
					}

					for (int i = 0; i < size; i++)
					{
						chars[i].Reverse();
					}
				}


				if (size == 3)
				{
					for (int i = 0; i < hashes.Length; i++)
						hashes[i] |= 1 << 16;
				}
			}
		}

		static List<Rule> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Split('\n')
				.Select(l => new Rule(l))
				.ToList();
		}

		class Enhancer
		{
			public List<List<char>> current;
			public List<Rule> rules;
			public Dictionary<int, Rule> lookup = new Dictionary<int, Rule>();

			public Enhancer(List<Rule> rules)
			{
				this.rules = rules;
				current = new List<List<char>>()
				{
					".#.".ToList(),
					"..#".ToList(),
					"###".ToList()
				};

				foreach (var rule in rules)
				{
					foreach (var hash in rule.hashes)
					{
						if(!lookup.ContainsKey(hash))
							lookup.Add(hash, rule);
					}
				}
			}

			public void Step()
			{
				if (current.Count % 2 == 0)
					StepWithSize(2);
				else
					StepWithSize(3);
			}

			void StepWithSize(int size)
			{
				int newSize = current.Count * (size + 1) / size;
				List<List<char>> next = new List<List<char>>();
				List<char> line = new List<char>();

				for (int i = 0; i < newSize; i++)
					line.Add('.');
				for (int i = 0; i < newSize; i++)
					next.Add(line.ToList());

				for (int i = 0; i < current.Count; i += size)
				{
					for (int j = 0; j < current.Count; j += size)
					{
						int hash = 0;
						for (int y = 0; y < size; y++)
						{
							for (int x = 0; x < size; x++)
							{
								if (current[i + y][j + x] == '#')
									hash |= 1 << (y * size + x);
							}
						}

						if (size == 3)
							hash |= 1 << 16;

						var rule = lookup[hash];
						for (int y = 0; y < (size + 1); y++)
						{
							for (int x = 0; x < (size + 1); x++)
							{
								next[i * (size + 1) / size + y][j * (size + 1) / size + x] = rule.result[y][x];
							}
						}
					}
				}

				current = next;
			}

			public override string ToString()
			{
				var builder = new StringBuilder();

				for (int i = 0; i < current.Count; i++)
				{
					for (int j = 0; j < current.Count; j++)
					{
						builder.Append(current[i][j]);
					}
					builder.AppendLine();
				}

				return builder.ToString();
			}

			public int NumPixelsOn()
			{
				return current.Sum(l => l.Count(c => c == '#'));
			}
		}

		static int Solve(string raw, int interations)
		{
			var input = Transform(raw);
			var enhancer = new Enhancer(input);

			for (int i = 0; i < interations; i++)
			{
				enhancer.Step();

				//Console.WriteLine(enhancer);
				//Console.WriteLine();
			}

			return enhancer.NumPixelsOn();
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}