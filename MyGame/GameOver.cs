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
    class GameOver : State
    {
        public int finalscore=0;
        public override void Create()
        {
            base.Create();
            GameObject GameOver = new GameObject("GameOver", 1024, 768, "GameOver.png");
            ObjectManager.AddGameObject(GameOver);
            GameObject Restart = new RestartButton();
            ObjectManager.AddGameObject(Restart);
            //GameObject cursor = new startCursor();
            //ObjectManager.AddGameObject(cursor);
            TextObject Score = TextManager.AddText("Score", "Score: ", FontTypes.Arial64);
            Score.Text = "Total Score: " + Tank.TankInstance.Score.ToString();
        }
    }
}
