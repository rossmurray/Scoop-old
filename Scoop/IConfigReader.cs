using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scoop
{
	public interface IConfigReader<T>
	{
		T Read(string configFile);
	}
}
