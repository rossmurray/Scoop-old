using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Scoop
{
	internal class JsonConfigReader<T> : IConfigReader<T> where T : class
	{
		public T Read(string configFile)
		{
			var json = File.ReadAllText(configFile);
			return Json.Deserialize<T>(json);
		}
	}
}
