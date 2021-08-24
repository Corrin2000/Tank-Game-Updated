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
    class Helicopter : EnemyBase
    {
        int ShootTimer = 60;
        float speed = 1.5f;

        public Helicopter()
            : base(EnemyTypes.Helicopter, 64, 64, "ShootingEnemy.png", 2, 1, 2, 0.05f)
        {
            CollisionData.SetCollisionData(16);
            AnimationData.Play();
        }

        protected override void InitializePosition()
        {
            // Spawn close to the tank's Y
            Position.Y = Tank.TankInstance.Position.Y + rand.Next(-1000, 1000);

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

            --ShootTimer;


            // Get unit vector to tank
            PointF VectorToTank = new PointF(Tank.TankInstance.Position.X - Position.X, Tank.TankInstance.Position.Y - Position.Y);
            float DistToTank = (float) Math.Sqrt(Math.Pow(VectorToTank.X,2) + Math.Pow(VectorToTank.Y, 2));
            if (DistToTank != 0)
            {
                VectorToTank.X /= -DistToTank;
                VectorToTank.Y /= -DistToTank;
            }

            // Move
            if (DistToTank >= 202)
            {
                Position.X += VectorToTank.X * -speed;
                Position.Y += VectorToTank.Y * -speed;
            }
            else if (DistToTank <= 200)
            {
                Position.X += VectorToTank.X * speed;
                Position.Y += VectorToTank.Y * speed;
            }
            else if (DistToTank == 201)
            {
                Position.X += 0;
                Position.Y += 0;
            }

            // Move faster away than towards
            if (DistToTank <= 75)
            {
                speed = 2.5f;
            }
            else
            {
                speed = 1.5f;
            }

            // Shoot
            if (ShootTimer <= 0)
            {
                ObjectManager.AddGameObject(new EnemyBullet(Position, Rotation));
                ShootTimer = 90;
            }


            //Rotate towards the tank
            float angleRadians = (float) Math.Acos(VectorToTank.X);
            float angleDegrees = (float) MathHelper.RadiansToDegres(angleRadians);
            if (VectorToTank.Y <= 0)
            {
                angleDegrees *= -1;
            }
            Rotation = (angleDegrees - 90) - 180;
        }

        protected override void HandleOnDeath()
        {
            base.HandleOnDeath();
            ObjectManager.AddGameObject(new HelicopterExplosion(Position));
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);            
            if (collisionInfo_.collidedWithGameObject.Name == "TankShell")
            {                
                CurrentHealth--;                
            }
        }
    }
}
