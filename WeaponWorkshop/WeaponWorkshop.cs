// Copyright (c) 2025 ConcatSpirity

using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.UI;
using GTATimers;
using LemonUI;
using LemonUI.Menus;
using Microsoft.Win32;

namespace WeaponWorkshop
{
    public class WeaponWorkshop : Script
    {
        private static string NAME_WEAPONCHEST = "prop_mil_crate_01";
        private static string TEXT_MENUBANNER = "Weapon Workshop";
        private static string TEXT_MENUNAME = "Get Yourself Strapped Up!";

        private ObjectPool pool = new ObjectPool();
        private NativeMenu menu = new NativeMenu(TEXT_MENUBANNER, TEXT_MENUNAME, "");
        private NativeItem menu_weaponPistol = new NativeItem("Pistol");
        private NativeItem menu_weaponSMG = new NativeItem("SMG");
        private NativeItem menu_weaponMicroSMG = new NativeItem("Micro SMG");
        private NativeItem menu_weaponCarbineRifle = new NativeItem("Carbine Rifle");
        private NativeItem menu_weaponAssaultRifle = new NativeItem("Assault Rifle");
        private NativeItem menu_weaponHeavySniper = new NativeItem("Heavy Sniper");
        private NativeItem menu_weaponShotgun = new NativeItem("Sawed-Off Shotgun");
        private NativeItem menu_weaponMolotov = new NativeItem("Molotov");
        private NativeItem menu_weaponBat = new NativeItem("Bat");

        private Vector3 weaponChestLocation = new Vector3(-192.2758f, -1362.0439f, 30.7082f);

        private List<NativeItem> menuItems = new List<NativeItem>();
        private List<Prop> props = new List<Prop>();

        private GTATimer cTimer;

        public WeaponWorkshop()
        {
            Tick += OnTick;
            Aborted += OnAbort;

            pool.Add(menu);
            CreateMenuWeaponItems();

            Prop weaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), weaponChestLocation, false, true);

            props.Add(weaponChest);

            props[0].Rotation = new Vector3(0.0f, 0.0f, 120.0f);

            props[0].AddBlip();
            props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
            props[0].AttachedBlip.Name = TEXT_MENUBANNER;
            props[0].AttachedBlip.IsShortRange = true;

            int interval = 60000;
            cTimer = new GTATimer("timer", interval);
            cTimer.onTimerElapsed += OnTimerElapsed;
            cTimer.Start();

            menu_weaponPistol.Activated += GiveWeapon(WeaponGroup.Pistol, WeaponHash.Pistol, menu_weaponPistol);
            menu_weaponSMG.Activated += GiveWeapon(WeaponGroup.SMG, WeaponHash.SMG, menu_weaponSMG);
            menu_weaponMicroSMG.Activated += GiveWeapon(WeaponGroup.SMG, WeaponHash.MicroSMG, menu_weaponMicroSMG);
            menu_weaponCarbineRifle.Activated += GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.CarbineRifle, menu_weaponCarbineRifle);
            menu_weaponAssaultRifle.Activated += GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.AssaultRifle, menu_weaponAssaultRifle);
            menu_weaponHeavySniper.Activated += GiveWeapon(WeaponGroup.Sniper, WeaponHash.HeavySniper, menu_weaponHeavySniper);
            menu_weaponShotgun.Activated += GiveWeapon(WeaponGroup.Shotgun, WeaponHash.SawnOffShotgun, menu_weaponShotgun);
            menu_weaponMolotov.Activated += GiveWeapon(WeaponGroup.Thrown, WeaponHash.Molotov, menu_weaponMolotov);
            menu_weaponBat.Activated += GiveWeapon(WeaponGroup.Melee, WeaponHash.Bat, menu_weaponBat);
        }

        private static Model RequestModel(string prop)
        {
            var model = new Model(prop);
            model.Request(250);

            if (model.IsInCdImage && model.IsValid)
            {
                while (!model.IsLoaded)
                {
                    Script.Wait(50);
                }
                return model;
            }

            model.MarkAsNoLongerNeeded();
            return model;
        }

        private EventHandler GiveWeapon(WeaponGroup group, WeaponHash hash, NativeItem menuItem)
        {
            switch (group)
            {
                case WeaponGroup.Pistol:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 80;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 80, true, true);
                    break;

                case WeaponGroup.SMG:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 165;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 165, true, true);
                    break;

                case WeaponGroup.AssaultRifle:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 250;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 250, true, true);
                    break;

                case WeaponGroup.Sniper:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 50;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 50, true, true);
                    break;

                case WeaponGroup.Shotgun:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 60;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 60, true, true);
                    break;

                case WeaponGroup.Melee:
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Notification.Show("You already have this melee weapon");
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 1, true, true);
                    menu.Remove(menuItem);
                    break;

                case WeaponGroup.Thrown:
                    menu.Remove(menuItem);
                    if (Game.Player.Character.Weapons.HasWeapon(hash))
                    {
                        Game.Player.Character.Weapons[hash].Ammo += 5;
                        break;
                    }
                    Game.Player.Character.Weapons.Give(hash, 5, true, true);
                    break;

                default:
                    break;
            }

            return null;
        }

        private void CreateMenuWeaponItems()
        {
            NativeItem[] allMenuWeaponItems =
            { 
                menu_weaponPistol,
                menu_weaponMicroSMG,
                menu_weaponSMG,
                menu_weaponAssaultRifle,
                menu_weaponCarbineRifle,
                menu_weaponHeavySniper,
                menu_weaponShotgun,
                menu_weaponMolotov,
                menu_weaponBat
            };
            var rand = new Random();
            int poolSize = rand.Next(3, 5);

            if (menu.Items.Count > 0)
            {
                menu.Clear();
            }

            for (int i = 0; i < poolSize; i++)
            {
                int selectedWeaponIndex = rand.Next(0, allMenuWeaponItems.Length);
                if (menuItems.Contains(allMenuWeaponItems[selectedWeaponIndex]))
                {
                    i--;
                }
                else
                {
                    menuItems.Add(allMenuWeaponItems[selectedWeaponIndex]);
                }
            }

            foreach (var item in menuItems)
            {
                menu.Add(item);
            }
        }

        private void OnTimerElapsed(string name)
        {
            menu.Clear();
            CreateMenuWeaponItems();
            Notification.Show("Weapon Workshop: Items have been restocked");
        }

        private void OnAbort(object sender, EventArgs e)
        {
            menu.Clear();
            pool.Remove(menu);
            props[0].AttachedBlip.Delete();
            props[0].Delete();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.IsLoading) return;

            pool.Process();
            
            if (World.GetDistance(Game.Player.Character.Position, props[0].Position) < 3)
            {
                menu.Visible = true;
            }
            else
            {
                menu.Visible = false;
            }

            if (cTimer.Running) cTimer.Update();
        }
    }
}
