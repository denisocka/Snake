using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake1
{
    public class Wall
    {
        int size = 48;
        public int x, y;

        Image Texture = Properties.Resources.wall;

        public Wall(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(
                Texture,
                75 + x * size,
                50 + y * size,
                size,
                size
            );
        }

    }
}
