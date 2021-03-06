﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pajbank.Connection;
using Pajbank.Twitch;
using Pajbank.Twitch.Messages;

namespace Pajbank.Twitch.Commands
{
	class CommandBalance : Command
	{
		public CommandBalance(Bankbot bot, uint cooldown)
			: base(cooldown)
		{
			bot.OnMessageReceive += this.chatResponse;
		}

		private void chatResponse(Bankbot sender, ChatMessage m)
		{
			if (!this.isOnCooldown())
			{
				List<string> args = GetArgs(m.Message);
				if (args.Count >= 1)
				{
					if (args[0] == "balance")
					{
						User u = User.GetUserFromDataBase(m.Username);
						sender.ChatConnection.SendChatMessage(m.Username + ", " + u.Balance + " points.");
						this.lastExecution = DateTime.Now;
					}
				}
			}
		}
	}
}
