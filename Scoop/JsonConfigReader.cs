using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scoop
{
	internal class JsonConfigReader<T> : IConfigReader<T> where T : class
	{
		public T Read(string configFile)
		{
			Console.WriteLine("reading..");
			return Json.Deserialize<T>(File.ReadAllText(configFile));
		}
	}
}
