using System.Collections.Generic;
using System.Drawing;
using Harder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace The_E_Project.Entities
{
    public class BaseEntity
    {
        private bool _hasAnimation;

        private Rectangle _rectangle;

        public Point PositionOfSprite;
        public Visual Visual;

        /// <summary>
        ///     Used for creating object with specific position and
        ///     has an animation. Position is middle coordinate.
        /// </summary>
        /// <param name="spritePosition">Position of the sprite (middle).</param>
        /// <param name="spriteSize">Size of the sprite.</param>
        /// <param name="polygon">Hitbox-detection polygon.</param>
        public BaseEntity(Point spritePosition, Size spriteSize, Polygon polygon)
        {
            Visual = new Visual();
            Polygon = polygon;
            _rectangle = new Rectangle(spritePosition.X - spriteSize.Width/2, spritePosition.Y - spriteSize.Height/2,
                spriteSize.Width, spriteSize.Height);
            PositionOfSprite = spritePosition;
        }

        /// <summary>
        ///     Most common for static entities.
        /// </summary>
        /// <param name="rectangle">Sprite for the visual.</param>
        /// <param name="polygon">Polygon hitbox for entity.</param>
        /// <param name="inputTexture">Texture name to use for visual.</param>
        public BaseEntity(Rectangle rectangle, Polygon polygon, string inputTexture)
        {
            Visual = new Visual(inputTexture);
            Polygon = polygon;
            _rectangle = rectangle;
            PositionOfSprite = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        /// <summary>
        ///     Used for static terrain. Position is the top-left
        ///     coordinate of the rectangle created.
        /// </summary>
        /// <param name="spritePosition">Position of the sprite (middle).</param>
        /// <param name="spriteSize">Size of the sprite.</param>
        /// <param name="inputTexture">Name of sprite without extension.</param>
        /// <param name="polygon">Hitbox-detection polygon.</param>
        public BaseEntity(Point spritePosition, Size spriteSize, Polygon polygon, string inputTexture)
        {
            Visual = new Visual(inputTexture);
            Polygon = polygon;
            _rectangle = new Rectangle(spritePosition.X - spriteSize.Width/2, spritePosition.Y - spriteSize.Height/2,
                spriteSize.Width, spriteSize.Height);
            PositionOfSprite = spritePosition;
        }

        /// <summary>
        ///     NOTE: This creates default Visual, use for testing.
        ///     Easy setup for sprite + polygon.
        /// </summary>
        /// <param name="rectangle">For sprite dimensions and position.</param>
        /// <param name="polygon">For actual object collision.</param>
        public BaseEntity(Rectangle rectangle, Polygon polygon)
        {
            Visual = new Visual();
            Polygon = polygon;
            _rectangle = rectangle;
            PositionOfSprite = new Point(rectangle.X + rectangle.Width/2, rectangle.Y + rectangle.Height/2);
        }

        //hitbox detection
        public Polygon Polygon { get; }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //if the shape is static
            if (!_hasAnimation)
            {
                //setting _rectangle to null draws whole sprite, and only that. Giving dimensions to it stretches the
                //sprite the fill all the of edges.
                spriteBatch.Draw(Visual.Texture, new Vector2(PositionOfSprite.X, PositionOfSprite.Y), _rectangle,
                    Visual.TintColor, Visual.Angle, new Vector2(_rectangle.Width, _rectangle.Height)/2, Visual.Scale,
                    Visual.Effects, Visual.Layer);
            }
            else
            {
                Visual.SourceRectangle = Visual.CurrentAnimation.CurrentRectangle;

                spriteBatch.Draw(Visual.Texture, new Vector2(PositionOfSprite.X, PositionOfSprite.Y),
                    Visual.SourceRectangle,
                    Visual.TintColor, Visual.Angle, new Vector2(_rectangle.Width, _rectangle.Height)/2, Visual.Scale,
                    Visual.Effects, Visual.Layer);
            }
        }

        public void AddAnimation(Animation.Animation animation, string spriteTextureName)
        {
            if (animation != null)
                _hasAnimation = true;

            if (Visual.Animations == null)
            {
                Visual.Animations = new List<Animation.Animation>();
                Visual.IdleAnimation = animation;
                Visual.CurrentAnimation = Visual.IdleAnimation;
            }

            Visual.Animations.Add(animation);

            Visual.TextureName = spriteTextureName;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (_hasAnimation)
                Visual.Update(gameTime);
        }
    }
}