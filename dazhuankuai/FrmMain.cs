using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using dazhuankuai.classes;

namespace dazhuankuai
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            bool isRunning = false;
            Mutex mutex = new Mutex(true, System.Diagnostics.Process.GetCurrentProcess().ProcessName, out isRunning);
            if (!isRunning)
            {
                MessageBox.Show("哎呀呀呀呀你已经打开一个程序了哦卖萌是不好的呦","打砖块");
                Environment.Exit(1);
            }
            InitializeComponent();
            this.Width = 700;
            this.Height = 600;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            this.button1.Width = 100;
            this.button1.Height = 30;
            this.button1.Left = (int)((this.Width - button1.Width) / 2);
            this.button1.Top = this.Height - 80;

            this.label1.Left = (int)((this.Width - label1.Width) / 2);
            this.label1.Top = this.Height - 110;
            this.label1.Visible = false;

            this.label2.Left = (int)((this.Width - label1.Width) / 2);
            this.label2.Top = this.Height - 70;
            this.label2.Visible = false;

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.FormClosed += new FormClosedEventHandler(Form1_Closed);

        }

        #region 全局变量
        static int FrushInterval = 10;
        static int MoveInterval = 5;
        static readonly int frameleft = 50;
        static readonly int frametop = 20;
        static int framewidth;
        static int frameheight;
        Frame mainframe;
        static int playerlevel = 0;

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            framewidth = this.Width - 2 * frameleft;
            frameheight = this.Height - 150;
            this.timer1.Stop();
            this.timer2.Stop();
            mainframe = new Frame(new Rectangle(frameleft, frametop, framewidth, frameheight));
            this.rectangleShape1.Location = new Point(frameleft - 5, frametop - 5);
            this.rectangleShape1.Size = new System.Drawing.Size(framewidth + 10, frameheight + 10);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //如果游戏已经开始
            if (label1.Visible)
            {
                Keys key = e.KeyCode;
                switch (key)
                {
                    case Keys.Left:
                    case Keys.A:
                        this.timer2.Stop();
                        mainframe.GoLeft();
                        this.timer2.Start();
                        e.Handled = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        this.timer2.Stop();
                        mainframe.GoRight();
                        this.timer2.Start();
                        e.Handled = true;
                        break;
                    case Keys.P:
                        this.timer1.Stop();
                        mainframe.AddBall();
                        this.timer1.Start();
                        e.Handled = true;
                        break;
                    case Keys.O:
                        this.timer1.Stop();
                        mainframe.BallLevelUp();
                        this.timer1.Start();
                        e.Handled = true;
                        break;
                    case Keys.K:
                        MessageBox.Show("玩家选择了自杀。", "打砖块");
                        this.ClearGame();
                        e.Handled = true;
                        break;
                    case Keys.L:
                        MessageBox.Show("玩家作弊。本关卡直接胜利。", "打砖块");
                        playerlevel++;
                        this.ClearGame();
                        e.Handled = true;
                        break;
                }
            }
        }

        void ClearGame()
        {
            this.timer1.Stop();
            this.label2.Text = "现在小球没有能量";
            label1.Visible = false;
            label2.Visible = false;
            button1.Visible = true;
        }

        void ShowFrame()
        {
            Graphics g = this.CreateGraphics();
            Bitmap b = mainframe.DrawFrame(this);
            g.DrawImage(b, frameleft, frametop);
            b.Dispose();
            g.Dispose();
        }

        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            switch (mainframe.UpdateData(this))
            {
                case 0:
                    this.ShowFrame();
                    this.timer1.Start();
                    break;
                case 1:
                    MessageBox.Show("你赢啦", "打砖块");
                    playerlevel++;
                    this.ClearGame();
                    break;
                case 2:
                    MessageBox.Show("你输啦", "打砖块");
                    this.ClearGame();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainframe.Init(playerlevel);
            this.label1.Text = "现在是第" + playerlevel.ToString() + "关";
            this.label1.Visible = true;
            this.label2.Visible = true;
            this.button1.Visible = false;
            this.Activate();
            this.Focus();

            mainframe.EnableGame();
            this.timer2.Interval = MoveInterval;
            this.timer2.Start();
            this.timer1.Interval = FrushInterval;
            this.timer1.Start();
            
            

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            mainframe.EnableMove();
        }
    }
}
