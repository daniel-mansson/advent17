using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day8
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 8;
		static string s_example =
@"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Statement
		{
			public string Register;
			public int Value;
			public string Operation;
			public string Condition;
			public string CmpLhs;
			public string CmpRhs;

			public int PerformOperation(int lhs, int rhs)
			{
				switch (Operation)
				{
					case "inc":
						return lhs + rhs;
					case "dec":
						return lhs - rhs;
				}

				throw new Exception("Unsupported: " + Operation);
			}

			public bool EvaluateCondition(int lhs, int rhs)
			{
				switch (Condition)
				{
					case ">":
						return lhs > rhs;
					case ">=":
						return lhs >= rhs;
					case "<":
						return lhs < rhs;
					case "<=":
						return lhs <= rhs;
					case "!=":
						return lhs != rhs;
					case "==":
						return lhs == rhs;
					default:
						throw new Exception("Unsupported: " + Condition);
				}
			}
		}

		static List<Statement> Transform(string raw)
		{
			return raw
				.Split('\n')
				.Select(l =>
				{
					var s = l.Split(' ');

					return new Statement()
					{
						Register = s[0],
						Operation = s[1],
						Value = int.Parse(s[2]),
						Condition = s[5],
						CmpLhs = s[4],
						CmpRhs = s[6]
					};
				}).ToList();
		}

		static int Solve(string raw)
		{
			var memory = new Dictionary<string, int>();
			var input = Transform(raw);

			foreach (var statement in input)
			{
				if (statement.EvaluateCondition(
					ToValue(memory, statement.CmpLhs),
					ToValue(memory, statement.CmpRhs)))
				{
					int result = statement.PerformOperation(
						ToValue(memory, statement.Register),
						statement.Value);

					memory[statement.Register] = result;
				}
			}

			int max = memory.Max(kvp => kvp.Value);

			return max;
		}

		static int ToValue(Dictionary<string, int> memory, string id)
		{
			int value;
			if (int.TryParse(id, out value))
			{
				return value;
			}
			else
			{
				if (!memory.ContainsKey(id))
				{
					memory[id] = 0;
				}

				return memory[id];
			}
		}

		static int Solve2(string raw)
		{
			var memory = new Dictionary<string, int>();
			var input = Transform(raw);

			int max = 0;

			foreach (var statement in input)
			{
				if (statement.EvaluateCondition(
					ToValue(memory, statement.CmpLhs),
					ToValue(memory, statement.CmpRhs)))
				{
					int result = statement.PerformOperation(
						ToValue(memory, statement.Register),
						statement.Value);

					memory[statement.Register] = result;
					max = Math.Max(max, result);
				}
			}

			return max;
		}
	}
}