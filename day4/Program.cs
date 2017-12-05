using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day4
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Run());

			Console.WriteLine(IsValid2(new List<string>()
			{
				"abcde", "xyz", "ecdab"
			}));

			Console.WriteLine(IsValid2(new List<string>()
			{
				"iiii", "oiii", "ooii", "oooi", "oooo"
			}));
			Console.WriteLine(Run2());

			Console.ReadKey();
		}

		static int Run()
		{
			var indata = LoadData("indata.txt");
			return Solve(indata);
		}

		static int Run2()
		{
			var indata = LoadData("indata.txt");
			return Solve2(indata);
		}

		static List<List<string>> LoadData(string filename)
		{
			var lines = File.ReadAllLines(filename);
			return lines.Select(l => l.Split(' ').ToList()).ToList();
		}

		static int Solve(List<List<string>> indata)
		{
			return indata.Count(pass => IsValid(pass));
		}

		static int Solve2(List<List<string>> indata)
		{
			return indata.Count(pass => IsValid2(pass));
		}

		static bool IsValid(List<string> pass)
		{
			var used = new List<string>();
			foreach (var word in pass)
			{
				if (used.Contains(word))
					return false;

				used.Add(word);
			}

			return true;
		}

		static bool IsValid2(List<string> pass)
		{
			var used = new List<string>();

			for (int i = 0; i < pass.Count; i++)
			{
				for (int j = i + 1; j < pass.Count; j++)
				{
					if (IsAnagrammable(pass[i], pass[j]))
					{
						return false;
					}
				}
			}

			return true;
		}

		static bool IsAnagrammable(string a, string b)//its a word
		{
			var al = a.ToList();
			al.Sort();
			var bl = b.ToList();
			bl.Sort();

			return al.SequenceEqual(bl);
		}
	}
}
