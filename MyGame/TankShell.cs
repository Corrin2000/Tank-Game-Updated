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
    class TankShell : ProjectileBase
    {
        public TankShell(PointF InPosition, float InRotation)
            : base(InPosition, InRotation, "TankShell", 24, 20, "TankShell.png", 3, 1, 3, 0.3f)
        {
            ZOrder = -1;            
            CollisionData.SetCollisionData(4);
            BaseSpeed = 10;

            //SoundManager.AddSoundEffect("shoot", "TankShot.mp3");
            //SoundManager.PlaySoundEffect("shoot");
        }
        public override void Update()
        {
            base.Update();

            Scale.X = Tank.TankInstance.PowerLevel;
            Scale.Y = Tank.TankInstance.PowerLevel;
            if (Tank.TankInstance.Power == PowerupTypes.SpikeShot)
            {
                CollisionData.SetCollisionData(10 * Tank.TankInstance.PowerLevel);
                AnimationData.GoToAndPlay(1);
            }
            else
            {
                CollisionData.SetCollisionData(4);
                AnimationData.GoToFrame(0);
            }
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if(Tank.TankInstance.Power != PowerupTypes.SpikeShot)
            {
                foreach(EnemyTypes EnemyType in EnemyBase.prototypes.Keys)
                {
                    if (collisionInfo_.collidedWithGameObject.Name.Equals(EnemyType.ToString()))
                    {
                        IsDead = true;
                    }
                }
            }
        }
    }
}
