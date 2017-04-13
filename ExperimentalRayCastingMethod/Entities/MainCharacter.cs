using System;
using System.Drawing;
using Harder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace The_E_Project.Entities
{
    public class MainCharacter
    {
        private readonly Texture2D _characterTexture2D;

        private readonly GraphicsDevice _graphicsDevice;

        private readonly Animation.Animation _idleAnimation;

        private readonly Size _sizeOfSprite;
        private Animation.Animation _currentAnimation;

        private Vector2 _position;
        private Rectangle _sourceRectangle;

        private float _touchInputX;
        private float _touchInputY;

        public float Angle { get; set; }

        public MainCharacter(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

            Angle = 0;

            _position.X = Activity1.SizeOfDeviceX/2;
            _position.Y = Activity1.SizeOfDeviceY/2;

            _sizeOfSprite = new Size(25, 56);

            if (_characterTexture2D == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/Character.png"))
                {
                    _characterTexture2D = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            _idleAnimation = new Animation.Animation();

            _idleAnimation.AddFrame(new Rectangle(0, 0, 25, 56), TimeSpan.FromSeconds(0.167));
            _idleAnimation.AddFrame(new Rectangle(26, 0, 24, 56), TimeSpan.FromSeconds(0.167));
            _idleAnimation.AddFrame(new Rectangle(51, 0, 24, 56), TimeSpan.FromSeconds(0.167));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sourceRectangle = _currentAnimation.CurrentRectangle;

            var tintColor = Color.White;
            const float scale = 3.2f;
            const float layer = 0.0f;

            spriteBatch.Draw(_characterTexture2D, _position, _sourceRectangle,
                tintColor, Angle + (float)Math.PI / 2, new Vector2(_sizeOfSprite.Width/2, _sizeOfSprite.Height/2),
                scale, SpriteEffects.None, layer);
        }

        public void Update(GameTime gameTime)
        {
            var touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                _touchInputX = touchCollection[0].Position.X;
                _touchInputY = touchCollection[0].Position.Y;

                var xLength = _touchInputX - _position.X;
                var yLength = _touchInputY - _position.Y;

                Angle = (float) Math.Atan2(yLength, xLength);

                ProjectileCreator.CreateBullet(_graphicsDevice, _position, Angle);
            }

            _currentAnimation = _idleAnimation;

            _currentAnimation.Update(gameTime);
        }
    }
}