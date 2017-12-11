using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day11
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 11;
		static string s_example =
@"ne,ne,ne";
		static string s_example2 =
@"ne,ne,sw,sw,ne,ne,s,s";
		static string s_example3 =
@"se,sw,se,sw,sw";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Example: " + Solve(s_example2));
			Console.WriteLine("Example: " + Solve(s_example3));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		static List<string> Transform(string raw)
		{
			return raw.Split(',').ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			var lookup = new Dictionary<string, AxialCoord>();
			lookup["n"] = AxialCoord.Right;
			lookup["ne"] = AxialCoord.DownRight;
			lookup["se"] = AxialCoord.DownLeft;
			lookup["s"] = AxialCoord.Left;
			lookup["sw"] = AxialCoord.UpLeft;
			lookup["nw"] = AxialCoord.UpRight;

			var pos = new AxialCoord();

			foreach (var op in input)
			{
				pos += lookup[op];
			}

			var result = AxialCoord.Distance(pos, new AxialCoord());
			return result;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			var lookup = new Dictionary<string, AxialCoord>();
			lookup["n"] = AxialCoord.Right;
			lookup["ne"] = AxialCoord.DownRight;
			lookup["se"] = AxialCoord.DownLeft;
			lookup["s"] = AxialCoord.Left;
			lookup["sw"] = AxialCoord.UpLeft;
			lookup["nw"] = AxialCoord.UpRight;

			var pos = new AxialCoord();
			int max = 0;

			foreach (var op in input)
			{
				pos += lookup[op];
				var dist = AxialCoord.Distance(pos, new AxialCoord());
				if (dist > max)
					max = dist;
			}

			return max;
		}
	}
}