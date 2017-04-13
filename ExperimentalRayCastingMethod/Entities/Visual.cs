using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_E_Project.Entities
{
    public class Visual
    {
        public Animation.Animation CurrentAnimation;
        public Animation.Animation IdleAnimation;
        public Rectangle SourceRectangle;

        /// <summary>
        ///     Lets be honest I'm probably not gonna use this.
        ///     Purpose is for cloning.
        /// </summary>
        public Visual(Visual visual)
        {
            TextureName = visual.TextureName;
            TintColor = visual.TintColor;
            Angle = visual.Angle;
            Scale = visual.Scale;
            Effects = visual.Effects;
            Layer = visual.Layer;
            Texture = visual.Texture;

            Animations = visual.Animations;

            CurrentAnimation = visual.CurrentAnimation;
            IdleAnimation = visual.IdleAnimation;
            SourceRectangle = visual.SourceRectangle;
        }

        /// <summary>
        ///     Default for testing. Uses TestWall sprite.
        /// </summary>
        public Visual()
        {
            TextureName = "TestWall";
            TintColor = Color.White;
            Angle = 0.0f;
            Scale = 1.0f;
            Effects = SpriteEffects.None;
            Layer = 0.0f;
        }

        /// <summary>
        ///     For creating basic visuals with texture.
        /// </summary>
        /// <param name="inputTexture">Name of texture (without extension).</param>
        public Visual(string inputTexture)
        {
            TextureName = inputTexture;
            TintColor = Color.White;
            Angle = 0.0f;
            Scale = 1.0f;
            Effects = SpriteEffects.None;
            Layer = 0.0f;
        }

        public List<Animation.Animation> Animations { get; set; }

        public Texture2D Texture { get; set; }
        public string TextureName { get; set; }
        public Color TintColor { get; }
        public float Angle { get; set; }
        public float Scale { get; }
        public SpriteEffects Effects { get; }
        public float Layer { get; }


        public void Update(GameTime gameTime)
        {
            if (Animations == null)
                return;

            CurrentAnimation.Update(gameTime);
        }
    }
}