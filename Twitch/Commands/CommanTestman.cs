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
	class CommanTestman
	{
		public static void ChatResponse(Bankbot sender, ChatMessage m)
		{
			if (m.Message.StartsWith("!pajbank testman"))
			{
				sender.ChatConnection.SendChatMessage("123 KKona");
			}
		}

		public static void WhisperResponse(Bankbot sender, WhisperMessage m)
		{
			if (m.Message.StartsWith("!pajbank testman"))
			{
				sender.WhisperConnection.SendWhisper(m.Username, "123 KKona");
			}
		}
	}
}
