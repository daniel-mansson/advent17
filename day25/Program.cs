using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day25
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 25;
		static string s_example =
@"Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
  If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
  If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
  If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		public class State
		{
			public State(string raw)
			{
				var lines = raw.Split('\n');

				id = lines[0][9];
				values[0] = lines[2][22] == '1' ? 1 : 0;
				moves[0] = lines[3][27] == 'r' ? 1 : -1;
				nextStates[0] = lines[4][26];

				values[1] = lines[6][22] == '1' ? 1 : 0;
				moves[1] = lines[7][27] == 'r' ? 1 : -1;
				nextStates[1] = lines[8][26];
			}

			public char id;
			public int[] values = new int[2];
			public int[] moves = new int[2];
			public char[] nextStates = new char[2];
		}

		public class Machine
		{
			public char startState;
			public Dictionary<char, State> states = new Dictionary<char, State>();
			public int steps = 0;
			public int stepTarget;
			public State currentState;
			public long currentPos = 0;
			public Dictionary<long, bool> tape = new Dictionary<long, bool>();

			public Machine(string raw)
			{
				var lines = raw.Split('\n');

				startState = lines[0]
					.Replace("Begin in state ", "")
					.Replace(".", "")[0];

				stepTarget = int.Parse(lines[1]
					.Replace("Perform a diagnostic checksum after ", "")
					.Replace(" steps.", ""));
			}

			public void SetStartState()
			{
				currentState = states[startState];
			}

			void Put(long idx, int value)
			{
				if (value == 1)
				{
					if (!tape.ContainsKey(idx))
					{
						tape.Add(idx, true);
					}
				}
				else
				{
					if (tape.ContainsKey(idx))
					{
						tape.Remove(idx);
					}
				}
			}

			int Get(long idx)
			{
				return tape.ContainsKey(idx) ? 1 : 0;
			}

			public void Step()
			{
				var value = Get(currentPos);

				Put(currentPos, currentState.values[value]);
				currentPos += currentState.moves[value];
				currentState = states[currentState.nextStates[value]];
				steps++;
			}

			public int GetDiagHash()
			{
				return tape.Count;
			}
		}

		static Machine Transform(string raw)
		{
			var parts = raw
				.Replace("\r", "")
				.Replace("\n\n", ";")
				.Split(';');

			var machine = new Machine(parts[0]);

			for (int i = 1; i < parts.Length; i++)
			{
				var state = new State(parts[i]);
				machine.states.Add(state.id, state);
			}

			machine.SetStartState();

			return machine;
		}

		static int Solve(string raw)
		{
			var machine = Transform(raw);

			for (int i = 0; i < machine.stepTarget; i++)
			{
				machine.Step();
			}

			return machine.GetDiagHash();
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}