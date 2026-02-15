using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;


namespace snake1
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    public class Element
    {

        public int x;
        public int y;
        public Direction dir;

    }
    public class Snake
    {
        int size = 48;
        public bool isLive = true;

        private Random rnd = new Random();
        public int Speed = 15;

        public bool Invincible = false;

        public List<Element> elements;
        public Color color;

        EffectManager effects = new EffectManager();

        public void AddEffect(Effects effect)
        {
            effects.AddEffects(effect, this);
        }

        public Element head()
        {
            return elements[0];
        }

        public int Lenght()
        {
            return elements.Count();
        }

        public Snake(Direction direction, Color c, int lenght, int x, int y)
        {
            elements = new List<Element>();
            elements.Add(new Element { dir = direction, x = x, y = y });
            for (int i = 1; i < lenght; i++)
                elements.Add(new Element { dir = direction, x = x, y = y + i });
            color = c;
        }

        public bool DoRotate(Direction fromRotate)
        {
            Direction toRotate = this.head().dir;

            if (fromRotate - toRotate == 1 || fromRotate - toRotate == -1)
                return true;
            else
                if (fromRotate == Direction.Left && toRotate == Direction.Up)
                return true;
            else
                if (fromRotate == Direction.Up && toRotate == Direction.Left)
                return true;
            return false;
        }

        bool flagRotate = true;

        public void SnakeRotate(Direction fromRotate)
        {
            if (this.DoRotate(fromRotate) && flagRotate == true)
            {
                this.head().dir = fromRotate;
                flagRotate = false;
            }
        }

        public void Dead()
        {
            isLive = false;

        }

        public void GetDamage(int value)
        {
            if (this.Invincible == false)
                for (int i = 0; i < value; i++)
                    if (elements.Count > 1)
                        this.elements.RemoveAt(elements.Count - 1);
                    else
                        Dead();
        }


        private const int countFrontsOnX = 20;
        private const int countFrontsOnY = 20;

        public bool CanMove(int dx, int dy, Map map)
        {
            int x = this.head().x;
            int y = this.head().y;

            if (x + dx > map.Width - 1)
                return false;
            if (x + dx < 0)
                return false;
            if (y + dy > map.Height - 1)
                return false;
            if (y + dy < 0)
                return false;

            return
                !(elements.Any(e => e.x == x + dx && e.y == y + dy) ||
                map.walls.Any(e => e.x == x + dx && e.y == y + dy) ||
                map.monsters.Any(e => e.x == x + dx && e.y == y + dy));
        }

        public void SnakeMove(Map map)
        {
            bool flag = false;
            Element e;
            int xLast = this.elements[0].x;
            int yLast = this.elements[0].y;

            e = this.elements[0];

            switch (e.dir)
            {
                case Direction.Up:
                    if (CanMove(0, -1, map))
                        e.y--;
                    else
                    {
                        GetDamage(1);
                        flag = true;
                    }
                    break;
                case Direction.Right:
                    if (CanMove(1, 0, map))
                        e.x++;
                    else
                    {
                        GetDamage(1);
                        flag = true;
                    }
                    break;
                case Direction.Down:
                    if (CanMove(0, 1, map))
                        e.y++;
                    else
                    {
                        GetDamage(1);
                        flag = true;
                    }
                    break;
                case Direction.Left:
                    if (CanMove(-1, 0, map))
                        e.x--;
                    else
                    {
                        GetDamage(1);
                        flag = true;
                    }
                    break;
            }


            if (flag == false)
                for (int i = 1; i < this.Lenght(); i++)
                {
                    e = this.elements[i];
                    int x1 = this.elements[i].x;
                    int y1 = this.elements[i].y;
                    e.x = xLast;
                    e.y = yLast;
                    xLast = x1;
                    yLast = y1;
                }
        }

        public void AddElement(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int elementLenght = this.elements.Count;
                int x = this.elements[elementLenght - 1].x;
                int y = this.elements[elementLenght - 1].y;

                this.elements.Add(new Element() { x = x, y = y });
            }
        }

        public void EatFruits(Map map)
        {
            var fruits = map.fruits;
            for (int i = 0; i < fruits.Count; i++)
                if (fruits[i].IsCollided(this))
                {
                    fruits[i].Effect(this);

                    this.AddElement(fruits[i].value);

                    fruits.RemoveAt(i);
                    break;
                }
        }

        public void DrowSnakeElement(PaintEventArgs e, Element element)
        {
            SolidBrush brush = new SolidBrush(color);
            e.Graphics.FillRectangle(
                brush,
                75 + element.x*size + 1,
                50 + element.y*size + 1,
                48 - 1,
                48 - 1
            );
        }

        public void DrowCyrle(Graphics g, Color color, int x, int y, int r)
        {
            SolidBrush brush = new SolidBrush(color);
            g.FillEllipse(brush, x - r, y - r, r * 2, r * 2);
        }

        public void DrowHead(PaintEventArgs e, Element element)
        {
            SolidBrush brush = new SolidBrush(color);
            e.Graphics.FillRectangle(
                brush,
                75 + element.x * size + 1,
                50 + element.y * size + 1,
                48 - 1,
                48 - 1
            );

            int x = (int)element.x * size + 75;
            int y = (int)element.y * size + 50;
            switch (element.dir)
            {
                case Direction.Up:
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 4, y + 48 / 3, 48 / 8);
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 4 * 3, y + 48 / 3, 48 / 8);
                    break;
                case Direction.Right:
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 3 * 2, y + 48 / 4, 48 / 8);
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 3 * 2, y + 48 / 4 * 3, 48 / 8);
                    break;
                case Direction.Down:
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 4, y + 48 / 3 * 2, 48 / 8);
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 4 * 3, y + 48 / 3 * 2, 48 / 8);
                    break;
                case Direction.Left:
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 3, y + 48 / 4, 48 / 8);
                    DrowCyrle(e.Graphics, Color.Black, x + 48 / 3, y + 48 / 4 * 3, 48 / 8);
                    break;
            }
        }

        public void Draw(PaintEventArgs e)
        {
            int state = 0;
            DrowHead(e, elements[0]);
            for (int i = 1; i < this.Lenght(); i++)
                DrowSnakeElement(e, elements[i]);
        }

        private int TimerCome = 0;

        public void Update(Map map)
        {
            this.effects.Update(this);
            EatFruits(map);

            this.TimerCome++;
            if (this.TimerCome >= this.Speed)
            {
                this.SnakeMove(map);
                flagRotate = true;
                TimerCome = 0;
            }
        }

    }
}
