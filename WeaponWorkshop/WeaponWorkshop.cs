using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;

namespace WeaponWorkshop
{
    public partial class WeaponWorkshop : Script
    {
        private readonly ObjectPool pool = new ObjectPool();
        private readonly NativeMenu menu = new NativeMenu("Weapon Workshop", "WW", "Welcome to weapon workshop!");
        private readonly Keys openMenuKey;

        public WeaponWorkshop()
        {
  
        }

        private void Initialize()
        {
            pool.Add(menu);
            Tick += OnTick;
            KeyUp += OnKeyUp;
            KeyDown += OnKeyDown;
        }

        private void OnTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }

    }
}
