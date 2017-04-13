using Harder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using The_E_Project.Entities;

namespace The_E_Project
{
    public static class ProjectileCreator
    {
        private static bool _touchIsDown;

        public static void CreateBullet(GraphicsDevice graphicsDevice, Vector2 position, float angle)
        {
            var touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                _touchIsDown = true;
            }
            else if (_touchIsDown)
            {
                if (Run.BulletList.Count > 3)
                {
                    Run.BulletList.Clear();
                }


                var bullet = new Bullet(graphicsDevice, position, angle, "TestBullets", Run.currentLevel.Moving);

                Run.BulletList.Add(bullet);

                _touchIsDown = false;
            }
        }
    }
}