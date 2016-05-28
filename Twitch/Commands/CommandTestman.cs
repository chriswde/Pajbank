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
	class CommandTestman : Command
	{
		public CommandTestman(Bankbot bot, uint cooldown)
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
					if (args[0] == "testman" && m.IsMod)
					{
						sender.ChatConnection.SendChatMessage("123 KKona");
						sender.WhisperConnection.SendWhisper(m.Username, "123 KKona");
						this.lastExecution = DateTime.Now;
					}
				}
			}
		}
	}
}
