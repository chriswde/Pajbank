using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pajbank.Twitch.Commands
{
	public class Command
	{
		public uint Cooldown { get; private set; }
		protected DateTime lastExecution;

		public Command(uint cooldown)
		{
			this.Cooldown = cooldown;
		}

		public Command()
		{
		}

		protected List<string> GetArgs(string msg)
		{
			List<string> args = new List<string>();
			args = msg.Split(' ').ToList();
			if (args.Count > 0)
			{
				args.RemoveAt(0);
			}
			//convert all to lowercase
			args = args.ConvertAll(woop => woop.ToLower());
			return args;
		}

		protected bool isOnCooldown()
		{
			TimeSpan span = DateTime.Now - this.lastExecution;
			if (span.TotalMilliseconds < (double)this.Cooldown * 1000)
			{
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine("on cooldown");
				Console.ResetColor();
				return true;
			}
			return false;
		}
	}
}
