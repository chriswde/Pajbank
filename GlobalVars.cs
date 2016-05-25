using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pajbank
{
	public static class GlobalVars
	{
		public static Dictionary<string, string> Settings = INIParser.ReadINI(@"config.ini");
	}
}
