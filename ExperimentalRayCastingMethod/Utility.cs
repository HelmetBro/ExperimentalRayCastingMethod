using System;
using System.Linq;
using Harder;
using Microsoft.Xna.Framework;

namespace The_E_Project
{
    public static class Utility
    {
        public static Polygon RectToPolygon(Rectangle rect)
        {
            var p = new Polygon();

            //top left
            p.Points.Add(new Point(rect.X, rect.Y));

            //top right
            p.Points.Add(new Point(rect.X + rect.Width, rect.Y));

            //bottom right
            p.Points.Add(new Point(rect.X + rect.Width,
                rect.Y + rect.Height));

            //bottom left
            p.Points.Add(new Point(rect.X, rect.Y + rect.Height));

            p.BuildEdges();

            return p;
        }

        public static Rectangle PolygonToRect(Polygon poly)
        {
            if (poly.Points.Count != 4)
            {
                Console.WriteLine("PolygonToRect - polygon does not have 4 points!");
                return new Rectangle();
            }

            var r = new Rectangle();

            var closestToOrigin = poly.Points.Select(p => new {Point = p, Distance2 = p.X*p.X + p.Y*p.Y})
                .Aggregate((p1, p2) => p1.Distance2 < p2.Distance2 ? p1 : p2)
                .Point;

            var farthestFromOrigin = poly.Points.Select(p => new {Point = p, Distance2 = p.X*p.X + p.Y*p.Y})
                .Aggregate((p1, p2) => p1.Distance2 > p2.Distance2 ? p1 : p2)
                .Point;

            //top left
            r.X = closestToOrigin.X;
            r.Y = closestToOrigin.Y;

            r.Width = farthestFromOrigin.X - closestToOrigin.X;
            r.Height = farthestFromOrigin.Y - closestToOrigin.Y;

            return r;
        }

        //finds intersection point of two lines
        public static Vector2 IntersectionPoint(float A1, float B1, float C1, float A2, float B2, float C2)
        {
            var delta = A1 * B2 - A2 * B1;
            if (delta == 0)
                throw new ArgumentException("Lines are parallel, angle calculation is wrong.");

            var x = (B2 * C1 - B1 * C2) / delta;
            var y = (A1 * C2 - A2 * C1) / delta;

            return new Vector2(x, y);
        }

        //Creates a line in Ax + By = C form
        public static void LineFromTwoPoints(Point p1, Point p2, out float a, out float b, out float c)
        {
            p1.Y *= -1;
            p2.Y *= -1;

            a = p2.Y - p1.Y;
            b = p1.X - p2.X;
            c = a * p1.X + b * p1.Y;
        }


        //slope. This method is also equal to subtracting the two vectors.
        public static Vector2 Vector2BetweenTwoPoints(Point point1, Point point2)
        {
            return new Vector2(point2.X - point1.X, point2.Y - point1.Y);
        }

        //average slope
        public static Vector2 Vector2BetweenMultipleVectors(Vector2[] vectors)
        {
            for (var i = 0; i < vectors.Length; i++)
                vectors[i].Normalize();

            float xValues = 0;
            float yValues = 0;
            foreach (var vector in vectors)
            {
                xValues += vector.X;
                yValues += vector.Y;
            }

            return new Vector2(xValues/vectors.Length, yValues/vectors.Length);
        }

        public static Point MidPoint(Point pt1, Point pt2)
        {
            var midX = (pt1.X + pt2.X)/2;
            var midY = (pt1.Y + pt2.Y)/2;
            return new Point(midX, midY);
        }

        //used for MEASURING/COMPARING distances (faster speed) fastDistance
        public static float FastDistance(Vector2 source, Vector2 target)
        {
            return (float) Math.Pow(target.X - source.X, 2) + (float) Math.Pow(target.Y - source.Y, 2);
        }

        //used for CALCULATING distances
        public static float DistanceTo(Point point1, Point point2)
        {
            var a = point2.X - point1.X;
            var b = point2.Y - point1.Y;

            return (float) Math.Sqrt(a*a + b*b);
        }

        private static double AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        public static bool PolyCollide(int nvert, float[] vertx, float[] verty, float testx, float testy)
        {
            int i, j;
            var result = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
                if ((verty[i] > testy != verty[j] > testy) &&
                    (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                    result = !result;
            return result;
        }
    }
}