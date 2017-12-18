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
@"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d";

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

			public List<long> sendQueue = new List<long>();
			public List<long> recvQueue;
			public bool blocked = false;

			public int sendCount = 0;

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

				machine.sendCount++;
				machine.sendQueue.Add(rhs);

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
			public string addr;

			public void Execute(ComputerMachine machine)
			{
				if (machine.recvQueue.Count != 0)
				{
					machine.SetValue(addr, machine.recvQueue[0]);
					machine.recvQueue.RemoveAt(0);
					machine.blocked = false;

					machine.PC++;
				}
				else
				{
					machine.blocked = true;
				}
			}

			public IInstruction Create(string[] data)
			{
				return new RcvInstruction()
				{
					addr = data[1]
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
			var computer0 = Transform(raw);
			var computer1 = Transform(raw);

			computer0.recvQueue = computer1.sendQueue;
			computer1.recvQueue = computer0.sendQueue;

			computer1.registers["p"] = 1;

			while (!computer0.blocked || !computer1.blocked)
			{
				computer0.Step();
				computer1.Step();
			}

			return computer1.sendCount;
		}

		static long Solve2(string raw)
		{
			var input = Transform(raw);

			return -1;
		}
	}
}