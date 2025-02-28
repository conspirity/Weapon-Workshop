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
        private ObjectPool _pool = new ObjectPool();
        private NativeMenu _menu = new NativeMenu("Weapon Workshop", "Get Yourself Strapped Up!", "");
        private static NativeItem _menu_items_Pistol = new NativeItem("Pistol");
        private static NativeItem _menu_items_SMG = new NativeItem("SMG");
        private static NativeItem _menu_items_MicroSMG = new NativeItem("Micro SMG");
        private static NativeItem _menu_items_CarbineRifle = new NativeItem("Carbine Rifle");
        private static NativeItem _menu_items_AssaultRifle = new NativeItem("Assault Rifle");
        private static NativeItem _menu_items_HeavySniper = new NativeItem("Heavy Sniper");
        private static NativeItem _menu_items_Shotgun = new NativeItem("Sawed-Off Shotgun");
        private static NativeItem _menu_items_Molotov = new NativeItem("Molotov");
        private static NativeItem _menu_items_Bat = new NativeItem("Bat");
        private List<NativeItem> _activeMenuItems = new List<NativeItem>();
        private NativeItem[] _allMenuItems =
        {
            _menu_items_Pistol,
            _menu_items_SMG,
            _menu_items_MicroSMG,
            _menu_items_CarbineRifle,
            _menu_items_AssaultRifle,
            _menu_items_HeavySniper,
            _menu_items_Shotgun,
            _menu_items_Molotov,
            _menu_items_Bat
        };
        private List<Prop> _props = new List<Prop>();
        private GTATimer _cTimer;
        private int _cTimerInterval = 900000;
        private bool _cTimerSet;
        private bool _initialized;

        public WeaponWorkshop()
        {
            Tick += OnTick;
            Aborted += OnAbort;

            _pool.Add(_menu);
            CreateMenuWeaponItems();

            _cTimer = new GTATimer("WeaponsResupplyTimer", _cTimerInterval);
            _cTimer.OnTimerElapsed += Resupply;

            _menu_items_Pistol.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.Pistol, WeaponHash.Pistol, _menu_items_Pistol);
            _menu_items_SMG.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.SMG, WeaponHash.SMG, _menu_items_SMG);
            _menu_items_MicroSMG.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.SMG, WeaponHash.MicroSMG, _menu_items_MicroSMG);
            _menu_items_CarbineRifle.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.CarbineRifle, _menu_items_CarbineRifle);
            _menu_items_AssaultRifle.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.AssaultRifle, _menu_items_AssaultRifle);
            _menu_items_HeavySniper.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.Sniper, WeaponHash.HeavySniper, _menu_items_HeavySniper);
            _menu_items_Shotgun.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.Shotgun, WeaponHash.SawnOffShotgun, _menu_items_Shotgun);
            _menu_items_Molotov.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.Thrown, WeaponHash.Molotov, _menu_items_Molotov);
            _menu_items_Bat.Activated += (object sender, EventArgs e) => GiveWeapon(WeaponGroup.Melee, WeaponHash.Bat, _menu_items_Bat);
        }

        // This function must only be called inside the main script loop
        private Model RequestModel(string prop)
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
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += 80;
                    break;

                case WeaponGroup.SMG:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += 165;
                    break;

                case WeaponGroup.AssaultRifle:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += 250;
                    break;

                case WeaponGroup.Sniper:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += 50;
                    break;

                case WeaponGroup.Shotgun:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += 60;
                    break;

                case WeaponGroup.Melee:
                    if (player_weapons.HasWeapon(hash))
                    {
                        Notification.Show("You already have this melee weapon");
                        break;
                    }
                    player_weapons.Give(hash, 1, true, true);
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    break;

                case WeaponGroup.Thrown:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    if (player_weapons.HasWeapon(hash))
                    {
                        player_weapons.Select(hash);
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
                int selectedItem = rand.Next(0, _allMenuItems.Length);
                if (_activeMenuItems.Contains(_allMenuItems[selectedItem]))
                {
                    i--;
                }
                else
                {
                    _activeMenuItems.Add(_allMenuItems[selectedItem]);
                }
            }

            foreach (var item in _activeMenuItems)
            {
                _menu.Add(item);
            }
        }

        private void Resupply()
        {
            _cTimer.Reset();
            _activeMenuItems.Clear();
            _menu.Clear();
            CreateMenuWeaponItems();
            Notification.Show("Weapon Workshop: Items have been restocked");
        }

        private void OnAbort(object sender, EventArgs e)
        {
            _pool.Remove(_menu);
            _props[0].AttachedBlip.Delete();
            _props[0].Delete();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.IsLoading) return;

            _pool.Process();

            if (!_initialized)
            {
                var weaponChest = World.CreateProp(RequestModel("prop_mil_crate_01"), new Vector3(-192.2758f, -1362.0439f, 30.7082f), false, true);
                _props.Add(weaponChest);
                _props[0].Rotation = new Vector3(0.0f, 0.0f, 120.0f);
                _props[0].AddBlip();
                _props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
                _props[0].AttachedBlip.Name = "Weapon Workshop";
                _props[0].AttachedBlip.IsShortRange = true;

                _initialized = true;
            }

            if (!_cTimerSet)
            {
                _cTimer.Start();
                _cTimerSet = true;
            }
            else
            {
                if (_cTimer.Running) _cTimer.Update();
            }

            if (World.GetDistance(Game.Player.Character.Position, _props[0].Position) < 3)
            {
                _menu.Visible = true;
            }
            else
            {
                _menu.Visible = false;
            }
        }
    }
}
