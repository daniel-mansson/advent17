using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day12
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 12;
		static string s_example =
@"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5";

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
			public Dictionary<int, List<int>> m_connections = new Dictionary<int, List<int>>();

			public void AddConnection(int a, int b)
			{
				List<int> l;
				if (!m_connections.TryGetValue(a, out l))
				{
					l = new List<int>();
					m_connections[a] = l;
				}
				if (!l.Contains(b))
					l.Add(b);

				if (!m_connections.TryGetValue(b, out l))
				{
					l = new List<int>();
					m_connections[b] = l;
				}
				if (!l.Contains(a))
					l.Add(a);
			}

			public List<int> GetConnections(int a)
			{
				List<int> l;
				if (!m_connections.TryGetValue(a, out l))
					l = new List<int>();

				return l;
			}
		}

		static Map Transform(string raw)
		{
			var nodeList = raw
				.Replace(" <-> ", ", ")
				.Replace(", ", ",")
				.Split('\n')
				.Select(s => s
					.Split(',')
					.Select(i => int.Parse(i))
					.ToList())
				.ToList();

			var map = new Map();

			foreach (var node in nodeList)
			{
				int from = node[0];
				for (int i = 0; i < node.Count; i++)
				{
					map.AddConnection(from, node[i]);
				}
			}

			return map;
		}

		static int Solve(string raw)
		{
			var map = Transform(raw);
			List<int> queue = new List<int>();

			int current = 0;
			queue.Add(0);

			while (current < queue.Count)
			{
				var connections = map.GetConnections(queue[current]);

				foreach (var c in connections)
				{
					if (!queue.Contains(c))
					{
						queue.Add(c);
					}
				}

				current++;
			}

			return current;
		}

		static int Solve2(string raw)
		{
			var map = Transform(raw);
			List<int> visited = new List<int>();
			int groupCount = 0;
			var values = map.m_connections.Keys.ToList();

			for (int i = 0; i < values.Count; i++)
			{
				int start = values[i];

				if (!visited.Contains(start))
				{
					groupCount++;

					int current = 0;
					List<int> queue = new List<int>();
					queue.Add(start);

					while (current < queue.Count)
					{
						var connections = map.GetConnections(queue[current]);

						foreach (var c in connections)
						{
							if (!queue.Contains(c))
							{
								queue.Add(c);
							}
						}

						current++;
					}

					visited.AddRange(queue);
				}
			}

			return groupCount;
		}
	}
}