using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day18
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 18;
		static string s_example =
@"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		public class ComputerMachine
		{
			public int PC = 0;
			public Dictionary<string, long> registers = new Dictionary<string, long>();
			public List<IInstruction> instructions = new List<IInstruction>();
			public long playingSound = 0;
			public event Action<long> ReceivedInterrupt;

			public long GetValue(string token)
			{
				long value = 0;
				if (!long.TryParse(token, out value))
				{
					registers.TryGetValue(token, out value);
				}

				return value;
			}

			public void SetValue(string addr, long value)
			{
				registers[addr] = value;
			}

			public void Step()
			{
				instructions[PC].Execute(this);
			}

			public void OnReceivedRecoverInterrupt()
			{
				ReceivedInterrupt?.Invoke(playingSound);
			}
		}

		public interface IInstruction
		{
			IInstruction Create(string[] data);
			void Execute(ComputerMachine machine);
		}

		public class SetInstruction : IInstruction
		{
			public string addr;
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				machine.SetValue(addr, rhs);
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new SetInstruction()
				{
					addr = data[1],
					value = data[2]
				};
			}
		}

		public class AddInstruction : IInstruction
		{
			public string addr;
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				long lhs = machine.GetValue(addr);
				machine.SetValue(addr, rhs + lhs);
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new AddInstruction()
				{
					addr = data[1],
					value = data[2]
				};
			}
		}

		public class MulInstruction : IInstruction
		{
			public string addr;
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				long lhs = machine.GetValue(addr);
				machine.SetValue(addr, rhs * lhs);
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new MulInstruction()
				{
					addr = data[1],
					value = data[2]
				};
			}
		}

		public class ModInstruction : IInstruction
		{
			public string addr;
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				long lhs = machine.GetValue(addr);
				machine.SetValue(addr, lhs % rhs);
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new ModInstruction()
				{
					addr = data[1],
					value = data[2]
				};
			}
		}

		public class SndInstruction : IInstruction
		{
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				machine.playingSound = rhs;
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new SndInstruction()
				{
					value = data[1]
				};
			}
		}

		public class RcvInstruction : IInstruction
		{
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long rhs = machine.GetValue(value);
				if (rhs != 0)
				{
					machine.OnReceivedRecoverInterrupt();
				}
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new RcvInstruction()
				{
					value = data[1]
				};
			}
		}

		public class JgzInstruction : IInstruction
		{
			public string cmp;
			public string offset;

			public void Execute(ComputerMachine machine)
			{
				long cmpValue = machine.GetValue(cmp);
				if (cmpValue > 0)
				{
					long offsetValue = machine.GetValue(offset);
					machine.PC += (int)offsetValue;
				}
				else
				{
					machine.PC++;
				}
			}

			public IInstruction Create(string[] data)
			{
				return new JgzInstruction()
				{
					cmp = data[1],
					offset = data[2]
				};
			}
		}

		static ComputerMachine Transform(string raw)
		{
			var lookup = new Dictionary<string, IInstruction>();
			lookup.Add("snd", new SndInstruction());
			lookup.Add("set", new SetInstruction());
			lookup.Add("add", new AddInstruction());
			lookup.Add("mul", new MulInstruction());
			lookup.Add("mod", new ModInstruction());
			lookup.Add("rcv", new RcvInstruction());
			lookup.Add("jgz", new JgzInstruction());

			var instructions = raw
				.Replace("\r", "")
				.Split('\n')
				.Select(l =>
				{
					var tokens = l.Split(' ');
					return lookup[tokens[0]].Create(tokens);
				})
				.ToList();

			return new ComputerMachine()
			{
				instructions = instructions
			};
		}

		static long Solve(string raw)
		{
			var computer = Transform(raw);
			bool done = false;
			computer.ReceivedInterrupt += (sound) =>
			{
				done = true;
			};

			while (!done)
			{
				computer.Step();
			}

			return computer.playingSound;
		}

		static long Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}