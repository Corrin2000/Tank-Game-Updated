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
    class MineExplosion : GameObject
    {
        public MineExplosion(PointF position)
            : base("mineExplosion", 60, 45, "mineExplode.png"/*Fix Later*/, 5, 1, 5, 0.15f)
        {
            Position = position;
            AnimationData.Play();
            //ZOrder = 100000;
        }
        public override void Update()
        {
            base.Update();
            if (AnimationData.CurrentFrame == 4)
            {
                IsDead = true;
            }
        }
    }
}
