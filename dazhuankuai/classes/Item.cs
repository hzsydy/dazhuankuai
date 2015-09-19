using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using dazhuankuai.Properties;

namespace dazhuankuai.classes
{
    public class Item
    {
        Point Pos;
        static readonly int height = 30;
        static readonly int width = 30;
        int ItemFallFactor;
        int Item_No;
        int BuffleTop;
        static readonly SolidBrush bgbrush = new SolidBrush(Color.WhiteSmoke);
        public Item(Point pos, int itemno, int buffletop, int itemfallfactor)
        {
            Pos = pos;
            Item_No = itemno;
            BuffleTop = buffletop;
            ItemFallFactor = itemfallfactor;
        }
        public bool ItemFall()
        {
            Pos.Y += ItemFallFactor;
            return (Pos.Y > BuffleTop - height) ? true : false;
        }
        public int GetItemNo()
        {
            return Item_No;
        }
        public void DrawItem(Graphics graf)
        {
            Image i;
            switch (Item_No)
            {
                case 0: i = Resources.yellowitem1; break;
                case 1: i = Resources.itemgreen1; break;
                case 2: i = Resources.itemgreen2; break;
                case 3: i = Resources.itemgreen3; break;
                case 4: i = Resources.itemred1; break;
                case 5: i = Resources.itemred2; break;
                default: i = Resources.yellowitem1; break;
            }
            graf.DrawImage(i, Pos);
        }
        public void CleanItem(Graphics graf)
        {
            graf.FillRectangle(bgbrush, Pos.X, Pos.Y, width, height);
        }
        public bool ItemReceived(int buffleleft, int bufflewidth)
        {
            if (buffleleft <= Pos.X && Pos.X <= buffleleft + bufflewidth
                && buffleleft <= Pos.X + width && Pos.X + width <= buffleleft + bufflewidth) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
