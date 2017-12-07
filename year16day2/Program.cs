using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year16day2
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Solve2(
@"ULL
RRDDD
LURDL
UUUUD"));

			Console.WriteLine(Solve2(ProblemInput.FetchBlocking(2016, 2)));

			Console.ReadKey();
		}

		static List<List<char>> Transform(string raw)
		{
			return raw.Split('\n').Select(s => s.ToList()).ToList();
		}

		static string Solve(string raw)
		{
			var input = Transform(raw);

			int x = 1;
			int y = 1;

			string code = "";

			foreach (var line in input)
			{
				foreach (var op in line)
				{
					if (op == 'U' && y > 0)
						--y;
					else if (op == 'D' && y < 2)
						++y;
					else if (op == 'L' && x > 0)
						--x;
					else if (op == 'R' && x < 2)
						++x;
				}

				code += (y * 3 + x + 1).ToString();
			}

			return code;
		}


		static string Solve2(string raw)
		{
			var input = Transform(raw);

			int x = 0;
			int y = 2;

			char[] maze = new char[]
			{
				'0','0','1','0','0',
				'0','2','3','4','0',
				'5','6','7','8','9',
				'0','A','B','C','0',
				'0','0','D','0','0',
			};

			string code = "";

			foreach (var line in input)
			{
				foreach (var op in line)
				{
					if (op == 'U' && y > 0 && maze[y * 5 + x - 5] != '0')
						--y;
					else if (op == 'D' && y < 4 && maze[y * 5 + x + 5] != '0')
						++y;
					else if (op == 'L' && x > 0 && maze[y * 5 + x - 1] != '0')
						--x;
					else if (op == 'R' && x < 4 && maze[y * 5 + x + 1] != '0')
						++x;
				}

				code += maze[y * 5 + x];
			}

			return code;
		}
	}
}
