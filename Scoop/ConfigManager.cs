using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scoop
{
	public class ConfigManager<T> : IDisposable where T : class
	{
		public event EventHandler<ConfigEventArgs<T>> ConfigChanged;
		private ConfigFileWatcher watcher;
		private IConfigReader<T> reader;
		private FileInfo configFile;
		private T config;
		private object updateLock = new object();

		public ConfigManager(string configFile)
		{
			this.watcher = new ConfigFileWatcher();
			this.configFile = new FileInfo(configFile);
			this.watcher.Start(this.configFile, ConfigChangedHandler);
			this.reader = new JsonConfigReader<T>();
		}

		internal ConfigManager(FileInfo configFile, ConfigFileWatcher watcher, IConfigReader<T> reader)
		{
			this.watcher = watcher;
			this.reader = reader;
			this.watcher.Start(configFile, ConfigChangedHandler);
		}

		public T GetConfig()
		{
			if (config == null) { UpdateConfig(); }
			return this.config;
		}

		private bool UpdateConfig()
		{
			lock (this.updateLock)
			{
				var newConfig = reader.Read(this.configFile.FullName);
				if (newConfig != null)
				{
					this.config = newConfig;
					return true;
				}
				return false;
			}
		}

		private void ConfigChangedHandler()
		{
			if (UpdateConfig())
			{
				var handler = ConfigChanged;
				if (handler != null)
				{
					var args = new ConfigEventArgs<T>(this.config);
					handler(this, args);
				}
			}
		}

		public void Dispose()
		{
			this.watcher.Dispose();
		}
	}
}
