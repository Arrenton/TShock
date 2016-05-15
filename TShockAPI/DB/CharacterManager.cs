/*
TShock, a server mod for Terraria
Copyright (C) 2011-2015 Nyx Studios (fka. The TShock Team)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Terraria;

namespace TShockAPI.DB
{
	public class CharacterManager
	{
		public IDbConnection database;

		public CharacterManager(IDbConnection db)
		{
			database = db;

			var table = new SqlTable("tsCharacter",
			                         new SqlColumn("Account", MySqlDbType.Int32) {Primary = true},
									 new SqlColumn("Health", MySqlDbType.Int32),
			                         new SqlColumn("MaxHealth", MySqlDbType.Int32),
									 new SqlColumn("Mana", MySqlDbType.Int32),
			                         new SqlColumn("MaxMana", MySqlDbType.Int32),
			                         new SqlColumn("Inventory", MySqlDbType.Text),
									 new SqlColumn("extraSlot", MySqlDbType.Int32),
									 new SqlColumn("spawnX", MySqlDbType.Int32),
									 new SqlColumn("spawnY", MySqlDbType.Int32),
									 new SqlColumn("skinVariant", MySqlDbType.Int32),
									 new SqlColumn("hair", MySqlDbType.Int32),
									 new SqlColumn("hairDye", MySqlDbType.Int32),
									 new SqlColumn("hairColor", MySqlDbType.Int32),
									 new SqlColumn("pantsColor", MySqlDbType.Int32),
									 new SqlColumn("shirtColor", MySqlDbType.Int32),
									 new SqlColumn("underShirtColor", MySqlDbType.Int32),
									 new SqlColumn("shoeColor", MySqlDbType.Int32),
									 new SqlColumn("hideVisuals", MySqlDbType.Int32),
									 new SqlColumn("skinColor", MySqlDbType.Int32),
									 new SqlColumn("eyeColor", MySqlDbType.Int32),
									 new SqlColumn("questsCompleted", MySqlDbType.Int32),
                                     new SqlColumn("Level", MySqlDbType.Int32),
                                     new SqlColumn("Experience", MySqlDbType.Int32),
                                     new SqlColumn("StatusPoints", MySqlDbType.Int32),
                                     new SqlColumn("Strength", MySqlDbType.Int32),
                                     new SqlColumn("Ranged", MySqlDbType.Int32),
                                     new SqlColumn("Magic", MySqlDbType.Int32),
                                     new SqlColumn("Vitality", MySqlDbType.Int32),
                                     new SqlColumn("BaseHP", MySqlDbType.Int32),
                                     new SqlColumn("BaseStrength", MySqlDbType.Int32),
                                     new SqlColumn("BaseRanged", MySqlDbType.Int32),
                                     new SqlColumn("BaseMagic", MySqlDbType.Int32),
                                     new SqlColumn("BaseVitality", MySqlDbType.Int32),
                                     new SqlColumn("RegenHP", MySqlDbType.Int32),
                                     new SqlColumn("RegenMP", MySqlDbType.Int32),
                                     new SqlColumn("StatSpec", MySqlDbType.Int32),
                                     new SqlColumn("ExperienceRate", MySqlDbType.Int32),
                                     new SqlColumn("Abilities", MySqlDbType.Text),
                                     new SqlColumn("ActiveAbilities", MySqlDbType.Text),
                                     new SqlColumn("Version", MySqlDbType.Int32)
                );
			var creator = new SqlTableCreator(db,
			                                  db.GetSqlType() == SqlType.Sqlite
			                                  	? (IQueryBuilder) new SqliteQueryCreator()
			                                  	: new MysqlQueryCreator());
			creator.EnsureTableStructure(table);
		}

		public PlayerData GetPlayerData(TSPlayer player, int acctid)
		{
			PlayerData playerData = new PlayerData(player);

			try
			{
				using (var reader = database.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", acctid))
				{
					if (reader.Read())
					{
						playerData.exists = true;
						playerData.health = reader.Get<int>("Health");
						playerData.maxHealth = reader.Get<int>("MaxHealth");
						playerData.mana = reader.Get<int>("Mana");
						playerData.maxMana = reader.Get<int>("MaxMana");
						List<NetItem> inventory = reader.Get<string>("Inventory").Split('~').Select(NetItem.Parse).ToList();
                        List<int> LearnedAbility = new List<int>();
                        List<int> ActiveAbility = new List<int>();
                        try
                        {
                            LearnedAbility = reader.Get<string>("Abilities").Split(',').Select(Int32.Parse).ToList();
                            ActiveAbility = reader.Get<string>("ActiveAbilities").Split(',').Select(Int32.Parse).ToList();
                        }
                        catch
                        {
                            //TShock.Log.Error("Abilities:" + reader.Get<string>("Abilities"));
                        }
                        //List<int> LearnedAbility = new List<int>();
                        //List<int> ActiveAbility = new List<int>();
                        if (inventory.Count < NetItem.MaxInventory)
						{
							//TODO: unhardcode this - stop using magic numbers and use NetItem numbers
							//Set new armour slots empty
							inventory.InsertRange(67, new NetItem[2]);
							//Set new vanity slots empty
							inventory.InsertRange(77, new NetItem[2]);
							//Set new dye slots empty
							inventory.InsertRange(87, new NetItem[2]);
							//Set the rest of the new slots empty
							inventory.AddRange(new NetItem[NetItem.MaxInventory - inventory.Count]);
						}
						playerData.inventory = inventory.ToArray();
						playerData.extraSlot = reader.Get<int>("extraSlot");
						playerData.spawnX = reader.Get<int>("spawnX");
						playerData.spawnY = reader.Get<int>("spawnY");
						playerData.skinVariant = reader.Get<int?>("skinVariant");
						playerData.hair = reader.Get<int?>("hair");
						playerData.hairDye = (byte)reader.Get<int>("hairDye");
						playerData.hairColor = TShock.Utils.DecodeColor(reader.Get<int?>("hairColor"));
						playerData.pantsColor = TShock.Utils.DecodeColor(reader.Get<int?>("pantsColor"));
						playerData.shirtColor = TShock.Utils.DecodeColor(reader.Get<int?>("shirtColor"));
						playerData.underShirtColor = TShock.Utils.DecodeColor(reader.Get<int?>("underShirtColor"));
						playerData.shoeColor = TShock.Utils.DecodeColor(reader.Get<int?>("shoeColor"));
						playerData.hideVisuals = TShock.Utils.DecodeBoolArray(reader.Get<int?>("hideVisuals"));
						playerData.skinColor = TShock.Utils.DecodeColor(reader.Get<int?>("skinColor"));
						playerData.eyeColor = TShock.Utils.DecodeColor(reader.Get<int?>("eyeColor"));
						playerData.questsCompleted = reader.Get<int>("questsCompleted");
                        playerData.Level = reader.Get<int>("Level");
                        playerData.Exp = reader.Get<int>("Experience");
                        playerData.StatPoints = reader.Get<int>("StatusPoints");
                        playerData.bonusStr = reader.Get<int>("Strength");
                        playerData.bonusRng = reader.Get<int>("Ranged");
                        playerData.bonusMag = reader.Get<int>("Magic");
                        playerData.bonusDef = reader.Get<int>("Vitality");
                        playerData.baseHP = reader.Get<int?>("BaseHP");
                        playerData.baseStr = reader.Get<int?>("BaseStrength");
                        playerData.baseRng = reader.Get<int?>("BaseRanged");
                        playerData.baseMag = reader.Get<int?>("BaseMagic");
                        playerData.baseDef = reader.Get<int?>("BaseVitality");
                        playerData.Regenerate = reader.Get<int?>("RegenHP");
                        playerData.MRegenerate = reader.Get<int?>("RegenMP");
                        playerData.Spec = reader.Get<int?>("StatSpec");
                        playerData.EXPRate = reader.Get<int?>("ExperienceRate");
                        playerData.Ability = ActiveAbility.ToArray();
                        playerData.LearnedAbilities = LearnedAbility.ToArray();
                        int V = reader.Get<int>("Version");
                        return playerData;
					}
				}
			}
			catch (Exception ex)
			{
				TShock.Log.Error(ex.ToString());
			}

			return playerData;
		}

		public bool SeedInitialData(User user)
		{
			var inventory = new StringBuilder();

			var items = new List<NetItem>(TShock.ServerSideCharacterConfig.StartingInventory);
			if (items.Count < NetItem.MaxInventory)
				items.AddRange(new NetItem[NetItem.MaxInventory - items.Count]);

			string initialItems = String.Join("~", items.Take(NetItem.MaxInventory));
			try
			{
				database.Query("INSERT INTO tsCharacter (Account, Health, MaxHealth, Mana, MaxMana, Inventory, spawnX, spawnY, questsCompleted, Level, Experience, StatusPoints, Strength, Ranged, Magic, Vitality, Abilities, ActiveAbilities) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17);",
                               user.ID,
							   TShock.ServerSideCharacterConfig.StartingHealth,
							   TShock.ServerSideCharacterConfig.StartingHealth,
							   TShock.ServerSideCharacterConfig.StartingMana,
							   TShock.ServerSideCharacterConfig.StartingMana, 
							   initialItems, 
							   -1, 
							   -1, 
							   0,
                               TShock.ServerSideCharacterConfig.StartingLevel,
                               0,
                               0,
                               0,
                               0,
                               0,
                               0,
                               "",
                               "");
                return true;
			}
			catch (Exception ex)
			{
				TShock.Log.Error(ex.ToString());
			}

			return false;
		}

		/// <summary>
		/// Inserts player data to the tsCharacter database table
		/// </summary>
		/// <param name="player">player to take data from</param>
		/// <returns>true if inserted successfully</returns>
		public bool InsertPlayerData(TSPlayer player)
		{
			PlayerData playerData = player.PlayerData;
			
			if (!player.IsLoggedIn)
				return false;
            
                       if (player.HasPermission(Permissions.bypassssc))
                           {
                TShock.Log.ConsoleInfo("Skipping SSC Backup for " + player.User.Name); // Debug code
                               return true;
                           }
            
            if (!GetPlayerData(player, player.User.ID).exists)
			{
				try
                {
                    Array.Clear(playerData.ActiveAbilties, 0, Main.AbilityCount);
                    Array.Clear(playerData.LrndAbilties, 0, Main.AbilityCount);
                    playerData.ActiveAbilties = player.TPlayer.Ability.ToArray();
                    playerData.LrndAbilties = player.TPlayer.LearnedAbilities.ToArray();
                    Array.Resize<int>(ref playerData.ActiveAbilties, Main.AbilityCount);
                    Array.Resize<int>(ref playerData.LrndAbilties, Main.AbilityCount);
                    database.Query(
                        "INSERT INTO tsCharacter (Account, Health, MaxHealth, Mana, MaxMana, Inventory, extraSlot, spawnX, spawnY, skinVariant, hair, hairDye, hairColor, pantsColor, shirtColor, underShirtColor, shoeColor, hideVisuals, skinColor, eyeColor, questsCompleted, Level, Experience, StatusPoints, Strength, Ranged, Magic, Vitality, BaseHP, BaseStrength, BaseRanged, BaseMagic, BaseVitality, RegenHP, RegenMP, StatSpec, ExperienceRate, Abilities, ActiveAbilities, Version) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23, @24, @25, @26, @27, @28, @29, @30, @31, @32, @33, @34, @35, @36, @37, @38, @39);",
						player.User.ID, playerData.health, playerData.maxHealth, playerData.mana, playerData.maxMana, String.Join("~", playerData.inventory), playerData.extraSlot, player.TPlayer.SpawnX, player.TPlayer.SpawnY, player.TPlayer.skinVariant, player.TPlayer.hair, player.TPlayer.hairDye, TShock.Utils.EncodeColor(player.TPlayer.hairColor), TShock.Utils.EncodeColor(player.TPlayer.pantsColor),TShock.Utils.EncodeColor(player.TPlayer.shirtColor), TShock.Utils.EncodeColor(player.TPlayer.underShirtColor), TShock.Utils.EncodeColor(player.TPlayer.shoeColor), TShock.Utils.EncodeBoolArray(player.TPlayer.hideVisual), TShock.Utils.EncodeColor(player.TPlayer.skinColor),TShock.Utils.EncodeColor(player.TPlayer.eyeColor), player.TPlayer.anglerQuestsFinished, player.TPlayer.Level, player.TPlayer.Exp, player.TPlayer.StatPoints, player.TPlayer.bonusStr, player.TPlayer.bonusRng, player.TPlayer.bonusMag, player.TPlayer.bonusDef, player.TPlayer.baseHP, player.TPlayer.baseStr, player.TPlayer.baseRng, player.TPlayer.baseMag, player.TPlayer.baseDef, (player.TPlayer.Regenerate ? 1 : 0), (player.TPlayer.MRegenerate ? 1 : 0), player.TPlayer.Spec, player.TPlayer.EXPRate, String.Join(",", playerData.LrndAbilties), String.Join(",", playerData.ActiveAbilties), TShock.ServerSideCharacterConfig.StatusVersion);
                    return true;
				}
				catch (Exception ex)
				{
					TShock.Log.Error(ex.ToString());
				}
			}
			else
			{
				try
                {
                    Array.Clear(playerData.ActiveAbilties, 0, Main.AbilityCount);
                    Array.Clear(playerData.LrndAbilties, 0, Main.AbilityCount);
                    playerData.ActiveAbilties = player.TPlayer.Ability.ToArray();
                    playerData.LrndAbilties = player.TPlayer.LearnedAbilities.ToArray();
                    Array.Resize<int>(ref playerData.ActiveAbilties, Main.AbilityCount);
                    Array.Resize<int>(ref playerData.LrndAbilties, Main.AbilityCount);
                    database.Query(
                        "UPDATE tsCharacter SET Health = @0, MaxHealth = @1, Mana = @2, MaxMana = @3, Inventory = @4, spawnX = @6, spawnY = @7, hair = @8, hairDye = @9, hairColor = @10, pantsColor = @11, shirtColor = @12, underShirtColor = @13, shoeColor = @14, hideVisuals = @15, skinColor = @16, eyeColor = @17, questsCompleted = @18, skinVariant = @19, extraSlot = @20, Level = @21, Experience = @22, StatusPoints = @23, Strength = @24, Ranged = @25, Magic = @26, Vitality = @27, BaseHP = @28, BaseStrength = @29, BaseRanged = @30, BaseMagic = @31, BaseVitality = @32, RegenHP = @33, RegenMP = @34, StatSpec = @35, ExperienceRate = @36, Abilities = @37, ActiveAbilities = @38, Version = @39 WHERE Account = @5;",
						playerData.health, playerData.maxHealth, playerData.mana, playerData.maxMana, String.Join("~", playerData.inventory), player.User.ID, player.TPlayer.SpawnX, player.TPlayer.SpawnY, player.TPlayer.hair, player.TPlayer.hairDye, TShock.Utils.EncodeColor(player.TPlayer.hairColor), TShock.Utils.EncodeColor(player.TPlayer.pantsColor), TShock.Utils.EncodeColor(player.TPlayer.shirtColor), TShock.Utils.EncodeColor(player.TPlayer.underShirtColor), TShock.Utils.EncodeColor(player.TPlayer.shoeColor), TShock.Utils.EncodeBoolArray(player.TPlayer.hideVisual), TShock.Utils.EncodeColor(player.TPlayer.skinColor), TShock.Utils.EncodeColor(player.TPlayer.eyeColor), player.TPlayer.anglerQuestsFinished, player.TPlayer.skinVariant, player.TPlayer.extraAccessory ? 1 : 0, player.TPlayer.Level, player.TPlayer.Exp, player.TPlayer.StatPoints, player.TPlayer.bonusStr, player.TPlayer.bonusRng, player.TPlayer.bonusMag, player.TPlayer.bonusDef, player.TPlayer.baseHP, player.TPlayer.baseStr, player.TPlayer.baseRng, player.TPlayer.baseMag, player.TPlayer.baseDef, (player.TPlayer.Regenerate ? 1 : 0), (player.TPlayer.MRegenerate ? 1 : 0), player.TPlayer.Spec, player.TPlayer.EXPRate, String.Join(",", playerData.LrndAbilties), String.Join(",", playerData.ActiveAbilties), TShock.ServerSideCharacterConfig.StatusVersion);
                    return true;
				}
				catch (Exception ex)
				{
					TShock.Log.Error(ex.ToString());
				}
			}
			return false;
		}

		/// <summary>
		/// Removes a player's data from the tsCharacter database table
		/// </summary>
		/// <param name="userid">User ID of the player</param>
		/// <returns>true if removed successfully</returns>
		public bool RemovePlayer(int userid)
		{
			try
			{
				database.Query("DELETE FROM tsCharacter WHERE Account = @0;", userid);
				return true;
			}
			catch (Exception ex)
			{
				TShock.Log.Error(ex.ToString());
			}

			return false;
		}
	}
}
