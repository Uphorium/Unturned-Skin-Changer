using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyTic.SkinChanger
{
	public class SQLDatabaseManager
	{
		internal SQLDatabaseManager()
		{
			new I18N.West.CP1250(); //Workaround for database encoding issues with mono
			CheckSchema();
		}

		public MySqlConnection createConnection()
		{
			MySqlConnection connection = null;
			try
			{
				if (SkinChanger.Instance.Configuration.Instance.DatabasePort == 0) SkinChanger.Instance.Configuration.Instance.DatabasePort = 3306;
				connection = new MySqlConnection(
					String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};",
						SkinChanger.Instance.Configuration.Instance.DatabaseAddress,
						SkinChanger.Instance.Configuration.Instance.DatabaseName,
						SkinChanger.Instance.Configuration.Instance.DatabaseUsername,
						SkinChanger.Instance.Configuration.Instance.DatabasePassword,
						SkinChanger.Instance.Configuration.Instance.DatabasePort
					)
				);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return connection;
		}

		public int[] GetSkins(string id)
		{
			int[] output = new int[0];
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				command.CommandText = "select `skins` from `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id + "';";
				//Logger.Log(command.CommandText);
				connection.Open();
				object result = command.ExecuteScalar();
				if (result != null)
				{
					output = SkinChanger.Instance.stringToSkins(result.ToString());
					//Logger.Log(SkinChanger.Instance.skinsToString(output));
				}
				connection.Close();
				//SkinChanger.Instance.OnBalanceChecked(id, output);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return output;
		}

		public bool SetSkins(string id, int[] skins)
		{
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				command.CommandText = "update `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` set `skins` = skins where `steamId` = '" + id.ToString() + "'";
				connection.Open();
				object result = command.ExecuteScalar();
				if (result != null)
				{
					Logger.Log(result.ToString());
				}
				connection.Close();
				return true;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return true;
		}

		/// <summary>								
		/// returns the current balance of an account
		/// </summary>								
		/// <param name="steamId"></param>			
		/// <returns></returns>						
		public decimal GetBalance(string id)
		{
			decimal output = 0;
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				command.CommandText = "select `balance` from `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "';";
				connection.Open();
				object result = command.ExecuteScalar();
				if (result != null) Decimal.TryParse(result.ToString(), out output);
				connection.Close();
				//SkinChanger.Instance.OnBalanceChecked(id, output);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return output;
		}

		/// <summary>
		/// Increasing balance to increaseBy (can be negative)
		/// </summary>
		/// <param name="steamId">steamid of the accountowner</param>
		/// <param name="increaseBy">amount to change</param>
		/// <returns>the new balance</returns>
		public decimal IncreaseBalance(string id, decimal increaseBy)
		{
			decimal output = 0;
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				command.CommandText = "update `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` set `balance` = balance + (" + increaseBy + ") where `steamId` = '" + id.ToString() + "'; select `balance` from `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "'";
				connection.Open();
				object result = command.ExecuteScalar();
				if (result != null) Decimal.TryParse(result.ToString(), out output);
				connection.Close();
				//SkinChanger.Instance.BalanceUpdated(id, increaseBy);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
			return output;
		}


		public void CheckSetupAccount(Steamworks.CSteamID id)
		{
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				int exists = 0;
				command.CommandText = "SELECT EXISTS(SELECT 1 FROM `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` WHERE `steamId` ='" + id + "' LIMIT 1);";
				connection.Open();
				object result = command.ExecuteScalar();
				if (result != null) Int32.TryParse(result.ToString(), out exists);
				connection.Close();

				if (exists == 0)
				{
					//command.CommandText = "insert ignore into `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` (balance,steamId,lastUpdated) values(" + SkinChanger.Instance.Configuration.Instance.InitialBalance + ",'" + id.ToString() + "',now())";
					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

		}

		internal void CheckSchema()
		{
			try
			{
				MySqlConnection connection = createConnection();
				MySqlCommand command = connection.CreateCommand();
				command.CommandText = "show tables like '" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "'";
				connection.Open();
				object test = command.ExecuteScalar();

				if (test == null)
				{
					command.CommandText = 
						"CREATE TABLE `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` (" +
							"`steamId` VARCHAR(32) NOT NULL," +
							"`skins` TEXT NULL," +
							" `length` INT UNSIGNED NOT NULL DEFAULT '0'," +
							"`lastUpdated` TIMESTAMP NOT NULL DEFAULT NOW() ON UPDATE CURRENT_TIMESTAMP," +
							"PRIMARY KEY (`steamId`)" +
							") ";
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}
		}
	}
}
