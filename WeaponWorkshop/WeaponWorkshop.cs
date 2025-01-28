// Copyright (c) 2024 ConcatSpirity

using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
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
        private readonly Vector3 chestLocation = new Vector3(-192.2758f, -1362.0439f, 30.7082f);
        private readonly List<NativeItem> menuItems = new List<NativeItem>();
        private readonly List<Prop> props = new List<Prop>();

        public WeaponWorkshop()
        {
            Initialize();
            Tick += OnTick;
            Aborted += OnAbort;
        }

        private void GivePistol(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.Pistol].Ammo += 80;
            menu.Remove(menu_weaponPistol);
            Notification.Show("Selected weapon ~b~Pistol");
        }

        private void GiveSMG(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.SMG, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.SMG].Ammo += 165;
            menu.Remove(menu_weaponSMG);
            Notification.Show("Selected weapon ~b~SMG");
        }

        private void GiveMicroSMG(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.MicroSMG, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.MicroSMG].Ammo += 165;
            menu.Remove(menu_weaponMicroSMG);
            Notification.Show("Selected weapon ~b~Micro SMG");
        }

        private void GiveCarbineRifle(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifle, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.CarbineRifle].Ammo += 250;
            menu.Remove(menu_weaponCarbineRifle);
            Notification.Show("Selected weapon ~b~Carbine Rifle");
        }

        private void GiveAssaultRifle(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.AssaultRifle, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.AssaultRifle].Ammo += 250;
            menu.Remove(menu_weaponAssaultRifle);
            Notification.Show("Selected weapon ~b~Assault Rifle");
        }

        private void GiveHeavySniper(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.HeavySniper, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.HeavySniper].Ammo += 50;
            menu.Remove(menu_weaponHeavySniper);
            Notification.Show("Selected weapon ~b~Heavy Sniper");
        }

        private void GiveShotgun(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.SawnOffShotgun, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.SawnOffShotgun].Ammo += 60;
            menu.Remove(menu_weaponShotgun);
            Notification.Show("Selected weapon ~b~Sawed-Off Shotgun");
        }

        private void GiveMolotov(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Molotov, 0, true, true);
            Game.Player.Character.Weapons[WeaponHash.Molotov].Ammo += 5;
            menu.Remove(menu_weaponMolotov);
            Notification.Show("Selected weapon ~b~Molotov (x5)");
        }

        private void GiveBat(object sender, EventArgs e)
        {
            if (!Game.Player.Character.Weapons.HasWeapon(WeaponHash.Bat))
            {
                Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, true);
                menu.Remove(menu_weaponBat);
                Notification.Show("Selected weapon ~b~Bat");
            }
            else
            {
                Notification.Show("You already have weapon ~b~Bat");
            }
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

        private void Initialize()
        {
            pool.Add(menu);
            CreateMenuWeaponItems();

            Model chestModel = new Model(NAME_WEAPONCHEST);
            Prop weaponChest = World.CreateProp(chestModel, chestLocation, false, true);
            props.Add(weaponChest);

            props[0].Rotation = new Vector3(0.0f, 0.0f, 0.0f);
            props[0].AddBlip();
            props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
            props[0].AttachedBlip.Name = TEXT_MENUBANNER;
            props[0].AttachedBlip.IsShortRange = true;
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
            menu_weaponPistol.Activated += GivePistol;
            menu_weaponSMG.Activated += GiveSMG;
            menu_weaponMicroSMG.Activated += GiveMicroSMG;
            menu_weaponCarbineRifle.Activated += GiveCarbineRifle;
            menu_weaponAssaultRifle.Activated += GiveAssaultRifle;
            menu_weaponHeavySniper.Activated += GiveHeavySniper;
            menu_weaponShotgun.Activated += GiveShotgun;
            menu_weaponMolotov.Activated += GiveMolotov;
            menu_weaponBat.Activated += GiveBat;
        }
    }
}
