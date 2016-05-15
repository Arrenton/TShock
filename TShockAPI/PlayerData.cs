﻿using Terraria;
using TShockAPI;

namespace TShockAPI
{
    public class PlayerData
    {
        public NetItem[] inventory = new NetItem[NetItem.MaxInventory];
        public int health = TShock.ServerSideCharacterConfig.StartingHealth;
        public int maxHealth = TShock.ServerSideCharacterConfig.StartingHealth;
        public int Level = TShock.ServerSideCharacterConfig.StartingLevel;
        public int mana = TShock.ServerSideCharacterConfig.StartingMana;
        public int maxMana = TShock.ServerSideCharacterConfig.StartingMana;
        public int Exp = 0;
        public int StatPoints = 0;
        public int bonusStr = 0;
        public int bonusRng = 0;
        public int bonusMag = 0;
        public int bonusDef = 0;
        public int[] Ability = new int[Main.AbilityCount];
        public int[] LearnedAbilities = new int[Main.AbilityCount];
        public int? baseHP;
        public int? baseStr;
        public int? baseRng;
        public int? baseMag;
        public int? baseDef;
        public int? Regenerate;
        public int? MRegenerate;
        public int? Spec = 0;
        public int? EXPRate = 0;
        public bool exists;
        public int spawnX = -1;
        public int spawnY = -1;
        public int? extraSlot;
        public int? skinVariant;
        public int? hair;
        public byte hairDye;
        public Color? hairColor;
        public Color? pantsColor;
        public Color? shirtColor;
        public Color? underShirtColor;
        public Color? shoeColor;
        public Color? skinColor;
        public Color? eyeColor;
        public bool[] hideVisuals;
        public int questsCompleted;
        public int[] ActiveAbilties = new int[Main.AbilityCount];
        public int[] LrndAbilties = new int[Main.AbilityCount];

        public PlayerData(TSPlayer player)
        {
            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                this.inventory[i] = new NetItem();
            }

            for (int i = 0; i < TShock.ServerSideCharacterConfig.StartingInventory.Count; i++)
            {
                var item = TShock.ServerSideCharacterConfig.StartingInventory[i];
                StoreSlot(i, item.NetId, item.PrefixId, item.Stack);
            }
        }

        /// <summary>
        /// Stores an item at the specific storage slot
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="netID"></param>
        /// <param name="prefix"></param>
        /// <param name="stack"></param>
        public void StoreSlot(int slot, int netID, byte prefix, int stack)
        {
            if (slot > (this.inventory.Length - 1)) //if the slot is out of range then dont save
            {
                return;
            }

            this.inventory[slot] = new NetItem(netID, stack, prefix);
        }

        /// <summary>
        /// Copies a characters data to this object
        /// </summary>
        /// <param name="player"></param>
        public void CopyCharacter(TSPlayer player)
        {
            this.health = player.TPlayer.statLife > 0 ? player.TPlayer.statLife : 1;
            this.maxHealth = player.TPlayer.statLifeMax;
            this.mana = player.TPlayer.statMana;
            this.maxMana = player.TPlayer.statManaMax;
            if (player.sX > 0 && player.sY > 0)
            {
                this.spawnX = player.sX;
                this.spawnY = player.sY;
            }
            else
            {
                this.spawnX = player.TPlayer.SpawnX;
                this.spawnY = player.TPlayer.SpawnY;
            }
            extraSlot = player.TPlayer.extraAccessory ? 1 : 0;
            this.skinVariant = player.TPlayer.skinVariant;
            this.hair = player.TPlayer.hair;
            this.hairDye = player.TPlayer.hairDye;
            this.hairColor = player.TPlayer.hairColor;
            this.pantsColor = player.TPlayer.pantsColor;
            this.shirtColor = player.TPlayer.shirtColor;
            this.underShirtColor = player.TPlayer.underShirtColor;
            this.shoeColor = player.TPlayer.shoeColor;
            this.hideVisuals = player.TPlayer.hideVisual;
            this.skinColor = player.TPlayer.skinColor;
            this.eyeColor = player.TPlayer.eyeColor;
            this.questsCompleted = player.TPlayer.anglerQuestsFinished;
            this.Level = player.TPlayer.Level;
            this.Exp = player.TPlayer.Exp;
            this.StatPoints = player.TPlayer.StatPoints;
            this.bonusStr = player.TPlayer.bonusStr;
            this.bonusRng = player.TPlayer.bonusRng;
            this.bonusMag = player.TPlayer.bonusMag;
            this.bonusDef = player.TPlayer.bonusDef;
            this.baseHP = player.TPlayer.baseHP;
            this.baseStr = player.TPlayer.baseStr;
            this.baseRng = player.TPlayer.baseRng;
            this.baseMag = player.TPlayer.baseMag;
            this.baseDef = player.TPlayer.baseDef;
            this.Regenerate = player.TPlayer.Regenerate ? (TShock.ServerSideCharacterConfig.EnableRegeneration ? 1 : 0) : 0;
            this.MRegenerate = player.TPlayer.MRegenerate ? (TShock.ServerSideCharacterConfig.EnableManaRegen ? 1 : 0) : 0;
            
            this.Spec = player.TPlayer.Spec;
            this.EXPRate = player.TPlayer.EXPRate;
            this.LearnedAbilities = player.TPlayer.LearnedAbilities.ToArray();
            this.Ability = player.TPlayer.Ability.ToArray();


            Item[] inventory = player.TPlayer.inventory;
            Item[] armor = player.TPlayer.armor;
            Item[] dye = player.TPlayer.dye;
            Item[] miscEqups = player.TPlayer.miscEquips;
            Item[] miscDyes = player.TPlayer.miscDyes;
            Item[] piggy = player.TPlayer.bank.item;
            Item[] safe = player.TPlayer.bank2.item;
            Item trash = player.TPlayer.trashItem;

            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i < NetItem.InventorySlots)
                {
                    //0-58
                    this.inventory[i] = (NetItem)inventory[i];
                }
                else if (i < NetItem.InventorySlots + NetItem.ArmorSlots)
                {
                    //59-78
                    var index = i - NetItem.InventorySlots;
                    this.inventory[i] = (NetItem)armor[index];
                }
                else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots)
                {
                    //79-88
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots);
                    this.inventory[i] = (NetItem)dye[index];
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots)
                {
                    //89-93
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
                    this.inventory[i] = (NetItem)miscEqups[index];
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots
                    + NetItem.MiscDyeSlots)
                {
                    //93-98
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots);
                    this.inventory[i] = (NetItem)miscDyes[index];
                }
                else if (i <
                   NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots +
                   NetItem.MiscDyeSlots + NetItem.PiggySlots)
                {
                    //98-138
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots);
                    this.inventory[i] = (NetItem)piggy[index];
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots +
                    NetItem.MiscDyeSlots + NetItem.PiggySlots + NetItem.SafeSlots)
                {
                    //138-178
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots + NetItem.PiggySlots);
                    this.inventory[i] = (NetItem)safe[index];
                }
                else
                {
                    //179
                    this.inventory[i] = (NetItem)trash;
                }
            }
        }

        /// <summary>
        /// Restores a player's character to the state stored in the database
        /// </summary>
        /// <param name="player"></param>
        public void RestoreCharacter(TSPlayer player)
        {
            // Start ignoring SSC-related packets! This is critical so that we don't send or receive dirty data!
            player.IgnoreSSCPackets = true;

            player.TPlayer.Level = (byte)this.Level;
            player.TPlayer.Exp = this.Exp;
            player.TPlayer.StatPoints = (short)this.StatPoints;
            player.TPlayer.bonusStr = (short)this.bonusStr;
            player.TPlayer.bonusRng = (short)this.bonusRng;
            player.TPlayer.bonusMag = (short)this.bonusMag;
            player.TPlayer.bonusDef = (short)this.bonusDef;
            try
            {
                if (this.baseHP != null)
                    player.TPlayer.baseHP = (short)this.baseHP.Value;
                if (this.baseStr != null)
                    player.TPlayer.baseStr = (short)this.baseStr.Value;
                if (this.baseRng != null)
                    player.TPlayer.baseRng = (short)this.baseRng.Value;
                if (this.baseMag != null)
                    player.TPlayer.baseMag = (short)this.baseMag.Value;
                if (this.baseDef != null)
                    player.TPlayer.baseDef = (short)this.baseDef.Value;
                if (this.Regenerate != null)
                    player.TPlayer.Regenerate = System.Convert.ToBoolean(this.Regenerate.Value);
                if (this.MRegenerate != null)
                    player.TPlayer.MRegenerate = System.Convert.ToBoolean(this.MRegenerate.Value);
                if (this.Spec != null)
                    player.TPlayer.Spec = (byte)this.Spec.Value;
                if (this.EXPRate != null)
                    player.TPlayer.EXPRate = (byte)this.EXPRate.Value;
                player.TPlayer.Ability = new System.Collections.Generic.List<int>(this.Ability);
                player.TPlayer.LearnedAbilities = new System.Collections.Generic.List<int>(this.LearnedAbilities);
            }
            catch (System.Exception ex)
            {
                TShock.Log.Error(ex.ToString());
            }

            player.TPlayer.statLife = this.health;
            player.TPlayer.statLifeMax = this.maxHealth;
            player.TPlayer.statMana = this.maxMana;
            player.TPlayer.statManaMax = this.maxMana;
            player.TPlayer.SpawnX = this.spawnX;
            player.TPlayer.SpawnY = this.spawnY;
            player.sX = this.spawnX;
            player.sY = this.spawnY;
            player.TPlayer.hairDye = this.hairDye;
            player.TPlayer.anglerQuestsFinished = this.questsCompleted;

            player.TPlayer.PlayerLevelCalculate();

            if (extraSlot != null)
                player.TPlayer.extraAccessory = extraSlot.Value == 1 ? true : false;
            if (this.skinVariant != null)
                player.TPlayer.skinVariant = this.skinVariant.Value;
            if (this.hair != null)
                player.TPlayer.hair = this.hair.Value;
            if (this.hairColor != null)
                player.TPlayer.hairColor = this.hairColor.Value;
            if (this.pantsColor != null)
                player.TPlayer.pantsColor = this.pantsColor.Value;
            if (this.shirtColor != null)
                player.TPlayer.shirtColor = this.shirtColor.Value;
            if (this.underShirtColor != null)
                player.TPlayer.underShirtColor = this.underShirtColor.Value;
            if (this.shoeColor != null)
                player.TPlayer.shoeColor = this.shoeColor.Value;
            if (this.skinColor != null)
                player.TPlayer.skinColor = this.skinColor.Value;
            if (this.eyeColor != null)
                player.TPlayer.eyeColor = this.eyeColor.Value;

            if (this.hideVisuals != null)
                player.TPlayer.hideVisual = this.hideVisuals;
            else
                player.TPlayer.hideVisual = new bool[player.TPlayer.hideVisual.Length];

            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i < NetItem.InventorySlots)
                {
                    //0-58
                    player.TPlayer.inventory[i].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.inventory[i].netID != 0)
                    {
                        player.TPlayer.inventory[i].stack = this.inventory[i].Stack;
                        player.TPlayer.inventory[i].prefix = this.inventory[i].PrefixId;
                    }
                }
                else if (i < NetItem.InventorySlots + NetItem.ArmorSlots)
                {
                    //59-78
                    var index = i - NetItem.InventorySlots;
                    player.TPlayer.armor[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.armor[index].netID != 0)
                    {
                        player.TPlayer.armor[index].stack = this.inventory[i].Stack;
                        player.TPlayer.armor[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots)
                {
                    //79-88
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots);
                    player.TPlayer.dye[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.dye[index].netID != 0)
                    {
                        player.TPlayer.dye[index].stack = this.inventory[i].Stack;
                        player.TPlayer.dye[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots)
                {
                    //89-93
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
                    player.TPlayer.miscEquips[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.miscEquips[index].netID != 0)
                    {
                        player.TPlayer.miscEquips[index].stack = this.inventory[i].Stack;
                        player.TPlayer.miscEquips[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots
                    + NetItem.MiscDyeSlots)
                {
                    //93-98
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots);
                    player.TPlayer.miscDyes[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.miscDyes[index].netID != 0)
                    {
                        player.TPlayer.miscDyes[index].stack = this.inventory[i].Stack;
                        player.TPlayer.miscDyes[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots +
                    NetItem.MiscDyeSlots + NetItem.PiggySlots)
                {
                    //98-138
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots);
                    player.TPlayer.bank.item[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.bank.item[index].netID != 0)
                    {
                        player.TPlayer.bank.item[index].stack = this.inventory[i].Stack;
                        player.TPlayer.bank.item[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else if (i <
                    NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots +
                    NetItem.MiscDyeSlots + NetItem.PiggySlots + NetItem.SafeSlots)
                {
                    var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots
                        + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots + NetItem.PiggySlots);
                    player.TPlayer.bank2.item[index].netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.bank2.item[index].netID != 0)
                    {
                        player.TPlayer.bank2.item[index].stack = this.inventory[i].Stack;
                        player.TPlayer.bank2.item[index].prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
                else
                {
                    player.TPlayer.trashItem.netDefaults(this.inventory[i].NetId);

                    if (player.TPlayer.trashItem.netID != 0)
                    {
                        player.TPlayer.trashItem.stack = this.inventory[i].Stack;
                        player.TPlayer.trashItem.prefix = (byte)this.inventory[i].PrefixId;
                    }
                }
            }

            float slot = 0f;
            for (int k = 0; k < NetItem.InventorySlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].inventory[k].name, player.Index, slot, (float)Main.player[player.Index].inventory[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.ArmorSlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].armor[k].name, player.Index, slot, (float)Main.player[player.Index].armor[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.DyeSlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].dye[k].name, player.Index, slot, (float)Main.player[player.Index].dye[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.MiscEquipSlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].miscEquips[k].name, player.Index, slot, (float)Main.player[player.Index].miscEquips[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.MiscDyeSlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].miscDyes[k].name, player.Index, slot, (float)Main.player[player.Index].miscDyes[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.PiggySlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].bank.item[k].name, player.Index, slot, (float)Main.player[player.Index].bank.item[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.SafeSlots; k++)
            {
                NetMessage.SendData(5, -1, -1, Main.player[player.Index].bank2.item[k].name, player.Index, slot, (float)Main.player[player.Index].bank2.item[k].prefix);
                slot++;
            }

            NetMessage.SendData(5, -1, -1, Main.player[player.Index].trashItem.name, player.Index, slot, (float)Main.player[player.Index].trashItem.prefix);

            NetMessage.SendData(4, -1, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(42, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(16, -1, -1, "", player.Index, 0f, 0f, 0f, 0);

            slot = 0f;
            for (int k = 0; k < NetItem.InventorySlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].inventory[k].name, player.Index, slot, (float)Main.player[player.Index].inventory[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.ArmorSlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].armor[k].name, player.Index, slot, (float)Main.player[player.Index].armor[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.DyeSlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].dye[k].name, player.Index, slot, (float)Main.player[player.Index].dye[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.MiscEquipSlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].miscEquips[k].name, player.Index, slot, (float)Main.player[player.Index].miscEquips[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.MiscDyeSlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].miscDyes[k].name, player.Index, slot, (float)Main.player[player.Index].miscDyes[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.PiggySlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].bank.item[k].name, player.Index, slot, (float)Main.player[player.Index].bank.item[k].prefix);
                slot++;
            }
            for (int k = 0; k < NetItem.SafeSlots; k++)
            {
                NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].bank2.item[k].name, player.Index, slot, (float)Main.player[player.Index].bank2.item[k].prefix);
                slot++;
            }

            NetMessage.SendData(5, player.Index, -1, Main.player[player.Index].trashItem.name, player.Index, slot, (float)Main.player[player.Index].trashItem.prefix);

            NetMessage.SendData(4, player.Index, -1, player.Name, player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(42, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(16, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);

            for (int k = 0; k < 22; k++)
            {
                player.TPlayer.buffType[k] = 0;
            }
            NetMessage.SendData(50, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(50, player.Index, -1, "", player.Index, 0f, 0f, 0f, 0);
            NetMessage.SendData(76, -1, -1, "", player.Index);

            NetMessage.SendData(39, player.Index, -1, "", 400);
        }
    }
}