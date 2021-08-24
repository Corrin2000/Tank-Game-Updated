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
    class EnemyBullet : ProjectileBase
    {
        public EnemyBullet(PointF InPosition, float InRotation)
            : base(InPosition, InRotation, "Bullet", 14, 14, "Bullet.png")
        {            
            CollisionData.SetCollisionData(4);

            //SoundManager.AddSoundEffect("fire", "shoot.mp3");
            if (IsInViewport())
            {
                //SoundManager.PlaySoundEffect("fire");
            }
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "TankBase")
            {
                IsDead = true;
            }
        }
    }
}
