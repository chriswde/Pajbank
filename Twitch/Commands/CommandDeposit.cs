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
	class CommandDeposit : Command
	{
		public CommandDeposit(Bankbot bot, uint cooldown)
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
				if (args.Count >= 1)
				{
					if (args[0] == "deposit")
					{
						sender.ChatConnection.SendChatMessage(m.Username + ", please use the !givepoints command to deposit points.");
						this.lastExecution = DateTime.Now;
					}
				}
			}
		}

		private void whisperResponse(Bankbot sender, WhisperMessage m)
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
