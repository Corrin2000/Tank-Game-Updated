using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Engine;
namespace MyGame
{
    class MainMenu : State
    {
        public override void Create()
        {
            base.Create();
            //create a background
            GameObject background = new GameObject("MainMenu", 1024, 768, "MainMenu.png");
            ObjectManager.AddGameObject(background);
            GameObject title = new GameObject("Title", 1024, 512, "Title.png");
            ObjectManager.AddGameObject(title);
            title.Position.Y = 100;
            //title.Position.X = 50;

            //create a cursor
            GameObject Cursor = new cursor();
            ObjectManager.AddGameObject(Cursor);

            GameObject sbutton = new StartButton();
            ObjectManager.AddGameObject(sbutton);
            GameObject qbutton = new QuitButton();
            ObjectManager.AddGameObject(qbutton);

            //GameStateManager.GotoState(new Main_Game());
        }
    }
}
