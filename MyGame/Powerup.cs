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
    public enum PowerupTypes
    {
        NoneShot,
        SpreadShot,
        SpikeShot,
        MachineShot
    }

    class Powerup : GameObject
    {
        Random _rand = new Random();
        public Powerup(string Name, PowerupTypes Type)
            : base(Name + "_Powerup", 68, 68, "powerups.png", 3, 1, 3, 20f)
        {
            CollisionData.CollisionEnabled = true;
            CollisionData.SetCollisionData(33, 33);
            ZOrder = 4;
            Position.Y = _rand.Next(-512, 7680);
            Position.X = _rand.Next(-500, 500);

            AnimationData.GoToFrame((uint)Type);
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
