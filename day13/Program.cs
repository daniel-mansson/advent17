using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day13
{
	class Program
	{
		static int s_year = 2017;
		static int s_day = 13;
		static string s_example =
@"0: 3
1: 2
4: 4
6: 4";

		static void Main(string[] args)
		{
			Console.WriteLine("Example: " + Solve(s_example));
			Console.WriteLine("Real: " + Solve(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.WriteLine("Example 2: " + Solve2(s_example));
			Console.WriteLine("Real 2: " + Solve2(ProblemInput.FetchBlocking(s_year, s_day)));

			Console.ReadKey();
		}

		class Firewall
		{
			public class Layer
			{
				public int size;
				public int scanner;
				public int delta = 1;
			}

			public Dictionary<int, Layer> m_layers = new Dictionary<int, Layer>();
			List<Layer> m_l = new List<Layer>();
			int[] m_newL = new int[1000];

			public void AddLayer(int layer, int size)
			{
				var l = new Layer()
				{
					scanner = 0,
					size = size
				};
				m_layers.Add(layer, l);
				m_l.Add(l);
				m_newL[layer] = size;
			}

			public void Step()
			{
				foreach (var layer in m_l)
				{
					if (layer.size <= 1)
						continue;

					if (layer.delta > 0)
					{
						layer.scanner++;
						if (layer.scanner == layer.size - 1)
							layer.delta = -1;
					}
					else
					{
						layer.scanner--;
						if (layer.scanner == 0)
							layer.delta = 1;
					}
				}
			}

			public int GetSeverityAt(int layerId)
			{
				Layer layer;
				if (m_layers.TryGetValue(layerId, out layer))
				{
					if (layer.scanner == 0)
						return layer.size * layerId;
				}

				return 0;
			}

			public bool GotCaught(int layerId)
			{
				Layer layer;
				if (m_layers.TryGetValue(layerId, out layer))
				{
					if (layer.scanner == 0)
						return true;
				}

				return false;
			}

			public bool GotCaughtImpl(int layerId, int it)
			{
				int size = m_newL[layerId];
				if (size <= 1)
					return size == 1;

				int mod = it % (size * 2 - 2);

				return mod == 0;
			}

			public void Reset()
			{
				foreach (var kvp in m_layers)
				{
					kvp.Value.scanner = 0;
					kvp.Value.delta = 1;
				}
			}
		}

		static Firewall Transform(string raw)
		{
			var values = raw
				.Replace(": ", ",")
				.Split('\n')
				.Select(s => s.Split(',').Select(s2 => int.Parse(s2)).ToList())
				.ToList();

			var firewall = new Firewall();

			foreach (var entry in values)
			{
				firewall.AddLayer(entry[0], entry[1]);
			}

			return firewall;
		}

		static int Solve(string raw)
		{
			var firewall = Transform(raw);

			int sum = 0;
			int end = firewall.m_layers.Keys.Max();

			for (int i = 0; i <= end; i++)
			{
				sum += firewall.GetSeverityAt(i);
				firewall.Step();
			}

			return sum;
		}

		static int Solve2(string raw)
		{
			var firewall = Transform(raw);
			int end = firewall.m_layers.Keys.Max();

			for (int delay = 0; delay < 100000000; delay++)
			{
				if (delay % 100000 == 0)
					Console.WriteLine(delay);

				bool caught = false;

				for (int i = 0; i <= end; i++)
				{
					caught = firewall.GotCaughtImpl(i, i + delay);
					if (caught)
						break;
				}

				if (!caught)
					return delay;
			}

			return -1;
		}
	}
}