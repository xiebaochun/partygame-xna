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
using System.IO;

namespace TouchGame
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { start, play };
        GameStates gameState = GameStates.start;

        MouseState mouseState, pre_mouseState;

        GameComponent_1 gameComponent_1;
        GameComponent_2 gameComponent_2;
        GameComponent_3 gameComponent_3;

        Texture2D startTexture_BG;
        Texture2D[] startTexture = new Texture2D[4];

        Rectangle[] rect_Button = { new Rectangle(403, 236, 204, 196), 
                                    new Rectangle(664, 31, 306, 260),                                     
                                    new Rectangle(62, 26, 301, 256), 
                                    new Rectangle(108, 422, 315, 238) };
        bool[] button_EnlargeFlag = { false, false, false, false };

        int continueFlag = 0;

        int level = 1;
        Texture2D levelTexture_null;
        Texture2D levelTexture_full;
        Vector2 levelTexturePosition = new Vector2(640, 655);
        Vector2 levelTextureOffset = new Vector2(59, 0);
        //bool levelConfirmFlag = false;
        int levelChoice = 0;
        int dummyLevel = 1;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;

            if (Properties.Game1.Default.IsFullScreen == 1)
            {
                graphics.IsFullScreen = true;
            }
            //graphics.IsFullScreen = true;

        }


        protected override void Initialize()
        {
            gameComponent_1 = new GameComponent_1(this);
            Components.Add(gameComponent_1);
            gameComponent_2 = new GameComponent_2(this);
            Components.Add(gameComponent_2);
            gameComponent_3 = new GameComponent_3(this);
            Components.Add(gameComponent_3);

            gameComponent_1.Visible = false;
            gameComponent_1.Enabled = false;
            gameComponent_2.Visible = false;
            gameComponent_2.Enabled = false;
            gameComponent_3.Visible = false;
            gameComponent_3.Enabled = false;

            gameComponent_1.gameOverFlag = false;
            gameComponent_2.gameOverFlag = false;
            gameComponent_3.gameOverFlag = false;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startTexture_BG = Content.Load<Texture2D>("Texture\\start\\bg_cover");
            startTexture[0] = Content.Load<Texture2D>("Texture\\start\\game_start");
            startTexture[1] = Content.Load<Texture2D>("Texture\\start\\game_b");
            startTexture[2] = Content.Load<Texture2D>("Texture\\start\\game_a");
            startTexture[3] = Content.Load<Texture2D>("Texture\\start\\game_c");

            levelTexture_null = Content.Load<Texture2D>("Texture\\start\\start_levelnull");
            levelTexture_full = Content.Load<Texture2D>("Texture\\start\\start_levelfull");
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mouseState = Mouse.GetState();

            switch (gameState)
            {
                case GameStates.start:

                    Point mousePosition = new Point(mouseState.X, mouseState.Y);
                    for (int i = 0; i < button_EnlargeFlag.Length; i++)
                    {
                        button_EnlargeFlag[i] = false;
                    }
                    if (continueFlag == 0)
                    {
                        #region MyRegion
                        if (rect_Button[0].Contains(mousePosition))
                        {
                            button_EnlargeFlag[0] = true;
                            if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                            {
                                continueFlag = 1;
                            }
                        }
                        else if (rect_Button[1].Contains(mousePosition))
                        {
                            button_EnlargeFlag[1] = true;
                            if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                            {
                                gameComponent_1 = new GameComponent_1(this);
                                Components.Add(gameComponent_1);
                                gameComponent_1.Enabled = true;
                                gameComponent_1.Visible = true;

                                gameComponent_1.level = level;

                                gameState = GameStates.play;
                            }
                        }
                        else if (rect_Button[2].Contains(mousePosition))
                        {
                            button_EnlargeFlag[2] = true;
                            if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                            {
                                gameComponent_2 = new GameComponent_2(this);
                                Components.Add(gameComponent_2);
                                gameComponent_2.Enabled = true;
                                gameComponent_2.Visible = true;

                                gameComponent_2.level = level;

                                gameState = GameStates.play;
                            }
                        }
                        else if (rect_Button[3].Contains(mousePosition))
                        {
                            button_EnlargeFlag[3] = true;
                            if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                            {
                                gameComponent_3 = new GameComponent_3(this);
                                Components.Add(gameComponent_3);
                                gameComponent_3.Enabled = true;
                                gameComponent_3.Visible = true;

                                gameComponent_3.level = level;

                                gameState = GameStates.play;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region MyRegion
                        if (continueFlag == 3)
                        {
                            gameComponent_1 = new GameComponent_1(this);
                            Components.Add(gameComponent_1);
                            gameComponent_1.Enabled = true;
                            gameComponent_1.Visible = true;

                            gameComponent_1.level = level;
                        }
                        else if (continueFlag == 1)
                        {
                            gameComponent_2 = new GameComponent_2(this);
                            Components.Add(gameComponent_2);
                            gameComponent_2.Enabled = true;
                            gameComponent_2.Visible = true;

                            gameComponent_2.level = level;
                        }
                        else if (continueFlag == 2)
                        {
                            gameComponent_3 = new GameComponent_3(this);
                            Components.Add(gameComponent_3);
                            gameComponent_3.Enabled = true;
                            gameComponent_3.Visible = true;

                            gameComponent_3.level = level;
                        }

                        gameState = GameStates.play; 
                        #endregion
                    }

                    Rectangle level_Rect = new Rectangle((int)levelTexturePosition.X - 25, (int)levelTexturePosition.Y - 25,
                                                         (int)levelTextureOffset.X * 10 + 100, (int)levelTextureOffset.Y * 10 + 100);
                    if (level_Rect.Contains(mousePosition))
                    {
                        if (levelChoice == 0)
                        {
                            levelChoice = 1;
                        }
                        else if (levelChoice == 1)
                        {
                            level = 0;
                            dummyLevel = (int)((mousePosition.X - levelTexturePosition.X) / levelTextureOffset.X) + 1;
                        }

                        if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed)
                            && (levelChoice != 2))
                        {
                            levelChoice = 2;
                            level = (int)((mousePosition.X - levelTexturePosition.X) / levelTextureOffset.X) + 1;
                            dummyLevel = 0;
                        }
                    }
                    else
                    {
                        if (levelChoice == 1)   //进入过选择区，但是没有确认选择的情况；所以，强制设置为1星难度
                        {
                            level = 1;
                            dummyLevel = 0;
                        }
                        levelChoice = 0;
                    }

                    break;

                case GameStates.play:

                    if (gameComponent_1.gameOverFlag || gameComponent_2.gameOverFlag || gameComponent_3.gameOverFlag)
                    {
                        if (continueFlag != 0)
                        {
                            continueFlag++;
                            if (continueFlag > 3)
                            {
                                continueFlag = 0;
                                level = 1;
                            }
                        }
                        else
                        {
                            level = 1;
                        }
                        gameComponent_1.gameOverFlag = false;
                        gameComponent_2.gameOverFlag = false;
                        gameComponent_3.gameOverFlag = false;

                        gameState = GameStates.start;
                    }

                    break;
            }

            pre_mouseState = mouseState;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255, 255, 255));

            spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.start:

                    if (continueFlag == 0)      //防止连续游戏时，中间看到闪动的画面
                    {
                        spriteBatch.Draw(startTexture_BG, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                        for (int i = 0; i < button_EnlargeFlag.Length; i++)
                        {
                            if (button_EnlargeFlag[i])
                            {
                                int scale_lenght = 30;
                                spriteBatch.Draw(startTexture[i], new Rectangle(-scale_lenght, -scale_lenght, graphics.PreferredBackBufferWidth + scale_lenght * 2, graphics.PreferredBackBufferHeight + scale_lenght * 2), new Rectangle(0, 0, startTexture[i].Width, startTexture[i].Height),
                                    Color.White);
                            }
                            else
                            {
                                spriteBatch.Draw(startTexture[i], new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                            }
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            spriteBatch.Draw(levelTexture_null, new Rectangle((int)(levelTexturePosition.X + levelTextureOffset.X * i), (int)(levelTexturePosition.Y + levelTextureOffset.Y * i), 50, 50),
                                new Rectangle(0, 0, levelTexture_null.Width, levelTexture_null.Height), Color.White);
                           
                            if (i < level)
                            {
                                spriteBatch.Draw(levelTexture_full, new Rectangle((int)(levelTexturePosition.X + levelTextureOffset.X * i), (int)(levelTexturePosition.Y + levelTextureOffset.Y * i), 50, 50),
                                    new Rectangle(0, 0, levelTexture_full.Width, levelTexture_full.Height), Color.White);
                            }
                            else if (i < dummyLevel)
                            {
                                spriteBatch.Draw(levelTexture_full, new Rectangle((int)(levelTexturePosition.X + levelTextureOffset.X * i), (int)(levelTexturePosition.Y + levelTextureOffset.Y * i), 50, 50),
                                    new Rectangle(0, 0, levelTexture_full.Width, levelTexture_full.Height), new Color(255, 255, 255, 200));
                            }

                        }
                    }

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
