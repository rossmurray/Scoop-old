using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Scoop
{
	internal static class Json
	{
		public static T Deserialize<T>(string s)
		{
			return JsonConvert.DeserializeObject<T>(s);
		}
	}
}
