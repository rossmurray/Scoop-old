using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoop
{
	public class ConfigProvider<T> where T : class
	{
		private ConfigFileWatcher watcher;

		public ConfigProvider(ConfigFileWatcher watcher)
		{
			this.watcher = watcher;
		}
	}
}
