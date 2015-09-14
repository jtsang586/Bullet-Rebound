using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bullet_Rebound
{
    class CollisionManager
    {        
        private PlayerManager playerManager;
        private EnemyManager enemyManager;
        private ExplosionManager explosionManager;
        PowerupLife life;
        private Vector2 offScreen = new Vector2(-500, -500);
        private int enemyPointValue = 100;
        

        public CollisionManager(PlayerManager playerManager, 
            EnemyManager enemyManager, 
            ExplosionManager explosionManager, PowerupLife life)
        {
            // TODO: Complete member initialization
            this.playerManager = playerManager;
            this.enemyManager = enemyManager;
            this.explosionManager = explosionManager;
            this.life = life;
        }

        private void checkShotToEnemyCollisions()
        {
            foreach (Sprite shot in playerManager.PlayerShotManager.Shots)
            {
                foreach (Enemy enemy in enemyManager.Enemies)
                {
                    if (shot.IsCircleColliding(
                        enemy.EnemySprite.Center,
                        enemy.EnemySprite.CollisionRadius))
                    {
                        //enemy is destroyed and score +100
                        shot.Location = offScreen;
                        enemy.Destroyed = true;
                        playerManager.PlayerScore += enemyPointValue;
                        explosionManager.AddExplosion(
                            enemy.EnemySprite.Center,
                            enemy.EnemySprite.Velocity / 10);
                    }
                }
            }
        }
        

        private void checkShotToPlayerCollisions()
        {
            foreach (Sprite shot in enemyManager.EnemyShotManager.Shots)
            {
                if (shot.IsCircleColliding(
                    playerManager.playerSprite.Center,
                    playerManager.playerSprite.CollisionRadius))
                {
                    //player is destroyed and loses one life
                    shot.Location = offScreen;
                    playerManager.Destroyed = true;
                    explosionManager.AddExplosion(
                        playerManager.playerSprite.Center,
                        Vector2.Zero);
                }
            }
        }

        private void checkEnemyToPlayerCollisions()
        {
            foreach (Enemy enemy in enemyManager.Enemies)
            {
                if (enemy.EnemySprite.IsCircleColliding(
                    playerManager.playerSprite.Center,
                    playerManager.playerSprite.CollisionRadius))
                {
                    //both enemy and player are destroyed
                    enemy.Destroyed = true;
                    explosionManager.AddExplosion(
                        enemy.EnemySprite.Center,
                        enemy.EnemySprite.Velocity / 10);

                    playerManager.Destroyed = true;

                    explosionManager.AddExplosion(
                        playerManager.playerSprite.Center,
                        Vector2.Zero);
                }
            }
        }

        private void checkLifePickup()
        {
            if (life.pwrLife.IsCircleColliding(
                playerManager.playerSprite.Center,
                playerManager.playerSprite.CollisionRadius))
            {   
                //life is picked up and is reset so it can respawn again
                playerManager.Lives ++;                
                life.spawnTime = 10f;
                life.Active = false;
                life.pwrLife.Location = new Vector2(life.rand.Next(0, life.screenWidth - 50),
                    life.rand.Next(0, life.screenHeight - 50));
            }
        }

        public void CheckCollisions()
        {
            checkShotToEnemyCollisions();
            if (!playerManager.Destroyed)
            {
                checkShotToPlayerCollisions();
                checkEnemyToPlayerCollisions();
            }

            if (life.Active == true)
            {
                checkLifePickup();
            }
        }    
        
    }
}
