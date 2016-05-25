using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pajbank.Twitch.Messages
{
	public class ChatMessage : TwitchMessage
	{
		public string Channel { get; protected set; }

		#region constructors

		public ChatMessage(string twitchusername, string message, string channel)
			: base(twitchusername, message)
		{
			this.Channel = channel;
		}

		public ChatMessage(string ircmessage)
			: base()
		{
			//get username
			string tmp = ircmessage.Substring(ircmessage.IndexOf("display-name="));
			//selects the string between '=' and ';'
			this.Username = tmp.Split('=', ';')[1].ToLower();

			//get mod status
			tmp = ircmessage.Substring(ircmessage.IndexOf("mod="));
			//selects the string between '=' and ';'
			if (tmp.Split('=', ';')[1] == "1")
			{
				this.IsMod = true;
			}
			//add mod status if broadcaster as well
			try
			{
				tmp = ircmessage.Substring(ircmessage.IndexOf("badges=broadcaster"));
				if (tmp.Split('/', ';')[1] == "1")
				{
					this.IsMod = true;
				}
			} catch(ArgumentOutOfRangeException) {}

			//get channelname
			tmp = ircmessage.Substring(ircmessage.IndexOf("PRIVMSG #"));
			//selects the string between '=' and ';'
			tmp = tmp.Substring("PRIVMSG ".Length);
			this.Channel = tmp.Split('#', ' ')[1].ToLower();

			//get message
			this.Message = tmp.Substring(("#" + this.Channel + " :").Length);

			//old code
			//string tmp = ircmessage.Substring(ircmessage.IndexOf(':') + 1);
			//tmp = tmp.Substring(tmp.IndexOf(':') + 1);
			//this.Message = tmp;
			this.Timestamp = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
		}

		public ChatMessage(string twitchusername, string message, string channel, DateTime timestamp)
			: base(twitchusername, message, timestamp)
		{
			this.Channel = channel;
		}

		#endregion

		public override string ToString()
		{
			return "[" + this.Timestamp.ToString("MM/dd/yyy HH:mm:ss") + " UTC]" + this.Channel + "|" + this.Username + ": " + this.Message;
		}


		public string ToStringNoChannel()
		{
			return "[" + this.Timestamp.ToString("MM/dd/yyy HH:mm:ss") + " UTC]" + this.Username + ": " + this.Message;
		}
	}
}
