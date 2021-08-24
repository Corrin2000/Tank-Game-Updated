using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class MathHelper
    {
        public static readonly double DegreesToRadiansFactor = Math.PI / 180.0;

        public static double DegreesToRadians(double InDegrees)
        {
            return InDegrees * DegreesToRadiansFactor;
        }

        public static double RadiansToDegres(double InRadians)
        {
            return InRadians / DegreesToRadiansFactor;
        }

    }
}
