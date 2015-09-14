using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Rebound
{
    class PowerupLife
    {
        public Sprite pwrLife;   
        public float spawnTime = 10.0f;
        public float timeActive = 4.0f;
        public bool Active = false;
        public Random rand = new Random();
        public int screenWidth = 800;
        public int screenHeight = 600;        

        public PowerupLife(Texture2D texture, Rectangle initialFrame, 
            int frameCount, Rectangle screenBounds)            
        {
            pwrLife = new Sprite(
                 new Vector2 (rand.Next (0, screenWidth-50),
                     rand.Next (0, screenHeight-50)), texture, initialFrame, Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                pwrLife.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
        }
                                     
        //spawns power up based on spawnTime
        public void Update(GameTime gameTime)
        {
            if (spawnTime == 0.0f)
            {
                Active = true;
                {
                    timeActive = MathHelper.Max(0, timeActive -
                        (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (timeActive == 0.0f)
                    {                        
                        spawnTime = 10f;
                        timeActive = 4f;
                    }                    
                }
                pwrLife.Update(gameTime);                
            }
            else 
            {
                //spawn resets to a random location 
                Active = false;
                pwrLife.Location = new Vector2(rand.Next(0, screenWidth - 50),
                    rand.Next(0, screenHeight - 50));
            }
            

            spawnTime = MathHelper.Max(0, spawnTime -
                (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active == true)
            {
                pwrLife.Draw(spriteBatch);
                
            }                         
        }
    }
}
