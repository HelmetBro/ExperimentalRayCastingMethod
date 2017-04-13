using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using The_E_Project.Entities;

namespace The_E_Project.Levels
{
    public class BaseLevel
    {
        //objects
        public List<BaseEntity> Entities;

        //moving level objects or not
        public bool Moving;

        public BaseLevel()
        {
            Entities = new List<BaseEntity>();
        }

        public ContentManager LoadContent(ContentManager content)
        {
            //how to use load/unload methods
            //http://rbwhitaker.wikidot.com/monogame-managing-content

            foreach (var entity in Entities)
                entity.Visual.Texture = content.Load<Texture2D>(entity.Visual.TextureName);

            return content;
        }

        public ContentManager UnloadContent(ContentManager content)
        {
            content.Unload();
            return content;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
                entity.Visual.Update(gameTime);

            foreach (var entity in Entities)
                entity.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var poly in Entities)
                poly.Draw(spriteBatch);
        }
    }
}