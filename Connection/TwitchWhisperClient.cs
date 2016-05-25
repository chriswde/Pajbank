using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Pajbank.Twitch.Messages;

namespace Pajbank.Connection
{
	public class TwitchWhisperClient : IRCClient
	{
		public delegate void WhisperReceive(TwitchWhisperClient sender, WhisperMessage Message);
		public event WhisperReceive OnWhispereReceive;
		private Thread WhisperListenerThread;

		public TwitchWhisperClient(string ip, int port, string username, string password)
			: base(ip, port, username, password)
		{
			this.sendIrcMessage(@"CAP REQ :twitch.tv/commands");
			this.WhisperListenerThread = new Thread(new ThreadStart(this.whisperWorker));
			this.WhisperListenerThread.IsBackground = true;
			this.WhisperListenerThread.Start();
		}

		/// <summary>
		/// Returns the last whisper message received from the IRC server
		/// </summary>
		/// <returns>Whisper message</returns>
		private WhisperMessage LastWhisperMessage()
		{
			string ircline = this.LastIrcMessage();
			if (!ircline.StartsWith(":tmi.twitch.tv") && ircline.StartsWith(":") && ircline.Contains(" WHISPER "))
			{
				return new WhisperMessage(ircline);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Sends a whisper to a user on twitch
		/// </summary>
		/// <param name="receivertwitchusername">Twitch username of the receiver</param>
		/// <param name="message">Whispermessage</param>
		public void SendWhisper(string receivertwitchusername, string message)
		{
			this.sendIrcMessage(@"PRIVMSG #jtv :/w " + receivertwitchusername + " " + message);
		}

		#region thread methods
		private void whisperWorker()
		{
			while (true)
			{
				WhisperMessage lastwhispermessage = this.LastWhisperMessage();

				if (lastwhispermessage != null)
				{
					OnWhispereReceive(this, lastwhispermessage);
				}
			}
		}
		#endregion
	}
}
