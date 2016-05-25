using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pajbank.Twitch.Messages
{
	public class WhisperMessage : TwitchMessage
	{
		#region constructors

		public WhisperMessage(string twitchusername, string message)
			: base(twitchusername, message)
		{ }

		public WhisperMessage(string ircmessage)
			: base()
		{
			this.Username = ircmessage.Split(':', '!')[1].ToLower();
			string tmp = ircmessage.Substring(ircmessage.IndexOf(':') + 1);
			tmp = tmp.Substring(tmp.IndexOf(':') + 1);
			this.Message = tmp;
			this.Timestamp = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
		}

		public WhisperMessage(string twitchusername, string message, DateTime timestamp)
			: base(twitchusername, message, timestamp)
		{ }

		#endregion
	}
}
