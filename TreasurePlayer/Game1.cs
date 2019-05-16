using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TreasurePlayer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        List<LoadLevel> SetWorld;
        Camera Cameratest;

        //List for Textures and Blocks...or rows
        List<Texture2D> Levels;
        List<Texture2D> Background;
        Random Rnd;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite Player; //the player sprite

        bool Up; //Bools to trigger the controls
        bool Right;
        bool Down;
        bool Left;
        bool Jump;
        bool ReadyToJump;
        bool Collision;

        bool SplashSound;
        bool Winner;

        bool Paused;
        Texture2D[] MBox;

        Texture2D BlackBack;

        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;
        AudioCategory generalCategory;
        AudioListener Listen;
        AudioEmitter Emiter;

        float generalVolume = 5.0f;
        //Song
        Cue Song;

        //Player Sounds
        Cue[] JumpSFX;
        Cue LandSFX;
        Cue DeathSFX;
        Cue[] WinSFX;
        Cue SplashSFX;


        //Pirate Sounds
        Cue [] PirateYarr;
        Cue PirateDeath;
        Cue PirateSwing;

        int History = 0;

        List<Sprite> Pirates;

        Vector2 Force; //This is the forces applied onto the player
        
        KeyboardState OldState;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this)
;
            Content.RootDirectory = "Content";
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            MBox = new Texture2D[1];

            MBox[0] = Content.Load<Texture2D>("hint");

            BlackBack = Content.Load<Texture2D>("BlackFront");

            Paused = true;

            SetWorld = new List<LoadLevel>();
            SetWorld.Add(new LoadLevel());
            SetWorld[0].level(5);
            Levels = new List<Texture2D>();
            SetBlocks();
            Background = new List<Texture2D>();
            SetBG();
            Cameratest = new Camera(graphics, new Vector2(0, 400));
            Rnd = new Random();
           // Addlevel();

            Collision = false;

            Player = new Sprite(10,6,1);

            Player.Standing = Content.Load<Texture2D>("Stand"); //Sets the images

            Player.Moving[0] = Content.Load<Texture2D>("Run\\Run2");
            Player.Moving[1] = Content.Load<Texture2D>("Run\\Run3");
            Player.Moving[2] = Content.Load<Texture2D>("Run\\Run4"); ;
            Player.Moving[3] = Content.Load<Texture2D>("Run\\Run5");
            Player.Moving[4] = Content.Load<Texture2D>("Run\\Run6");
            Player.Moving[5] = Content.Load<Texture2D>("Run\\Run7");
            Player.Moving[6] = Content.Load<Texture2D>("Run\\Run8");
            Player.Moving[7] = Content.Load<Texture2D>("Run\\Run9");
            Player.Moving[8] = Content.Load<Texture2D>("Run\\Run10");
            Player.Moving[9] = Content.Load<Texture2D>("Run\\Run10");
            
            
            Player.Jumping[5] = Content.Load<Texture2D>("Jump\\jump2");
            Player.Jumping[4] = Content.Load<Texture2D>("Jump\\jump3");
            Player.Jumping[3] = Content.Load<Texture2D>("Jump\\jump4");
            Player.Jumping[2] = Content.Load<Texture2D>("Jump\\jump5");
            Player.Jumping[1] = Content.Load<Texture2D>("Jump\\jump6");
            Player.Jumping[0] = Content.Load<Texture2D>("Jump\\jump7");
           
           

            Player.FacingRight = true;

            Player.SpritePosition = new Vector2(-200,0);

            Player.InAir = false;

            Player.update();

            Pirates = new List<Sprite>();

            OldState = Keyboard.GetState();

            Force = new Vector2(0, 1);

            Up = false;
            Right = false;
            Down = false;
            Left = false;
            Jump = false;
            ReadyToJump = false;

            Winner = false;

            SplashSound = false;
            

            engine = new AudioEngine("Content/Music.xgs");
            waveBank = new WaveBank(engine, "Content/Wave Bank.xwb");
            soundBank = new SoundBank(engine, "Content/Sound Bank.xsb");
            Listen = new AudioListener();
            Emiter = new AudioEmitter();

            //listen and emmiter data
            Listen.Position = new Vector3(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2, 0.0f);
            Emiter.Position = new Vector3(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2, 0.0f);

            generalCategory = engine.GetCategory("Default");

            //Song
            Song = soundBank.GetCue("Ditty");

            //Player Sounds
            JumpSFX = new Cue[4];
            WinSFX = new Cue[2];

            JumpSFX[0] = soundBank.GetCue("jump1");
            JumpSFX[1] = soundBank.GetCue("jump2");
            JumpSFX[2] = soundBank.GetCue("jump3");
            JumpSFX[3] = soundBank.GetCue("jump4");

            WinSFX[0] = soundBank.GetCue("success1");
            WinSFX[1] = soundBank.GetCue("success2");

            SplashSFX = soundBank.GetCue("sploosh");

            DeathSFX = soundBank.GetCue("death");

            LandSFX = soundBank.GetCue("land");


            //Pirate Sounds
            PirateYarr = new Cue[2];

            PirateYarr[0] = soundBank.GetCue("PirateYarr1");
            PirateYarr[1] = soundBank.GetCue("PirateYarr2");

            PirateDeath = soundBank.GetCue("Piratedeath");

            PirateSwing = soundBank.GetCue("Pirateswing");


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            

            //Controls
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) //Tells me which buttons have been pressed
            {
                this.Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
               Left = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Down = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Right = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Up = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Jump = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H) && !OldState.IsKeyDown(Keys.H))
            {
                if (Paused)
                    Paused = false;
                else
                    Paused = true;
            }

            if (!Song.IsPlaying)
            {
                if (!Song.IsPrepared)
                {
                    Song = soundBank.GetCue("Ditty");
                }
                Song.Play();
            }

            if (!Paused)
            {
                if (!Player.Dead)
                {
                    //Game Logic
                    if (Right) //movement
                    {
                        Player.MoveSprite(new Vector2((float).3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                        Player.FacingRight = true;
                        Player.Walking = true;
                    }
                    else if (Left)
                    {
                        if (Player.SpritePosition.X > SetWorld[0].maxLeft - 400)
                        {

                            Player.MoveSprite(new Vector2((float)-.3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                        }
                        Player.FacingRight = false;
                        Player.Walking = true;
                    }

                    if (Jump && !Player.InAir) //If the player is jumping and not already in the air...
                    {
                        ReadyToJump = true;
                    }

                    if (Player.Walking && !Player.InAir) //If the player is walking there is an animation
                    {
                        Player.Animate(gameTime);
                    }

                    if (ReadyToJump)
                    {
                        Player.AnimateJump(gameTime); //Animate a jump

                        if (Player.WhichJumpingTexture == 0)//At the end launch the player up
                        {
                            int Bother = Rnd.Next(0, 3);
                            if (!JumpSFX[Bother].IsPrepared)
                            {
                                if (Bother == 0)
                                {
                                    JumpSFX[0] = soundBank.GetCue("jump1");
                                }
                                else if (Bother == 1)
                                {
                                    JumpSFX[1] = soundBank.GetCue("jump2");
                                }
                                else if (Bother == 2)
                                {
                                    JumpSFX[2] = soundBank.GetCue("jump3");
                                }
                                else if (Bother == 3)
                                {
                                    JumpSFX[3] = soundBank.GetCue("jump4");
                                }
                            }
                            JumpSFX[Bother].Play();

                            Player.InAir = true;

                            if (Right || Left)
                            {
                                Force.Y = -((float).01 * (float)Player.Standing.Height) * 3.5f; //Apply some force to bring the player up.
                            }
                            else
                            {
                                Force.Y = -((float).01 * (float)Player.Standing.Height) * 4.5f; //Apply some force to bring the player up.
                            }
                            ReadyToJump = false;
                            
                            Player.Land = true;
                        }
                    }
                    Force.Y += (float).01 * (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Adds a constent force going down on the player;

                    Player.MoveSprite(Force); //Applies the force to the player

                    Player.update();

                    if(Player.Source.Intersects(SetWorld[0].WinBox[0])&& !Winner)
                    {
                        int Win = Rnd.Next(0, 1); 
                        if (!JumpSFX[Win].IsPrepared)
                        {
                            if (Win == 0)
                            {
                                WinSFX[0] = soundBank.GetCue("success1");
                            }
                            else if (Win == 1)
                            {
                                WinSFX[1] = soundBank.GetCue("success2");
                            }
                           
                        }
                        WinSFX[Win].Play();
                        Winner = true;
                    }

                    foreach (LoadLevel L in SetWorld)
                    {
                        foreach (Rectangle W in L.Watersboxes)
                        {
                            if (Player.Source.Intersects(W)) //If the player is on ground
                            {
                                if (!SplashSFX.IsPrepared)
                                {
                                    SplashSFX = soundBank.GetCue("sploosh");
                                }
                                if (!SplashSound&& L.water==true)
                                {
                                    SplashSFX.Play();
                                    L.water = false;
                                }
                                SplashSound = true;

                            }
                            else
                            {
                                L.water = true;
                                SplashSound = false;
                            }
                        }

                        foreach (Rectangle R in L.Collisionbox)
                        {
                            if (Player.Source.Intersects(R) && Player.Source.Bottom >= R.Y + 25) //If the player is on ground
                            {
                                if (Player.Source.Bottom - R.Y <= 35 && Force.Y > 0)
                                //if (Player.Source.Bottom - R.Y <= R.Height)
                                {
                                    if (Player.InAir) //If the player is in the air yet touching the ground they're landing
                                    {
                                        //Player.AnimateJump(gameTime);

                                        if (!LandSFX.IsPrepared)
                                        {
                                            LandSFX= soundBank.GetCue("land");
                                        }
                                        LandSFX.Play();
                                        Player.InAir = false;
                                        
                                    }

                                    Collision = true;
                                    Player.SpritePosition.Y = (R.Y - (Player.Source.Bottom - Player.Source.Y)) + 25; //They can't go through the ground.
                                    Force.Y = 1; //The force wont be building if you're not going faster down.
                                }
                                else
                                {
                                    if (Right)
                                    {
                                        Player.MoveSprite(new Vector2((float)-.3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                                    }
                                    else if (Left)
                                    {
                                        Player.MoveSprite(new Vector2((float).3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                                    }
                                }

                            }

                        }


                    }

                    if (!Collision)
                    {
                        Player.InAir = true;
                    }
                    Collision = false;
                    Player.update(); //Updates the player's rectangle.
                }
                else
                {
                    Player.update();
                    Player.AnimateDeath(gameTime);

                    Force.Y += (float).01 * (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Adds a constent force going down on the player;

                    Player.MoveSprite(Force); //Applies the force to the player
                    Player.update();

                    foreach (LoadLevel L in SetWorld)
                    {

                        foreach (Rectangle R in L.Collisionbox)
                        {
                            if (Player.Source.Intersects(R) && Player.Source.Bottom >= R.Y + 25) //If the player is on ground
                            {
                                if (Player.Source.Bottom - R.Y <= 35)
                                {
                                    Collision = true;
                                    Player.SpritePosition.Y = (R.Y - (Player.Source.Bottom - Player.Source.Y)) + 25; //They can't go through the ground.
                                    Force.Y = 1; //The force wont be building if you're not going faster down.
                                }


                            }

                        }


                    }

                    if (Player.SpritePosition.Y > 1600)
                    {
                        Player.Dead = true;
                    }


                    Collision = false;
                    Player.update(); //Updates the player's rectangle.

                    if (Jump)
                    {
                        Player.Dead = false;
                        Player.WhichDeathTexture = 0;
                        Player.SpritePosition = new Vector2(SetWorld[0].maxLeft - 200, 0);
                        Player.WhichJumpingTexture = 0;
                        Player.update();
                        Player.InAir = true;
                        Jump = false;
                        Cameratest._position.X = SetWorld[0].maxLeft;

                        foreach (Sprite p in Pirates)
                        {
                            p.Swing = false;
                        }
                    }
                }

                //MONSTERS
                if (Pirates.Count != 0)
                {
                    for (int i = Pirates.Count - 1; i >= 0; i--)
                    {
                       

                        Pirates[i].MoveSprite(Pirates[i].Force);
                        if (!Pirates[i].Dead)
                        {
                            if (!Player.Dead && Player.SpritePosition.X - Pirates[i].Source.Right <= 300 && Player.SpritePosition.X - Pirates[i].SpritePosition.X >= 0) //Tests if the player is in chasing range.
                            {
                                Pirates[i].TimeSinceSoundEffectUpdate += gameTime.ElapsedGameTime.Milliseconds;
                                if (Pirates[i].TimeSinceSoundEffectUpdate >= Pirates[i].TimeToUpdateSoundEffect)
                                {
                                    Pirates[i].TimeSinceSoundEffectUpdate = 0;
                                    int Yarr = Rnd.Next(0, 1);
                                    if (!PirateYarr[Yarr].IsPlaying)
                                    {
                                        if (!PirateYarr[Yarr].IsPrepared)
                                        {
                                            if (Yarr == 0)
                                            {
                                                PirateYarr[Yarr] = soundBank.GetCue("PirateYarr1");
                                            }
                                            else if (Yarr == 1)
                                            {
                                                PirateYarr[Yarr] = soundBank.GetCue("PirateYarr2");
                                            }
                                        }
                                        PirateYarr[Yarr].Play();
                                    }
                                }
                                Pirates[i].Walking = true;
                                Pirates[i].FacingRight = false;
                                Pirates[i].Animate(gameTime);

                                Pirates[i].MoveSprite(new Vector2((float).1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                            }
                            else if (!Player.Dead && Player.SpritePosition.X - Pirates[i].SpritePosition.X >= -300 && Player.SpritePosition.X - Pirates[i].SpritePosition.X <= 0)
                            {
                                Pirates[i].TimeSinceSoundEffectUpdate += gameTime.ElapsedGameTime.Milliseconds;
                                if (Pirates[i].TimeSinceSoundEffectUpdate >= Pirates[i].TimeToUpdateSoundEffect)
                                {
                                    Pirates[i].TimeSinceSoundEffectUpdate = 0;
                                    int Yarr = Rnd.Next(0, 1);
                                    if (!PirateYarr[Yarr].IsPlaying)
                                    {
                                        if (!PirateYarr[Yarr].IsPrepared)
                                        {
                                            if (Yarr == 0)
                                            {
                                                PirateYarr[Yarr] = soundBank.GetCue("PirateYarr1");
                                            }
                                            else if (Yarr == 1)
                                            {
                                                PirateYarr[Yarr] = soundBank.GetCue("PirateYarr2");
                                            }
                                        }
                                        PirateYarr[Yarr].Play();
                                    }
                                }
                                Pirates[i].Walking = true;
                                Pirates[i].FacingRight = true;
                                Pirates[i].Animate(gameTime);

                                Pirates[i].MoveSprite(new Vector2((float)-.1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                            }
                            else
                            {
                                Pirates[i].Walking = false;
                            }

                            Pirates[i].Force.Y += (float).01 * (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Adds a constent force going down on the monster;

                            Pirates[i].update();

                            bool MonsterCollide = false;

                            foreach (LoadLevel L in SetWorld)
                            {
                                foreach (Rectangle R in L.Collisionbox)
                                {
                                    if (Pirates[i].Source.Intersects(R) && Pirates[i].Source.Bottom >= R.Y + 30) //If the Monster is on ground
                                    {
                                        if (Pirates[i].Source.Bottom - R.Y <= 35)
                                        {
                                            if (Pirates[i].InAir) //If the player is in the air yet touching the ground they're landing
                                            {
                                                //Pirates[i].AnimateJump(gameTime);
                                                Pirates[i].InAir = false;
                                            }
                                            MonsterCollide = true;
                                            Pirates[i].SpritePosition.Y = (R.Y - (Pirates[i].Source.Bottom - Pirates[i].Source.Y)) + 30; //They can't go through the ground.
                                            Pirates[i].Force.Y = 1; //The force wont be building if you're not going faster down.
                                        }
                                        else
                                        {
                                            if (Player.SpritePosition.X - Pirates[i].Source.Right <= 200 && Player.SpritePosition.X - Pirates[i].SpritePosition.X >= 0)
                                            {
                                                Pirates[i].MoveSprite(new Vector2((float)-.1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                                            }
                                            else if (Player.SpritePosition.X - Pirates[i].SpritePosition.X >= -200 && Player.SpritePosition.X - Pirates[i].SpritePosition.X <= 0)
                                            {
                                                Pirates[i].MoveSprite(new Vector2((float).1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0));
                                            }
                                        }

                                    }
                                    Pirates[i].update();
                                }
                            }


                            if (MonsterCollide)
                            {
                                Pirates[i].InAir = false;
                                MonsterCollide = false;
                            }
                            else
                            {
                                Pirates[i].InAir = true;
                            }
                            Player.update();
                            Pirates[i].update();
                            if (Pirates[i].Source.Intersects(Player.Source))
                            {
                                if (Player.Source.Bottom <= Pirates[i].SpritePosition.Y + 10)
                                {
                                    Pirates[i].Dead = true;
                                    Force.Y = -((float).01 * (float)Player.Standing.Height) * 3;

                                    if (!PirateDeath.IsPlaying)
                                    {
                                        if (!PirateDeath.IsPrepared)
                                        {
                                            PirateDeath = soundBank.GetCue("Piratedeath");
                                        }
                                        PirateDeath.Play();
                                    }
                                }
                                else
                                {
                                    if (!Player.Dead && !Pirates[i].Dead)
                                    {
                                        if (!DeathSFX.IsPrepared)
                                        {
                                            DeathSFX = soundBank.GetCue("death");
                                        }
                                        DeathSFX.Play();

                                        Pirates[i].WhichDeathTexture = 0;
                                        Pirates[i].Swing = true;

                                        if (!PirateSwing.IsPlaying)
                                        {
                                            if (!PirateSwing.IsPrepared)
                                            {
                                                PirateSwing = soundBank.GetCue("Pirateswing");
                                            }
                                            PirateSwing.Play();
                                        }

                                        Player.Dead = true;
                                    }  
                                        
                                }
                                
                            }
                            
                            if (Pirates[i].Swing)
                                {
                                    //YOU NEED TO FIX THIS IF WE ADD A PIRATE DEATH ANIMATION
                                    Pirates[i].AnimateDeath(gameTime);
                                }
                            Pirates[i].update();
                        }


                        else
                        {
                            Pirates[i].Force.Y += (float).01 * (float)gameTime.ElapsedGameTime.TotalMilliseconds; //Adds a constent force going down on the monster;
                        }

                        Pirates[i].update();

                        if (Pirates[i].SpritePosition.Y > 1600)
                        {
                            Pirates.Remove(Pirates[i]);
                        }
                    }

                }

                //____________________ Background stuff
                Player.update();

                if (Player.SpritePosition.X > SetWorld[0].maxLeft)
                {
                    Cameratest._position.X = Player.SpritePosition.X;
                }
                if (Player.Source.Bottom > 600)
                {
                    Cameratest._position.Y = Player.SpritePosition.Y;
                }

                if (Player.SpritePosition.X - History >= 0)
                {
                    Addlevel();
                    History += 800;
                }

            }
            else
            {

            }




            //Controls Release______________________________________________________________________________________________
            if (Keyboard.GetState().IsKeyUp(Keys.A)) //Turns off controls.
            {
                Left = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.S))
            {
                Down = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D))
            {
                Right = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.W))
            {
                Up = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                Jump = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Addlevel();
            }

            OldState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.SkyBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //spriteBatch.Begin(SpriteBlendMode.None);

            Vector2 CamPosition;


            foreach (LoadLevel World in SetWorld)
            //Draw Background
            {
                CamPosition = Cameratest.Transform(World.Background);
                //spriteBatch.Draw(Background[0], CamPosition+new Vector2(0,400) , Color.White);
                //Draw Level
                CamPosition = Cameratest.Transform(World.LevelsVectors);
                spriteBatch.Draw(Levels[World.Stage], CamPosition + new Vector2(0, 400), World.Rect, Color.White);
                //Draw Sprite
                
            }
            CamPosition = Cameratest.Transform(Player.SpritePosition);
            CamPosition += new Vector2(400, 400);
            
            if (!Player.InAir && Player.FacingRight && !Player.Walking && !ReadyToJump) //Depending on the situation this will draw the player diffrently.
            {
                spriteBatch.Draw(Player.Standing, CamPosition , Color.White);
            }
            else if (!Player.InAir && !Player.FacingRight && !Player.Walking && !ReadyToJump)
            {
                spriteBatch.Draw(Player.Standing, CamPosition , null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
            }
            else if (!Player.InAir && Player.FacingRight && !ReadyToJump)
            {
                spriteBatch.Draw(Player.Moving[Player.WhichTexture], CamPosition , Color.White);
                Player.Walking = false;
            }
            else if (!Player.InAir && !(Player.FacingRight) && !ReadyToJump)
            {
                spriteBatch.Draw(Player.Moving[Player.WhichTexture], CamPosition , null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                Player.Walking = false;
            }
            else if ((Player.InAir || ReadyToJump) && !(Player.FacingRight))
            {
                spriteBatch.Draw(Player.Jumping[Player.WhichJumpingTexture], CamPosition , null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
            }
            else if ((Player.InAir || ReadyToJump) && (Player.FacingRight))
            {
                spriteBatch.Draw(Player.Jumping[Player.WhichJumpingTexture], CamPosition , Color.White);
            }

            //_______________________
            foreach(Sprite P in Pirates)
            {
                CamPosition = Cameratest.Transform(P.SpritePosition);
                CamPosition += new Vector2(400, 400);


                if (P.Swing && P.FacingRight) //Depending on the situation this will draw the player diffrently.
                {
                    spriteBatch.Draw(P.Death[P.WhichDeathTexture], CamPosition, Color.White);
                }
                else if (!P.Swing && !P.FacingRight)
                {
                    spriteBatch.Draw(P.Death[P.WhichDeathTexture], CamPosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                }
                else if (!P.InAir && P.FacingRight && !P.Walking) //Depending on the situation this will draw the player diffrently.
                {
                    spriteBatch.Draw(P.Standing, CamPosition, Color.White);
                }
                else if (!P.InAir && !P.FacingRight && !P.Walking)
                {
                    spriteBatch.Draw(P.Standing, CamPosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                }
                else if (!P.InAir && P.FacingRight)
                {
                    spriteBatch.Draw(P.Moving[P.WhichTexture], CamPosition, Color.White);
                    P.Walking = false;
                }
                else if (!P.InAir && !(P.FacingRight))
                {
                    spriteBatch.Draw(P.Moving[P.WhichTexture], CamPosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                    P.Walking = false;
                }
                else if ((P.InAir || ReadyToJump) && !(P.FacingRight))
                {
                    spriteBatch.Draw(P.Jumping[P.WhichJumpingTexture], CamPosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.FlipHorizontally, 0f);
                }
                else if ((P.InAir || ReadyToJump) && (P.FacingRight))
                {
                    spriteBatch.Draw(P.Jumping[P.WhichJumpingTexture], CamPosition, Color.White);
                }
            }

            if (Paused)
            {
                spriteBatch.Draw(BlackBack, new Vector2(CamPosition.X-CamPosition.X,CamPosition.Y-CamPosition.Y) , Color.White);
                spriteBatch.Draw(MBox[0], new Vector2(CamPosition.X - CamPosition.X, CamPosition.Y - CamPosition.Y), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void SetBlocks()
        {
            Levels.Add(Content.Load<Texture2D>("LevelOne"));
            Levels.Add(Content.Load<Texture2D>("LevelTwo"));
            Levels.Add(Content.Load<Texture2D>("LevelThree"));
            Levels.Add(Content.Load<Texture2D>("LevelFour"));
            Levels.Add(Content.Load<Texture2D>("LevelFive"));
            Levels.Add(Content.Load<Texture2D>("LevelZero"));
        }
        public void SetBG()
        {
            Background.Add(Content.Load<Texture2D>("Background"));
        }
        public void Addlevel()
        {
            int temp = 0;
            temp = Rnd.Next(0, 5);
            SetWorld.Add(new LoadLevel());
            SetWorld[SetWorld.Count - 1].level(temp);

            SetWorld[SetWorld.Count - 1].LevelsVectors.X = SetWorld[SetWorld.Count - 2].LevelsVectors.X + 800;
            SetWorld[SetWorld.Count - 1].Background.X = SetWorld[SetWorld.Count - 2].LevelsVectors.X + 800;
            SetWorld[SetWorld.Count - 1].Increase(SetWorld[SetWorld.Count - 2].LevelsVectors.X + 800, temp);


            
          
            
            if (SetWorld[SetWorld.Count - 1].Stage == 0)
            {
                Pirates.Add(new Sprite(5, 1, 6));

                Pirates[Pirates.Count - 1].MoveSprite(new Vector2(SetWorld[SetWorld.Count - 1].LevelsVectors.X, 0));

                Pirates[Pirates.Count - 1].Standing = Content.Load<Texture2D>("Pirate\\standing_v3"); //Sets the images

                Pirates[Pirates.Count - 1].Moving[0] = Content.Load<Texture2D>("Pirate\\run2_v3");
                Pirates[Pirates.Count - 1].Moving[1] = Content.Load<Texture2D>("Pirate\\run3_v3");
                Pirates[Pirates.Count - 1].Moving[2] = Content.Load<Texture2D>("Pirate\\run4_v3");
                Pirates[Pirates.Count - 1].Moving[3] = Content.Load<Texture2D>("Pirate\\run5_v3");
                Pirates[Pirates.Count - 1].Moving[4] = Content.Load<Texture2D>("Pirate\\run6_v3");

                Pirates[Pirates.Count - 1].Jumping[0] = Content.Load<Texture2D>("Pirate\\standing_v3");

                Pirates[Pirates.Count - 1].Death[0] = Content.Load<Texture2D>("Pirate\\swing1_v3");
                Pirates[Pirates.Count - 1].Death[1] = Content.Load<Texture2D>("Pirate\\swing2_v3");
                Pirates[Pirates.Count - 1].Death[2] = Content.Load<Texture2D>("Pirate\\swing3_v3");
                Pirates[Pirates.Count - 1].Death[3] = Content.Load<Texture2D>("Pirate\\swing3_v3");
                Pirates[Pirates.Count - 1].Death[4] = Content.Load<Texture2D>("Pirate\\swing2_v3");
                Pirates[Pirates.Count - 1].Death[5] = Content.Load<Texture2D>("Pirate\\swing1_v3");

                Pirates[Pirates.Count - 1].FacingRight = true;

                Pirates[Pirates.Count - 1].InAir = false;

                Pirates[Pirates.Count - 1].update();
            }
            if (SetWorld[SetWorld.Count - 1].Stage == 2)
            {
                Pirates.Add(new Sprite(5, 1, 6));

                Pirates[Pirates.Count - 1].MoveSprite(new Vector2(SetWorld[SetWorld.Count - 1].LevelsVectors.X, 300));

                Pirates[Pirates.Count - 1].Standing = Content.Load<Texture2D>("Pirate\\standing_v3"); //Sets the images

                Pirates[Pirates.Count - 1].Moving[0] = Content.Load<Texture2D>("Pirate\\run2_v3");
                Pirates[Pirates.Count - 1].Moving[1] = Content.Load<Texture2D>("Pirate\\run3_v3");
                Pirates[Pirates.Count - 1].Moving[2] = Content.Load<Texture2D>("Pirate\\run4_v3");
                Pirates[Pirates.Count - 1].Moving[3] = Content.Load<Texture2D>("Pirate\\run5_v3");
                Pirates[Pirates.Count - 1].Moving[4] = Content.Load<Texture2D>("Pirate\\run6_v3");

                Pirates[Pirates.Count - 1].Jumping[0] = Content.Load<Texture2D>("Pirate\\standing_v3");

                Pirates[Pirates.Count - 1].Death[0] = Content.Load<Texture2D>("Pirate\\swing1_v3");
                Pirates[Pirates.Count - 1].Death[1] = Content.Load<Texture2D>("Pirate\\swing2_v3");
                Pirates[Pirates.Count - 1].Death[2] = Content.Load<Texture2D>("Pirate\\swing3_v3");
                Pirates[Pirates.Count - 1].Death[3] = Content.Load<Texture2D>("Pirate\\swing3_v3");
                Pirates[Pirates.Count - 1].Death[4] = Content.Load<Texture2D>("Pirate\\swing2_v3");
                Pirates[Pirates.Count - 1].Death[5] = Content.Load<Texture2D>("Pirate\\swing1_v3");

                Pirates[Pirates.Count - 1].FacingRight = true;

                Pirates[Pirates.Count - 1].InAir = false;

                Pirates[Pirates.Count - 1].update();
            }

           
       

            if (SetWorld.Count == 5)
            {
                SetWorld[0].LevelsVectors = SetWorld[1].LevelsVectors;
                SetWorld[0].Background = SetWorld[1].Background;
                SetWorld[1] = SetWorld[0];
                SetWorld[1].maxLeft += 800;
                SetWorld.RemoveAt(0);
                SetWorld[0].Increase(SetWorld[0].LevelsVectors.X, 5);

            }

        }
    }
}
