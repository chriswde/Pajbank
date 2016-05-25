using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Pajbank
{
	class Program
	{
		static void Main(string[] args)
		{	
			Bankbot pajbank = new Bankbot("irc.chat.twitch.tv", 6667, GlobalVars.Settings["botname"], GlobalVars.Settings["oauth"], GlobalVars.Settings["activechannel"]);
			GC.KeepAlive(pajbank);

			//Test Database connection
			Connection.DBConnection db = new Connection.DBConnection(GlobalVars.Settings["dbserver"], GlobalVars.Settings["dbname"], GlobalVars.Settings["dbuser"], GlobalVars.Settings["dbpassword"]);
			if (db.OpenConnection())
			{
				Console.WriteLine("Connection to database successful.");
			}
			else
			{
				Console.WriteLine("Failed to connect to database.");
			}

			//mainloop
			while (true)
			{
				Console.WriteLine(Console.ReadLine());
			}
		}
	}
}
