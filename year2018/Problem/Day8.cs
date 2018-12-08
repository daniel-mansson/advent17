using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day8 : BaseDay
	{
		public override string Example1 =>
@"2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

		List<int> Transform(string raw)
		{
			return raw.Split(' ').Select(i => int.Parse(i)).ToList();
		}

		class Node
		{
			public List<Node> children = new List<Node>();
			public List<int> metadata = new List<int>();
		}

		Node Parse(Queue<int> queue)
		{
			var node = new Node();

			var numChildren = queue.Dequeue();
			var numMetadata = queue.Dequeue();

			for (int i = 0; i < numChildren; i++)
			{
				node.children.Add(Parse(queue));
			}

			for (int i = 0; i < numMetadata; i++)
			{
				node.metadata.Add(queue.Dequeue());
			}

			return node;
		}

		int SumMeta(Node node)
		{
			int sum = node.metadata.Sum();
			sum += node.children.Select(c => SumMeta(c)).Sum();

			return sum;
		}

		int GetValue(Node node)
		{
			if (node.children.Count == 0)
			{
				return node.metadata.Sum();
			}
			else
			{
				int sum = 0;

				for (int i = 0; i < node.metadata.Count; i++)
				{
					int idx = node.metadata[i] - 1;
					if (idx >= 0 && idx < node.children.Count)
					{
						sum += GetValue(node.children[idx]);
					}
				}

				return sum;
			}
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);
			var root = Parse(new Queue<int>(input));
			var sum = SumMeta(root);

			return sum;
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);
			var root = Parse(new Queue<int>(input));
			var value = GetValue(root);

			return value;
		}
	}
}