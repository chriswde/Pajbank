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
	class CommandDebug : Command
	{
		public CommandDebug(Bankbot bot, uint cooldown)
			: base(cooldown)
		{
			bot.OnMessageReceive += this.chatResponse;
		}

		private void chatResponse(Bankbot sender, ChatMessage m)
		{
			if (!this.isOnCooldown())
			{
				List<string> args = GetArgs(m.Message);
				if (args.Count >= 2)
				{
					if (args[0] == "debug" && m.IsMod)
					{
						User u = User.GetUserFromDataBase(args[1]);
						sender.WhisperConnection.SendWhisper(m.Username, string.Format("Username: {0}; UserId: {1}; Balance: {2};", u.Name, u.Id, u.Balance));
						this.lastExecution = DateTime.Now;
					}
				}
			}
		}
	}
}
