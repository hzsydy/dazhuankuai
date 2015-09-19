using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace dazhuankuai.classes
{
    public class Ball
    {
        static readonly Color[] Color_lv = { Color.Black, Color.Blue, Color.Green, Color.Yellow, Color.Red };
        static readonly Color backcolor = Color.WhiteSmoke;
        static readonly SolidBrush[] SolidBrush_lv = { new SolidBrush(Color_lv[0]), new SolidBrush(Color_lv[1]),
                                       new SolidBrush(Color_lv[2]), new SolidBrush(Color_lv[3]),
                                       new SolidBrush(Color_lv[4])};
        static readonly SolidBrush ClearSolidBrush = new SolidBrush(backcolor);

        public Vector2 MoveDirection;
        public float Radius = 15.0f;
        public PointF CenterPositionF;
        public int lv = 0;


        public void DrawBall(Graphics graf)
        {
            graf.FillEllipse(SolidBrush_lv[lv], CenterPositionF.X, CenterPositionF.Y, Radius, Radius);
        }
        public void CleanBall(Graphics graf)
        {
            graf.FillRectangle(ClearSolidBrush, CenterPositionF.X, CenterPositionF.Y, Radius, Radius);
        }


    }
}
