using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace snake1
{
    public partial class Front : Form
    {
        private System.Windows.Forms.Timer timer;
        private const int size = 48;
        private const int countFrontsOnX = 20;
        private const int countFrontsOnY = 20;

        private Random rnd = new Random();

        int Level = 1;

        public bool game = true;

        Snake snake;

        public Front()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;

            // События нажатия и отпускания клавиши
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;


            timer = new System.Windows.Forms.Timer();
            timer.Interval = 20; // ~50 FPS
            timer.Tick += Timer_Tick;
            timer.Start();
            this.Load += Form1_Load;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(countFrontsOnX * size + 150, countFrontsOnY * size + 100);
            this.Text = "Snake";
            CreateMap();
            snake = new Snake(Direction.Left, Color.Green, 1, maps[Level].snakeStart.X, maps[Level].snakeStart.Y);
            maps[Level].snake = snake;

        }

        public void Restart()
        {
            snake = new Snake(Direction.Left, Color.Green, 1, maps[Level].snakeStart.X, maps[Level].snakeStart.Y);
            maps[Level].RestartMap();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            snake.Draw(e);
            maps[Level].DrowMap(g);
            TitleScore(g);
            if (snake.isLive == false)
                DrowTitleGameOver(g);
            foreach (var monster in maps[Level].monsters)
                monster.Drow(g);
        }

        private void DrowTitleGameOver(Graphics g)
        {
            string text = "GAME OVER";

            Font font = new Font("Arial", 48, FontStyle.Bold);
            Brush brush = Brushes.Red;

            SizeF textSize = g.MeasureString(text, font);

            float x = (ClientSize.Width - textSize.Width) / 2;
            float y = (ClientSize.Height - textSize.Height) / 2 - 50;

            g.DrawString(text, font, brush, x, y);
        }

        private void TitleScore(Graphics g)
        {
            string text = "score: " + snake.Lenght().ToString();

            Font font = new Font("Times new roman", 20, FontStyle.Underline);
            Brush brush = Brushes.Black;

            SizeF textSize = g.MeasureString(text, font);

            float x = 70;
            float y = 5;

            g.DrawString(text, font, brush, x, y);
        }


        Button restartButton = new Button();

        private void Form1_Load(object sender, EventArgs e)
        {
            restartButton.Text = "Restart";
            restartButton.Size = new Size(300, 80);
            restartButton.Font = new Font("Arial", 20, FontStyle.Bold);
            restartButton.Visible = false;
            restartButton.Click += RestartButton_Click;

            Controls.Add(restartButton);
        }

        void ShowRestartButton()
        {
            restartButton.Left = (ClientSize.Width - restartButton.Width) / 2;
            restartButton.Top = (ClientSize.Height / 2) + 20;
            restartButton.Visible = true;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            restartButton.Visible = false;
            snake.isLive = true;

            Restart();
            timer.Start();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) snake.SnakeRotate(Direction.Up);
            if (e.KeyCode == Keys.S) snake.SnakeRotate(Direction.Down);
            if (e.KeyCode == Keys.A) snake.SnakeRotate(Direction.Left); 
            if (e.KeyCode == Keys.D) snake.SnakeRotate(Direction.Right); 
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { };
            if (e.KeyCode == Keys.S) { };
            if (e.KeyCode == Keys.A) { };
            if (e.KeyCode == Keys.D) { };
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            maps[Level].monsters.Update(maps[Level]);
            snake.Update(maps[Level]);
            Invalidate();
            if (snake.isLive == false)
            {
                ShowRestartButton();
                timer.Stop();
            }
            
        }

    }
}