using System;
using System.Diagnostics;

namespace year2018.Problem
{
	public abstract class BaseDay
	{
		public abstract string Example1 { get; }
		public virtual string Example2 => Example1;

		public abstract object Solve1(string raw);
		public abstract object Solve2(string raw);

		public void Run(int year, int day)
		{
			try
			{
				Console.WriteLine();

				{
					var input = Example1;
					var sw = new Stopwatch();
					sw.Start();
					var result = Solve1(input);
					sw.Stop();
					Console.Write($"Example 1 ({sw.ElapsedMilliseconds} ms): ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(result);
					Console.ResetColor();
				}

				{
					var input = ProblemInput.FetchBlocking(year, day);
					var sw = new Stopwatch();
					sw.Start();
					var result = Solve1(input);
					sw.Stop();
					Console.Write($"Real 1 ({sw.ElapsedMilliseconds} ms): ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(result);
					Console.ResetColor();
				}

				Console.WriteLine();

				{
					var input = Example2;
					var sw = new Stopwatch();
					sw.Start();
					var result = Solve2(input);
					sw.Stop();
					Console.Write($"Example 2 ({sw.ElapsedMilliseconds} ms): ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(result);
					Console.ResetColor();
				}

				{
					var input = ProblemInput.FetchBlocking(year, day);
					var sw = new Stopwatch();
					sw.Start();
					var result = Solve2(input);
					sw.Stop();
					Console.Write($"Real 2 ({sw.ElapsedMilliseconds} ms): ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(result);
					Console.ResetColor();
				}

				Console.WriteLine();
				Console.ReadKey();
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.Message);
				Console.WriteLine();

				var lines = e.StackTrace.Split('\n');
				foreach (var line in lines)
				{
					var s = line.Split(new string[] { "line" }, StringSplitOptions.None);

					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(s[0]);

					if (s.Length > 1)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine("line" + s[1]);
					}
				}

				Console.ReadKey();
			}
		}
	}
}
