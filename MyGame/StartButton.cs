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
    class StartButton : GameObject
    {
        bool ButtonCheck;
         public StartButton()
            : base("StartButton", 256, 128, "SButton.png", 2, 1, 2, .1f)
    {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(128, 64);
            Position = new PointF(-150, -200);
            AnimationData.GoToFrame(0);
        }
         public override void Update()
         {
             base.Update();
             if (InputManager.IsTriggered(Keys.LButton) && ButtonCheck == true)
             {
                 GameStateManager.GotoState(new Main_Game());
             }
             ButtonCheck = false;
             AnimationData.GoToFrame(0);

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
