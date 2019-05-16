/**********************************************
 * Bullets Class
 * 
 * This class contains data about bullets and how
 * they'll work.
 * 
 * Jan 30, 2010
 * By Dylan Wilson
 * *******************************************/
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TreasurePlayer
{
    class Bullets
    {
        public Texture2D Texture;

        public Vector2 Position;

        public Vector2 Direction;

        public Rectangle Source;

        public Bullets()
        {
            Position = new Vector2(0, 0);

            Direction = new Vector2(0, 0);

            Source = new Rectangle(0, 0, 0, 0);
        }
    }
}
