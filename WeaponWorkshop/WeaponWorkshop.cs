// Copyright (c) 2024 ConcatSpirity

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
        private static readonly string TEXT_MENUDESC = "Coded by ConcatSpirity (c)";
        private static readonly string TEXT_MENUITEM_PISTOL = "Pistol";
        private static readonly string TEXT_MENUITEM_SMG = "SMG";
        private static readonly string TEXT_MENUITEM_MICROSMG = "Micro SMG";
        private static readonly string TEXT_MENUITEM_CARBINERIFLE = "Carbine Rifle";
        private static readonly string TEXT_MENUITEM_BAT = "Bat";
        private readonly ObjectPool pool = new ObjectPool();
        private readonly NativeMenu menu = new NativeMenu(TEXT_MENUBANNER, TEXT_MENUNAME, TEXT_MENUDESC);
        private readonly NativeItem menu_weaponPistol = new NativeItem(TEXT_MENUITEM_PISTOL);
        private readonly NativeItem menu_weaponSMG = new NativeItem(TEXT_MENUITEM_SMG);
        private readonly NativeItem menu_weaponMicroSMG = new NativeItem(TEXT_MENUITEM_MICROSMG);
        private readonly NativeItem menu_weaponCarbineRifle = new NativeItem(TEXT_MENUITEM_CARBINERIFLE);
        private readonly NativeItem menu_weaponBat = new NativeItem(TEXT_MENUITEM_BAT);
        private readonly List<Prop> props = new List<Prop>();
        private Prop WeaponChest { get; set; }
        private readonly bool devMode = true;
        public string ModName = "Weapon Workshop (.NET)";
        public string ModVersion = "0.0.1";
        public string ModMaker = "ConcatSpirity";

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

        private void Initialize()
        {
            if (devMode)
            {
                Notification.Show($"{ModName} {ModVersion}, coded by {ModMaker}", false);
            }

            // Initialize the weapon workshop menu and the menu items
            pool.Add(menu);
            menu.Add(menu_weaponPistol);
            menu.Add(menu_weaponSMG);
            menu.Add(menu_weaponMicroSMG);
            menu.Add(menu_weaponCarbineRifle);
            menu.Add(menu_weaponBat);
            menu.Visible = false;

            WeaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), new Vector3(-192.2758f, -1362.0439f, 30.7082f), false, true);
            WeaponChest.AddBlip();
            WeaponChest.AttachedBlip.Sprite = BlipSprite.AmmuNation;
            WeaponChest.AttachedBlip.Name = TEXT_MENUBANNER;
        }

        private void OnAbort(object sender, EventArgs e)
        {
            WeaponChest.AttachedBlip.Delete();
            WeaponChest.Delete();
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();
            
            if (World.GetDistance(Game.Player.Character.Position, props[0].Position) < 1)
            {
                menu.Visible = true;
            }
            else
            {
                menu.Visible = false;
            }

            menu_weaponPistol.Activated += Menu_weaponPistol_Activated;
            menu_weaponSMG.Activated += Menu_weaponSMG_Activated;
            menu_weaponMicroSMG.Activated += Menu_weaponMicroSMG_Activated;
            menu_weaponCarbineRifle.Activated += Menu_weaponCarbineRifle_Activated;
            menu_weaponBat.Activated += Menu_weaponBat_Activated;
        }

        private void Menu_weaponPistol_Activated(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Pistol, 200, true, true);
        }

        private void Menu_weaponSMG_Activated(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.SMG, 200, true, true);
        }

        private void Menu_weaponMicroSMG_Activated(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.MicroSMG, 200, true, true);
        }

        private void Menu_weaponCarbineRifle_Activated(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.CarbineRifle, 200, true, true);
        }

        private void Menu_weaponBat_Activated(object sender, EventArgs e)
        {
            Game.Player.Character.Weapons.Give(WeaponHash.Bat, 1, true, true);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }

    }
}
