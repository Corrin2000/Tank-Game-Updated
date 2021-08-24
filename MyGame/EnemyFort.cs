using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;  

namespace MyGame
{
    class EnemyFort : EnemyBase
    {
        int stimer = 60;

        public EnemyFort()
            : base(EnemyTypes.Fort, 64, 64, "EnemyBase.png")
        { 
            CollisionData.SetCollisionData(32);
            ZOrder = -1;
        }

        protected override void InitializePosition()
        {
            Position.X = rand.Next(-500, 500);
            Position.Y = rand.Next(-512, 7680);
        }

        public override void Update()
        {
            base.Update();
            --stimer;
            if (stimer <= 0)
            {                
                stimer = 45;
                for(int rot = 0; rot <= 360; rot += 45)
                {
                    ObjectManager.AddGameObject(new EnemyBullet(Position, rot));
                }
            }
            
            if (!IsInViewport() && Tank.TankInstance.PlayerHealth <= 0)
            {
                IsDead = true;
            }
        }

        protected override void HandleOnDeath()
        {
            base.HandleOnDeath();
            ObjectManager.AddGameObject(new MineExplosion(Position));
        }

        public override void CollisionReaction(CollisionInfo collisionInfo_)
        {
            base.CollisionReaction(collisionInfo_);
            if (collisionInfo_.collidedWithGameObject.Name == "TankShell")
            {
                --CurrentHealth;
                if (CurrentHealth <= MaxHealth / 2)
                {
                    SetModulationColor(0, 0, 0, 0.5f);
                }
            }
        }
    }
}
