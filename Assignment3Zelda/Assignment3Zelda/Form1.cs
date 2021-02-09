// Shayan Firouzian
// May 3, 2016
// Zelda-Like Game
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment3Zelda
{
    public partial class Form1 : Form
    {
        // Player weapon attack booleans
        bool attackUp = false;
        bool attackDown = false;
        bool attackLeft = false;
        bool attackRight = false;

        // Animation state constants for graphic updates
        const int ANIMATION_STATE_FIRST = 0;
        const int ANIMATION_STATE_SECOND = 10;
        const int ANIMATION_STATE_THIRD = 20;
        const int ANIMATION_RESET = 1;

        // Variable to determine which way the player is facing when idle
        int idleState;

        // Idle state constants for each direction
        const int IDLE_UP = 0;
        const int IDLE_DOWN = 1;
        const int IDLE_LEFT = 2;
        const int IDLE_RIGHT = 3;

        // Variable to update the player graphics
        int playerGraphicCounter;

        // Variable for getting the current tick count
        int previousTime;

        // Variable to determine if the game is running or not
        int gameState;

        // Constants that determine the state for gameState
        const int GAME_STOPPED = 0;
        const int GAME_RUNNING = 1;
        const int GAME_OVER = 2;

        // Constant for boomerang speed
        const int BOOMERANG_SPEED = 15;

        // Variable to determine if the boomerang is going to the player or the enemy
        int boomerangTarget;

        // Constants to determine who the boomerang's target is
        const int TARGET_ENEMY = 0;
        const int TARGET_PLAYER = 1;

        // Constants for the frame rate for the game loop to run at
        const int FRAME_RATE = 60;
        const int FRAME_TIME = 1000 / FRAME_RATE;

        // Player movement booleans
        bool moveUp = false;
        bool moveDown = false;
        bool moveLeft = false;
        bool moveRight = false;

        // Constants for the max and minimum health
        const int PLAYER_MAX_HEALTH = 10;
        const int ENEMY_MAX_HEALTH = 20;
        const int MIN_HEALTH = 0;

        // Constant for the max weapon timer
        const int MAX_WEAPON_TIMER = 20;

        // Player and enemy health variables
        int playerHealth = PLAYER_MAX_HEALTH;
        int enemyHealth = ENEMY_MAX_HEALTH;

        // Counter variable for weapon timer
        int weaponCounter;

        // Boolean to limit damage dealt from weapon
        bool damageCap = false;

        // Player graphic data
        PointF playerLocation;
        SizeF playerSize;
        RectangleF playerHitbox;

        // Player weapon graphic data
        PointF playerWeaponLocation;
        SizeF playerWeaponSize;
        RectangleF playerWeaponHitbox;

        // Enemy graphic data
        PointF enemyLocation;
        SizeF enemySize;
        RectangleF enemyHitbox;

        // Background graphic data
        PointF backgroundLocation;
        SizeF backgroundSize;
        RectangleF backgroundHitbox;

        // Fireball graphic data
        PointF fireballLocation;
        SizeF fireballSize;
        RectangleF fireballHitbox;

        // Fireball speed variables
        float fireballXSpeed, fireballYSpeed;

        // Boolean to determine if the fireball is destroyed
        bool isFireballDestroyed = true;

        // Boomerang graphic data
        PointF boomerangLocation;
        SizeF boomerangSize;
        RectangleF boomerangHitbox;

        // Boomerang speed variables
        float boomerangXSpeed, boomerangYSpeed;

        // Boolean to determine if the boomerang is destroyed
        bool isBoomerangDestroyed = true;

        // Booleans to check if the player is dead
        bool isPlayerDead = false;
        bool isEnemyDead = false;

        // Player health graphic data
        PointF playerHealthTextLocation;
        SizeF playerHealthTextSize;
        RectangleF playerHealthTextHitbox;
        Font playerHealthTextFont;

        // Enemy health graphic data
        PointF enemyHealthTextLocation;
        SizeF enemyHealthTextSize;
        RectangleF enemyHealthTextHitbox;
        Font enemyHealthTextFont;

        // Obstacle graphic data
        PointF boulder1Location;
        SizeF boulder1Size;
        RectangleF boulder1Hitbox;

        // Obstacle graphic data
        PointF boulder2Location;
        SizeF boulder2Size;
        RectangleF boulder2Hitbox;

        // Obstacle graphic data
        PointF boulder3Location;
        SizeF boulder3Size;
        RectangleF boulder3Hitbox;

        // Win screen graphic data
        PointF winLocation;
        SizeF winSize;
        RectangleF winHitbox;

        // Lose screen graphic data
        PointF loseLocation;
        SizeF loseSize;
        RectangleF loseHitbox;

        // Intro screen graphic data
        PointF introLocation;
        SizeF introSize;
        RectangleF introHitbox;

        public Form1()
        {
            InitializeComponent();

            SetupIntro();
        }

        // Subprogram for setting up player graphic
        void SetupPlayer()
        {
            playerSize = new SizeF(52, 96);
            playerLocation = new PointF(ClientSize.Width / 2 - playerSize.Width / 2, ClientSize.Height - playerSize.Height);
            playerHitbox = new RectangleF(playerLocation, playerSize);
        }

        // Subprogram for setting up player weapon up graphic
        void SetupPlayerWeaponUp()
        {
            playerWeaponSize = new SizeF(35, 187);
            playerWeaponLocation = new PointF(playerLocation.X + playerSize.Width / 2 - playerWeaponSize.Width / 2, playerLocation.Y - playerWeaponSize.Height);
            playerWeaponHitbox = new RectangleF(playerWeaponLocation, playerWeaponSize);
        }

        // Subprogram for setting up player weapon down graphic
        void SetupPlayerWeaponDown()
        {
            playerWeaponSize = new SizeF(35, 187);
            playerWeaponLocation = new PointF(playerLocation.X + playerSize.Width / 2 - playerWeaponSize.Width / 2, playerLocation.Y + playerSize.Height);
            playerWeaponHitbox = new RectangleF(playerWeaponLocation, playerWeaponSize);
        }

        // Subprogram for setting up player weapon left graphic
        void SetupPlayerWeaponLeft()
        {
            playerWeaponSize = new SizeF(187, 35);
            playerWeaponLocation = new PointF(playerLocation.X - playerWeaponSize.Width, playerLocation.Y + playerSize.Height / 2 - playerWeaponSize.Height / 2);
            playerWeaponHitbox = new RectangleF(playerWeaponLocation, playerWeaponSize);
        }

        // Subprogram for setting up player weapon right graphic
        void SetupPlayerWeaponRight()
        {
            playerWeaponSize = new SizeF(187, 35);
            playerWeaponLocation = new PointF(playerLocation.X + playerSize.Width, playerLocation.Y + playerSize.Height / 2 - playerWeaponSize.Height / 2);
            playerWeaponHitbox = new RectangleF(playerWeaponLocation, playerWeaponSize);
        }

        // Subprogram for setting up enemy graphic
        void SetupEnemy()
        {
            enemySize = new SizeF(118, 124);
            enemyLocation = new PointF(ClientSize.Width / 2 - enemySize.Width / 2, 0);
            enemyHitbox = new RectangleF(enemyLocation, enemySize);
        }

        // Subprogram for setting up background graphic
        void SetupBackground()
        {
            backgroundSize = new SizeF(ClientSize.Width, ClientSize.Height);
            backgroundLocation = new PointF(0, 0);
            backgroundHitbox = new RectangleF(backgroundLocation, backgroundSize);
        }

        // Subprogram for setting up intro graphic
        void SetupIntro()
        {
            introSize = new SizeF(ClientSize.Width, ClientSize.Height);
            introLocation = new PointF(0, 0);
            introHitbox = new RectangleF(introLocation, introSize);
        }

        // Subprogram for setting up fireball graphic
        void SetupFireball()
        {
            fireballSize = new SizeF(23, 26);
            fireballLocation = new PointF(enemyLocation.X + enemySize.Width / 2 - fireballSize.Width / 2, enemyLocation.Y + enemySize.Height / 2 - fireballSize.Height / 2);
            fireballHitbox = new RectangleF(fireballLocation, fireballSize);
        }

        // Subprogram for setting up boomerang graphic
        void SetupBoomerang()
        {
            boomerangSize = new SizeF(33, 38);
            boomerangLocation = new PointF(playerLocation.X + playerSize.Width / 2 - boomerangSize.Width / 2, playerLocation.Y + playerSize.Height / 2 - boomerangSize.Height / 2);
            boomerangHitbox = new RectangleF(boomerangLocation, boomerangSize);
        }

        // Subprogram for setting up obstacle graphics
        void SetupBoulders()
        {
            boulder1Size = new SizeF(62, 62);
            boulder1Location = new PointF(ClientSize.Width / 4 - boulder1Size.Width / 2, ClientSize.Height / 2 - boulder1Size.Height / 2);
            boulder1Hitbox = new RectangleF(boulder1Location, boulder1Size);

            boulder2Size = boulder1Size;
            boulder2Location = new PointF(ClientSize.Width / 2 - boulder2Size.Width / 2, ClientSize.Height / 2 - boulder2Size.Height / 2);
            boulder2Hitbox = new RectangleF(boulder2Location, boulder2Size);

            boulder3Size = boulder1Size;
            boulder3Location = new PointF(ClientSize.Width / 4 * 3 - boulder2Size.Width / 2, ClientSize.Height / 2 - boulder3Size.Height / 2);
            boulder3Hitbox = new RectangleF(boulder3Location, boulder3Size);
        }

        // Subprogram for creating fireballs
        void CreateFireball()
        {
            // Only create a fireball if the previous one is destroyed
            if (isFireballDestroyed == true)
            {
                // Fireball speed
                const int FIREBALL_SPEED = 15;

                // Reset the fireball graphic data
                SetupFireball();

                // calculate rise and run to get slope
                float rise = (playerLocation.Y + playerSize.Height / 2) - (enemyLocation.Y + enemySize.Height / 2);
                float run = (playerLocation.X + playerSize.Width / 2) - (enemyLocation.X + enemySize.Width / 2);

                // calculate hypotenuse
                float hypotenuse = (float)Math.Sqrt(rise * rise + run * run);

                // fireball speeds
                fireballXSpeed = run / hypotenuse * FIREBALL_SPEED;
                fireballYSpeed = rise / hypotenuse * FIREBALL_SPEED;

                // Make the fireball be labeled as not destroyed
                isFireballDestroyed = false;
            }
        }

        // Subprogram for moving the fireball
        void MoveFireball()
        {
            // Update the fireballs location
            fireballLocation.X = fireballLocation.X + fireballXSpeed;
            fireballLocation.Y = fireballLocation.Y + fireballYSpeed;

            // Update the fireballs hitbox
            fireballHitbox.Location = fireballLocation;
        }

        // Subprogram for creating boomerang
        void CreateBoomerang()
        {
            // Only create a boomerang if the previous one is destroyed
            if (isBoomerangDestroyed == true)
            {
                // Reset the boomerangs position
                SetupBoomerang();

                // calculate rise and run to get slope
                float rise = (enemyLocation.Y + enemySize.Height / 2) - (playerLocation.Y + playerSize.Height / 2);
                float run = (enemyLocation.X + enemySize.Width / 2) - (playerLocation.X + playerSize.Width / 2);

                // calculate hypotenuse
                float hypotenuse = (float)Math.Sqrt(rise * rise + run * run);

                // Boomerang speeds
                boomerangXSpeed = run / hypotenuse * BOOMERANG_SPEED;
                boomerangYSpeed = rise / hypotenuse * BOOMERANG_SPEED;

                // Label the boomerang as no longer destroyed
                isBoomerangDestroyed = false;
            }
        }

        // Subprogram for moving the boomerang
        void MoveBoomerang()
        {
            if (isBoomerangDestroyed == false)
            {
                // Update location
                boomerangLocation.X = boomerangLocation.X + boomerangXSpeed;
                boomerangLocation.Y = boomerangLocation.Y + boomerangYSpeed;

                // Update hitbox
                boomerangHitbox.Location = boomerangLocation;
            }
        }

        // Subprogram for checking boomerang's collisions
        void CheckBoomerangCollisions()
        {
            // If it intersects with an enemy make it take damage and make the boomerang come back
            if (boomerangHitbox.IntersectsWith(enemyHitbox) && boomerangTarget == TARGET_ENEMY)
            {
                enemyHealth--;
                CheckGameOver();
                boomerangTarget = TARGET_PLAYER;
            }

            // If it intersects with a boulder make it come back
            if (boomerangHitbox.IntersectsWith(boulder1Hitbox) || boomerangHitbox.IntersectsWith(boulder2Hitbox) || boomerangHitbox.IntersectsWith(boulder3Hitbox))
            {
                boomerangTarget = TARGET_PLAYER;
            }

            // If the boomerang is coming back constantly update its target
            if (boomerangTarget == TARGET_PLAYER)
            {
                // calculate rise and run to get slope
                float rise = (playerLocation.Y + playerSize.Height / 2) - (boomerangLocation.Y + boomerangSize.Height / 2);
                float run = (playerLocation.X + playerSize.Width / 2) - (boomerangLocation.X + boomerangSize.Width / 2);

                // calculate hypotenuse
                float hypotenuse = (float)Math.Sqrt(rise * rise + run * run);

                // boomerang speeds
                boomerangXSpeed = run / hypotenuse * BOOMERANG_SPEED;
                boomerangYSpeed = rise / hypotenuse * BOOMERANG_SPEED;
            }

            // If it intersects with the player move it off the screen to prevent further damage being dealt
            if (boomerangTarget == TARGET_PLAYER && boomerangHitbox.IntersectsWith(playerHitbox))
            {
                boomerangLocation.X = 0 - boomerangSize.Width;
                boomerangLocation.Y = 0 - boomerangSize.Height;
                boomerangHitbox.Location = boomerangLocation;
                boomerangTarget = TARGET_ENEMY;
                isBoomerangDestroyed = true;
            }
        }

        // Subprogram for setting up player health text
        void SetupPlayerHealth()
        {
            playerHealthTextSize = new SizeF(250, 30);
            playerHealthTextLocation = new PointF(0, ClientSize.Height - playerHealthTextSize.Height);
            playerHealthTextHitbox = new RectangleF(playerHealthTextLocation, playerHealthTextSize);
            playerHealthTextFont = new Font("Arial", 20);
        }

        // Subprogram for setting up enemy health text
        void SetupEnemyHealth()
        {
            enemyHealthTextSize = new SizeF(250, 30);
            enemyHealthTextLocation = new PointF(0, 0);
            enemyHealthTextHitbox = new RectangleF(enemyHealthTextLocation, enemyHealthTextSize);
            enemyHealthTextFont = new Font("Arial", 20);
        }

        // Subprogram for setting up win screen graphic
        void SetupWin()
        {
            winSize = new SizeF(ClientSize.Width, ClientSize.Height);
            winLocation = new PointF(0, 0);
            winHitbox = new RectangleF(winLocation, winSize);
        }

        // Subprogram for setting up lose screen graphic
        void SetupLose()
        {
            loseSize = new SizeF(ClientSize.Width, ClientSize.Height);
            loseLocation = new PointF(0, 0);
            loseHitbox = new RectangleF(loseLocation, loseSize);
        }

        // Subprogram for the frame rate/game loop
        void FrameRateLoop()
        {
            gameState = GAME_RUNNING;
            previousTime = Environment.TickCount;

            // While the game is running, keep running the game loop
            while (gameState == GAME_RUNNING)
            {
                // Update the time passed
                int timePassed = Environment.TickCount - previousTime;

                if (timePassed >= FRAME_TIME)
                {
                    // Keep updating the previous time
                    previousTime = Environment.TickCount;

                    // Game actions to be done in the loop
                    MovePlayer();
                    WeaponTimer();
                    MoveBoomerang();
                    CheckBoomerangCollisions();
                    CreateFireball();
                    MoveFireball();
                    CheckCollisions();
                    Refresh();
                }

                Application.DoEvents();
            }
        }

        // Subprogram for the weapon duration
        void WeaponTimer()
        {
            // If the player is attacking make the counter go up
            if (attackUp == true || attackDown == true || attackLeft == true || attackRight == true)
            {
                weaponCounter++;
            }

            // If the counter passed the maximum threshhold move the weapon off the screen to prevent further damage being dealt
            if (weaponCounter > MAX_WEAPON_TIMER)
            {
                playerWeaponLocation.X = 0 - playerWeaponSize.Width;
                playerWeaponLocation.Y = 0 - playerWeaponSize.Height;
                playerWeaponHitbox.Location = playerWeaponLocation;
            }
        }

        // Subprogram for chaning weapon direction
        void WeaponDirection()
        {
            // Change weapon direction to up
            if (attackUp == true)
            {
                SetupPlayerWeaponUp();
                idleState = IDLE_UP;
            }

            // Change weapon direction to down
            else if (attackDown == true)
            {
                SetupPlayerWeaponDown();
                idleState = IDLE_DOWN;
            }

            // Change weapon direction to left
            else if (attackLeft == true)
            {
                SetupPlayerWeaponLeft();
                idleState = IDLE_LEFT;
            }

            // Change weapon direction to right
            else if (attackRight == true)
            {
                SetupPlayerWeaponRight();
                idleState = IDLE_RIGHT;
            }
        }

        // Subprogram for player movement
        void MovePlayer()
        {
            // Player speed
            const int PLAYER_SPEED = 5;

            // Move the player up
            if (moveUp == true)
            {
                // Update player location, hitbox and graphic
                playerLocation.Y = playerLocation.Y - PLAYER_SPEED;
                playerHitbox.Location = playerLocation;
                PlayerGraphicCounter();

                // Don't let the player go past the client's boundaries
                if (playerLocation.Y < 0)
                {
                    playerLocation.Y = 0;
                    playerHitbox.Location = playerLocation;
                }

                // Don't let the player walk through obstacles
                else if (playerHitbox.IntersectsWith(boulder1Hitbox) || playerHitbox.IntersectsWith(boulder2Hitbox) || playerHitbox.IntersectsWith(boulder3Hitbox))
                {
                    playerLocation.Y = playerLocation.Y + PLAYER_SPEED;
                    playerHitbox.Location = playerLocation;
                }
            }

            // Move the player down
            if (moveDown == true)
            {
                // Update player location, hitbox and graphic
                playerLocation.Y = playerLocation.Y + PLAYER_SPEED;
                playerHitbox.Location = playerLocation;
                PlayerGraphicCounter();

                // Don't let the player go past the client's boundaries
                if (playerLocation.Y > ClientSize.Height - playerHitbox.Height)
                {
                    playerLocation.Y = ClientSize.Height - playerHitbox.Height;
                    playerHitbox.Location = playerLocation;
                }

                // Don't let the player walk through obstacles
                else if (playerHitbox.IntersectsWith(boulder1Hitbox) || playerHitbox.IntersectsWith(boulder2Hitbox) || playerHitbox.IntersectsWith(boulder3Hitbox))
                {
                    playerLocation.Y = playerLocation.Y - PLAYER_SPEED;
                    playerHitbox.Location = playerLocation;
                }
            }

            // Move the player left
            if (moveLeft == true)
            {
                // Update player location, hitbox and graphic
                playerLocation.X = playerLocation.X - PLAYER_SPEED;
                playerHitbox.Location = playerLocation;
                PlayerGraphicCounter();

                // Don't let the player go past the client's boundaries
                if (playerLocation.X < 0)
                {
                    playerLocation.X = 0;
                    playerHitbox.Location = playerLocation;
                }

                // Don't let the player walk through obstacles
                else if (playerHitbox.IntersectsWith(boulder1Hitbox) || playerHitbox.IntersectsWith(boulder2Hitbox) || playerHitbox.IntersectsWith(boulder3Hitbox))
                {
                    playerLocation.X = playerLocation.X + PLAYER_SPEED;
                    playerHitbox.Location = playerLocation;
                }
            }

            // Move the player right
            if (moveRight == true)
            {
                // Update player location, hitbox and graphic
                playerLocation.X = playerLocation.X + PLAYER_SPEED;
                playerHitbox.Location = playerLocation;
                PlayerGraphicCounter();

                // Don't let the player go past the client's boundaries
                if (playerLocation.X > ClientSize.Width - playerHitbox.Width)
                {
                    playerLocation.X = ClientSize.Width - playerHitbox.Width;
                    playerHitbox.Location = playerLocation;
                }

                // Don't let the player walk through obstacles
                else if (playerHitbox.IntersectsWith(boulder1Hitbox) || playerHitbox.IntersectsWith(boulder2Hitbox) || playerHitbox.IntersectsWith(boulder3Hitbox))
                {
                    playerLocation.X = playerLocation.X - PLAYER_SPEED;
                    playerHitbox.Location = playerLocation;
                }
            }

            // Update the player's weapon direction
            WeaponDirection();
        }

        // Subprogram for updating the player graphic
        void PlayerGraphicCounter()
        {
            // Make the graphic counter count up
            playerGraphicCounter++;

            // Reset if it reaches the last animation state
            if (playerGraphicCounter > ANIMATION_STATE_THIRD)
            {
                playerGraphicCounter = ANIMATION_RESET;
            }
        }

        // Subprogram for checking collisions
        void CheckCollisions()
        {
            // If the player touches the enemy make player lose health
            if (playerHitbox.IntersectsWith(enemyHitbox))
            {
                playerHealth = playerHealth - 1;
                CheckGameOver();
            }

            // If fireball hits the player make player lose health and break the fireball
            if (fireballHitbox.IntersectsWith(playerHitbox))
            {
                isFireballDestroyed = true;
                playerHealth = playerHealth - 1;
                CheckGameOver();
            }

            // If the fireball goes off the screen break it
            else if (fireballLocation.X + fireballSize.Width < 0 || fireballLocation.X > ClientSize.Width || fireballLocation.Y + fireballSize.Height < 0 || fireballLocation.Y > ClientSize.Height || fireballHitbox.IntersectsWith(boulder1Hitbox) || fireballHitbox.IntersectsWith(boulder2Hitbox) || fireballHitbox.IntersectsWith(boulder3Hitbox))
            {
                isFireballDestroyed = true;
            }

            // If the fireball and boomerang intersect, break them both
            else if (fireballHitbox.IntersectsWith(boomerangHitbox))
            {
                isFireballDestroyed = true;
                boomerangTarget = TARGET_ENEMY;
                isBoomerangDestroyed = true;
                SetupBoomerang();
            }

            // If the sword hits the enemy make it deal damage and cap that damage
            if (damageCap == false && playerWeaponHitbox.IntersectsWith(enemyHitbox))
            {
                enemyHealth--;
                CheckGameOver();
                damageCap = true;
            }
        }

        // Subprogram for checking if the game is over
        void CheckGameOver()
        {
            // If the enemy goes below 0 health make the player win
            if (enemyHealth <= 0)
            {
                isEnemyDead = true;
                gameState = GAME_OVER;
                SetupWin();
            }

            // If the player goes below 0 health make the player lose
            else if (playerHealth <= 0)
            {
                isPlayerDead = true;
                gameState = GAME_OVER;
                SetupLose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // If the game has not started yet show the intro screen
            if (gameState == GAME_STOPPED)
            {
                e.Graphics.DrawImage(Properties.Resources.Intro, introHitbox);
            }

            // Only draw these images if the game is running
            if (gameState == GAME_RUNNING)
            {
                // Draw all the base visuals if the game is running
                e.Graphics.DrawImage(Properties.Resources.Background, backgroundHitbox);
                e.Graphics.DrawString("Player HP: " + playerHealth.ToString() + "/" + PLAYER_MAX_HEALTH.ToString(), playerHealthTextFont, Brushes.LightBlue, playerHealthTextHitbox);
                e.Graphics.DrawString("Enemy HP: " + enemyHealth.ToString() + "/" + ENEMY_MAX_HEALTH.ToString(), enemyHealthTextFont, Brushes.Red, enemyHealthTextHitbox);
                e.Graphics.DrawImage(Properties.Resources.Obstacle, boulder1Hitbox);
                e.Graphics.DrawImage(Properties.Resources.Obstacle, boulder2Hitbox);
                e.Graphics.DrawImage(Properties.Resources.Obstacle, boulder3Hitbox);

                // If the player is on the left side of the screen make the enemy face left
                if (playerLocation.X < boulder1Location.X)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyLeft, enemyHitbox);
                }

                // If the player is on the right side of the screen make the enemy face right
                else if (playerLocation.X > boulder3Location.X)
                {
                    e.Graphics.DrawImage(Properties.Resources.EnemyRight, enemyHitbox);
                }

                // Otherwise just make the enemy face straight
                else
                {
                    e.Graphics.DrawImage(Properties.Resources.Enemy, enemyHitbox);
                }

                // Draw the fireball if it isn't destroyed
                if (isFireballDestroyed == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.Fireball, fireballHitbox);
                }

                // Draw the boomerang if it isn't destroyed
                if (isBoomerangDestroyed == false)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerBoomerang, boomerangHitbox);
                }

                // Draw player sword if the player is attacking
                if (attackUp == true && weaponCounter <= MAX_WEAPON_TIMER)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerWeaponUp, playerWeaponHitbox);
                }

                // Draw player sword if the player is attacking
                else if (attackDown == true && weaponCounter <= MAX_WEAPON_TIMER)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerWeaponDown, playerWeaponHitbox);
                }

                // Draw player sword if the player is attacking
                else if (attackLeft == true && weaponCounter <= MAX_WEAPON_TIMER)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerWeaponLeft, playerWeaponHitbox);
                }

                // Draw player sword if the player is attacking
                else if (attackRight == true && weaponCounter <= MAX_WEAPON_TIMER)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerWeaponRight, playerWeaponHitbox);
                }

                // Player up idle state
                if (playerGraphicCounter == ANIMATION_STATE_FIRST && idleState == IDLE_UP)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerUp0, playerHitbox);
                }

                // Player down idle state
                else if (playerGraphicCounter == ANIMATION_STATE_FIRST && idleState == IDLE_DOWN)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerDown0, playerHitbox);
                }

                // Player left idle state
                else if (playerGraphicCounter == ANIMATION_STATE_FIRST && idleState == IDLE_LEFT)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeft0, playerHitbox);
                }
                
                // Player right idle state
                else if (playerGraphicCounter == ANIMATION_STATE_FIRST && idleState == IDLE_RIGHT)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRight0, playerHitbox);
                }

                // Animation graphic for moving while attacking up
                else if (attackUp == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerUp1, playerHitbox);
                }

                // Animation graphic for moving while attacking up
                else if (attackUp == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerUp2, playerHitbox);
                }

                // Animation graphic for moving while attacking down
                else if (attackDown == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerDown1, playerHitbox);
                }

                // Animation graphic for moving while attacking down
                else if (attackDown == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerDown2, playerHitbox);
                }

                // Animation graphic for moving while attacking left
                else if (attackLeft == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeft1, playerHitbox);
                }

                // Animation graphic for moving while attacking left
                else if (attackLeft == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeft2, playerHitbox);
                }

                // Animation graphic for moving while attacking right
                else if (attackRight == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRight1, playerHitbox);
                }

                // Animation graphic for moving while attacking right
                else if (attackRight == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRight2, playerHitbox);
                }
                // Animation graphic for moving up
                else if (moveUp == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerUp1, playerHitbox);
                }

                // Animation graphic for moving up
                else if (moveUp == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerUp2, playerHitbox);
                }

                // Animation graphic for moving down
                else if (moveDown == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerDown1, playerHitbox);
                }

                // Animation graphic for moving down
                else if (moveDown == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerDown2, playerHitbox);
                }

                // Animation graphic for moving left
                else if (moveLeft == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeft1, playerHitbox);
                }

                // Animation graphic for moving left
                else if (moveLeft == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerLeft2, playerHitbox);
                }

                // Animation graphic for moving right
                else if (moveRight == true && playerGraphicCounter > ANIMATION_STATE_FIRST && playerGraphicCounter <= ANIMATION_STATE_SECOND)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRight1, playerHitbox);
                }

                // Animation graphic for moving right
                else if (moveRight == true && playerGraphicCounter > ANIMATION_STATE_SECOND && playerGraphicCounter <= ANIMATION_STATE_THIRD)
                {
                    e.Graphics.DrawImage(Properties.Resources.PlayerRight2, playerHitbox);
                }
            }

            // Draw these only if the game is over
            if (gameState == GAME_OVER)
            {
                // If the enemy is dead draw the win screen
                if (isEnemyDead == true)
                {
                    e.Graphics.DrawImage(Properties.Resources.Win, winHitbox);
                }

                // If the player is dead draw the lose screen
                else if (isPlayerDead == true)
                {
                    e.Graphics.DrawImage(Properties.Resources.Lose, loseHitbox);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // If the game is not running, start the game
            if (gameState == GAME_STOPPED && e.KeyCode == Keys.Space)
            {
                SetupPlayer();
                SetupEnemy();
                SetupBackground();
                SetupPlayerHealth();
                SetupEnemyHealth();
                SetupBoulders();
                FrameRateLoop();
            }

            // If the game is running enable these keys
            if (gameState == GAME_RUNNING)
            {
                // Move up
                if (e.KeyCode == Keys.Up)
                {
                    moveUp = true;
                }

                // Move down
                if (e.KeyCode == Keys.Down)
                {
                    moveDown = true;
                }

                // Move left
                if (e.KeyCode == Keys.Left)
                {
                    moveLeft = true;
                }

                // Move right
                if (e.KeyCode == Keys.Right)
                {
                    moveRight = true;
                }

                // Attack up
                if (e.KeyCode == Keys.W)
                {
                    attackUp = true;
                    damageCap = false;
                }

                // Attack down
                else if (e.KeyCode == Keys.S)
                {
                    attackDown = true;
                    damageCap = false;
                }

                // Attack left
                else if (e.KeyCode == Keys.A)
                {
                    attackLeft = true;
                    damageCap = false;
                }

                // Attack right
                else if (e.KeyCode == Keys.D)
                {
                    attackRight = true;
                    damageCap = false;
                }

                // Boomerang
                if (e.KeyCode == Keys.Q)
                {
                    CreateBoomerang();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // If the game is running allow these key ups
            if (gameState == GAME_RUNNING)
            {
                // Stop moving up
                if (e.KeyCode == Keys.Up)
                {
                    moveUp = false;
                    playerGraphicCounter = 0;
                    idleState = IDLE_UP;
                }

                // Stop moving down
                if (e.KeyCode == Keys.Down)
                {
                    moveDown = false;
                    playerGraphicCounter = 0;
                    idleState = IDLE_DOWN;
                }

                // Stop moving left
                if (e.KeyCode == Keys.Left)
                {
                    moveLeft = false;
                    playerGraphicCounter = 0;
                    idleState = IDLE_LEFT;
                }

                // Stop moving right
                if (e.KeyCode == Keys.Right)
                {
                    moveRight = false;
                    playerGraphicCounter = 0;
                    idleState = IDLE_RIGHT;
                }

                // Stop attacking up
                if (e.KeyCode == Keys.W)
                {
                    attackUp = false;
                    weaponCounter = 0;
                }

                // Stop attacking down
                else if (e.KeyCode == Keys.S)
                {
                    attackDown = false;
                    weaponCounter = 0;
                }

                // Stop attacking left
                else if (e.KeyCode == Keys.A)
                {
                    attackLeft = false;
                    weaponCounter = 0;
                }

                // Stop attacking right
                else if (e.KeyCode == Keys.D)
                {
                    attackRight = false;
                    weaponCounter = 0;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close the application if "X" is hit
            gameState = GAME_STOPPED;
            Application.Exit();
        }
    }
}
