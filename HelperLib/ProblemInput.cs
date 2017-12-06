using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static class ProblemInput
{
	static string s_root = "../../../Temp/";

	public static async Task<string> Fetch(int year, int day)
	{
		PrepareRootDir();

		Console.WriteLine($"Fetching input for year {year}, day {day}.");

		var cached = LoadFromCache(year, day);
		if (!string.IsNullOrEmpty(cached))
		{
			Console.WriteLine($"Found cached.");
			return cached;
		}

		for (int i = 0; i < 2; i++)
		{
			var request = WebRequest.Create(new Uri($"http://adventofcode.com/{year}/day/{day}/input")) as HttpWebRequest;
			request.Method = "GET";
			request.Headers.Add("Cookie", "session=" + GetSessionToken());

			try
			{
				HttpWebResponse responseObject = (HttpWebResponse)await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);

				if (responseObject.StatusCode == HttpStatusCode.OK)
				{
					var responseStream = responseObject.GetResponseStream();
					var sr = new StreamReader(responseStream);

					string received = await sr.ReadToEndAsync();
					received = received.TrimEnd('\n');

					Console.WriteLine($"Received.");
					SaveToCache(year, day, received);

					return received;
				}
			}
			catch (Exception)
			{
			}

			Console.WriteLine("Something went wrong! Maybe your session token has expired!");
			Console.Write("Enter a new one: ");
			var sessionToken = Console.ReadLine();
			File.WriteAllText(s_root + "session.txt", sessionToken);
		}

		return "";
	}

	public static string FetchBlocking(int year, int day)
	{
		var task = Fetch(year, day);
		task.Wait();
		return task.Result;
	}

	static string GetSessionToken()
	{
		try
		{
			return File.ReadAllText(s_root + "session.txt");
		}
		catch (Exception)
		{
			return "";
		}
	}

	static string LoadFromCache(int year, int day)
	{
		try
		{
			return File.ReadAllText(GetInputFilename(year, day));
		}
		catch (Exception)
		{
			return null;
		}
	}

	static void SaveToCache(int year, int day, string data)
	{
		File.WriteAllText(GetInputFilename(year, day), data);
	}

	static string GetInputFilename(int year, int day)
	{
		return s_root + year + "-" + day + ".txt";
	}

	static void PrepareRootDir()
	{
		if (!Directory.Exists(s_root))
		{
			Directory.CreateDirectory(s_root);
		}
	}
}
