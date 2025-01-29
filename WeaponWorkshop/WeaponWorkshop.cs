// Copyright (c) 2024 ConcatSpirity

using System;
using System.Collections.Generic;
using System.Timers;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;

namespace WeaponWorkshop
{
    public class WeaponWorkshop : Script
    {
        private static readonly string NAME_WEAPONCHEST = "prop_mil_crate_01";
        private static readonly string TEXT_MENUBANNER = "Weapon Workshop";
        private static readonly string TEXT_MENUNAME = "Get Yourself Strapped Up!";
        private static readonly string TEXT_MENUDESCRIPTION = "Coded by ConcatSpirity";

        private readonly ObjectPool pool = new ObjectPool();
        private readonly NativeMenu menu = new NativeMenu(TEXT_MENUBANNER, TEXT_MENUNAME, TEXT_MENUDESCRIPTION);
        private readonly NativeItem menu_weaponPistol = new NativeItem("Pistol");
        private readonly NativeItem menu_weaponSMG = new NativeItem("SMG");
        private readonly NativeItem menu_weaponMicroSMG = new NativeItem("Micro SMG");
        private readonly NativeItem menu_weaponCarbineRifle = new NativeItem("Carbine Rifle");
        private readonly NativeItem menu_weaponAssaultRifle = new NativeItem("Assault Rifle");
        private readonly NativeItem menu_weaponHeavySniper = new NativeItem("Heavy Sniper");
        private readonly NativeItem menu_weaponShotgun = new NativeItem("Sawed-Off Shotgun");
        private readonly NativeItem menu_weaponMolotov = new NativeItem("Molotov");
        private readonly NativeItem menu_weaponBat = new NativeItem("Bat");

        private readonly Vector3 weaponChestLocation = new Vector3(-192.2758f, -1362.0439f, 30.7082f);

        private readonly List<NativeItem> menuItems = new List<NativeItem>();
        private readonly List<Prop> props = new List<Prop>();

        public WeaponWorkshop()
        {
            Tick += OnTick;
            Aborted += OnAbort;

            pool.Add(menu);
            CreateMenuWeaponItems();

            Model weaponChestModel = new Model(NAME_WEAPONCHEST);
            Prop weaponChest = World.CreateProp(weaponChestModel, weaponChestLocation, false, true);

            props.Add(weaponChest);

            props[0].Rotation = new Vector3(0.0f, 0.0f, 120.0f);

            props[0].AddBlip();
            props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
            props[0].AttachedBlip.Name = TEXT_MENUBANNER;
            props[0].AttachedBlip.IsShortRange = true;
        }

        private void GivePistol(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 80, true, true);
            menu.Remove(menu_weaponPistol);
            Notification.Show("Selected weapon ~b~Pistol");
        }

        private void GiveSMG(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.SMG, 165, true, true);
            menu.Remove(menu_weaponSMG);
            Notification.Show("Selected weapon ~b~SMG");
        }

        private void GiveMicroSMG(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.MicroSMG, 165, true, true);
            menu.Remove(menu_weaponMicroSMG);
            Notification.Show("Selected weapon ~b~Micro SMG");
        }

        private void GiveCarbineRifle(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifle, 250, true, true);
            menu.Remove(menu_weaponCarbineRifle);
            Notification.Show("Selected weapon ~b~Carbine Rifle");
        }

        private void GiveAssaultRifle(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 250, true, true);
            menu.Remove(menu_weaponAssaultRifle);
            Notification.Show("Selected weapon ~b~Assault Rifle");
        }

        private void GiveHeavySniper(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.HeavySniper, 50, true, true);
            menu.Remove(menu_weaponHeavySniper);
            Notification.Show("Selected weapon ~b~Heavy Sniper");
        }

        private void GiveShotgun(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.SawnOffShotgun, 60, true, true);
            menu.Remove(menu_weaponShotgun);
            Notification.Show("Selected weapon ~b~Sawed-Off Shotgun");
        }

        private void GiveMolotov(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Molotov, 5, true, true);
            menu.Remove(menu_weaponMolotov);
            Notification.Show("Selected weapon ~b~Molotov (x5)");
        }

        private void GiveBat(object sender, SelectedEventArgs e)
        {
            if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.Bat))
            {
                Notification.Show("You already have weapon ~b~Bat");
                return;
            }

            Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, true);
            menu.Remove(menu_weaponBat);
            Notification.Show("Selected weapon ~b~Bat");
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
            int poolSize = 5;
            var rand = new Random();

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

        private void OnAbort(object sender, EventArgs e)
        {
            props[0].AttachedBlip.Delete();
            props[0].Delete();
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();
            
            if (World.GetDistance(Game.Player.Character.Position, props[0].Position) < 3)
            {
                menu.Visible = true;
            }
            else
            {
                menu.Visible = false;
            }

            menu_weaponPistol.Selected += GivePistol;
            menu_weaponSMG.Selected += GiveSMG;
            menu_weaponMicroSMG.Selected += GiveMicroSMG;
            menu_weaponCarbineRifle.Selected += GiveCarbineRifle;
            menu_weaponAssaultRifle.Selected += GiveAssaultRifle;
            menu_weaponHeavySniper.Selected += GiveHeavySniper;
            menu_weaponShotgun.Selected += GiveShotgun;
            menu_weaponMolotov.Selected += GiveMolotov;
            menu_weaponBat.Selected += GiveBat;
        }
    }
}
