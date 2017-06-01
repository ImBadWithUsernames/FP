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

namespace FP
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle Player;
        Vector2 location;
        Texture2D squareTexture;
        Sprite square;
        Texture2D GO;
        List<Sprite> otherSquares;
        enum GameStates { playing, gameover};
        GameStates gamestate = GameStates.playing;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here

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
          
            squareTexture = Content.Load<Texture2D>("square");
            location = new Vector2(this.Window.ClientBounds.Width / 2 - 16, this.Window.ClientBounds.Height - 64);
            square = new Sprite(location, squareTexture, new Rectangle(0, 0, 32, 32), Vector2.Zero);

            int step = 0;
            otherSquares = new List<Sprite>();
            for (int i = 0; i < 200; i++)
            {
                if (i < 5)
                    step += 64;
                else if (i >= 5 && i < 10)
                    step -= 64;
                else if (i >= 10 && i < 15)
                    step = 0;
                else if (i >= 15 && i <= 20)
                    step = (i % 2) * 64;
                else if (i >= 100 && i <= 200)
                    step = (i % 2) * 64;
                else step = 0;

                location = new Vector2(this.Window.ClientBounds.Width / 2 - 16 + 600 + i * 160, this.Window.ClientBounds.Height - 64 - step);
                otherSquares.Add(new Sprite(location, squareTexture, new Rectangle(0, 0, 32, 32), new Vector2(-290, 0)));
            }


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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            KeyboardState kb = Keyboard.GetState();

            square.Velocity += new Vector2(0, 64);
            
            if (square.Location.Y >= this.Window.ClientBounds.Height-64)
            {
                square.Velocity = new Vector2(0, 0);
                square.Rotation = 0;
                square.Location = new Vector2(square.Location.X, this.Window.ClientBounds.Height - 64);                
            }

            foreach (Sprite s in otherSquares)
            {
                s.Update(gameTime);

                if (square.IsBoxColliding(s.BoundingBoxRect))
                {
                    if (square.Center.Y < s.Center.Y)
                    {
                        square.Velocity = new Vector2(0, 0);
                        square.Rotation = 0;
                        square.Location = new Vector2(square.Location.X, s.Location.Y - 32);
                    }
                    else
                    {
                        // GAME OVER!!!
                        gamestate = GameStates.gameover;
                        GO = Content.Load<Texture2D>("gameover");
                        //WELL I CANT GET IT TO WORK SO WHATEVER CLOSING THE GAME WORKS TOO

                        }
                }
            }

            if (kb.IsKeyDown(Keys.Space) && square.Velocity.Y == 0)
            {
                square.Velocity = new Vector2(0, -900);
            }

            if (Math.Abs(square.Velocity.Y) > 0)
                square.Rotation += MathHelper.Pi / 32;



            // TODO: Add your update logic here
            square.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            square.Draw(spriteBatch);
            foreach (Sprite s in otherSquares)
            {
                s.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
