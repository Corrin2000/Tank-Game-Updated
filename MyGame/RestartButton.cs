using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using System.Drawing;

namespace MyGame
{
    class RestartButton : GameObject
    {
        bool ButtonCheck;
        public RestartButton()
            : base("RestartButton", 256, 128, "RestartButton.png", 2, 1, 2, .1f)
        {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(256, 128);
            Position.Y = -200;
            ZOrder = 21;
        }
        public override void Update()
        {
            base.Update();
            AnimationData.GoToFrame(0);
            if (InputManager.IsTriggered(Keys.LButton) && ButtonCheck == true)
            {
                GameStateManager.GotoState(new Main_Game());
            }
            ButtonCheck = false;
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "Cursor")
            {
                AnimationData.GoToFrame(1);
                ButtonCheck = true;
            }
        }
    }
}
