using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Harder
{
    public class Polygon
    {
        public List<Vector2> Edges { get; } = new List<Vector2>();

        public List<Point> Points { get; } = new List<Point>();

        public Point Center
        {
            get
            {
                var totalX = 0;
                var totalY = 0;
                for (var i = 0; i < Points.Count; i++)
                {
                    totalX += Points[i].X;
                    totalY += Points[i].Y;
                }

                return new Point(totalX/Points.Count, totalY/Points.Count);
            }
        }

        public void AddPoint(Point point)
        {
            Points.Add(point);
        }

        public void BuildEdges()
        {
            Point p1;
            Point p2;
            Edges.Clear();
            for (var i = 0; i < Points.Count; i++)
            {
                p1 = Points[i];
                p2 = i + 1 >= Points.Count ? Points[0] : Points[i + 1];
                Edges.Add(p2.ToVector2() - p1.ToVector2());
            }
        }

        //is this really working? ;)))))
        public void Rotate(float radians)
        {
            for (var i = 0; i < Points.Count; i++)
            {

                var x = Math.Cos(radians)*(Points[i].X - Center.X) - Math.Sin(radians)*(Points[i].Y - Center.Y) + Center.X;
                var y = Math.Sin(radians)*(Points[i].X - Center.X) + Math.Cos(radians)*(Points[i].Y - Center.Y) + Center.Y;

                Points[i] = new Point((int)x, (int)y);
            }
        }

        public static Vector2 PointOnCircle(int radius, float radians, float originX, float originY)
        {
            var x = originX + radius*(float) Math.Cos(radians);
            var y = originY + radius*(float) Math.Sin(radians);

            return new Vector2(x, y);
        }

        public void Offset(Point v)
        {
            Offset(v.X, v.Y);
        }

        public void Offset(int x, int y)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var p = Points[i];
                Points[i] = new Point(p.X + x, p.Y + y);
            }
        }

        public override string ToString()
        {
            var result = "";

            foreach (var p in Points)
            {
                if (result != "") result += " ";
                result += "{" + p + "}";
            }

            return result;
        }
    }
}