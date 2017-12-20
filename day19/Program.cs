using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day19
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 19;
		static string s_example =
@"     |          
     |  +--+    
     A  |  C    
 F---|----E|--+ 
     |  |  |  D 
     +B-+  +--+ ";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Map
		{
			public struct Vec2
			{
				public int x;
				public int y;
			}

			public int m_width;
			public int m_height;
			public List<List<char>> m_data;
			public List<char> m_memory = new List<char>();

			public Vec2 m_pos;
			public int m_dir = 3;
			public int m_steps = 0;

			public List<Vec2> m_dirs = new List<Vec2>()
			{
				new Vec2(){ x = 1, y = 0 },
				new Vec2(){ x = 0, y = -1 },
				new Vec2(){ x = -1, y = 0 },
				new Vec2(){ x = 0, y = 1 },
			};

			public Map(List<List<char>> chars)
			{
				m_width = chars[0].Count;
				m_height = chars.Count;
				m_data = chars;

				m_pos.x = m_data[0].IndexOf('|');
			}

			public char Get(int x, int y)
			{
				if (x < 0 || x >= m_width || y < 0 || y >= m_height)
					return ' ';

				return m_data[y][x];
			}

			public char Step()
			{
				++m_steps;
				var d = m_dirs[m_dir];
				m_pos.x += d.x;
				m_pos.y += d.y;

				var value = Get(m_pos.x, m_pos.y);
				if (value == '+')
				{
					//Time to turn yo!
					int uncoolDir = (m_dir + 2) % 4;
					for (int i = 0; i < 4; i++)
					{
						if (i == uncoolDir)
							continue;

						var nd = m_dirs[i];
						var next = Get(m_pos.x + nd.x, m_pos.y + nd.y);

						if (next != ' ')
						{
							m_dir = i;
							break;
						}
					}
				}

				if (value >= 'A' && value <= 'Z')
				{
					m_memory.Add(value);
				}

				return value;
			}
		}

		static Map Transform(string raw)
		{
			var chars = raw
				.Replace("\r", "")
				.Split('\n')
				.Select(l => l.ToList())
				.ToList();

			return new Map(chars);
		}

		static string Solve(string raw)
		{
			var maze = Transform(raw);

			while (maze.Step() != ' ')
			{ }

			return string.Join("", maze.m_memory);
		}

		static int Solve2(string raw)
		{
			var maze = Transform(raw);

			while (maze.Step() != ' ')
			{ }

			return maze.m_steps;
		}
	}
}