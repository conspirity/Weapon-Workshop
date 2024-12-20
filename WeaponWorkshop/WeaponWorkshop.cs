using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
        private const int HASH_PISTOL = 453432689;
        private const int HASH_SMG = 736523883;
        private const int HASH_MICROSMG = 324215364;
        private const int HASH_CARBINERIFLE = -2084633992;
        private const int HASH_BAT = -1786099057;
        private static readonly string NAME_WEAPONCHEST = "prop_mil_crate_01";
        private static readonly string TEXT_MENUBANNER = "Weapon Workshop";
        private static readonly string TEXT_MENUNAME = "Get Yourself Strapped Up!";
        private static readonly string TEXT_MENUDESC = "Made by ConcatSpirity (c)";
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
        private readonly Keys openMenuKey;
        private Blip workshopBlip;

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
            // Initialize the weapon workshop menu and the menu items
            pool.Add(menu);
            menu.Add(menu_weaponPistol);
            menu.Add(menu_weaponSMG);
            menu.Add(menu_weaponMicroSMG);
            menu.Add(menu_weaponCarbineRifle);
            menu.Add(menu_weaponBat);
            menu.Visible = false;

            Prop weaponChest = World.CreateProp(RequestModel(NAME_WEAPONCHEST), Game.Player.Character.GetOffsetPosition(new Vector3(0, 5f, 0)), false, true);
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
        }

        private void Menu_weaponPistol_Selected(object sender, SelectedEventArgs e)
        {
            Game.Player.Character.Weapons.Give($"{HASH_PISTOL}", 200, true, true);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }

    }
}
