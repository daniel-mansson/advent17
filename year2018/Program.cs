using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace year2018
{
	class Program
	{
		static void Main(string[] args)
		{
			var types = Assembly.GetAssembly(typeof(Program)).GetTypes();
			var days = types.Where(t => t.Name.StartsWith("Day")).ToList();

			int dayToRun = -1;

			if (dayToRun < 0)
			{
				dayToRun = days.Select(d => int.Parse(d.Name.Replace("Day", ""))).Max();
			}

			var type = days.FirstOrDefault(d => d.Name == "Day" + dayToRun);
			Console.WriteLine("Running day " + dayToRun);

			if (type == null)
			{
				Console.WriteLine("Could not find day " + dayToRun);
			}
			else
			{
				type.GetMethod("Run", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Invoke(null, new object[] { 2018, dayToRun });
			}
		}
	}
}
