using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day7 : BaseDay
	{
		public override string Example1 =>
@"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.";

		class Node
		{
			public char id;
			public List<Node> to = new List<Node>();
			public List<Node> parents = new List<Node>();
		}

		Dictionary<char, Node> Transform(string raw)
		{
			var pairs = raw
				.Replace("\r", "")
				.Split('\n')
				.Select(s => new KeyValuePair<char,char>(s[5], s[36]))
				.ToList();

			var nodes = new Dictionary<char, Node>();
			foreach (var p in pairs)
			{
				if (!nodes.ContainsKey(p.Key))
					nodes.Add(p.Key, new Node() { id = p.Key });

				if (!nodes.ContainsKey(p.Value))
					nodes.Add(p.Value, new Node() { id = p.Value });
			}

			foreach (var p in pairs)
			{
				nodes[p.Key].to.Add(nodes[p.Value]);
				nodes[p.Value].parents.Add(nodes[p.Key]);
			}

			return nodes;
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);
			var queue = input.Values
				.Where(i => i.parents.Count == 0)
				.ToList();

			var result = new List<char>();

			while (queue.Count != 0)
			{
				queue.Sort((a, b) => a.id - b.id);

				var n = queue[0];
				queue.RemoveAt(0);
				result.Add(n.id);

				foreach (var t in n.to)
				{
					if (!result.Contains(t.id) && !queue.Contains(t))
					{
						if(t.parents.All(p => result.Contains(p.id)))
							queue.Add(t);
					}
				}
			}

			return string.Join("", result);
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);
			var queue = input.Values
				.Where(i => i.parents.Count == 0)
				.ToList();

			var result = new List<char>();

			while (queue.Count != 0)
			{
				queue.Sort((a, b) => a.id - b.id);

				var n = queue[0];
				queue.RemoveAt(0);
				result.Add(n.id);

				foreach (var t in n.to)
				{
					if (!result.Contains(t.id) && !queue.Contains(t))
					{
						if (t.parents.All(p => result.Contains(p.id)))
							queue.Add(t);
					}
				}
			}

			int workers = result.Count > 6 ? 5 : 2;
			int baseTime = result.Count > 6 ? 60 : 0;

			var workLeft = result.Select(r => r - 'A' + 1 + baseTime).ToList();
			var workState = new List<char>();
			for (int i = 0; i < workers; i++)
				workState.Add((char)0);

			int step = 1;
			for (; step < 1000000; step++)
			{
				for (int i = 0; i < result.Count; i++)
				{
					if (workLeft[i] > 0 && !workState.Contains(result[i]))
					{
						if (input[result[i]].parents
							.All(p => workLeft[result.IndexOf(p.id)] == 0))
						{
							var idx = workState.IndexOf((char)0);
							if (idx >= 0)
							{
								workState[idx] = result[i];
							}
							else
							{
								break;
							}
						}
					}
				}

				for (int i = 0; i < workState.Count; i++)
				{
					if (workState[i] != (char)0)
					{
					//	Console.WriteLine("worker " + (1 + i) + " => " + (char)(workState[i]) + "  left: " + workLeft[result.IndexOf(workState[i])]);
						workLeft[result.IndexOf(workState[i])]--;

						if (workLeft[result.IndexOf(workState[i])] == 0)
							workState[i] = (char)0;
					}
				}

				if (workLeft.Sum() == 0)
					break;
			}

			return step;
		}

	}
}