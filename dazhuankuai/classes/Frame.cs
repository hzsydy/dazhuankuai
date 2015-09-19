using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace dazhuankuai.classes
{
    public class Frame
    {
        #region 基本全局变量
        Rectangle Pos;
        Board buffle;
        List<Brick> bricks = new List<Brick>();
        List<Ball> balls = new List<Ball>();
        List<Item> items = new List<Item>();

        float BallMoveFactor;
        int BuffleMoveDistance;
        bool canmove = false;
        bool gamerunning = false;
        int bricks_remained = 0;
        int ItemFallFactor;

        int BuffleTop;
        static readonly int brickwidth = 50;
        static readonly int brickheight = 20;
        static readonly int itemwidth = 30;
        int BuffleWidth = 80;
        float Radius = 15.0f;


        
        #endregion
        #region 特殊全局变量

        static readonly Int32 BuffleLengthChangeFactor = 15;
        static readonly float MoveChangeFactor = 1.0f;

        int Energy = 0;//

        #endregion
        public Frame(Rectangle pos)
        {
            Pos = pos;
        }

        #region 基本实现
        public void Init(int lv)
        {
            //def global values
            BuffleTop = Pos.Height - 30;
            BallMoveFactor = 6.0f + 1.0f * lv;
            BuffleMoveDistance = 15 + 3 * lv;
            ItemFallFactor = 4 + 1 * lv;
            Energy = 0;
            canmove = false;
            gamerunning = false;
            //add buffle
            buffle = new Board(0, Pos.Width / 2 - BuffleWidth / 2, BuffleTop, BuffleWidth);
            //add first ball
            balls.Clear();
            Ball thisball = new Ball();
            thisball.CenterPositionF = new PointF(buffle.left + BuffleWidth / 2, BuffleTop - Radius);
            thisball.MoveDirection = Vector2.GetRandomVector2(0.9f, 1.0f, -0.9f, -1.0f);
            thisball.lv = 0;
            balls.Add(thisball);
            //add bricks
            bricks.Clear();
            bricks_remained = 0;

            int bricktotalheight = 8 + lv;
            bricktotalheight = (bricktotalheight > 12) ? 12 : bricktotalheight;

            for (int i = 0; i < Pos.Width / brickwidth; i++)
            {
                for (int j = 0; j < bricktotalheight; j++)
                {
                    if (GetRandom.GetRandomFloat() <= 0.5f)
                    {
                        int lv_rnd = lv + 1;
                        lv_rnd = lv_rnd > 5 ? 5 : lv_rnd;
                        lv_rnd = GetRandom.GetRandomInt(lv_rnd);

                        Brick newbrick = new Brick(lv_rnd, new Rectangle(i * brickwidth, j * brickheight, brickwidth, brickheight));
                        bricks.Add(newbrick);
                        if (lv_rnd != 4)
                        {
                            bricks_remained++;
                        }

                    }
                }
            }
            //clear items
            items.Clear();
        }
        public Bitmap DrawFrame(FrmMain frm)
        {
            Bitmap bufferimage = new Bitmap(Pos.Width, Pos.Height);
            Graphics graf = Graphics.FromImage(bufferimage);
            graf.Clear(frm.BackColor);
            //draw buffle
            buffle.DrawBoard(graf);
            //draw ball
            foreach (Ball thisball in balls)
            {
                thisball.DrawBall(graf);
            }
            //draw brick
            foreach (Brick thisbrick in bricks)
            {
                thisbrick.DrawBrick(graf);
            }
            //draw items
            foreach (Item thisitem in items)
            {
                thisitem.DrawItem(graf);
            }
            graf.Dispose();
            return bufferimage;
            
        }
        public void GoLeft()
        {
            if (canmove && buffle.left >= BuffleMoveDistance)
            {
                buffle.left -= BuffleMoveDistance;
                canmove = false;
            }
        }
        public void GoRight()
        {
            if (canmove && buffle.left <= Pos.Width - BuffleWidth - BuffleMoveDistance)
            {
                buffle.left += BuffleMoveDistance;
                canmove = false;
            }
        }
        public void EnableMove()
        {
            canmove = true;
        }
        public void EnableGame()
        {
            gamerunning = true;
        }
        public int UpdateData(FrmMain frm)
        {
            //main
            Ball thisball;
            int ballnum = balls.Count();
            List<Ball> balltokill = new List<Ball>();
            for (int i = 0; i < ballnum; i++)
            {
                thisball = balls[i];
                for (float newX = thisball.CenterPositionF.X, newY = thisball.CenterPositionF.Y, t = 0;
                    (t <= BallMoveFactor) && gamerunning;
                    newX += (0.002f * thisball.MoveDirection.X),
                    newY += (0.002f * thisball.MoveDirection.Y),
                    t += 0.002f)
                {
                    thisball.CenterPositionF = new PointF(newX, newY);
                    if (this.CheckCollision(thisball))
                    {
                        //ball caught collision
                        ;
                    }
                    if (!gamerunning)
                    {
                        if (bricks_remained == 0)
                        {
                            //win
                            return 1;
                        }
                        else
                        {
                            balltokill.Add(thisball);
                            if (ballnum == balltokill.Count())
                            {
                                //lose
                                return 2;
                            }
                        }
                        gamerunning = true;
                        break;
                    }
                }
            }
            foreach (Ball b in balltokill)
            {
                balls.Remove(b);
            }
            //consider items()
            List<Item> items_rm = new List<Item>();
            foreach (Item thisitem in items)
            {
                if (thisitem.ItemFall())
                {
                    if (thisitem.ItemReceived(buffle.left - itemwidth, BuffleWidth + 2 * itemwidth)) 
                    {
                        //item received
                        ItemReceived(thisitem.GetItemNo(), frm);
                    }
                    items_rm.Add(thisitem);
                }
            }
            foreach (Item thisitem in items_rm)
            {
                items.Remove(thisitem);
            }
            items_rm.Clear();
            //nothing happened
            return 0;
        }
        bool CheckCollision(Ball thisball)
        {
            bool flag = false;
            bool x_turn = false;
            bool y_turn = false;
            if (thisball.CenterPositionF.Y > BuffleTop)
            {
                gamerunning = false;
                return false;
            }
            if (bricks_remained == 0)
            {
                gamerunning = false;
                return false;
            }
            if (thisball.CenterPositionF.Y > BuffleTop - Radius)
            {
                if (thisball.CenterPositionF.X > buffle.left && thisball.CenterPositionF.X < buffle.left + BuffleWidth)
                {
                    flag = true;
                    y_turn = true;
                    thisball.MoveDirection.X = ((float)(thisball.CenterPositionF.X - buffle.left - BuffleWidth / 2) / (float)Radius);
                    while (thisball.MoveDirection.X * thisball.MoveDirection.X >= 4.0f)
                        thisball.MoveDirection.X += (thisball.MoveDirection.X > 0) ? (-1.0f) : 1.0f;
                }
            }

            Brick br;
            List<Brick> br_rm = new List<Brick>();
            for (int i = 0; i < bricks.Count(); i++)
            {
                br = bricks[i];
                if (thisball.CenterPositionF.X > br.Pos.Left - Radius
                    && thisball.CenterPositionF.X < br.Pos.Left + br.Pos.Width + Radius)
                {
                    if (thisball.CenterPositionF.Y > br.Pos.Top - Radius
                    && thisball.CenterPositionF.Y < br.Pos.Top + br.Pos.Height + Radius)
                    {
                        flag = true;
                        for (int j = 0; j < 1 + thisball.lv; j++ )
                        {
                            if (br.HitBrick())
                            {
                                bricks_remained--;
                                br_rm.Add(br);
                                break;
                            }
                        }
                        if (thisball.CenterPositionF.X < br.Pos.Left
                            || thisball.CenterPositionF.X > br.Pos.Left + br.Pos.Width)
                            x_turn = true;
                        if (thisball.CenterPositionF.Y < br.Pos.Top
                            || thisball.CenterPositionF.Y > br.Pos.Top + br.Pos.Height)
                            y_turn = true;
                    }
                }
            }
            if (thisball.CenterPositionF.Y <Radius)
            {
                flag = true;
                y_turn = true;
            }
            if (thisball.CenterPositionF.X <Radius
                || thisball.CenterPositionF.X > Pos.Width - Radius)
            {
                flag = true;
                x_turn = true;
            }
            if (x_turn) thisball.MoveDirection.X *= -1;
            if (y_turn) thisball.MoveDirection.Y *= -1;

            Item newitem;
            foreach (Brick b in br_rm)
            {
                bricks.Remove(b);
                //item produced
                if (GetRandom.GetRandomFloat() <= 0.4f)
                {
                    newitem = new Item(b.Pos.Location, GetRandom.GetRandomInt(6), BuffleTop, ItemFallFactor);
                    items.Add(newitem);
                }
            }

            return flag;
        }
        void ItemReceived(int no, FrmMain frm)
        {
            switch (no)
            {
                case 0:
                    this.AddBall();
                    break;
                case 1:
                    this.MoveSlower();
                    break;
                case 2:
                    this.LongerBuffle();
                    break;
                case 3:
                    Energy++;
                    frm.label2.Text = "现在小球的能量为" + Energy.ToString();
                    if (Energy % 5 == 0)
                    {
                        this.BallLevelUp();
                    }
                    break;
                case 4:
                    this.ShorterBuffle();
                    break;
                case 5:
                    this.MoveQuicker();
                    break;

            }
        }
        #endregion
        #region 特别效果
        public void AddBall()
        {
            Ball anotherball = new Ball();
            anotherball.CenterPositionF = new Point(buffle.left + BuffleWidth / 2, BuffleTop - (int)Radius);
            anotherball.MoveDirection = Vector2.GetRandomVector2(0.9f, 1.0f, -0.9f, -1.0f);
            anotherball.lv = Energy/5;
            balls.Add(anotherball);
        }
        public void LongerBuffle()
        {
            if (BuffleWidth + BuffleLengthChangeFactor < Pos.Width) 
            {
                buffle.AddWidth(BuffleLengthChangeFactor);
                BuffleWidth += BuffleLengthChangeFactor;
            }
        }
        public void ShorterBuffle()
        {
            if (BuffleWidth - BuffleLengthChangeFactor > 10)
            {
                buffle.AddWidth(-1 * BuffleLengthChangeFactor);
                BuffleWidth -= BuffleLengthChangeFactor;
            }
        }
        public void MoveQuicker()
        {
            BallMoveFactor += MoveChangeFactor;
        }
        public void MoveSlower()
        {
            if (BallMoveFactor > 1.0f) 
            {
                BallMoveFactor -= MoveChangeFactor;
            }
        }
        public void BallLevelUp()
        {
            foreach (Ball aball in balls)
            {
                if (aball.lv <= 3)
                    aball.lv++;
            }
        }
        #endregion
    }
}
