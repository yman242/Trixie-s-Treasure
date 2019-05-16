using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    class LoadLevel
    {

        public Rectangle Rect;
        public List<Rectangle> Collisionbox;
        public List<Rectangle> Sideboxes;
        public List<Rectangle> WinBox;
        public List<Rectangle> Watersboxes;
        public bool water;

        public string SetLevel;


        public Vector2 LevelsVectors;
        public Vector2 Background;
        public int Stage;
        public float maxLeft;


        public LoadLevel()
        {
            Sideboxes = new List<Rectangle>();
            Rect = new Rectangle(0, 0, 800, 600);
            Collisionbox = new List<Rectangle>();
            LevelsVectors = new Vector2(0, 0);
            Background = new Vector2(0, 0);
            WinBox = new List<Rectangle>();
            Watersboxes = new List<Rectangle>();
            water =false;
        }

        public void level(int World)
        {
            Stage = World;
            LoadRect(World);
        }
        public void LoadRect(int World)
        {
            switch (World)
            {
                case 0:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400, 500, 800, 200));
                    break;
                case 1:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-200, 400, 150, 106));
                    Collisionbox.Add(new Rectangle(-150, 300, 95, 200));
                    Collisionbox.Add(new Rectangle(-148, 400, 95, 215));
                    Collisionbox.Add(new Rectangle(-50, 450, 50, 50));
                    Collisionbox.Add(new Rectangle(200, 400, 50, 110));

                    break;
                case 2:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-200, 450, 400, 50));
                    break;
                case 3:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-300, 450, 600, 50));
                    Collisionbox.Add(new Rectangle(-150, 400, 100, 50));
                    Collisionbox.Add(new Rectangle(-50, 350, 150, 50));
                    Collisionbox.Add(new Rectangle(100, 450, 50, 50));
                    Collisionbox.Add(new Rectangle(150, 400, 100, 50));
                    break;
                case 4:
                    //add collosions to each level
                    Watersboxes.Add(new Rectangle(-200, 530, 450, 100));
                    Collisionbox.Add(new Rectangle(-400, 500, 150, 200));
                    Collisionbox.Add(new Rectangle(-203, 575, 60, 50));
                    Collisionbox.Add(new Rectangle(-150, 500, 60, 50));
                    Collisionbox.Add(new Rectangle(-100, 570, 100, 50));
                    Collisionbox.Add(new Rectangle(0, 500, 60, 50));
                    Collisionbox.Add(new Rectangle(50, 570, 100, 50));
                    Collisionbox.Add(new Rectangle(150, 500, 60, 50));
                    Collisionbox.Add(new Rectangle(200, 575, 60, 50));
                    water = true;
                    break;
                case 5:
                    Rect.Height = 800;
                    //add collosions to each level
                    WinBox.Add(new Rectangle(-400, 610, 100, 100));
                    Collisionbox.Add(new Rectangle(-400, 692, 800, 200));
                    Collisionbox.Add(new Rectangle(-185, 350, 160, 100));
                    Collisionbox.Add(new Rectangle(0, 400, 100, 100));
                    Collisionbox.Add(new Rectangle(115, 450, 160, 100));
                    Collisionbox.Add(new Rectangle(-185, 500, 800, 200));
                    break;

            }
            for (int i = 0; i < Collisionbox.Count; i++)
            {
                Sideboxes.Add(new Rectangle(Collisionbox[i].X - 10, Collisionbox[i].Y + 10, Collisionbox[i].Width + 20, Collisionbox[i].Height - 10));
            }

        }
        public void Increase(float inc, int world)
        {
            Stage = world;
            Sideboxes.Clear();
            Collisionbox.Clear();
            Watersboxes.Clear();
            WinBox.Clear();
            switch (world)
            {
                case 0:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 500, 800, 200));
                    break;
                case 1:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-200 + (int)inc, 400, 50, 106));
                    Collisionbox.Add(new Rectangle(-150 + (int)inc, 300, 50, 200));
                    Collisionbox.Add(new Rectangle(-148 + (int)inc, 420, 95, 215));
                    Collisionbox.Add(new Rectangle(195 + (int)inc, 400, 30, 50));
                    Collisionbox.Add(new Rectangle(210 + (int)inc, 455, 50, 110));

                    break;
                case 2:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-200 + (int)inc, 450, 400, 50));
                    break;
                case 3:
                    //add collosions to each level
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 500, 800, 200));
                    Collisionbox.Add(new Rectangle(-300 + (int)inc, 450, 600, 50));
                    Collisionbox.Add(new Rectangle(-150 + (int)inc, 400, 100, 50));
                    Collisionbox.Add(new Rectangle(-50 + (int)inc, 350, 150, 50));
                    Collisionbox.Add(new Rectangle(100 + (int)inc, 450, 50, 50));
                    Collisionbox.Add(new Rectangle(150 + (int)inc, 400, 75, 50));
                    break;
                case 4:
                    //add collosions to each level
                    Watersboxes.Add(new Rectangle(-200 + (int)inc, 530, 450, 100));
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 500, 150, 200));
                    Collisionbox.Add(new Rectangle(-203 + (int)inc, 575, 60, 50));
                    Collisionbox.Add(new Rectangle(-150 + (int)inc, 500, 30, 50));
                    Collisionbox.Add(new Rectangle(-120 + (int)inc, 570, 70, 50));
                    Collisionbox.Add(new Rectangle(0 + (int)inc, 500, 20, 50));
                    Collisionbox.Add(new Rectangle(20 + (int)inc, 570, 100, 50));
                    Collisionbox.Add(new Rectangle(160 + (int)inc, 500, 30, 50));
                    Collisionbox.Add(new Rectangle(190 + (int)inc, 575, 60, 50));
                    Collisionbox.Add(new Rectangle(240 + (int)inc, 500, 300, 50));
                    water = true;
                    break;
                case 5:

                    //add collosions to each level
                    WinBox.Add(new Rectangle(-400 + (int)inc, 610, 100, 100));
                    Watersboxes.Add(new Rectangle(200 + (int)inc, 530, 450, 100));
                    Collisionbox.Add(new Rectangle(-400 + (int)inc, 692, 800, 200));
                    Collisionbox.Add(new Rectangle(-165 + (int)inc, 350, 180, 100));
                    Collisionbox.Add(new Rectangle(0 + (int)inc, 400, 100, 100));
                    Collisionbox.Add(new Rectangle(115 + (int)inc, 450, 160, 100));
                    Collisionbox.Add(new Rectangle(-250 + (int)inc, 500, 800, 200));
                 
                    break;
            }
            for (int i = 0; i < Collisionbox.Count; i++)
            {
                Sideboxes.Add(new Rectangle(Collisionbox[i].X - 10, Collisionbox[i].Y + 10, Collisionbox[i].Width + 20, Collisionbox[i].Height - 10));
            }
        }

        //public void Update()
        //{
        //    for (int i = 0; i <= Collisionbox.Count; i++)
        //    {
        //        Collisionbox[i].X -= 800;
        //    }

        //}


    }
}
