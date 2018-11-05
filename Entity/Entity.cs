using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lorn.Entity {
    public abstract class Entity {
        public Vector3 Position;
        public Vector3 Velocity;

        Model Model;

        Game Game;

        public Entity(Game game, Vector3 positon) {
            Game = game;
            Position = positon;
        }

        public virtual void Update(GameTime gameTime) {

        }
        public virtual void Draw(GameTime gametime) {

        }
    }
}
