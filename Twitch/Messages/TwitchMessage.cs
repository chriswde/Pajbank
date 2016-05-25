using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pajbank.Twitch.Messages
{
	public class TwitchMessage
	{
		public string Username { get; protected set; }
		public string Message { get; protected set; }
		public DateTime Timestamp { get; protected set; }
		private bool isMod = false;

		public bool IsMod
		{
			get { return this.isMod; }
			protected set { this.isMod = value; }
		}

		#region constructors
		public TwitchMessage() {}
		
		public TwitchMessage(string twitchusername, string message)
		{
			this.Username = twitchusername;
			this.Message = message;
			this.Timestamp = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
		}

		public TwitchMessage(string twitchusername, string message, DateTime timestamp)
		{
			this.Username = twitchusername;
			this.Message = message;
			this.Timestamp = TimeZoneInfo.ConvertTimeToUtc(timestamp);
		}
		#endregion

		public override string ToString()
		{
			return "[" + this.Timestamp.ToString("MM/dd/yyy HH:mm:ss") + " UTC]" + this.Username + ": " + this.Message;
		}
	}
}
