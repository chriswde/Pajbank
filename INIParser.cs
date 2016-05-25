using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pajbank
{
	public class INIParser
	{
		private string INIFile;

		//public INIParser(string filepath)
		//{
		//	if (File.Exists(filepath))
		//	{
		//		this.INIFile = filepath;
		//	}
		//	else
		//	{
		//		Console.WriteLine(filepath + " not found.");
		//		return;
		//	}
		//}

		public static Dictionary<string, string> ReadINI(string filename)
		{
			if (File.Exists(filename))
			{
				Dictionary<string, string> content = new Dictionary<string, string>();

				foreach (string line in File.ReadAllLines(filename))
				{
					//skip if line is comment
					if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
					{
						continue;
					}
					string trimmedline = line.Replace(" ", string.Empty);
					string[] lineparts = trimmedline.Split('=');
					content.Add(lineparts[0], lineparts[1]);
				}

				return content;
			}
			else
			{
				throw new IOException(filename + " does not exist.");
			}
		}
	}
}
