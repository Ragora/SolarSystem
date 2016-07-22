using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystem
{
    public class RandomHelper
    {
        private static Random Rng;

        public static int RandomInt(int min, int max)
        {
            if (Rng == null)
                Rng = new Random();

            return Rng.Next(min, max);
        }
        public static float RandomFloat(float min, float max)
        {
            if (Rng == null)
                Rng = new Random();

            return Rng.Next((int)min, (int)max);
        }
    }
}
