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
    class Mine : EnemyBase
    {
        int _TimerExplosionDelayAfterSpawn = 60;
        int BeepTimer = 60;

        public Mine()
            : base(EnemyTypes.Mine, 40, 20, "LandMine.png", 2, 1, 2, 0.5f)
        {
            CollisionData.SetCollisionData(40, 20);          
            AnimationData.Play();
            //Position.Y = 100;
            //SoundManager.AddSoundEffect("beep", "Beep.mp3");
        }
        protected override void InitializePosition()
        {
            Position.Y = rand.Next(-512, 7680);//-512, 7680
            Position.X = rand.Next(-475, 475);
        }

        public override void Update()
        {
            base.Update();
            TimerExplosionDelayAfterSpawn--;

            // Beep
            BeepTimer--;
            if(BeepTimer==0)
            {
                BeepTimer = 60;
                if(IsInViewport())
                {
                    //SoundManager.PlaySoundEffect("beep");
                }
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
            if (collisionInfo_.collidedWithGameObject.Name == "TankBase")
            {
                if (TimerExplosionDelayAfterSpawn < 0)
                {
                    ObjectManager.AddGameObject(new MineExplosion(Position));
                }
                IsDead = true;
                
            }
            else if (collisionInfo_.collidedWithGameObject.Name == "TankShell")
            {
                CurrentHealth--;
            }            
        }

        public int TimerExplosionDelayAfterSpawn {
            get { return _TimerExplosionDelayAfterSpawn; }
            set { _TimerExplosionDelayAfterSpawn = Math.Min(0, value); }
        }

    }
}
