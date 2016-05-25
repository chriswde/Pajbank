using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//using System.Globalization;

using Pajbank.Connection;

namespace Pajbank.Twitch
{
	public class User
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public long Balance { get; set; }
		private bool newuser;

		private User(int id, string username, long balance, bool newuser = false)
		{
			this.Id = id;
			this.Name = username;
			this.Balance = balance;
			this.newuser = newuser;
		}

		public bool Save()
		{
			if (this.newuser)
			{
				DBConnection conn = new DBConnection(GlobalVars.Settings["dbserver"], GlobalVars.Settings["dbname"], GlobalVars.Settings["dbuser"], GlobalVars.Settings["dbpassword"]);
				string query = "INSERT INTO users (username, balance) VALUES (@username, @balance);";

				if (conn.OpenConnection())
				{
					int affectedRows = 0;

					using (MySqlCommand cmd = new MySqlCommand(query, conn.Connection))
					{
						cmd.Parameters.AddWithValue("username", this.Name);
						cmd.Parameters.AddWithValue("balance", this.Balance);

						try
						{
							affectedRows = cmd.ExecuteNonQuery();
						}
						catch (MySqlException e)
						{
							Console.WriteLine("MySQL Error {0}: {1}", e.Number, e.Message);
							return false;
						}
					}
					conn.CloseConnection();

					if (affectedRows > 0)
					{
						return true;
					}
				}
				return false;
			}
			else
			{
				DBConnection conn = new DBConnection(GlobalVars.Settings["dbserver"], GlobalVars.Settings["dbname"], GlobalVars.Settings["dbuser"], GlobalVars.Settings["dbpassword"]);
				string query = "UPDATE users SET balance=@balance WHERE id=@id";
				if (conn.OpenConnection())
				{
					int affectedRows = 0;
					using (MySqlCommand cmd = new MySqlCommand(query, conn.Connection))
					{
						cmd.Parameters.AddWithValue("id", this.Id);
						cmd.Parameters.AddWithValue("balance", this.Balance);

						affectedRows = cmd.ExecuteNonQuery();
					}
					conn.CloseConnection();
					if (affectedRows > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static User GetUserFromDataBase(string username)
		{
			DBConnection conn = new DBConnection(GlobalVars.Settings["dbserver"], GlobalVars.Settings["dbname"], GlobalVars.Settings["dbuser"], GlobalVars.Settings["dbpassword"]);

			username = username.ToLower();
			if (conn.Count("users", "username", username) == 1)
			{
				if (conn.OpenConnection())
				{
					string query = String.Format("SELECT id, username, balance FROM users WHERE username=@username;");
					using (MySqlCommand cmd = new MySqlCommand(query, conn.Connection))
					{
						cmd.Parameters.AddWithValue("username", username);
						MySqlDataReader dr = cmd.ExecuteReader();

						string id = "b";
						string name = "b";
						string balance = "b";
						while (dr.Read())
						{
							id = dr["id"] + "";
							name = dr["username"] + "";
							balance = dr["balance"] + "";
						}

						//convert types
						int re_id;
						long re_balance;
						string re_username;
						if (int.TryParse(id, out re_id) && long.TryParse(balance, out re_balance))
						{
							re_id = Convert.ToInt32(id);
							re_balance = Convert.ToInt64(balance);
							re_username = name;
							return new User(re_id, re_username, re_balance);
						}

						conn.CloseConnection();
					}
				}
			}
			return new User(-1, username, 0, true);
		}

		public static User GetUserFromDataBase(int userid)
		{
			DBConnection conn = new DBConnection(GlobalVars.Settings["dbserver"], GlobalVars.Settings["dbname"], GlobalVars.Settings["dbuser"], GlobalVars.Settings["dbpassword"]);
			if (conn.Count("users", "id", userid+"") == 1)
			{
				if (conn.OpenConnection())
				{
					string query = String.Format("SELECT id, username, balance FROM users WHERE id=@id;");
					using (MySqlCommand cmd = new MySqlCommand(query, conn.Connection))
					{
						cmd.Parameters.AddWithValue("id", userid);
						MySqlDataReader dr = cmd.ExecuteReader();

						string id = "b";
						string name = "b";
						string balance = "b";
						while (dr.Read())
						{
							id = dr["id"] + "";
							name = dr["username"] + "";
							balance = dr["balance"] + "";
						}

						//convert types
						int re_id;
						long re_balance;
						string re_username;
						if (int.TryParse(id, out re_id) && long.TryParse(balance, out re_balance))
						{
							re_id = Convert.ToInt32(id);
							re_balance = Convert.ToInt64(balance);
							re_username = name;
							return new User(re_id, re_username, re_balance);
						}

						conn.CloseConnection();
					}
				}
			}
			return null;
		}
	}
}
