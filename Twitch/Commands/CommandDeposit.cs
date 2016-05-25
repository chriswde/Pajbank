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
	class CommandDeposit
	{
		public static void ChatResponse(Bankbot sender, ChatMessage m)
		{
			if (m.Message.StartsWith("!pajbank deposit"))
			{
				//actual code
				sender.ChatConnection.SendChatMessage(m.Username + ", please use the !givepoints command to deposit points.");

				////Testing code
				//long amount;
				//if (long.TryParse(m.Message.Split(' ')[2], out amount))
				//{
				//	if (amount > 0)
				//	{
				//		User u = User.GetUserFromDataBase(m.Username);
				//		u.Balance += amount;
				//		u.Save();
				//		sender.ChatConnection.SendChatMessage(m.Username + " successfully deposited " + amount + " points.");
				//	}
				//}
			}
		}

		public static void WhisperResponse(Bankbot sender, WhisperMessage m)
		{
			//does the whisper come from pajbot?
			if (m.Username == GlobalVars.Settings["masterchannelbot"])
			{
				if (m.Message.Contains(" points! You should probably thank them "))
				{
					List<string> splitwhispermessage = new List<string>();
					splitwhispermessage = m.Message.Split(' ').ToList();
					if (splitwhispermessage.Count == 12)
					{
						string depositSender = splitwhispermessage[0];
						string amountString = splitwhispermessage[4];
						long amount;
						if (long.TryParse(amountString, out amount))
						{
							User u = User.GetUserFromDataBase(depositSender);
							u.Balance += amount;
							u.Save();
							sender.ChatConnection.SendChatMessage(depositSender + " successfully deposited " + amount + " points.");
						}
					}
				}
			}
		}
	}
}
