using System;
using System.Drawing;
using Harder;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace The_E_Project.Entities
{
    public class FinalGoal : BaseEntity
    {
        private byte _index;

        public FinalGoal(Point spritePosition, Size spriteSize, Polygon polygon, byte index) : base(spritePosition, spriteSize, polygon)
        {
            _index = index;
        }

        public FinalGoal(Rectangle rectangle, Polygon polygon, string inputTexture, byte index) : base(rectangle, polygon, inputTexture)
        {
            _index = index;
        }

        public FinalGoal(Point spritePosition, Size spriteSize, Polygon polygon, string inputTexture, byte index) : base(spritePosition, spriteSize, polygon, inputTexture)
        {
            _index = index;
        }

        public FinalGoal(Rectangle rectangle, Polygon polygon, byte index) : base(rectangle, polygon)
        {
            _index = index;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Run.BulletList.Count <= 0)
                return;

            Visual.Angle = (float)Math.Atan2(Run.BulletList[0].Position.Y, Run.BulletList[0].Position.X);
            Polygon.Rotate(Bullet.NormalizeAngle(Visual.Angle));

            foreach (var p in Polygon.Points)
            {
                Console.WriteLine(p);
            }


        }
    }
}