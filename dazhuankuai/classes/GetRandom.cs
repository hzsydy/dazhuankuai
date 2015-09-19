using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dazhuankuai.classes
{
    public class GetRandom
    {
        public static Random globalRandomGenerator = GenerateNewRandomGenerator();
        public static Random GenerateNewRandomGenerator()
        {
            globalRandomGenerator = new Random((int)DateTime.Now.Ticks);
            return globalRandomGenerator;
        }

        public static int GetRandomInt(int MaxInt)
        {
            return globalRandomGenerator.Next(MaxInt);
        }
        public static int GetRandomInt()
        {
            return globalRandomGenerator.Next(10);
        }
        public static float GetRandomFloat(float min, float max)
        {
            return (float)globalRandomGenerator.NextDouble() * (max - min) + min;
        }
        public static float GetRandomFloat()
        {
            return (float)globalRandomGenerator.NextDouble();
        }
    }
}
