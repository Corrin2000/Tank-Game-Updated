using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace MyGame
{
    abstract class ProjectileBase : GameObject
    {
        protected double BaseSpeed = 8;

        public ProjectileBase(PointF InPosition, float InRotation, string name_, uint objectWidth_, uint objectHeight_, string textureName_,
            uint totalFrames_ = 1, uint numberOfRows_ = 1, uint numberOfColumns_ = 1, float frameLifetime_ = 1)
            : base(name_, objectWidth_, objectHeight_, textureName_, totalFrames_, numberOfRows_, numberOfColumns_, frameLifetime_)
        {
            Position = InPosition;
            Rotation = InRotation;

            CollisionData.CollisionEnabled = true;
        }

        public override void Update()
        {
            base.Update();

            Position.Y += (float)(BaseSpeed * Math.Cos(MathHelper.DegreesToRadians(Rotation)));
            Position.X += (float)(-1 * BaseSpeed * Math.Sin(MathHelper.DegreesToRadians(Rotation)));

            if (!IsInViewport())
            {
                IsDead = true;
            }
        }

    }
}
