using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scoop
{
	internal class ConfigFileWatcher : IDisposable
	{
		private FileSystemWatcher fileWatcher;
		private bool running;
		private object startStopLock;
		private Action notifyHandle;
		private bool disposed;
		private DateTime configLastChanged;
		private DateTime lastEventTriggered;

		public ConfigFileWatcher()
		{
			this.fileWatcher = new FileSystemWatcher();
			this.startStopLock = new object();
		}

		public void Start(FileInfo file, Action notifyHandle)
		{
			lock(startStopLock)
			{
				if (running) { return; }
				if (disposed) { throw new ObjectDisposedException(typeof(ConfigFileWatcher).Name); }
				running = true;
				this.notifyHandle = notifyHandle;
				if (file.Exists)
				{
					this.configLastChanged = File.GetLastWriteTimeUtc(file.FullName);
				}
				fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
				fileWatcher.Path = file.DirectoryName;
				fileWatcher.Filter = file.Name;
				fileWatcher.Changed += FileChanged;
				fileWatcher.EnableRaisingEvents = true;
			}
		}

		public void Stop()
		{
			lock (startStopLock)
			{
				if (!running) { return; }
				if (disposed) { throw new ObjectDisposedException(typeof(ConfigFileWatcher).Name); }
				running = false;
				notifyHandle = null;
			}
		}

		private void FileChanged(object source, FileSystemEventArgs e)
		{
			lock (startStopLock)
			{
				if (!running || disposed || IsDoubleFire()) { return; }
				if (File.Exists(e.FullPath))
				{
					var changed = File.GetLastWriteTimeUtc(e.FullPath);
					if (changed > this.configLastChanged)
					{
						this.configLastChanged = changed;
						this.notifyHandle();
					}
				}
			}
		}

		private bool IsDoubleFire()
		{
			var result = true;
			var now = DateTime.UtcNow;
			if (now - lastEventTriggered > TimeSpan.FromMilliseconds(500.0))
			{
				result = false;
			}
			lastEventTriggered = now;
			return result;
		}

		public void Dispose()
		{
			lock (startStopLock)
			{
				disposed = true;
				notifyHandle = null;
				fileWatcher.EnableRaisingEvents = false;
				fileWatcher.Dispose();
			}
		}
	}
}
