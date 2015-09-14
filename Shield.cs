using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Bullet_Rebound
{
    class Shield
    {
        public Sprite ShieldSprite;
        private float shieldDistance = 50f;
        private float shieldRotation = MathHelper.ToRadians(0);
        private float shieldRotationSpeed = 720f;
        private readonly Rectangle shieldSize = new Rectangle(0, 0, 10, 30);
        private readonly Vector2 shieldOrigin = new Vector2(5, 5);
        readonly Texture2D pixel;
        private float playerRotation = 0f;
        PlayerManager playerManager;

        public Shield( Texture2D texture, Rectangle Initialframe,
            Rectangle ScreenBounds)

        private Matrix GetPlayerWorldMatrix()
        {
            return Matrix.CreateRotationZ(playerRotation) * Matrix.CreateTranslation(new Vector3(playerManager.playerSprite.Location, 0f));
        }     
        

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboard.IsKeyDown(Keys.Up)) shieldRotation += MathHelper.ToRadians(shieldRotationSpeed) * elapsed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 localShield = new Vector2(0, shieldDistance);
            Matrix shieldMatrix = Matrix.CreateRotationZ(shieldRotation) *
                GetPlayerWorldMatrix();
            Vector2 shieldPosition = Vector2.Transform(localShield, shieldMatrix);
            
        }

        public Texture2D texture { get; set; }

        public Rectangle initialFrame { get; set; }

        public int frameCount { get; set; }
    }
}
