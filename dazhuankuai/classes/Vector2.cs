using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dazhuankuai.classes
{
    public class Vector2
    {
        protected float VectorX, VectorY;

        public Vector2(float SetVectorX, float SetVectorY)
        {
            VectorX = SetVectorX;
            VectorY = SetVectorY;
        }

        public float X
        {
            get
            {
                return VectorX;
            }
            set
            {
                VectorX = value;
            }
        }
        public float Y
        {
            get
            {
                return VectorY;
            }
            set
            {
                VectorY = value;
            }
        }
        public static Vector2 GetRandomVector2(float minX, float maxX, float minY, float maxY)
        {
            Vector2 v = new Vector2(GetRandom.GetRandomFloat(minX, maxX), GetRandom.GetRandomFloat(minY, maxY));
            return v;
        }
    }
}
