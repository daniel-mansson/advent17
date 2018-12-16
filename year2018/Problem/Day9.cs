using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	public class Day9 : BaseDay
	{
		//public override string Example1 => @"9 players; last marble is worth 25 points";
		//public override string Example1 => @"10 players; last marble is worth 1618 points";
		public override string Example1 => @"13 players; last marble is worth 7999 points";
		//public override string Example1 => @"17 players; last marble is worth 1104 points";
		//public override string Example1 => @"21 players; last marble is worth 6111 points";
		//public override string Example1 => @"30 players; last marble is worth 5807 points";

		class Input
		{
			public int players;
			public long points;
		}

		Input Transform(string raw)
		{
			var s = raw.Split(' ');
			return new Input()
			{
				players = int.Parse(s[0]),
				points = long.Parse(s[6])
			};
		}

		public override object Solve1(string raw)
		{
			var input = Transform(raw);
			return SolveArrayList(input);
		}

		object SolveArrayList(Input input)
		{
			var players = input.players;
			var score = new long[players];

			var lastMarble = input.points;
			long nextMarble = 1;

			var circle = new List<long>();
			circle.Add(0);
			int current = 0;
			int player = 0;

			while (nextMarble <= lastMarble)
			{
				if (nextMarble % 23 == 0)
				{
					int n = (current - 7 + circle.Count) % circle.Count;
					score[player] += circle[n] + nextMarble;
					circle.RemoveAt(n);
					current = n;
				}
				else
				{
					int n = (current + 2) % circle.Count;
					circle.Insert(n, nextMarble);
					current = n;
				}

				nextMarble++;
				player = (player + 1) % players;
			}

			return score.Max();
		}

		object SolveLinkedList(Input input)
		{
			var players = input.players;
			var score = new long[players];

			var lastMarble = input.points;
			long nextMarble = 1;

			var circle = new LinkedList<long>();
			var current = circle.AddFirst(0);
			long player = 0;

			while (nextMarble <= lastMarble)
			{
				if (nextMarble % 23 == 0)
				{
					for (long i = 0; i < 7; i++)
					{
						current = current.Previous ?? current.List.Last;
					}

					score[player] += current.Value + nextMarble;

					current = current.Next ?? current.List.First;
					circle.Remove(current.Previous ?? current.List.Last);
				}
				else
				{
					current = current.Next ?? current.List.First;
					current = circle.AddAfter(current, nextMarble);
				}

				nextMarble++;
				player = (player + 1) % players;
			}

			return score.Max();
		}

		public override object Solve2(string raw)
		{
			var input = Transform(raw);
			return SolveLinkedList(new Input()
			{
				players = input.players,
				points = input.points * 100
			});
		}
	}
}