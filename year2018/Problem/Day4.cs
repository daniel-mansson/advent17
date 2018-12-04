using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2018.Problem
{
	class Day4
	{
		static string s_example =
@"[1518-11-01 00:00] Guard #10 begins shift
[1518-11-01 00:05] falls asleep
[1518-11-01 00:25] wakes up
[1518-11-01 00:30] falls asleep
[1518-11-01 00:55] wakes up
[1518-11-01 23:58] Guard #99 begins shift
[1518-11-02 00:40] falls asleep
[1518-11-02 00:50] wakes up
[1518-11-03 00:05] Guard #10 begins shift
[1518-11-03 00:24] falls asleep
[1518-11-03 00:29] wakes up
[1518-11-04 00:02] Guard #99 begins shift
[1518-11-04 00:36] falls asleep
[1518-11-04 00:46] wakes up
[1518-11-05 00:03] Guard #99 begins shift
[1518-11-05 00:45] falls asleep
[1518-11-05 00:55] wakes up";

		static void Run(int year, int day)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(year, day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(year, day)));

			Console.ReadKey();
		}

		enum Type
		{
			Begin,
			FallsAsleep,
			WakesUp
		}

		class Entry
		{
			public int time;
			public int month;
			public int day;
			public int id;
			public Type type;

			public int DayOfYear => month * 31 + day;
		}

		static List<Entry> Transform(string raw)
		{
			var list = raw
				.Replace("\r", "")
				.Replace("[1518-", "")
				.Replace("-", " ")
				.Replace(":", " ")
				.Replace("]", "")
				.Replace("#", "")
				.Split('\n')
				.Select(s =>
				{
					var split = s.Split(' ');

					var e = new Entry()
					{
						month = int.Parse(split[0]),
						day = int.Parse(split[1]) + (split[2] == "23" ? 1 : 0),
						time = int.Parse(split[3]) - (split[2] == "23" ? 60 : 0),
					};

					switch (split[4])
					{
						case "Guard":
							e.type = Type.Begin;
							break;
						case "falls":
							e.type = Type.FallsAsleep;
							break;
						case "wakes":
							e.type = Type.WakesUp;
							break;
						default:
							throw new Exception();
					}

					if (e.type == Type.Begin)
					{
						e.id = int.Parse(split[5]);
					}

					return e;
				})
				.OrderBy(e => e.DayOfYear)
				.ThenBy(e => e.time)
				.ToList();

			return list;
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			var guards = new Dictionary<int, Dictionary<int, int>>();
			input
				.Where(i => i.type == Type.Begin)
				.Select(i => i.id)
				.Distinct()
				.ToList()
				.ForEach(id =>
				{
					var d = new Dictionary<int, int>();
					for (int j = 0; j < 60; j++)
					{
						d[j] = 0;
					}
					guards[id] = d;
				});

			int lastId = -1;
			int lastSleepTime = -1;
			foreach (var item in input)
			{
				switch (item.type)
				{
					case Type.Begin:
						lastId = item.id;
						break;
					case Type.FallsAsleep:
						lastSleepTime = item.time;
						break;
					case Type.WakesUp:
						for (int i = lastSleepTime; i < item.time; i++)
						{
							guards[lastId][i]++;
						}
						break;
					default:
						break;
				}
			}

			int maxId = 0;
			int maxSleep = 0;

			foreach (var guard in guards)
			{
				var m = guard.Value.Values.Sum();
				if (m > maxSleep)
				{
					maxId = guard.Key;
					maxSleep = m;
				}
			}

			int ma = guards[maxId].Values.Max();
			int maxMinute = guards[maxId].First(kvp => kvp.Value == ma).Key;


			return maxId * maxMinute;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			var guards = new Dictionary<int, Dictionary<int, int>>();
			input
				.Where(i => i.type == Type.Begin)
				.Select(i => i.id)
				.Distinct()
				.ToList()
				.ForEach(id =>
				{
					var d = new Dictionary<int, int>();
					for (int j = 0; j < 60; j++)
					{
						d[j] = 0;
					}
					guards[id] = d;
				});

			int lastId = -1;
			int lastSleepTime = -1;
			foreach (var item in input)
			{
				switch (item.type)
				{
					case Type.Begin:
						lastId = item.id;
						break;
					case Type.FallsAsleep:
						lastSleepTime = item.time;
						break;
					case Type.WakesUp:
						for (int i = lastSleepTime; i < item.time; i++)
						{
							guards[lastId][i]++;
						}
						break;
					default:
						break;
				}
			}

			int maxId = 0;
			int maxMinute = 0;
			int maxSleep = 0;

			foreach (var guard in guards)
			{
				var m = guard.Value.Values.Max();
				if (m > maxSleep)
				{
					maxId = guard.Key;
					maxMinute = guard.Value.First(kvp => kvp.Value == m).Key;
					maxSleep = m;
				}
			}

			return maxId * maxMinute;
		}
	}
}