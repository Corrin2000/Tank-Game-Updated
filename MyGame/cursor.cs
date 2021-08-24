using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    
    class cursor : GameObject
    {
       public cursor()
            : base("Cursor", 32, 32, "Bullet.png")
        {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(16);
            ZOrder = 22;
        }
        public override void Update()
        {
            base.Update();
            Position.Y = -InputManager.MousePosition.Y + Game.WindowHeight / 2 + Camera.Position.Y;
            Position.X = InputManager.MousePosition.X - Game.WindowWidth / 2;
            System.Windows.Forms.Cursor.Hide();
        }

        //Pause Menu Code - transfer to new cursor
        /*public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            GameObject obj = collisionInfo_.collidedWithGameObject;
            if (obj.Name == "resume")
            {
                //(lvl as Main_Game).deletePauseScreen();
                ObjectManager.UnpauseAllObjects();
                //(lvl as Main_Game).paused = false;
            }
            if (obj.Name == "exit")
            {
                Game.quit = true;
            }
            if (obj.Name == "restart")
            {
                GameStateManager.GotoState(new Main_Game());
            }
        }*/

    }
}
