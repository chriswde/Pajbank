using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Pajbank.Connection
{
	public abstract class IRCClient
	{
		protected string username, ip, passwd;
		protected int port;

		protected TcpClient tcpClient;
		protected StreamReader inputStream;
		protected StreamWriter outputStream;

		protected DateTime lastMessageTime;

		/// <summary>
		/// Initializes a new instance of the <see cref="IrcClient"/> class.
		/// </summary>
		/// <param name="ip">Host ip</param>
		/// <param name="port">Host port</param>
		/// <param name="username">User login name</param>
		/// <param name="password">User Login password</param>
		public IRCClient(string ip, int port, string username, string password)
		{
			//setup Properties
			this.username = username;
			this.ip = ip;
			this.port = port;
			this.passwd = password;
			
			//setup 
			this.Init();
		}

		protected void Init()
		{
			//Try to connect to given IP and Port
			try
			{
				this.tcpClient = new TcpClient(ip, port);
				this.inputStream = new StreamReader(this.tcpClient.GetStream());
				this.outputStream = new StreamWriter(this.tcpClient.GetStream());
				this.outputStream.NewLine = "\n\r";
				this.outputStream.AutoFlush = true;

				this.outputStream.WriteLine("PASS " + this.passwd);
				this.outputStream.WriteLine("NICK " + this.username);
				this.outputStream.WriteLine("USER " + this.username + " 8 * :" + this.username);
			}
			// Catch SocketException, write to log and exit
			catch (SocketException e)
			{
				Console.WriteLine("Could not connect to " + ip + ":" + port);
				Environment.Exit(128);
			}
		}

		/// <summary>
		/// Reconnects to the host
		/// </summary>
		public virtual void Reconnect()
		{
			this.Init();
		}

		/// <summary>
		/// Closes the IRC connection to the server
		/// </summary>
		public virtual void closeConnection()
		{
			this.inputStream.Dispose();
			this.outputStream.Dispose();
			this.tcpClient.Close();
		}

		/// <summary>
		/// Send a raw string to the irc server
		/// </summary>
		/// <param name="message">IRC TwitchMessage</param>
		public void sendIrcMessage(string message)
		{
			this.outputStream.WriteLine(message);
		}

		//#region sending methods
		///// <summary>
		///// Sends Twitchchat message
		///// </summary>
		///// <param name="message">Chatmessage</param>
		//public virtual void SendMessage(string message) { }

		///// <summary>
		///// Providing a visrtual method to override for whispers
		///// </summary>
		///// <param name="receivertwitchusername"></param>
		///// <param name="message"></param>
		//public virtual void SendMessage(string receivertwitchusername, string message) { }

		///// <summary>
		///// Sends a twitch message to the connected channel
		///// </summary>
		///// <param name="msg">TwitchMessage to send</param>
		///// <param name="delay">Delay in seconds</param>
		//public virtual async void SendMessage(string msg, uint delay = 0)
		//{
		//	//check if the bot already sent a mesage in the last 1.45 seconds
		//	TimeSpan span = DateTime.Now - this.lastMessageTime;
		//	while (span.TotalMilliseconds < 1450)
		//	{
		//		await Task.Delay(100);
		//		span = DateTime.Now - this.lastMessageTime;
		//	}

		//	//wait if a delay is specified
		//	await Task.Delay((int)delay * 1000);

		//	//set the time of the last sent chat message to current time
		//	this.lastMessageTime = DateTime.Now;
		//	this.SendMessage(msg);
		//}
		//#endregion

		/// <summary>
		/// Returns the last received message from the IRC server
		/// </summary>
		/// <returns>Last IRC message</returns>
		protected string LastIrcMessage()
		{
			string line = this.inputStream.ReadLine();

			//check if disconnected and reconnect if needed
			int tries = 1;
			while (string.IsNullOrEmpty(line))
			{
				Console.WriteLine("Lost IRC connection. Reconnecting... (" + tries + " tries)");
				this.Reconnect();
				line = this.inputStream.ReadLine();
				Thread.Sleep(1000);
				tries++;
			}

			//answer Pings from the IRC server wo we wont get disconnected
			if (line.StartsWith("PING"))
			{
				string answer = line.Substring(4, line.Length - 4);
				this.sendIrcMessage("PONG tmi.twitch.tv");
				return string.Empty;
			}

			return line;
		}

	}
}
