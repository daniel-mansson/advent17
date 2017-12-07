using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day7
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Solve2(
@"pbga (66)
xhth (57)
ebii (61)
havc (66)
ktlj (57)
fwft (72) -> ktlj, cntj, xhth
qoyq (66)
padx (45) -> pbga, havc, qoyq
tknk (41) -> ugml, padx, fwft
jptl (61)
ugml (68) -> gyxo, ebii, jptl
gyxo (61)
cntj (57)"));
			Console.WriteLine(Solve2(ProblemInput.FetchBlocking(2017, 7)));

			Console.ReadKey();
		}

		class Node
		{
			public List<string> RawChildren = new List<string>();
			public List<Node> Children = new List<Node>();
			public Node Parent;
			public int Weight;
			public string Name;
		}

		static List<Node> Transform(string raw)
		{
			var nodes = raw
				.Replace(", ", ",")
				.Replace(" (", ";")
				.Replace(")", ";")
				.Replace(" -> ", "")
				.Split('\n')
				.Select(l => 
				{
					var data = l.TrimEnd('\r').Split(';');
					return new Node()
					{
						Name = data[0],
						Weight = int.Parse(data[1]),
						RawChildren = data[2].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
					};
				})
				.ToList();

			foreach (var node in nodes)
			{
				foreach (var rawChild in node.RawChildren)
				{
					var child = nodes.First(n => n.Name == rawChild);
					node.Children.Add(child);
					child.Parent = node;
				}
			}

			return nodes;
		}

		static string Solve(string raw)
		{
			var input = Transform(raw);
			return GetRoot(input[0]).Name;
		}

		static int Solve2(string raw)
		{
			try
			{
				var input = Transform(raw);
				var root = GetRoot(input[0]);

				int w = GetWeight(root);
				return w;
			}
			catch (OutOfBalanceException e)
			{
				return e.ExpectedWeight;
			}
		}

		static Node GetRoot(Node node)
		{
			while (node.Parent != null)
				node = node.Parent;

			return node;
		}

		static int GetWeight(Node node)
		{
			int sum = 0;
			var stackWeights = new List<int>();

			foreach (var child in node.Children)
			{
				int w = GetWeight(child);
				stackWeights.Add(w);
				sum += w;
			}

			if (stackWeights.Count > 1)
			{
				var distinct = stackWeights.Distinct().ToList();
				if (distinct.Count != 1)
				{
					var balanced = distinct.First(w => stackWeights.Count(sw => sw == w) != 1);
					var unbalanced = distinct.First(w => stackWeights.Count(sw => sw == w) == 1);

					int diff = balanced - unbalanced;

					int idx = stackWeights.FindIndex(w => w == unbalanced);
					int badChildWeight = node.Children[idx].Weight;

					throw new OutOfBalanceException(badChildWeight + diff);
				}
			}

			return sum + node.Weight;
		}

		class OutOfBalanceException : System.Exception
		{
			public int ExpectedWeight { get; private set; }

			public OutOfBalanceException(int expectedWeight)
				: base("Expected weight: " + expectedWeight)
			{
				ExpectedWeight = expectedWeight;
			}
		}
	}
}
