using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scoop;
using Newtonsoft.Json;

namespace TempTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var p = @"D:\temp\foo.config.json";
			using (var m = new ConfigManager<FooConfig>(p))
			{
				m.GetConfig().Dump();
				m.ConfigChanged += (s, e) => { e.Config.Dump(); };
				Console.ReadKey();
			}
		}
	}

	public static class Ext
	{
		public static void Dump(this object o)
		{
			Console.WriteLine(JsonConvert.SerializeObject(o, Formatting.Indented));
		}
	}

	public class FooConfig
	{
		public int Count { get; set; }
		public string Path { get; set; }
		public float[] Reads { get; set; }
	}
}
