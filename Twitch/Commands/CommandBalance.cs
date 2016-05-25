using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pajbank.Connection;
using Pajbank.Twitch;
using Pajbank.Twitch.Messages;

namespace Pajbank.Twitch.Commands
{
	class CommandBalance
	{
		public static void ChatResponse(Bankbot sender, ChatMessage m)
		{
			if (m.Message.StartsWith("!pajbank balance"))
			{
				User u = User.GetUserFromDataBase(m.Username);
				sender.ChatConnection.SendChatMessage(u.Balance + "");
			}
		}
	}
}
