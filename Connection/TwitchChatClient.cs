using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Pajbank.Twitch.Messages;

namespace Pajbank.Connection
{
	public class TwitchChatClient : IRCClient
	{
		public delegate void MessageReceive(TwitchChatClient sender, ChatMessage chatMessage);
		public event MessageReceive OnMessageReceive;
		private string channel;
		private Thread ChatListenerThread;

		public TwitchChatClient(string ip, int port, string username, string password, string channelname)
			: base(ip, port, username, password)
		{
			this.sendIrcMessage(@"CAP REQ :twitch.tv/tags");
			this.channel = channelname;
			this.joinRoom(this.channel);
			this.ChatListenerThread = new Thread(new ThreadStart(this.chatWorker));
			this.ChatListenerThread.IsBackground = true;
			this.ChatListenerThread.Start();
		}

		/// <summary>
		/// Reconnects to the host
		/// </summary>
		public override void Reconnect()
		{
			this.Init();
			this.joinRoom(this.channel);
		}

		/// <summary>
		/// Joins a Room on the Irc
		/// </summary>
		/// <param name="channel">IRC Roomname (NO #!)</param>
		private void joinRoom(string channel)
		{
			this.channel = channel;
			this.sendIrcMessage("JOIN #" + this.channel);
		}

		/// <summary>
		/// Closes the IRC connection to the server
		/// </summary>
		public override void closeConnection()
		{
			if (!string.IsNullOrEmpty(this.channel))
			{
				this.sendIrcMessage("PART #" + this.channel);
			}
			this.inputStream.Dispose();
			this.outputStream.Dispose();
			this.tcpClient.Close();
		}

		#region sending methods
		/// <summary>
		/// Sends Twitchchat message
		/// </summary>
		/// <param name="message">Chatmessage</param>
		public void SendChatMessage(string message)
		{
			this.sendIrcMessage(@":" + username + "!" + username + "@" + username + "tmi.twitch.tv PRIVMSG #" + channel + " :" + message);
		}

		/// <summary>
		/// Sends a twitch message to the connected channel
		/// </summary>
		/// <param name="msg">TwitchMessage to send</param>
		/// <param name="delay">Delay in seconds</param>
		public async void SendChatMessage(string msg, uint delay = 0)
		{
			//check if the bot already sent a mesage in the last 1.45 seconds
			TimeSpan span = DateTime.Now - this.lastMessageTime;
			while (span.TotalMilliseconds < 1450)
			{
				await Task.Delay(100);
				span = DateTime.Now - this.lastMessageTime;
			}

			//wait if a delay is specified
			await Task.Delay((int)delay * 1000);

			//set the time of the last sent chat message to current time
			this.lastMessageTime = DateTime.Now;
			this.SendChatMessage(msg);
		}
		#endregion

		#region receiving methods
		/// <summary>
		/// Returns the last twitch chat message received from the IRC server
		/// </summary>
		/// <returns>Chat message</returns>
		public ChatMessage LastChatMessage()
		{
			string ircline = this.LastIrcMessage();
			if (!ircline.StartsWith(":tmi.twitch.tv") && ircline.Contains("PRIVMSG"))
			{
				return new ChatMessage(ircline);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region threadmethods
		private void chatWorker()
		{
			while (true)
			{
				ChatMessage lastchatmessage = this.LastChatMessage();

				if (lastchatmessage != null)
				{
					OnMessageReceive(this, lastchatmessage);
				}
			}
		}
		#endregion
	}
}
