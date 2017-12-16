using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day16
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 16;
		static string s_example =
@"s1,x3/4,pe/b";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example, "abcde"));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day), "abcdefghijklmnop"));

			Console.WriteLine("Example 2: " + Solve2(s_example, "abcde"));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day), "abcdefghijklmnop"));

			Console.ReadKey();
		}

		interface IOperation
		{
			void Perform(char[] input);
		}

		class Spin : IOperation
		{
			int m_size;

			public Spin(string raw)
			{
				m_size = int.Parse(raw);
			}

			public void Perform(char[] input)
			{
				//0,1,2,3,4,5
				string s = new string(input);
				int p = input.Length - m_size;
				s = s.Substring(p) + s.Substring(0, p);
				var res = s.ToCharArray();
				for (int i = 0; i < input.Length; i++)
					input[i] = res[i];
			}
		}

		class Exchange : IOperation
		{
			int m_a;
			int m_b;

			public Exchange(string raw)
			{
				string[] i = raw.Split('/');
				m_a = int.Parse(i[0]);
				m_b = int.Parse(i[1]);
			}

			public void Perform(char[] input)
			{
				char tmp = input[m_a];
				input[m_a] = input[m_b];
				input[m_b] = tmp;
			}
		}

		class Partner : IOperation
		{
			char m_a;
			char m_b;

			public Partner(string raw)
			{
				m_a = raw[0];
				m_b = raw[2];
			}

			public void Perform(char[] input)
			{
				int it1 = -1;
				int it2 = -1;

				for (int i = 0; i < input.Length; i++)
				{
					if (input[i] == m_a)
						it1 = i;
					if (input[i] == m_b)
						it2 = i;
				}

				char tmp = input[it1];
				input[it1] = input[it2];
				input[it2] = tmp;
			}
		}

		static bool IsHealthy(char[] data, char[] orig)
		{
			foreach (var c in orig)
			{
				if (data.Count(i => i == c) != 1)
					return false;
			}

			return true;
		}

		static List<IOperation> Transform(string raw)
		{
			return raw
				.Split(',')
				.Select(s => 
				{
					switch (s[0])
					{
						case 's':
							return (IOperation)new Spin(s.Substring(1));
						case 'x':
							return (IOperation)new Exchange(s.Substring(1));
						case 'p':
							return (IOperation)new Partner(s.Substring(1));
						default:
							throw new Exception("Bad op: "  + s[0]);
					}
				})
				.ToList();
		}

		static string Solve(string raw, string dancers)
		{
			var input = Transform(raw);

			var data = dancers.ToCharArray();

			for (int i = 0; i < input.Count; i++)
			{
				input[i].Perform(data);
			}

			return new string(data);
		}

		static string Solve2(string raw, string dancers)
		{
			var input = Transform(raw);

			List<string> memoryList = new List<string>();
			Dictionary<string, int> memoryLookup = new Dictionary<string, int>();

			string res = dancers;
			for (int i = 0; i < 1000000000; i++)
			{
				if (!memoryLookup.ContainsKey(res))
				{
					memoryList.Add(res);
					memoryLookup[res] = i;
				}
				else
				{
					break;
				}

				res = Solve(raw, res);
			}

			return memoryList[1000000000 % memoryList.Count];
		}
	}
}