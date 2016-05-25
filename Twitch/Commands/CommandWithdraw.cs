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
	class CommandWithdraw
	{
		public static void ChatResponse(Bankbot sender, ChatMessage m)
		{
			if (m.Message.StartsWith("!pajbank withdraw"))
			{
				//actual code
				long amount;
				if (long.TryParse(m.Message.Split(' ')[2], out amount))
				{
					User u = User.GetUserFromDataBase(m.Username);
					if (amount > 0)
					{
						if (u.Balance < amount)
						{
							sender.ChatConnection.SendChatMessage(m.Username + ", you can't withdraw more points than you have 4Head");
						}
						else
						{
							sender.ChatConnection.SendChatMessage("!givepoints " + m.Username + " " + amount);
						}
					}
					else
					{
						sender.ChatConnection.SendChatMessage(m.Username + ", no scamming OMGScoots");
					}
				}
				else
				{
					sender.ChatConnection.SendChatMessage(m.Username + ", invalid amount LUL");
				}

				////testing code
				//long amount;
				//if (long.TryParse(m.Message.Split(' ')[2], out amount))
				//{
				//	if (amount > 0)
				//	{
				//		User u = User.GetUserFromDataBase(m.Username);
				//		if (u.Balance < amount)
				//		{
				//			sender.ChatConnection.SendChatMessage(m.Username + ", you can't withdraw more points than you have 4Head");
				//		}
				//		else
				//		{
				//			u.Balance -= amount;
				//			u.Save();
				//			sender.ChatConnection.SendChatMessage(m.Username + ", successfully withdrawn " + amount + " points.");
				//		}
				//	}
				//	else
				//	{
				//		sender.ChatConnection.SendChatMessage(m.Username + ", no scamming OMGScoots");
				//	}
				//}
				//else
				//{
				//	sender.ChatConnection.SendChatMessage(m.Username + ", invalid amount LUL");
				//}
			}
		}

		public static void WhisperResponse(Bankbot sender, WhisperMessage m)
		{
			//does the whisper come from pajbot?
			if (m.Username == GlobalVars.Settings["masterchannelbot"] && m.Message.StartsWith("Successfully gave away "))
			{
				List<string> splitwhispermessage = new List<string>();
				splitwhispermessage = m.Message.Split(' ').ToList();
				if (splitwhispermessage.Count == 7)
				{
					string withdrawSender = splitwhispermessage[6];
					string amountString = splitwhispermessage[3];
					int amount;
					if (int.TryParse(amountString, out amount))
					{
						User u = User.GetUserFromDataBase(withdrawSender);
						u.Balance -= amount;
						u.Save();
						sender.ChatConnection.SendChatMessage(withdrawSender + ", successfully withdrawn " + amount + " points.");
					}
				}
			}
		}
	}
}
