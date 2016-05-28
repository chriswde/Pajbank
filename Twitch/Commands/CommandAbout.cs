using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pajbank.Connection;
using Pajbank.Twitch.Messages;

namespace Pajbank.Twitch.Commands
{
	class CommandAbout : Command
	{
		public CommandAbout(Bankbot bot, uint cooldown)
			: base(cooldown)
		{
			bot.OnMessageReceive += this.chatResponse;
		}

		private void chatResponse(Bankbot sender, ChatMessage m)
		{
			if (!this.isOnCooldown())
			{
				List<string> args = this.GetArgs(m.Message);
				if (args.Count >= 1)
				{
					if (args[0] == "about")
					{
						sender.ChatConnection.SendChatMessage("/me Pajbank is an automated bankbot coded in c#. The source code is available on github: https://github.com/chriswde/Pajbank");
						this.lastExecution = DateTime.Now;
					}
				}
			}
		}
	}
}
