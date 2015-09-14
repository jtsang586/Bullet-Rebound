using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameInfo;

namespace Bullet_Rebound
{
    class PlayerManager
    {        
        public Sprite playerSprite;
        public float playerSpeed = 250.0f;
        private Vector2 playerPosition = Vector2.Zero;
        private static MouseState mouseState, lastmouseState;
        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2
                    (mouseState.X, mouseState.Y);
            }
        }

        public long PlayerScore = 0;
        public int Lives = 1;
        public float playerShotSpeed = 350f;
        public bool Destroyed = false;
        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;
        private readonly Rectangle playerSize = new Rectangle(0, 0, 48, 48);
        private readonly Vector2 playerOrigin = new Vector2(15, 15);
        PlayerInfo playerinfo;
        
        public PlayerManager (
            Texture2D texture,            
            Rectangle initialFrame,
            int frameCount, 
            Rectangle screenBounds, Stream fileStream)
        {
            playerSprite = new Sprite(
                new Vector2 (370, 200), texture,
                initialFrame, Vector2.Zero);

            PlayerShotManager = new ShotManager(
                texture, new Rectangle(0, 300, 5, 5), 4, 2,350f, screenBounds);
            

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite.CollisionRadius = playerRadius;
            
            /*this.playerSpeed = playerinfo.playerSpeed;
            this.playerShotSpeed = playerinfo.playershotSpeed;
            this.Lives = playerinfo.playerLives;*/
        }

        

        private void FireShot()
        {
            if (shotTimer >= minShotTimer)
            {
                Vector2 shotDirection = MousePosition - playerSprite.Location;
                shotDirection.Normalize();

                PlayerShotManager.FireShot(
                    playerSprite.Location + gunOffset,
                    shotDirection,
                    true);
                shotTimer = 0.0f;
            }
        }        

        //each movement function
        public void Up(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                playerSprite.Velocity += new Vector2(0, -1);
            }
            else
            {
                playerSprite.Velocity += new Vector2(0, 0);
            }
        }

        public void Down(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                playerSprite.Velocity += new Vector2(0, 1);
            }
            else
            {
                playerSprite.Velocity += new Vector2(0, 0);
            }
        }

        public void Left(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                playerSprite.Velocity += new Vector2(-1, 0);
            }
            else
            {
                playerSprite.Velocity += new Vector2(0, 0);
            }
        }

        public void Right(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                playerSprite.Velocity += new Vector2(1, 0);
            }
            else
            {
                playerSprite.Velocity += new Vector2(0, 0);
            }
        }

        public void fire(eButtonState buttonState, Vector2 amount)
        {
            if (buttonState == eButtonState.DOWN)
            {
                FireShot();
            }
        }  
              

        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);
            lastmouseState = mouseState;
            mouseState = Mouse.GetState();
            if (MousePosition != new Vector2 (lastmouseState.X, lastmouseState.Y));

            if (!Destroyed)
            {
                //if destroyed then all those below will not be updated
                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;             
                playerSprite.Velocity.Normalize();
                playerSprite.Velocity *= playerSpeed;
                playerSprite.Update(gameTime);  
                playerSprite.Velocity = Vector2.Zero;           
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                playerSprite.Draw(spriteBatch);
            }
        }
    }
}
