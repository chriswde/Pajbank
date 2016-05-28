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
	class CommandWithdraw : Command
	{
		public CommandWithdraw(Bankbot bot, uint cooldown)
			: base(cooldown)
		{
			bot.OnMessageReceive += this.chatResponse;
			bot.OnWhispereReceive += this.whisperResponse;
		}

		private void chatResponse(Bankbot sender, ChatMessage m)
		{
			if (!this.isOnCooldown())
			{
				List<string> args = GetArgs(m.Message);
				if (args.Count >= 2)
				{
					if (args[0] == "withdraw")
					{
						long amount;
						if (long.TryParse(args[1], out amount))
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
					}
					this.lastExecution = DateTime.Now;
				}
			}
		}

		private void whisperResponse(Bankbot sender, WhisperMessage m)
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
					long amount;
					if (long.TryParse(amountString, out amount))
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
