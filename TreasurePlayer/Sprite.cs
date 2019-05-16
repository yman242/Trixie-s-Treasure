/*******************************************
 * Sprite Class
 * 
 * This class contains data used for sprites.
 *
 * Jan 30, 2010
 * By Dylan Wilson
 *******************************************/

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
    class Sprite
    {
        public Rectangle Source; //Source rectangle to tell the size of the sprite
        public Rectangle Default; //Source rectangle to tell the size of the sprite
        public Vector2 SpritePosition; //the location of the sprite

        public int TimeSinceUpdate; //How long it's been since we last updated.
        public int TimeSinceDeathUpdate;
        public int TimeSinceJumpUpdate; //How long it's been since we last updated the jump.
        public int TimeToUpdate; //How long should we wait to change the frame.
        public Vector2 Force;


        public int TimeSinceSoundEffectUpdate; //How long it's been since we last updated the jump.
        public int TimeToUpdateSoundEffect; //How long should we wait to change the frame.

        public Texture2D [] Moving; //This is the sprite
        public Texture2D Standing;
        public Texture2D [] Jumping; //This is the sprite
        public Texture2D[] Death;

        public bool FacingRight; //Bools used as flags to give information
        public bool Walking;
        public bool InAir;
        public bool Land;
        public bool Swing;

        public bool Dead;

        public int WhichTexture; //Tells which texture the player should be seeing;
        public int WhichJumpingTexture;
        public int WhichDeathTexture;
        public int MaxTexture; //The max amount of textures used for walking.
        public int MaxJumpTexture; //The max amount of textures used for jumping.
        public int MaxDeathtexture;

        //Brandon Code here
        

        public Sprite(int AmountOfTextures, int AmountOfJumpTextures, int AmountOfDeathTextures)
        {
            Moving = new Texture2D[AmountOfTextures];

            Jumping = new Texture2D[AmountOfJumpTextures];

            Death = new Texture2D[AmountOfDeathTextures];

            SpritePosition = Vector2.Zero;

            TimeSinceSoundEffectUpdate = 0; //How long it's been since we last updated the jump.
            TimeToUpdateSoundEffect = 500; //How long should we wait to change the frame.

            MaxTexture = AmountOfTextures - 1;
            MaxJumpTexture = AmountOfJumpTextures - 1;
            MaxDeathtexture = AmountOfDeathTextures - 1;

            TimeSinceUpdate = 0;
            TimeSinceDeathUpdate = 0;
            TimeSinceJumpUpdate = 0;
            TimeToUpdate = 60;

            WhichTexture = 0;
            WhichJumpingTexture = 0;

            FacingRight = true;
            Walking = false;

            Dead = false;
            Force = new Vector2(0,1);

            Land = true;

            Swing = false;
        }

        public void MoveSprite(Vector2 Move)
        {
            SpritePosition += Move;
        }

        public void Animate(GameTime gameTime) //Animates the walking
        {
            TimeSinceUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if (TimeSinceUpdate >= TimeToUpdate)
            {
                WhichTexture++;

                if (WhichTexture > MaxTexture)
                {
                    WhichTexture = 0;
                }

                TimeSinceUpdate -= TimeToUpdate;
            }
        }

        public void AnimateJump(GameTime gameTime) //Animates the Jump
        {
            TimeSinceJumpUpdate += gameTime.ElapsedGameTime.Milliseconds;
            if (Land) //If land
            {
                WhichJumpingTexture = MaxJumpTexture; //Reset the value to max
                Land = false;
            }

            if (TimeSinceJumpUpdate >= TimeToUpdate) //Update frames acording to time. 
            {
                WhichJumpingTexture--; //Work backwords

                if (WhichJumpingTexture <0)
                {
                    WhichJumpingTexture = 0;
                    Land = true;
                    InAir = false;
                }

                TimeSinceJumpUpdate -= TimeToUpdate;
            }
        }

        public void AnimateDeath(GameTime gameTime) //Animates the Jump
        {
            TimeSinceDeathUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if (TimeSinceDeathUpdate >= TimeToUpdate) //Update frames acording to time. 
            {
                WhichDeathTexture++; //Work backwords

                if (WhichDeathTexture > MaxDeathtexture)
                {
                    WhichDeathTexture = MaxDeathtexture;
                }

                TimeSinceDeathUpdate -= TimeToUpdate;
            }
            
        }
        public void update() //Updates the Source rectangle
        {
            Default = new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, Standing.Width, Standing.Height);

            if (Walking)
            {
                Source = new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, Standing.Width-20, Standing.Height);
            }
            else if (!(InAir) && FacingRight)
            {
                Source = new Rectangle((int)SpritePosition.X + (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Moving[WhichTexture].Width, Moving[WhichTexture].Height);
            }
            //else if (!(InAir))
            //{
            //    Source = new Rectangle((int)SpritePosition.X - (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Moving[WhichTexture].Width, Moving[WhichTexture].Height);
            //}
            else if (InAir)
            {
                Source = new Rectangle((int)SpritePosition.X + (Default.Width - Jumping[WhichJumpingTexture].Width), (int)SpritePosition.Y + (Default.Height - Jumping[WhichJumpingTexture].Height), Jumping[WhichJumpingTexture].Width, Jumping[WhichJumpingTexture].Height);
            }

        }
        //public void update() //Updates the Source rectangle
        //{
        //    Default = new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, Standing.Width, Standing.Height);
 
        //    if(!Walking)
        //    {
        //        Source = new Rectangle((int)SpritePosition.X , (int)SpritePosition.Y, Standing.Width, Standing.Height);
        //    }
        //    else if (!(InAir) && FacingRight)
        //    {
        //        Source = new Rectangle((int)SpritePosition.X + (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Moving[WhichTexture].Width, Moving[WhichTexture].Height);
        //        //Source = new Rectangle((int)SpritePosition.X + (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Default.Width, Default.Height);
                
        //    }
        //    else if (!(InAir))
        //    {
        //        Source = new Rectangle((int)SpritePosition.X - (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Moving[WhichTexture].Width, Moving[WhichTexture].Height);
        //        //Source = new Rectangle((int)SpritePosition.X - (Default.Width - Moving[WhichTexture].Width), (int)SpritePosition.Y + (Default.Height - Moving[WhichTexture].Height), Default.Width, Default.Height);
        //    }
        //    else if (InAir && FacingRight)
        //    {
        //        Source = new Rectangle((int)SpritePosition.X + (Default.Width - Jumping[WhichJumpingTexture].Width), (int)SpritePosition.Y + (Default.Height - Jumping[WhichJumpingTexture].Height), Jumping[WhichJumpingTexture].Width, Jumping[WhichJumpingTexture].Height);
        //        //Source = new Rectangle((int)SpritePosition.X + (Default.Width - Jumping[WhichJumpingTexture].Width), (int)SpritePosition.Y + (Default.Height - Jumping[WhichJumpingTexture].Height), Default.Width, Default.Height);
        //    }
        //    else if (InAir)
        //    {
        //        Source = new Rectangle((int)SpritePosition.X - (Default.Width - Jumping[WhichJumpingTexture].Width), (int)SpritePosition.Y + (Default.Height - Jumping[WhichJumpingTexture].Height), Jumping[WhichJumpingTexture].Width, Jumping[WhichJumpingTexture].Height);
        //        //Source = new Rectangle((int)SpritePosition.X - (Default.Width - Jumping[WhichJumpingTexture].Width), (int)SpritePosition.Y + (Default.Height - Jumping[WhichJumpingTexture].Height), Default.Width, Default.Height);
        //    }

        //}

    }
}
