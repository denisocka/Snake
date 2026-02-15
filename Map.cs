using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace snake1
{
    public class Map
    {
        private Random rnd = new Random();

        public int Height;
        public int Width;

        public Point snakeStart;

        public int size = 48;

        public Snake snake;

        public List<Wall> BaseWalls = new List<Wall>();
        public List<Fruit> BaseFruits = new List<Fruit>();

        public MonsterMeneger monsters;
        MonsterMeneger BaseMonsters;

        public List<Wall> walls = new List<Wall>();
        public List<Fruit> fruits = new List<Fruit>();

        public Map(int height, int width, Snake snake)
        {
            Height = height;
            Width = width;
            this.snake = snake;
        }

        public Map(char[,] mapGrid, Snake snake)
        {
            Height = mapGrid.GetLength(0);
            Width = mapGrid.GetLength(1);
            walls = CreateWallsInGrid(mapGrid);
            fruits = CreateFruitsInGrid(mapGrid);
            snakeStart = CreateSnakeInGrid(mapGrid);
            monsters = CreateMonstersInGrid(mapGrid);
            this.snake = snake;
            BaseMonsters = monsters.Clone();
            BaseWalls = walls.ToList();
            BaseFruits = fruits.ToList();
        }

        public void RestartMap()
        {
            walls = BaseWalls.ToList();
            fruits = BaseFruits.ToList();
            monsters = BaseMonsters.Clone();
        }

        public void DrowMap(Graphics g)
        {
            DrowGrid(g);
            foreach (var wall in walls)
                wall.Draw(g);
            foreach (var fruit in fruits)
                fruit.Draw(g);
        }

        public List<Wall> CreateWallsInGrid(char[,] mapGrid)
        {
            List < Wall > walls = new List<Wall>();
            for (int i = 0; i < mapGrid.GetLength(0);i++)
                for (int j = 0; j < mapGrid.GetLength(1);j++)
                    if (mapGrid[i,j] == '#')
                        walls.Add(new Wall(j,i));

            return walls;
        }

        public List<Fruit> CreateFruitsInGrid(char[,] mapGrid)
        {
            List<Fruit> fruits = new List<Fruit>();

            for (int i = 0; i < mapGrid.GetLength(0); i++)
                for (int j = 0; j < mapGrid.GetLength(1); j++)
                    switch (mapGrid[i,j])
                    {
                        case 'w':
                            fruits.Add(new Won(j, i));
                            break;
                        case 'a':
                            fruits.Add(new Apple(j, i));
                            break;
                        case 'd':
                            fruits.Add(new Daphne(j, i));
                            break;
                        case 'p':
                            fruits.Add(new Pear(j, i));
                            break;
                        case 'k':
                            fruits.Add(new Kiwi(j, i));
                            break;
                    }

            return fruits;
        }

        public Point CreateSnakeInGrid(char[,] mapGrid)
        {
            List<Wall> walls = new List<Wall>();
            for (int i = 0; i < mapGrid.GetLength(0); i++)
                for (int j = 0; j < mapGrid.GetLength(1); j++)
                    if (mapGrid[i, j] == 's')
                        return new Point(j, i);
            return new Point(-1,-1);
        }

        public MonsterMeneger CreateMonstersInGrid(char[,] mapGrid)
        {
            MonsterMeneger monsters = new MonsterMeneger();
            for (int i = 0; i < mapGrid.GetLength(0); i++)
                for (int j = 0; j < mapGrid.GetLength(1); j++)
                    if (mapGrid[i, j] == 'm')
                        monsters.AddMonster(new MonsterMedusa(j,i));

            return monsters;
        }

        protected void DrowGrid(Graphics g)
        {
            Pen pen = new Pen(Color.Gray, 1);

            g.DrawRectangle(new Pen(Color.Gray, 2), 75, 50, Width * size, Height * size);
            for (int i = 0; i < Width; i++)
                g.DrawLine(pen, 75 + i * size, 50, 75 + i * size, 50 + Width * size);
            for (int i = 0; i < Height; i++)
                g.DrawLine(pen, 75, 50 + i * size, 75 + Height * size, 50 + i * size);
        }

        Point GetRandomFreePosition()
        {
            while (true)
            {
                int f = 0;

                int x = rnd.Next(0, Width);
                int y = rnd.Next(0, Height);

                if (snake.elements.Any(e => e.x == x && e.y == y))
                    f = 1;

                if (fruits.Any(e => e.x == x && e.y == y))
                    f = 1;

                if (walls.Any(e => e.x == x && e.y == y))
                    f = 1;

                if (f == 0)
                    return new Point(x, y);
            }
        }

    }
}
