using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Pajbank.Connection
{
	public class DBConnection
	{
		public MySqlConnection Connection;
		private string server;
		private string database;
		private string user;
		private string password;

		public DBConnection(string dbserver, string dbname, string dbuser, string dbpassword)
		{
			this.server = dbserver;
			this.database = dbname;
			this.user = dbuser;
			this.password = dbpassword;
			string connectionstring = "SERVER=" + this.server + ";" + "DATABASE=" + this.database + ";" + "UID=" + this.user + ";" + "PASSWORD=" + this.password + ";";

			this.Connection = new MySqlConnection(connectionstring);
		}

		#region open/close connetion
		public bool OpenConnection()
		{
			try
			{
				this.Connection.Open();
				return true;
			}
			catch (MySqlException e)
			{
				switch (e.Number)
				{
					case 0:
						Console.WriteLine("Could not connect to databese server.");
						break;

					case 1045:
						Console.WriteLine("Invalid username or password.");
						break;
				}
				return false;
			}
		}

		public bool CloseConnection()
		{
			try
			{
				this.Connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
		#endregion

		#region Count
		public int Count(string tablename)
		{
			string query = "SELECT COUNT(*) FROM " + tablename + ";";

			if (this.OpenConnection())
			{
				using (MySqlCommand cmd = new MySqlCommand(query, this.Connection))
				{
					int result = 0;

					if (int.TryParse(cmd.ExecuteScalar() + "", out result))
					{
						this.CloseConnection();
						return result;
					}
					else
					{
						this.CloseConnection();
						return result;
					}
				}
			}
			else
			{
				return 0;
			}
		}

		public int Count(string tablename, string whereCol, string whereValue)
		{
			string query = "SELECT COUNT(*) from " + tablename + " WHERE " + whereCol + "=@" + whereCol + ";";

			if (this.OpenConnection())
			{
				using (MySqlCommand cmd = new MySqlCommand(query, this.Connection))
				{
					cmd.Parameters.AddWithValue(whereCol, whereValue);

					int result = 0;

					if (int.TryParse(cmd.ExecuteScalar() + "", out result))
					{
						this.CloseConnection();
						return result;
					}
					else
					{
						this.CloseConnection();
						return result;
					}
				}
			}
			else
			{
				return 0;
			}
		}
		#endregion
	}
}
