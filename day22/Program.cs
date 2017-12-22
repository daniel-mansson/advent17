using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day22
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 22;
		static string s_example =
@"..#
#..
...";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		public struct Vec2
		{
			public int x;
			public int y;
		}

		public class MoverThing
		{
			public Vec2 pos;
			public int dir;
		}

		public static List<Vec2> lookup = new List<Vec2>()
		{
			new Vec2() { x = 0, y = 1 },
			new Vec2() { x = 1, y = 0 },
			new Vec2() { x = 0, y = -1 },
			new Vec2() { x = -1, y = 0 }
		};

		public class Map
		{
			public Dictionary<int, bool> map = new Dictionary<int, bool>();

			public Map(string raw)
			{

			}

			public bool Get(int x, int y)
			{

			}

			public void Flip(int x, int y)
			{

			}
		}

		static string Transform(string raw)
		{
			return raw;
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			return -1;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}