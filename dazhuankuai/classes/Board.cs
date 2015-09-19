using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace dazhuankuai.classes
{
    public class Board
    {
        static readonly Color[] Color_lv = {Color.Black, Color.Gray, Color.Brown, Color.Blue, Color.Red};
        static readonly Color backcolor = Color.WhiteSmoke;
        static readonly SolidBrush[] SolidBrush_lv = { new SolidBrush(Color_lv[0]), new SolidBrush(Color_lv[1]),
                                       new SolidBrush(Color_lv[2]), new SolidBrush(Color_lv[3]),
                                       new SolidBrush(Color_lv[4])};
        static readonly SolidBrush ClearSolidBrush = new SolidBrush(backcolor);
        static readonly Pen borderpen = new Pen(backcolor, 2.0f);

        static readonly Int32 height = 10;
        public Int32 left;
        public Int32 top;
        public Int32 width;
        int lv;
        public Board(int level, Int32 left_in, Int32 top_in, Int32 width_in)
        {
            lv = level;
            left = left_in;
            top = top_in;
            width = width_in;
        }
        public void DrawBoard(Graphics graf)
        {
            Rectangle Pos = new Rectangle(left, top, width, height);
            graf.FillRectangle(SolidBrush_lv[lv], Pos);
        }
        public void CleanBoard(Graphics graf)
        {
            Rectangle Pos = new Rectangle(left, top, width, height);
            graf.FillRectangle(ClearSolidBrush, Pos);
        }
        public void AddWidth(Int32 d_width)
        {
            width += d_width;
        }
    }
}
