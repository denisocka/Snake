using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace snake1
{
    public class MonsterMeneger : IEnumerable<Monster>
    {
        List<Monster> monsters = new List<Monster>();

        public IEnumerator<Monster> GetEnumerator()
            => monsters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public int Lenght()
        {
            return monsters.Count;
        }

        public void AddMonster(Monster monster)
        {
            monsters.Add(monster);
        }

        public void Update(Map map)
        {
            foreach (var monster in monsters)
                monster.Update(map);

            monsters.RemoveAll(m => !m.isAlive);
        }

        public MonsterMeneger Clone()
        {
            var copy = new MonsterMeneger();
            foreach (var monster in monsters)
                copy.AddMonster(monster.Clone());

            return copy;
        }
    }

    public abstract class Monster
    {
        protected int size = 48;

        public int x;
        public int y;

        float drawX;
        float drawY;

        int targetX;
        int targetY;

        const float PixelSpeed = 4f; 

        public bool isAlive = true;

        int Damage;
        int HealPoint;
        int Speed;

        Image Texture = Properties.Resources.medusa;

        protected Monster(int x, int y)
        {
            Speed = 20;
            HealPoint = 5;
            Damage = 1;

            this.x = x;
            this.y = y;

            targetX = x;
            targetY = y;

            drawX = x * size;
            drawY = y * size;
        }

        public void Drow(Graphics g)
        {
            g.DrawImage(
                Texture,
                75 + drawX,
                50 + drawY,
                size,
                size
            );
        }

        public void Kill()
        {
            isAlive = false;
        }

        public bool CanMove(int dx, int dy, Map map)
        {
            if (x + dx > map.Width - 1) return false;
            if (x + dx < 0) return false;
            if (y + dy > map.Height - 1) return false;
            if (y + dy < 0) return false;

            return
                !(map.snake.elements.Any(e => e.x == x + dx && e.y == y + dy) ||
                  map.walls.Any(e => e.x == x + dx && e.y == y + dy) ||
                  map.fruits.Any(e => e.x == x + dx && e.y == y + dy) ||
                  map.monsters.Any(e => e.x == x + dx && e.y == y + dy));
        }

        public virtual void Move(int dx, int dy, Map map)
        {
            if (CanMove(dx, dy, map))
            {
                targetX = x + dx;
                targetY = y + dy;
            }
        }

        public abstract Monster Clone();
        public virtual void MoveRule(Map map) { }

        int TimerCome = 0;

        public void Update(Map map)
        {
            TimerCome++;
            if (TimerCome >= Speed)
            {
                MoveRule(map);
                TimerCome = 0;
            }

            float targetPx = targetX * size;
            float targetPy = targetY * size;

            drawX = MoveTowards(drawX, targetPx, PixelSpeed);
            drawY = MoveTowards(drawY, targetPy, PixelSpeed);

            if (drawX == targetPx && drawY == targetPy)
            {
                x = targetX;
                y = targetY;
            }
        }

        float MoveTowards(float current, float target, float maxDelta)
        {
            if (Math.Abs(target - current) <= maxDelta)
                return target;

            return current + Math.Sign(target - current) * maxDelta;
        }
    }

    public class MonsterMedusa : Monster
    {
        public override Monster Clone()
            => new MonsterMedusa(x, y);

        public MonsterMedusa(int x, int y) : base(x, y) { }

        int movePoint;
        int moveDirection = 1;

        public override void Move(int dx, int dy, Map map)
        {
            base.Move(dx, dy, map);

            if (!CanMove(dx, dy, map))
            {
                moveDirection = 1 - moveDirection;
                movePoint = 0;
            }
        }

        public override void MoveRule(Map map)
        {
            movePoint++;

            if (movePoint > 10)
            {
                moveDirection = 1 - moveDirection;
                movePoint = 0;
            }

            int dx = moveDirection == 0 ? -1 : 1;
            Move(dx, 0, map);
        }
    }
}