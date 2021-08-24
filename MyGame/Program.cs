using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace MyGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Game.Initialize(1024, 768, 60, new MainMenu());

            //Run a game loop to keep the game running
            while (Game.quit == false)
            {
                Application.DoEvents();

                Game.Update();
            }
        }
    }
}
