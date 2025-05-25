// Copyright (c) 2025 ConcatSpirity

using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using GTATimers;
using LemonUI;
using LemonUI.Menus;

namespace WeaponWorkshop
{
    public class WeaponWorkshop : Script
    {
        private ScriptSettings _config;

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

        private int _ammoToGive_pistol;
        private int _ammoToGive_smg;
        private int _ammoToGive_rifle;
        private int _ammoToGive_shotgun;
        private int _ammoToGive_sniper;
        private int _ammoToGive_throw;

        private List<Prop> _props = new List<Prop>();

        private GTATimer _resupplyTimer;
        private int _resupplyInterval;
        private bool _isResupplyTimerSet;

        private bool _isInitialized;

        public WeaponWorkshop()
        {
            Tick += OnTick;
            Aborted += OnAbort;

            LoadIniFile("scripts//WeaponWorkshop.ini");
            _resupplyInterval = _config.GetValue("General", "resupply_interval", 900000);

            _ammoToGive_pistol = _config.GetValue("Ammo", "pistol", 80);
            _ammoToGive_smg = _config.GetValue("Ammo", "smg", 120);
            _ammoToGive_rifle = _config.GetValue("Ammo", "rifle", 250);
            _ammoToGive_shotgun = _config.GetValue("Ammo", "shotgun", 60);
            _ammoToGive_sniper = _config.GetValue("Ammo", "sniper", 40);
            _ammoToGive_throw = _config.GetValue("Ammo", "throw", 5);

            _pool.Add(_menu);
            CreateMenuWeaponItems();

            _resupplyTimer = new GTATimer("WeaponsResupplyTimer", _resupplyInterval);
            _resupplyTimer.OnTimerElapsed += Resupply;

            _menu_items_Pistol.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.Pistol, WeaponHash.Pistol, _ammoToGive_pistol, _menu_items_Pistol);
            _menu_items_SMG.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.SMG, WeaponHash.SMG, _ammoToGive_smg, _menu_items_SMG);
            _menu_items_MicroSMG.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.SMG, WeaponHash.MicroSMG, _ammoToGive_smg, _menu_items_MicroSMG);
            _menu_items_CarbineRifle.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.CarbineRifle, _ammoToGive_rifle, _menu_items_CarbineRifle);
            _menu_items_AssaultRifle.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.AssaultRifle, WeaponHash.AssaultRifle, _ammoToGive_rifle, _menu_items_AssaultRifle);
            _menu_items_HeavySniper.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.Sniper, WeaponHash.HeavySniper, _ammoToGive_sniper, _menu_items_HeavySniper);
            _menu_items_Shotgun.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.Shotgun, WeaponHash.SawnOffShotgun, _ammoToGive_shotgun, _menu_items_Shotgun);
            _menu_items_Molotov.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.Thrown, WeaponHash.Molotov, _ammoToGive_throw, _menu_items_Molotov);
            _menu_items_Bat.Activated += (object sender, EventArgs e)
                => GiveWeapon(WeaponGroup.Melee, WeaponHash.Bat, 1, _menu_items_Bat);
        }

        private void LoadIniFile(string iniFile)
        {
            try
            {
                _config = ScriptSettings.Load(iniFile);
            }
            catch
            {
                Notification.Show("~r~Error~w~: Failed to load WeaponWorkshop.ini");
            }
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

        private void GiveWeapon(WeaponGroup group, WeaponHash hash, int ammo, NativeItem menuItem)
        {
            var player_weapons = Game.Player.Character.Weapons;

            switch (group)
            {
                case WeaponGroup.Pistol:
                case WeaponGroup.SMG:
                case WeaponGroup.AssaultRifle:
                case WeaponGroup.Sniper:
                case WeaponGroup.Shotgun:
                case WeaponGroup.Thrown:
                    _menu.Remove(menuItem);
                    _activeMenuItems.Remove(menuItem);
                    player_weapons.Give(hash, 0, true, true);
                    player_weapons[hash].Ammo += ammo;
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
            _resupplyTimer.Reset();
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

            if (_isResupplyTimerSet)
            {
                _resupplyTimer.Stop();
                _resupplyTimer = null;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (Game.IsLoading) return;

            _pool.Process();

            if (!_isInitialized)
            {
                var weaponChest = World.CreateProp(RequestModel("prop_mil_crate_01"), new Vector3(-192.2758f, -1362.0439f, 30.7082f), new Vector3(0.0f, 0.0f, 120.0f), false, true);
                _props.Add(weaponChest);
                _props[0].AddBlip();
                _props[0].AttachedBlip.Sprite = BlipSprite.AmmuNation;
                _props[0].AttachedBlip.Name = "Weapon Workshop";
                _props[0].AttachedBlip.IsShortRange = true;

                _isInitialized = true;
            }

            if (!_isResupplyTimerSet)
            {
                _resupplyTimer.Start();
                _isResupplyTimerSet = true;
            }
            else
            {
                if (_resupplyTimer.Running) _resupplyTimer.Update();
            }

            if (World.GetDistance(Game.Player.Character.Position, _props[0].Position) < 3)
            {
                _menu.Visible = true;
            }
            else _menu.Visible = false;
        }
    }
}
