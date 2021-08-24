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
    class Heart : GameObject
    {
        static Random rand = new Random();
        int randomspawnY;
        int randomspawnX;
        public Heart()
            : base("Heart", 32, 32, "Heart.png")
        {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(16);
            randomspawnY = rand.Next(-512, 7680);//-512, 7680
            randomspawnX = rand.Next(-475, 475);
            Position.X = randomspawnX;
            Position.Y = randomspawnY;
            SetModulationColor(1, 1, 1, 0);
        }
        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "TankBase")
            {
                GameObject HPickup = new HeartPickup(Position);
                ObjectManager.AddGameObject(HPickup);
                IsDead = true;
            }
            /*if (collisionInfo_.collidedWithGameObject.Name == "Bullet")
            {
                IsDead = true;
            }*/
        }
    }
}
