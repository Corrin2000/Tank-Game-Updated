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
    class Missile : EnemyBase
    {
        float speed = 6f;

        public Missile()
            : base(EnemyTypes.Missile, 20, 40, "Missile.png", 2, 1, 2, 0.1f)
        {
            CollisionData.SetCollisionData(10);
            AnimationData.Play();

            //SoundManager.AddSoundEffect("rocket", "Rocket.mp3");
            //SoundManager.PlaySoundEffect("rocket");
        }

        protected override void InitializePosition()
        {
            // Spawn close to the tank's Y
            Position.Y = Tank.TankInstance.Position.Y + rand.Next(-1000, 1000); ;

            // Spawn on either side of the track
            if (rand.Next(-1, 1) == -1)
            {
                Position.X = -600;
            }
            else
            {
                Position.X = 600;
            }
        }

        public override void Update()
        {
            base.Update();

            // Get unit vector to tank
            PointF VectorToTank = new PointF(Tank.TankInstance.Position.X - Position.X, Tank.TankInstance.Position.Y - Position.Y);
            float DistToTank = (float) Math.Sqrt(Math.Pow(VectorToTank.X,2) + Math.Pow(VectorToTank.Y, 2));
            if (DistToTank != 0)
            {
                VectorToTank.X /= -DistToTank;
                VectorToTank.Y /= -DistToTank;
            }
            
            // Move
            Position.X += VectorToTank.X * speed;
            Position.Y += VectorToTank.Y * speed;

            //Rotate towards the tank
            float angleRadians = (float) Math.Acos(VectorToTank.X);
            float angleDegrees = (float) MathHelper.DegreesToRadians(angleRadians);
            if (VectorToTank.Y <= 0)
            {
                angleDegrees *= -1;
            }
            Rotation = angleDegrees - 90;
        }

        protected override void HandleOnDeath()
        {
            base.HandleOnDeath();
            ObjectManager.AddGameObject(new MissileExplosion(Position));
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "TankBase")
            {
                ObjectManager.AddGameObject(new MissileExplosion(Position));
                //SoundManager.StopSoundEffect("rocket");
                IsDead=true;
            }
            else if (collisionInfo_.collidedWithGameObject.Name == "TankShell")
            {
                CurrentHealth--;
            }
        }
    }
}
