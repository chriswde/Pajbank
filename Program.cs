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
				//Twitch.User nsk = Twitch.User.GetUserFromDataBase("nightnacht");
				//Twitch.User nsk2 = Twitch.User.GetUserFromDataBase(3);
				//nsk2.Balance *= 2;
				//nsk2.Save();
				//Twitch.User p = new Twitch.User(400, "pajlada", 1337, true);
				//p.Save();
			}
			else
			{
				Console.WriteLine("Failed to connect to database.");
			}

			//add event handlers
			pajbank.ChatConnection.OnMessageReceive += printChatMessage;
			pajbank.WhisperConnection.OnWhispereReceive += printWhisperMessage;

			//mainloop
			while (true)
			{
				Console.WriteLine(Console.ReadLine());
			}
		}

		static void printChatMessage(Pajbank.Connection.TwitchChatClient sender, Pajbank.Twitch.Messages.ChatMessage m)
		{
			Console.WriteLine("{0}: {1}", m.Username, m.Message);
			File.WriteAllText(@"test.txt", m.Message);
		}

		static void printWhisperMessage(Pajbank.Connection.TwitchWhisperClient sender, Pajbank.Twitch.Messages.WhisperMessage m)
		{
			Console.WriteLine("|w|{0}: {1}", m.Username, m.Message);
		}
	}
}
