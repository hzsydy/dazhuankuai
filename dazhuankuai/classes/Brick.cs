using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using dazhuankuai.Properties;

namespace dazhuankuai.classes
{
    public class Brick
    {
        static readonly Color[] Color_lv = {Color.Black, Color.Gray, Color.Brown, Color.Blue, Color.Red};
        static readonly Color backcolor = Color.WhiteSmoke;
        //static readonly SolidBrush[] SolidBrush_lv = { new SolidBrush(Color_lv[0]), new SolidBrush(Color_lv[1]),
        //                               new SolidBrush(Color_lv[2]), new SolidBrush(Color_lv[3]),
        //                               new SolidBrush(Color_lv[4])};
        static readonly SolidBrush ClearSolidBrush = new SolidBrush(backcolor);
        static readonly Pen borderpen = new Pen(backcolor, 2.0f);
        public Rectangle Pos;
        int lv;
        public Brick(int level, Rectangle pos)
        {
            lv = level;
            Pos = pos;
        }
        public void DrawBrick(Graphics graf)
        {
            Point p = Pos.Location;
            Image i = Resources.emptybrick;
            switch (lv)
            {
                case 0: i = Resources.brick1; break;
                case 1: i = Resources.brick2; break;
                case 2: i = Resources.brick3; break;
                case 3: i = Resources.brick4; break;
                case 4: i = Resources.brick5; break;
            }
            graf.DrawImage(i, p);
            graf.DrawRectangle(borderpen, Pos);
        }
        public void CleanBrick(Graphics graf)
        {
            graf.FillRectangle(ClearSolidBrush, Pos);
        }
        public bool HitBrick()
        {
            if (lv > 0 && lv < 4)
            {
                lv--;
                return false;
            } 
            else if (lv == 4)
            {
                if (GetRandom.GetRandomFloat()<0.1f)
                {
                    return true;
                } 
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
