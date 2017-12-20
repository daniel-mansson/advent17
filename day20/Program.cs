using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day20
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 20;
		static string s_example =
@"p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Particle
		{
			public long x, y, z, vx, vy, vz, ax, ay, az;

			public Particle(string raw)
			{
				var l = raw
					.Replace("p=<", "")
					.Replace("v=<", "")
					.Replace("a=<", "")
					.Replace(" ", "")
					.Replace(">", "")
					.Replace("\r", "")
					.Split(',')
					.Select(i => int.Parse(i))
					.ToList();

				x = l[0];
				y = l[1];
				z = l[2];
				vx = l[3];
				vy = l[4];
				vz = l[5];
				ax = l[6];
				ay = l[7];
				az = l[8];
			}

			public long GetDistance()
			{
				return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
			}

			public void Step()
			{
				vx += ax;
				vy += ay;
				vz += az;
				x += vx;
				y += vy;
				z += vz;
			}
		}

		static List<Particle> Transform(string raw)
		{
			return raw
				.Split('\n')
				.Select(l => new Particle(l))
				.ToList();
		}

		static int Solve(string raw)
		{
			var input = Transform(raw);

			int minI = -1;

			for (int i = 0; i < 20; i++)
			{
				minI = StepAndGetClosest(input, 1000);
				Console.Write(minI + " ");
			}
			Console.WriteLine();

			return minI;
		}

		static int StepAndGetClosest(List<Particle> input, int steps)
		{
			foreach (var p in input)
			{
				for (int i = 0; i < steps; i++)
				{
					p.Step();
				}
			}

			long minD = long.MaxValue;
			int minI = -1;
			for (int i = 0; i < input.Count; i++)
			{
				long d = input[i].GetDistance();
				if (d < minD)
				{
					minI = i;
					minD = d;
				}
			}

			return minI;
		}
		static int StepAndKill(List<Particle> input, int steps)
		{
			for (int i = 0; i < steps; i++)
			{
				foreach (var p in input)
				{
					p.Step();
				}

				var lookup = new Dictionary<long, int>();
				var collisions = new List<int>();
				for (int j = 0; j < input.Count; j++)
				{
					var p = input[j];
					int other;
					long hash = p.z * 1000000 * 1000000 + p.y * 1000000 + p.z;
					if (lookup.TryGetValue(hash, out other))
					{
						if (!collisions.Contains(j))
							collisions.Add(j);
						if (!collisions.Contains(other))
							collisions.Add(other);
					}
					else
					{
						lookup.Add(hash, j);
					}
				}

				collisions.Sort();

				for (int j = collisions.Count - 1; j >= 0; j--)
				{
					input.RemoveAt(collisions[j]); 
				}
			}

			return input.Count;
		}

		static int Solve2(string raw)
		{
			var input = Transform(raw);

			int minI = -1;

			for (int i = 0; i < 20; i++)
			{
				minI = StepAndKill(input, 1000);
				Console.Write(minI + " ");
			}
			Console.WriteLine();

			return minI;
		}
	}
}