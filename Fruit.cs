using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace snake1
{


    public abstract class Fruit
    {
        private Random rnd = new Random();
        private const int size = 48;

        public int x , y;
        public int value;
        protected Image Texture;
        public abstract void Effect(Snake snake);
        public virtual void Draw(Graphics g)
        {
            g.DrawImage(
                Texture,
                75 + x * size,
                50 + y * size,
                size,
                size
            );
        }
        public bool IsCollided(Snake snake)
        {
            int xSnake = snake.head().x;
            int ySnake = snake.head().y;

            if ( xSnake == x &&  ySnake == y )
                return true;

            return false;
        }

        Fruit GetRandomFruits(int x, int y)
        {
            int r = rnd.Next(20);

            return r switch
            {
                >= 0 and <= 10 => new Apple(x, y),
                >= 11 and <= 14 => new Kiwi(x, y),
                >= 15 and <= 17 => new Won(x, y),
                18 => new Daphne(x, y),
                _ => new Pear(x, y)
            };
        }

    }

    class Apple: Fruit
    {
        public Apple(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 1;
            this.Texture = Properties.Resources.apple;
        }

            public override void Effect(Snake snake)
            {
            }
    }

    class Kiwi : Fruit
    {
        public Kiwi(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 2;
            this.Texture = Properties.Resources.kiwi;
        }

        public override void Effect(Snake snake)
        {
        }
    }

    class Won : Fruit
    {
        public Won(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 0;
            this.Texture = Properties.Resources.won;
        }

        public override void Effect(Snake snake)
        {
            snake.AddEffect(new SpeedEffect(300, 10));
            snake.AddEffect(new ColorEffect(300,Color.Purple));
        }
    }

    class Daphne : Fruit
    {
        public Daphne(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 0;
            this.Texture = Properties.Resources.daphne;
        }

        public override void Effect(Snake snake)
        {
            snake.AddEffect(new ColorEffect(100, Color.LightGreen));
            snake.AddEffect(new DamageEffect(100));
        }
    }

    class Pear : Fruit
    {
        public Pear(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 0;
            this.Texture = Properties.Resources.pear;
        }

        public override void Effect(Snake snake)
        {
            snake.AddEffect(new InvincibleEffect(200));
            snake.AddEffect(new ColorEffect(200, Color.DarkGreen));

        }
    }
}
