using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pajbank.Connection;
using Pajbank.Twitch.Messages;

namespace Pajbank
{
	public class Bankbot
	{
		public TwitchChatClient ChatConnection { get; private set; }
		public TwitchWhisperClient WhisperConnection { get; private set; }

		//events
		public delegate void MessageReceive(Bankbot sender, ChatMessage chatMessage);
		public event MessageReceive OnMessageReceive;
		public delegate void WhisperReceive(Bankbot sender, WhisperMessage Message);
		public event WhisperReceive OnWhispereReceive;

		public Bankbot(string ip, int port, string username, string password, string channelname)
		{
			this.ChatConnection = new TwitchChatClient(ip, port, username, password, channelname);
			this.WhisperConnection = new TwitchWhisperClient(ip, port, username, password);

			//add triggers
			this.ChatConnection.OnMessageReceive += this.chatMessageTrigger;
			this.WhisperConnection.OnWhispereReceive += this.whisperMessageTrigger;

			//add commandhandlers (chat)
			this.OnMessageReceive += this.printChatMessage;
			this.OnMessageReceive += Twitch.Commands.CommanTestman.ChatResponse;
			this.OnMessageReceive += Twitch.Commands.CommandBalance.ChatResponse;
			this.OnMessageReceive += Twitch.Commands.CommandDeposit.ChatResponse;
			this.OnMessageReceive += Twitch.Commands.CommandWithdraw.ChatResponse;

			//add commandhandlers (whisper)
			this.OnWhispereReceive += this.printWhisperMessage;
			this.OnWhispereReceive += Twitch.Commands.CommandDeposit.WhisperResponse;
			this.OnWhispereReceive += Twitch.Commands.CommanTestman.WhisperResponse;
			this.OnWhispereReceive += Twitch.Commands.CommandWithdraw.WhisperResponse;
		}

		#region event triggers
		private void chatMessageTrigger(TwitchChatClient sender, ChatMessage m)
		{
			this.OnMessageReceive(this, m);
		}
		
		private void whisperMessageTrigger(TwitchWhisperClient sender, WhisperMessage m)
		{
			this.OnWhispereReceive(this, m);
		}
		#endregion

		private void printChatMessage(Bankbot sender, ChatMessage m)
		{
			Console.WriteLine("{0}: {1}", m.Username, m.Message);
		}

		private void printWhisperMessage(Bankbot sender, WhisperMessage m)
		{
			Console.WriteLine("|w|{0}: {1}", m.Username, m.Message);
		}
	}
}
