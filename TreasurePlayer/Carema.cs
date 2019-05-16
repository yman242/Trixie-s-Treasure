#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace TreasurePlayer
{
    public class Camera
    {
        public Vector2 _position = Vector2.Zero;

        private float _viewportWidth = 0.0f;
        private float _viewportHeight = 0.0f;

        private float _moveSpeed = 1.5f;

        public Camera(GraphicsDeviceManager graphics, Vector2 position)
        {
            _viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            _viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            _position = position;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.X -= (_viewportWidth / 2.0f);
            position.Y -= (_viewportHeight / 2.0f);

            _position = Vector2.Lerp(_position, position, _moveSpeed * delta);
        }

        public Vector2 Transform(Vector2 point)
        {
            return new Vector2(
                (point.X - _position.X),
                (point.Y - _position.Y));
        }

        public bool IsObjectVisible(Vector2 position, Texture2D obj)
        {
            if (((position.X) > _viewportWidth) || ((position.X + obj.Width) < 0.0f)) return false;
            if (((position.Y) > _viewportHeight) || ((position.Y + obj.Height) < 0.0f)) return false;

            return true;
        }
    }
}
