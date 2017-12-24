using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day23
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 23;
		static string s_example =
@"set b 84
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b
sub c -17000
set f 1
set d 2
set e 2
set g b
mod g d
sub g 0
jnz g 7
set f 0
jnz 1 10
sub e -1
set g e
sub g b
jnz g -9
sub d -1
set g d
sub g b
jnz g -14
jnz f 2
sub h -1
set g b
sub g c
jnz g 2
jnz 1 3
sub b -17
jnz 1 -24";

		static void Main(string[] args)
		{
			//Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			//Console.WriteLine("Example 2: " + Solve2(s_example));
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
			public int mulCount = 0;

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

			public void InsertInstruction(int idx, IInstruction instruction)
			{
				instructions.Insert(idx, instruction);

				for (int i = 0; i < idx; i++)
				{
					var instr = instructions[i] as JnzInstruction;
					if (instr != null)
					{
						int offset = int.Parse(instr.offset);
						if (i + offset >= idx)
							instr.offset = (offset + 1).ToString();
					}
				}

				for (int i = idx + 1; i < instructions.Count; i++)
				{
					var instr = instructions[i] as JnzInstruction;
					if (instr != null)
					{
						int offset = int.Parse(instr.offset);
						if (i + offset <= idx)
							instr.offset = (offset - 1).ToString();
					}
				}
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

		public class SubInstruction : IInstruction
		{
			public string addr;
			public string value;

			public void Execute(ComputerMachine machine)
			{
				long lhs = machine.GetValue(addr);
				long rhs = machine.GetValue(value);
				machine.SetValue(addr, lhs - rhs);
				machine.PC++;
			}

			public IInstruction Create(string[] data)
			{
				return new SubInstruction()
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
				machine.mulCount++;
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

		public class JnzInstruction : IInstruction
		{
			public string cmp;
			public string offset;

			public void Execute(ComputerMachine machine)
			{
				long cmpValue = machine.GetValue(cmp);

				if (cmpValue != 0)
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
				return new JnzInstruction()
				{
					cmp = data[1],
					offset = data[2]
				};
			}
		}

		static ComputerMachine Transform(string raw)
		{
			var lookup = new Dictionary<string, IInstruction>();
			lookup.Add("set", new SetInstruction());
			lookup.Add("sub", new SubInstruction());
			lookup.Add("mul", new MulInstruction());
			lookup.Add("jnz", new JnzInstruction());
			lookup.Add("mod", new ModInstruction());

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

		static int Solve(string raw)
		{
			var computer0 = Transform(raw);

			while (computer0.PC < computer0.instructions.Count)
			{
				computer0.Step();
			}

			return computer0.mulCount;
		}

		static long Solve2(string raw)
		{
			return SneakyWay(1);
		}

		static long SneakyWay(long a)
		{
			long b = 0;
			long c = 0;
			long d = 0;
			long e = 0;
			long f = 0;
			long g = 0;
			long h = 0;

			int mulc = 0;

			b = 84;
			c = b;
			if (a != 0)
			{
				b = b * 100;
				mulc++;
				b = b + 100000;
				c = b;
				c = c + 17000;
			}

			while (true)
			{
				f = 1;
				d = 2;
				do
				{
					e = 2;
					do
					{
						mulc++;

						g = d;
						g = d * e - b;

						if (g == 0)
						{
							f = 0;
							g = 0;
							d = b - 1;
							break;
						}

						e = e + 1;
						if (d * e > b)
						{
							e = b;
						}

						g = e;
						g = g - b;
					} while (g != 0);
					d = d + 1;

					g = d;
					g = g - b;
				} while (g != 0);

				if (f == 0)
				{
					h = h + 1;
				}

				g = b;
				g = g - c;

				if (g == 0)
				{
					return h;
				}
				b = b + 17;
			}
		}
	}
}