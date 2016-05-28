using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pajbank.Connection;
using Pajbank.Twitch.Messages;
using System.Timers;

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
			this.ChatConnection.Start();
			this.WhisperConnection = new TwitchWhisperClient(ip, port, username, password);

			//add triggers
			this.ChatConnection.OnMessageReceive += this.chatMessageTrigger;
			this.WhisperConnection.OnWhispereReceive += this.whisperMessageTrigger;

			//add commandhandlers (chat)
			this.ChatConnection.OnMessageReceive += this.printChatMessage;

			Twitch.Commands.CommandAbout commandAbout = new Twitch.Commands.CommandAbout(this, 10);
			Twitch.Commands.CommandBalance commandBalance = new Twitch.Commands.CommandBalance(this, 0);
			Twitch.Commands.CommandDebug commandDebug = new Twitch.Commands.CommandDebug(this, 0);
			Twitch.Commands.CommandDeposit commandDeposit = new Twitch.Commands.CommandDeposit(this, 10);
			Twitch.Commands.CommandWithdraw commadWithdraw = new Twitch.Commands.CommandWithdraw(this, 0);
			Twitch.Commands.CommandTestman commandTestman = new Twitch.Commands.CommandTestman(this, 0);

			//add commandhandlers (whisper)
			this.OnWhispereReceive += this.printWhisperMessage;
		}

		#region event triggers
		private void chatMessageTrigger(TwitchChatClient sender, ChatMessage m)
		{
			if (m.Message.StartsWith("!" + GlobalVars.Settings["commandprefix"]))
			{
				this.OnMessageReceive(this, m);
			}
		}
		
		private void whisperMessageTrigger(TwitchWhisperClient sender, WhisperMessage m)
		{
			this.OnWhispereReceive(this, m);
		}
		#endregion

		private void printChatMessage(object sender, ChatMessage m)
		{
			Console.WriteLine("{0}: {1}", m.Username, m.Message);
		}

		private void printWhisperMessage(Bankbot sender, WhisperMessage m)
		{
			Console.WriteLine("|w|{0}: {1}", m.Username, m.Message);
		}
	}
}
