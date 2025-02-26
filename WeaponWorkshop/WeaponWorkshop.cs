// Copyright (c) 2025 ConcatSpirity

using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.UI;
using GTATimers;
using LemonUI;
using LemonUI.Menus;

namespace WeaponWorkshop
{
    public class WeaponWorkshop : Script
    {
        private static string NAME_WEAPONCHEST = "prop_mil_crate_01";
        private static string TEXT_MENUBANNER = "Weapon Workshop";
        private static string TEXT_MENUNAME = "Get Yourself Strapped Up!";
        private ObjectPool pool = new ObjectPool();
        private NativeMenu menu = new NativeMenu(TEXT_MENUBANNER, TEXT_MENUNAME, "");
        private List<NativeItem> menu_items = new List<NativeItem>();
        private static NativeItem menu_items_Pistol = new NativeItem("Pistol");
        private static NativeItem menu_items_SMG = new NativeItem("SMG");
        private static NativeItem menu_items_MicroSMG = new NativeItem("Micro SMG");
        private static NativeItem menu_items_CarbineRifle = new NativeItem("Carbine Rifle");
        private static NativeItem menu_items_AssaultRifle = new NativeItem("Assault Rifle");
        private static NativeItem menu_items_HeavySniper = new NativeItem("Heavy Sniper");
        private static NativeItem menu_items_Shotgun = new NativeItem("Sawed-Off Shotgun");
        private static NativeItem menu_items_Molotov = new NativeItem("Molotov");
        private static NativeItem menu_items_Bat = new NativeItem("Bat");
        private NativeItem[] allMenuItems =
        {
            menu_items_Pistol,
            menu_items_SMG,
            menu_items_MicroSMG,
            menu_items_CarbineRifle,
            menu_items_AssaultRifle,
            menu_items_HeavySniper,
            menu_items_Shotgun,
            menu_items_Molotov,
            menu_items_Bat
        };
        private Vector3 chestLocation = new Vector3(-192.2758f, -1362.0439f, 30.7082f);
        private List<Prop> props = new List<Prop>();
        private GTATimer cTimer;
        private bool cTimerSet = false;
        private int cTimerInterval = 60000;

        public WeaponWorkshop()
        {
            Tick += OnTick;
            Aborted += OnAbort;

            pool.Add(menu);
            CreateMenuWeaponItems();

            Prop weaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), chestLocation, false, true);
            props.Add(weaponChest);
            props[0].Rotation = new Vector3(0.0f, 0.0f, 120.0f);
            props[0].AddBlip();
            props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
            props[0].AttachedBlip.Name = TEXT_MENUBANNER;
            props[0].AttachedBlip.IsShortRange = true;

            cTimer = new GTATimer("WeaponsResupplyTimer", cTimerInterval);
            cTimer.onTimerElapsed += OnTimerElapsed;

            menu_items_Pistol.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.Pistol, WeaponHash.Pistol, menu_items_Pistol);
            };
            menu_items_SMG.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.SMG, WeaponHash.SMG, menu_items_SMG);
            };
            menu_items_SMG.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.SMG, WeaponHash.SMG, menu_items_SMG);
            };
            menu_items_MicroSMG.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.SMG, WeaponHash.MicroSMG, menu_items_MicroSMG);
            };
            menu_items_CarbineRifle.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.CarbineRifle, menu_items_CarbineRifle);
            };
            menu_items_AssaultRifle.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.AssaultRifle, menu_items_AssaultRifle);
            };
            menu_items_HeavySniper.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.Sniper, WeaponHash.HeavySniper, menu_items_HeavySniper);
            };
            menu_items_Shotgun.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.Shotgun, WeaponHash.SawnOffShotgun, menu_items_Shotgun);
            };
            menu_items_Molotov.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.Thrown, WeaponHash.Molotov, menu_items_Molotov);
            };
            menu_items_Bat.Activated += (object sender, EventArgs e) =>
            {
                GiveWeapon(WeaponGroup.Melee, WeaponHash.Bat, menu_items_Bat);
            };
        }

        public Model RequestModel(string prop)
        {
            var model = new Model(prop);
            model.Request(250);

            if (model.IsInCdImage && model.IsValid)
            {
                while (!model.IsLoaded) Wait(50);
                return model;
            }

            model.MarkAsNoLongerNeeded();
            return model;
        }

        private void GiveWeapon(WeaponGroup group, WeaponHash hash, NativeItem menuItem)
        {
            var player_weapons = Game.Player.Character.Weapons;

            switch (group)
            {
                case WeaponGroup.Pistol:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 80;
                        break;
                    }
                    player_weapons.Give(hash, 80, true, true);
                    break;

                case WeaponGroup.SMG:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 165;
                        break;
                    }
                    player_weapons.Give(hash, 165, true, true);
                    break;

                case WeaponGroup.AssaultRifle:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 250;
                        break;
                    }
                    player_weapons.Give(hash, 250, true, true);
                    break;

                case WeaponGroup.Sniper:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 50;
                        break;
                    }
                    player_weapons.Give(hash, 50, true, true);
                    break;

                case WeaponGroup.Shotgun:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 60;
                        break;
                    }
                    player_weapons.Give(hash, 60, true, true);
                    break;

                case WeaponGroup.Melee:
                    if (player_weapons.HasWeapon(hash))
                    {
                        Notification.Show("You already have this melee weapon");
                        break;
                    }
                    player_weapons.Give(hash, 1, true, true);
                    menu.Remove(menuItem);
                    break;

                case WeaponGroup.Thrown:
                    menu.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons[hash].Ammo += 5;
                        break;
                    }
                    player_weapons.Give(hash, 5, true, true);
                    break;

                default:
                    break;
            }
        }

        private void CreateMenuWeaponItems()
        {
            var rand = new Random();
            int poolSize = rand.Next(3, 5);

            for (int i = 0; i < poolSize; i++)
            {
                int selectedWeaponIndex = rand.Next(0, allMenuItems.Length);
                if (menu_items.Contains(allMenuItems[selectedWeaponIndex]))
                {
                    i--;
                }
                else
                {
                    menu_items.Add(allMenuItems[selectedWeaponIndex]);
                }
            }

            foreach (var item in menu_items)
            {
                menu.Add(item);
            }
        }

        private void OnTimerElapsed(string name)
        {
            foreach (var item in menu_items)
            {
                menu.Remove(item);
            }
            CreateMenuWeaponItems();
            Notification.Show("Weapon Workshop: Items have been restocked");
            cTimer.Reset();
        }

        private void OnAbort(object sender, EventArgs e)
        {
            props[0].AttachedBlip.Delete();
            props[0].Delete();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.IsLoading) return;

            pool.Process();

            if (!cTimerSet)
            {
                cTimer.Start();
                cTimerSet = true;
            }

            if (cTimer.Running) cTimer.Update();

            if (World.GetDistance(Game.Player.Character.Position, props[0].Position) < 3)
            {
                menu.Visible = true;
            }
            else
            {
                menu.Visible = false;
            }
        }
    }
}
