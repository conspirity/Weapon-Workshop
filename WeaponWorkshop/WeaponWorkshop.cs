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
        private const int HASH_PISTOL = 453432689;
        private const int HASH_SMG = 736523883;
        private const int HASH_MICROSMG = 324215364;
        private const int HASH_CARBINERIFLE = -2084633992;
        private const int HASH_BAT = -1786099057;
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
        private readonly bool devMode = true;
        public string ModName = "Weapon Workshop (.NET)";
        public string ModVersion = "0.0.1";
        public string ModMaker = "ConcatSpirity";

        public WeaponWorkshop()
        {
            Initialize();
            Tick += OnTick;
        }

        private static Model RequestModel(string prop)
        {
            var model = new Model(prop);
            model.Request(250);
            if (model.IsInCdImage && model.IsValid)
            {
                while (model.IsLoaded)
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

            Prop weaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), new Vector3(-209.037f, -1360.366f, 0), false, true);
            props.Add(weaponChest);
            props[0].AddBlip();
            props[0].AttachedBlip.Sprite = BlipSprite.Pistol;
            props[0].AttachedBlip.Name = TEXT_MENUBANNER;
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

            menu_weaponPistol.Selected += Menu_weaponPistol_Selected;
            menu_weaponSMG.Selected += Menu_weaponSMG_Selected;
            menu_weaponMicroSMG.Selected += Menu_weaponMicroSMG_Selected;
            menu_weaponCarbineRifle.Selected += Menu_weaponCarbineRifle_Selected;
            menu_weaponBat.Selected += Menu_weaponBat_Selected;
        }

        private void Menu_weaponPistol_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_PISTOL}", 200, true, true);
        }

        private void Menu_weaponSMG_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_SMG}", 200, true, true);
        }

        private void Menu_weaponMicroSMG_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_MICROSMG}", 200, true, true);
        }

        private void Menu_weaponCarbineRifle_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_CARBINERIFLE}", 200, true, true);
        }

        private void Menu_weaponBat_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_BAT}", 200, true, true);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }

    }
}
