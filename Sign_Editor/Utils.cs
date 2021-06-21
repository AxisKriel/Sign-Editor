using System;
using System.Data;
using System.IO;
using System.Linq;
using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;

namespace Sign_Editor
{
	public class Utils 
	{
		private static IDbConnection db;

		public static bool DbConnect()
		{
			try
			{
				switch (TShock.Config.Settings.StorageType.ToLower())
				{
					case "mysql":
						string[] host = TShock.Config.Settings.MySqlHost.Split(':');
						db = new MySqlConnection()
						{
							ConnectionString = string.Format("Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4};",
								host[0],
								host.Length == 1 ? "3306" : host[1],
								TShock.Config.Settings.MySqlDbName,
								TShock.Config.Settings.MySqlUsername,
								TShock.Config.Settings.MySqlPassword)
						};
						break;
					case "sqlite":
						var path = Path.Combine(TShock.SavePath, "signs.sqlite");
						db = new SqliteConnection(String.Format("uri=file://{0},Version=3", path));
						break;
				}
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError("An exception has occured while attempting to connect to the sign database: {0}",
					ex.Message);
				TShock.Log.Error(ex.ToString());
				return false;
			}
			return true;
		}

		public static Sign DbGetSign(int x, int y)
		{
			try
			{
				Sign sign = null;
				string query = "SELECT Text FROM Signs WHERE X=@0 AND Y=@1 AND WorldID=@2;";
				using (var reader = db.QueryReader(query, x, y, Main.worldID))
				{
					while (reader.Read())
					{
						sign = new Sign()
						{
							x = x,
							y = y,
							text = reader.Get<string>("Text")
						};
					}
				}
				return sign;
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError(ex.ToString());
				return null;
			}
		}

		public static bool DbSetSignText(int x, int y, string text)
		{
			try
			{
				string query = "UPDATE Signs SET Text=@0 WHERE X=@1 AND Y=@2 AND WorldID=@3;";
				return db.Query(query, text, x, y, Main.worldID) > 0;
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError(ex.ToString());
				return false;
			}
		}
	}
}
