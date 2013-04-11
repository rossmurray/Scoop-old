using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

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

		private void UpdateConfig()
		{
			lock (this.updateLock)
			{
				this.config = reader.Read(this.configFile.FullName);
			}
		}

		private void ConfigChangedHandler()
		{
			Thread.Sleep(200); //however long you think it takes for whatever changed the file to release it.
			UpdateConfig();
			var handler = ConfigChanged;
			if (handler != null)
			{
				var args = new ConfigEventArgs<T>(this.config);
				handler(this, args);
			}
		}

		public void Dispose()
		{
			this.watcher.Dispose();
		}
	}
}
