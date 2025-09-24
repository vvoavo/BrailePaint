using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrailePaint.Classes
{
    public class Point_d
    {
        public double x;
        public double y;

        public Point_d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point_d(Point p)
        {
            this.x = p.X;
            this.y = p.Y;
        }
        public Point_d(Point_d p)
        {
            this.x = p.x;
            this.y = p.y;
        }
        public void scale(double scale)
        {
            this.x *= scale;
            this.y *= scale;
        }
    }
}
