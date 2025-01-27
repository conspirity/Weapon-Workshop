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
        private readonly NativeItem menu_weaponPistol = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.Pistol));
        private readonly NativeItem menu_weaponSMG = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.SMG));
        private readonly NativeItem menu_weaponMicroSMG = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.MicroSMG));
        private readonly NativeItem menu_weaponCarbineRifle = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.CarbineRifle));
        private readonly NativeItem menu_weaponAssaultRifle = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.AssaultRifle));
        private readonly NativeItem menu_weaponHeavySniper = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.HeavySniper));
        private readonly NativeItem menu_weaponShotgun = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.SawnOffShotgun));
        private readonly NativeItem menu_weaponMolotov = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.Molotov));
        private readonly NativeItem menu_weaponBat = new NativeItem(GTA.Weapon.GetDisplayNameFromHash(WeaponHash.Bat));
        private readonly List<NativeItem> menuItems = new List<NativeItem>();
        private readonly List<Prop> props = new List<Prop>();

        public WeaponWorkshop()
        {
            Initialize();
            Tick += OnTick;
            Aborted += OnAbort;
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

        private EventHandler GiveWeapon(GTA.WeaponHash wepHash, GTA.WeaponGroup wepGroup, NativeItem menuItem)
        {
            if (wepGroup == WeaponGroup.Melee && Game.Player.Character.Weapons.HasWeapon(wepHash))
            {
                Notification.Show("You already have this melee weapon");
                return null;
            }
            else if (wepGroup == WeaponGroup.Pistol)
            {
                Game.Player.Character.Weapons.Give(wepHash, 0, true, true);
                Game.Player.Character.Weapons[wepHash].Ammo += 100;
            }
            else if (wepGroup == WeaponGroup.SMG)
            {
                Game.Player.Character.Weapons.Give(wepHash, 0, true, true);
                Game.Player.Character.Weapons[wepHash].Ammo += 200;
            }
            else if (wepGroup == WeaponGroup.AssaultRifle)
            {
                Game.Player.Character.Weapons.Give(wepHash, 0, true, true);
                Game.Player.Character.Weapons[wepHash].Ammo += 300;
            }
            else if (wepGroup == WeaponGroup.Shotgun || wepGroup == WeaponGroup.Sniper)
            {
                Game.Player.Character.Weapons.Give(wepHash, 0, true, true);
                Game.Player.Character.Weapons[wepHash].Ammo += 70;
            }
            else if (wepGroup == WeaponGroup.Thrown)
            {
                Game.Player.Character.Weapons.Give(wepHash, 0, true, true);
                Game.Player.Character.Weapons[wepHash].Ammo += 5;
            }

            menu.Remove(menuItem);

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

            Prop weaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), new Vector3(-192.2758f, -1362.0439f, 30.7082f), false, true);
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

            menu_weaponPistol.Activated += GiveWeapon(WeaponHash.Pistol, WeaponGroup.Pistol, menu_weaponPistol);
            menu_weaponSMG.Activated += GiveWeapon(WeaponHash.SMG, WeaponGroup.SMG, menu_weaponSMG);
            menu_weaponMicroSMG.Activated += GiveWeapon(WeaponHash.MicroSMG, WeaponGroup.SMG, menu_weaponMicroSMG);
            menu_weaponCarbineRifle.Activated += GiveWeapon(WeaponHash.CarbineRifle, WeaponGroup.AssaultRifle, menu_weaponCarbineRifle);
            menu_weaponAssaultRifle.Activated += GiveWeapon(WeaponHash.AssaultRifle, WeaponGroup.AssaultRifle, menu_weaponAssaultRifle);
            menu_weaponHeavySniper.Activated += GiveWeapon(WeaponHash.HeavySniper, WeaponGroup.Sniper, menu_weaponHeavySniper);
            menu_weaponShotgun.Activated += GiveWeapon(WeaponHash.SawnOffShotgun, WeaponGroup.Shotgun, menu_weaponShotgun);
            menu_weaponMolotov.Activated += GiveWeapon(WeaponHash.Molotov, WeaponGroup.Thrown, menu_weaponMolotov);
            menu_weaponBat.Activated += GiveWeapon(WeaponHash.Bat, WeaponGroup.Melee, menu_weaponBat);
        }
    }
}
