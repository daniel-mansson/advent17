using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day24
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 24;
		static string s_example =
@"0/2
2/2
2/3
3/4
3/5
0/1
10/1
9/10";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Node
		{
			public int a;
			public int b;

			public bool usingA;

			public Node(string raw)
			{
				var l = raw
					.Split('/');

				a = int.Parse(l[0]);
				b = int.Parse(l[1]);
			}

			public bool TryConnect(Node other)
			{
				if (usingA)
				{
					if (b == other.a)
					{
						other.usingA = true;
						return true;
					}
					else if(b == other.b)
					{
						other.usingA = false;
						return true;
					}
				}
				else
				{
					if (a == other.a)
					{
						other.usingA = true;
						return true;
					}
					else if (a == other.b)
					{
						other.usingA = false;
						return true;
					}
				}

				return false;
			}

			public int GetStrength()
			{
				return a + b;
			}
		}

		static List<Node> Transform(string raw)
		{
			return raw
				.Replace("\r", "")
				.Split('\n')
				.Select(l => new Node(l))
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			int best = DoWorkStart(input);

			return best;
		}

		static int DoWorkStart(List<Node> available)
		{
			int max = 0;

			foreach (var n in available)
			{
				if (n.a == 0 || n.b == 0)
				{
					n.usingA = n.a == 0;

					var nextAv = available.ToList();
					nextAv.Remove(n);
					int v = DoWork(n, nextAv);
					if (v > max)
					{
						max = v;
					}
				}
			}

			return max;
		}

		static int DoWork(Node current, List<Node> available)
		{
			int max = 0;

			foreach (var n in available)
			{
				if (current.TryConnect(n))
				{
					var nextAv = available.ToList();
					nextAv.Remove(n);
					int v = DoWork(n, nextAv);
					if (v > max)
					{
						max = v;
					}
				}
			}

			return max + current.GetStrength();
		}


		static int Solve2(string raw)
		{
			var input = Transform(raw);

			int best = DoWorkStart2(input);

			return best % 1000000;
		}

		static int DoWorkStart2(List<Node> available)
		{
			int max = 0;

			foreach (var n in available)
			{
				if (n.a == 0 || n.b == 0)
				{
					n.usingA = n.a == 0;

					var nextAv = available.ToList();
					nextAv.Remove(n);
					int v = DoWork2(n, nextAv, 1);
					if (v > max)
					{
						max = v;
					}
				}
			}

			return max;
		}

		static int DoWork2(Node current, List<Node> available, int depth)
		{
			int max = 0;
			bool isLeaf = true;

			foreach (var n in available)
			{
				if (current.TryConnect(n))
				{
					isLeaf = false;
					var nextAv = available.ToList();
					nextAv.Remove(n);
					int v = DoWork2(n, nextAv, depth + 1);
					if (v > max)
					{
						max = v;
					}
				}
			}
			if (isLeaf)
			{
				return 1000000 * depth + max + current.GetStrength();
			}
			else
			{
				return max + current.GetStrength();
			}
		}
	}
}