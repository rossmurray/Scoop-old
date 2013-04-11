using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoop
{
	public class ConfigEventArgs<T> : EventArgs
	{
		public ConfigEventArgs(T config)
		{
			this.Config = config;
		}

		public T Config { get; set; }
	}
}
