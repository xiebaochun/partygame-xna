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


namespace TouchGame
{
    public class Cache
    {
        static ContentManager content;
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        static Dictionary<string, Texture2D> fontTextures = new Dictionary<string, Texture2D>();
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();


        public static void Initialize(ContentManager c)
        {
            content = c;
        }

        public static Texture2D Texture(string file)
        {
            if (!textures.ContainsKey(file))
            {
                try
                {
                    textures.Add(file, content.Load<Texture2D>(@"Texture/" + file));
                }
                catch
                {
                    throw new Exception("加载图像文件失败：" + file);
                }
            }
            return textures[file];
        }

        public static Texture2D FontTexture(string fontIndex, int imageIndex)
        {
            string file = fontIndex + "/" + imageIndex;
            if (!fontTextures.ContainsKey(file))
            {
                try
                {
                    fontTextures.Add(file, content.Load<Texture2D>(@"GameContent/Fonts/" + file));
                }
                catch
                {
                    throw new Exception("加载字体文件失败：" + file);
                }

            }
            return fontTextures[file];
        }

        public static SoundEffect BGM(string file)
        {
            if (!sounds.ContainsKey("BGM/" + file))
            {
                try
                {
                    SoundEffect bgm = content.Load<SoundEffect>(@"GameContent/Sounds/BGM/" + file);
                    sounds.Add("BGM/" + file, bgm);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return sounds["BGM/" + file];
        }

        public static SoundEffect SE(string file)
        {
            if (!sounds.ContainsKey("SE/" + file))
            {
                try
                {
                    sounds.Add("SE/" + file, content.Load<SoundEffect>(@"GameContent/Sounds/SE/" + file));
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return sounds["SE/" + file];
        }
    }

    public class sprite
    {
        public float Z;
        public sprite effectSprite;
        public Texture2D SourceTexture;
        public Vector2 position;
        public Boolean isVisible;
        public Boolean isAlive = true;
        public Rectangle ShowingRect;
        public int CurrentPositionX = 0;
        public int CurrentPositionY = 0;
        public int spriteSizeX;
        public int spriteSizeY;
        public Boolean isPool;
        public Boolean isHit = true;
        public int iCount = 0;
        public Rectangle CollisionRect;
        Texture2D stageNumberTexture;
        public Vector2 scale = new Vector2(1, 1);

        public sprite(string file, float z, Boolean isVisible, Vector2 position, Boolean isPool, int sprite_X, int sprite_Y)
        {
            this.SourceTexture = Cache.Texture(file);
            this.isVisible = isVisible;
            this.isPool = isPool;
            this.position = position;
            this.spriteSizeX = sprite_X;
            this.spriteSizeY = sprite_Y;
            this.Z = z;
            this.ShowingRect = this.SourceRect(this);
            this.CollisionRect = this.collisionRect(this);
        }

        private Rectangle collisionRect(sprite sprite)
        {
            Rectangle rect;
            rect = new Rectangle((int)sprite.position.X, (int)sprite.position.Y, (int)sprite.SourceTexture.Width / sprite.spriteSizeX,
                                         sprite.SourceTexture.Height / sprite.spriteSizeY);
            return rect;
        }

        public Rectangle SourceRect(sprite sprite)
        {
            Rectangle SourceRect;
            SourceRect = new Rectangle(sprite.CurrentPositionX * sprite.SourceTexture.Width / sprite.spriteSizeX,
                                     sprite.CurrentPositionY * sprite.SourceTexture.Height / sprite.spriteSizeY,
                                     sprite.SourceTexture.Width / sprite.spriteSizeX,
                                     sprite.SourceTexture.Height / sprite.spriteSizeY);
            return SourceRect;

        }

        public void ObjectAnimation(sprite sprite, int time)
        {
            if (sprite.isVisible == true)
            {
                sprite.iCount++;
                if (sprite.iCount >= time)
                {
                    sprite.iCount = 0;
                    sprite.CurrentPositionX++;

                    if (sprite.CurrentPositionX >= sprite.spriteSizeX)
                    {
                        sprite.CurrentPositionX = 0;
                        sprite.CurrentPositionY++;
                    }
                    if (sprite.CurrentPositionY >= sprite.spriteSizeY)
                    {
                        sprite.CurrentPositionY = 0;
                        if (sprite.isPool == false)
                        {
                            sprite.isVisible = false;
                        }
                    }
                    sprite.ShowingRect = SourceRect(sprite);
                }
            }
        }

        public void disappear(sprite sprite, int time)
        {
            if (sprite.isVisible == true && sprite.isAlive == false)
            {
                //if (sprite.isHit == false)
                //{
                //    sprite.isVisible = false;
                //    sprite.isAlive = true;                   
                //    sprite.effectSprite.ShowingRect = sprite.effectSprite.SourceRect(sprite.effectSprite);
                //    sprite.effectSprite.isVisible = true;
                //    sprite.CurrentPositionY = sprite.spriteSizeY - 1;
                //    sprite.CurrentPositionX = sprite.spriteSizeX - 1;
                //}
                sprite.iCount++;
                if (sprite.iCount >= time)
                {
                    sprite.iCount = 0;


                    sprite.CurrentPositionX++;

                    if (sprite.CurrentPositionX >= sprite.spriteSizeX)
                    {
                        sprite.CurrentPositionX = 0;
                        sprite.CurrentPositionY++;
                    }
                    if (sprite.CurrentPositionY >= sprite.spriteSizeY)
                    {
                        sprite.CurrentPositionY = sprite.spriteSizeY - 1;
                        sprite.CurrentPositionX = sprite.spriteSizeX - 1;
                        if (sprite.isPool == false)
                        {
                            sprite.isVisible = false;
                            sprite.isAlive = true;
                        }
                    }
                    sprite.ShowingRect = SourceRect(sprite);
                }
            }
        }

        public void ShowUp(sprite sprite, int time)
        {
            if (sprite.isVisible == true && sprite.isAlive == true)
            {
                sprite.iCount++;
                if (sprite.iCount >= time)
                {
                    sprite.iCount = 0;

                    sprite.CurrentPositionX--;
                    if (sprite.CurrentPositionX < 0)
                    {
                        sprite.CurrentPositionX = sprite.spriteSizeX;
                        sprite.CurrentPositionY--;
                    }
                    if (sprite.CurrentPositionY < 0)
                    {
                        sprite.CurrentPositionY = 0;
                        sprite.CurrentPositionX = 0;

                    }
                    sprite.ShowingRect = SourceRect(sprite);
                }
            }
        }
    }

    public class GameComponent_1 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region declaration
        SpriteBatch spriteBatch;
        public bool gameOverFlag { get; set; }
        enum GameState { start, play, gameOver };
        GameState gamestate = GameState.start;
        MouseState mouseState, pre_mouseState;
        Point mousePosition;
        Texture2D texture;
        SpriteFont word;
        sprite background;
        sprite[] holeBackgrounds = new sprite[3];
        sprite hammer;
        sprite GameOver;
        sprite StateBar;
        sprite TimeBar;

        sprite rePlay;
        sprite Continue;
        sprite gameStart;
        List<sprite> Sprites = new List<sprite>();
        List<sprite> Moles = new List<sprite>();
        List<sprite> Bombs = new List<sprite>();
        int mole_iCount = 0;
        int theMoleCount = 0;
        int theMoleKilled = 0;
        Vector2 exitPosition = new Vector2(900, 700);
        Vector2 backGroundPosition = new Vector2(283, 342);
        Vector2 stateBarPosition = new Vector2(16, 2);
        Vector2 gameOverPosition = new Vector2(400, 300);
        Vector2 gameStartPosition = new Vector2(400, 300);
        Vector2 timeBarMarginPosition = new Vector2(281, 117);



        Vector2 thebackGroundCenter = new Vector2(531, 429);
        Vector2 BombsPositionFixed = new Vector2(15, -33);
        List<int> moleList = new List<int>();
        int holeList_X = 2;
        int holeList_Y = 2;
        bool isNext = true;//wheather next moles group in the corner？
        Vector2 startPosition = new Vector2(500, 100);
        List<Vector2> MolePositions = new List<Vector2>();//the mole`s prePosition
        List<sprite> Holes = new List<sprite>();
        int theMaxMoleNumber = 3;//the mole number max of everytime
        int theMaxTimeEach = 5;//the max time of each group
        const int theMaxHoleCount = 9;
        const int thePreMaxBombCount = 9;
        int theMaxBombCount = 1;
        int theBombCount = 1;
        int stage = 1;
        int eachStageTime = 10;//each stage time
        int score = 0;
        int target = 5;//the player must get 40,can entry next stage
        float timePassed = 0;
        int timeClick = 0;
        int timePassed_min = 0;
        int moleShowUpSpeed = 5;
        int moleDisappearSpeed = 2;

        const int theHoleWidth = 372;
        const int theHoleHeight = 153;
        const int theHoleMargin_left = 0;
        const int theHoleMargin_top = 0;
        const int theMoleMargin_left = theHoleMargin_left + 70;
        const int theMoleMargin_top = theHoleMargin_top - 37;
        Vector2 theScoreMargin_left_top = new Vector2(241, 52);
        Vector2 theStageMargin_left_top = new Vector2(594, 52);
        Vector2 theTargetMargin_left_top = new Vector2(929, 52);
        Vector2 theRePlayMargin_left_top = new Vector2(0, 0);
        Texture2D stageNumberTexture;
        Random rnd = new Random();
        Boolean isGamePause = true;
        Vector2 rate = new Vector2(1, 1);
        #endregion

        public int level = 0;

        public GameComponent_1(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            rate = new Vector2((float)GraphicsDevice.Viewport.Width / 1280, (float)GraphicsDevice.Viewport.Height / 768);
            // texture = Game.Content.Load<Texture2D>("Texture\\texture_1");
            word = Game.Content.Load<SpriteFont>("Font\\word");


            Cache.Initialize(Game.Content);
            background = new sprite("moleGame/background", 1, false, backGroundPosition, true, 1, 1);
            background.ShowingRect = new Rectangle(0, 0, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y);
            background.CollisionRect = new Rectangle((int)background.position.X, (int)background.position.Y, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y);
            hammer = new sprite("moleGame/hammer", 4, false, background.position, true, 2, 1);
            GameOver = new sprite("moleGame/GameOver", 5, false, gameOverPosition, true, 1, 1);
            StateBar = new sprite("StateBar", 4, true, stateBarPosition, true, 1, 1);
            StateBar.CollisionRect = new Rectangle(1100, 98, 120, 60);
            TimeBar = new sprite("TimeBar", 5, true, timeBarMarginPosition, true, 1, 1);

            rePlay = new sprite("moleGame/replay", 6, false, (GameOver.position + theRePlayMargin_left_top) * rate, false, 1, 1);

            Continue = new sprite("moleGame/Continue", 6, false, (GameOver.position + theRePlayMargin_left_top) * rate, false, 1, 1);
            stageNumberTexture = Game.Content.Load<Texture2D>("Texture\\stageNumber");

            AddChild(Continue);
            AddChild(rePlay);


            AddChild(background);
            AddChild(hammer);
            AddChild(GameOver);
            AddChild(StateBar);
            AddChild(TimeBar);

            for (int i = 0; i < 3; i++)
            {
                sprite holeBackground = new sprite("moleGame/bg" + (i + 1), 1, false, Vector2.Zero, true, 1, 1);
                AddChild(holeBackground);
                holeBackgrounds[i] = holeBackground;
            }
            holeBackgrounds[0].isVisible = true;
            gameStart = new sprite("moleGame/GameStart", 10, true, gameStartPosition, true, 1, 1);
            AddChild(gameStart);


            for (int i = 0; i < theMaxHoleCount; i++)
            {
                sprite sp = new sprite("moleGame/hit_1", 3, false, background.position, false, 7, 1);
                sp.CurrentPositionX = sp.spriteSizeX - 1;
                sp.ShowingRect = sp.SourceRect(sp);
                AddChild(sp);
                Moles.Add(sp);
            }

            //Moles.Add(bomb);
            for (int i = 0; i < thePreMaxBombCount; i++)
            {
                sprite bb = new sprite("moleGame/hit_2", 3, false, new Vector2(0, 0), false, 7, 1);

                bb.isHit = false;
                Bombs.Add(bb);
                AddChild(bb);
                //AddChild(bb.effectSprite);
                Moles.Add(bb);

            }
            //pre define the mole position
            for (int i = 0; i < holeList_Y; i++)
            {
                for (int j = 0; j < holeList_X; j++)
                {
                    Vector2 vt = new Vector2(background.position.X + theMoleMargin_left + theHoleWidth * j, background.position.Y + theMoleMargin_top + theHoleHeight * i);
                    MolePositions.Add(vt);
                }
            }

            foreach (var sp in Sprites)
            {
                sp.CollisionRect = new Rectangle((int)(sp.CollisionRect.X * rate.X), (int)(sp.CollisionRect.Y * rate.X), (int)(sp.CollisionRect.Width * rate.X), (int)(sp.CollisionRect.Height * rate.X));
            }

            base.LoadContent();
        }
        //increase the holes and Moles
        //increase the holes and Moles
        private void AddHolesAndMoles()
        {



            //pre define the mole position
            MolePositions.Clear();
            for (int i = 0; i < holeList_Y; i++)
            {
                for (int j = 0; j < holeList_X; j++)
                {

                    Vector2 vt = new Vector2(background.position.X + theMoleMargin_left + theHoleWidth * j, background.position.Y + theMoleMargin_top + theHoleHeight * i);
                    MolePositions.Add(vt);

                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousePosition = new Point(mouseState.X, mouseState.Y);
            switch (gamestate)
            {
                case GameState.start:

                    #region //via level to add the game degree of difficult
                    if (level > 5) level = 6;

                    target += level;
                    stage += level;
                    if (stage >= 2 && stage <= 5)
                    {

                        background.position = new Vector2(100, 300);
                        holeBackgrounds[0].isVisible = false;
                        holeBackgrounds[1].isVisible = true;
                        theMaxMoleNumber = 4;
                        theMaxBombCount = 2;
                        holeList_X = 3;
                        holeList_Y = 2;

                    }
                    if (stage >= 6)
                    {
                        background.position = new Vector2(100, 300);
                        holeBackgrounds[1].isVisible = false;
                        holeBackgrounds[2].isVisible = true;
                        theMaxMoleNumber = 6;
                        theMaxBombCount = 3;
                        holeList_X = 3;
                        holeList_Y = 3;
                    }

                    background.ShowingRect = new Rectangle(0, 0, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y);

                    AddHolesAndMoles();

                    #endregion
                    gamestate = GameState.play;
                    isGamePause = true;

                    foreach (var sp in Sprites)
                    {
                        sp.CurrentPositionX = 0;

                    }
                    foreach (var mole in Moles)
                    {
                        mole.isVisible = false;
                        mole.CurrentPositionX = mole.spriteSizeX - 1;
                    }

                    timePassed = 0;
                    timePassed_min = 0;
                    timeClick = 0;
                    theMoleCount = 0;
                    theMoleKilled = 0;
                    score = 0;
                    TimeBar.ShowingRect = new Rectangle(0, 0, (int)(TimeBar.SourceTexture.Width * (float)((eachStageTime - timePassed) / eachStageTime)), TimeBar.SourceTexture.Height);//show the timebar width
                    //}
                    break;
                case GameState.play:

                    spriteRectUpdate();//each sprites`Rect Update
                    if (isGamePause == false)
                    {
                        foreach (var sp in Sprites)
                        {
                            if (sp.isVisible == true)
                            {
                                sp.ShowingRect = sp.SourceRect(sp);
                            }
                        }
                        if (GameOver.isVisible == false)//if gameover or start unVisible
                        {
                            timeUpdate();//time Update          
                        }
                        hammerUpdate();//the hammer Update 
                        moleUpdate();//the mole Update
                        background.ShowingRect = new Rectangle(0, 0, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y);
                        background.CollisionRect = new Rectangle((int)background.position.X, (int)background.position.Y - 100, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y + 200);
                    }
                    break;
                case GameState.gameOver:
                    gamestate = GameState.start;
                    gameOverFlag = true;
                    this.Enabled = false;
                    this.Visible = false;
                    GameOver.isVisible = false;
                    this.Dispose();
                    break;
            }
            pre_mouseState = mouseState;
            base.Update(gameTime);
        }
        //each sprite`Rect update
        private void spriteRectUpdate()
        {
            foreach (var sp in Moles)
            {
                sp.ShowUp(sp, moleShowUpSpeed);
                sp.disappear(sp, moleDisappearSpeed);
            }

            if (mouseLeftButtonClick())
            {

                if (gameStart.CollisionRect.Contains(mousePosition) && gameStart.isVisible == true)
                {
                    isGamePause = false;
                    isNext = true;
                    gameStart.isVisible = false;
                }
                if (StateBar.CollisionRect.Contains(mousePosition))
                {
                    gamestate = GameState.gameOver;
                }
                if (GameOver.CollisionRect.Contains(mousePosition) && GameOver.isVisible == true)
                {
                    GameOver.isVisible = false;
                    gamestate = GameState.gameOver;
                }
            }
            if (isGamePause == true)
            {
                Game.IsMouseVisible = true;
                hammer.isVisible = false;
                if (rePlay.isVisible == true)
                {
                    if (rePlay.CollisionRect.Contains(mousePosition) && mouseLeftButtonClick())
                    {
                        //replay
                        #region rePaly
                        rePlay.isVisible = false;
                        GameOver.isVisible = false;
                        isGamePause = false;
                        //gamestate = GameState.start;
                        isNext = true;
                        foreach (var sp in Sprites)
                        {
                            sp.CurrentPositionX = 0;

                        }
                        foreach (var mole in Moles)
                        {
                            //mole.isVisible = false;
                            mole.isAlive = true;
                            mole.CurrentPositionX = mole.spriteSizeX - 1;
                        }

                        //stage = 1;
                        timePassed = 0;
                        timePassed_min = 0;
                        timeClick = 0;
                        theMoleCount = 0;
                        theMoleKilled = 0;
                        score = 0;
                        TimeBar.ShowingRect = new Rectangle(0, 0, (int)(TimeBar.SourceTexture.Width * (float)((eachStageTime - timePassed) / eachStageTime)), TimeBar.SourceTexture.Height);//show the timebar width
                        #endregion
                    }
                }
                if (Continue.isVisible == true)
                {
                    if (Continue.CollisionRect.Contains(mousePosition) && mouseLeftButtonClick())
                    {
                        #region Continue
                        Continue.isVisible = false;
                        isGamePause = false;
                        isNext = true;
                        if (stage >= 2 && stage <= 5)
                        {

                            background.position = new Vector2(100, 300);
                            holeBackgrounds[0].isVisible = false;
                            holeBackgrounds[1].isVisible = true;
                            theMaxMoleNumber = 4;
                            theMaxBombCount = 2;
                            holeList_X = 3;
                            holeList_Y = 2;

                        }
                        if (stage >= 6)
                        {
                            background.position = new Vector2(100, 300);
                            holeBackgrounds[1].isVisible = false;
                            holeBackgrounds[2].isVisible = true;
                            theMaxMoleNumber = 6;
                            theMaxBombCount = 3;
                            holeList_X = 3;
                            holeList_Y = 3;
                        }

                        background.ShowingRect = new Rectangle(0, 0, theHoleMargin_left * 2 + theHoleWidth * holeList_X, theHoleMargin_top * 2 + theHoleHeight * holeList_Y);
                        // background.position = new Vector2(thebackGroundCenter.X - (theHoleMargin_left * 2 + theHoleWidth * holeList_X) / 2, thebackGroundCenter.Y - (theHoleMargin_top * 2 + theHoleHeight * holeList_Y) / 2);
                        foreach (var sp in Sprites)
                        {
                            sp.CurrentPositionX = 0;
                        }
                        foreach (var mole in Moles)
                        {
                            mole.isAlive = true;
                            mole.CurrentPositionX = mole.spriteSizeX - 1;
                        }

                        //stage = 1;
                        timePassed = 0;
                        timePassed_min = 0;
                        timeClick = 0;
                        theMoleCount = 0;
                        theMoleKilled = 0;
                        score = 0;
                        AddHolesAndMoles();
                        TimeBar.ShowingRect = new Rectangle(0, 0, (int)(TimeBar.SourceTexture.Width * (float)((eachStageTime - timePassed) / eachStageTime)), TimeBar.SourceTexture.Height);//show the timebar width
                        #endregion
                    }
                }
            }
        }
        //time Update
        private void timeUpdate()
        {
            if (mouseLeftButtonClick())
            {
                foreach (var sp in Moles)
                {
                    if (sp.isVisible == true && sp.isAlive == true)
                    {
                        if (new Rectangle((int)sp.position.X - 10, (int)sp.position.Y - 10, (int)sp.SourceTexture.Width / sp.spriteSizeX + 20, (int)sp.SourceTexture.Height + 20).Contains(mousePosition))
                        {
                            timePassed_min = 0;
                        }
                    }
                }
            }
            TimeBar.ShowingRect = new Rectangle(0, 0, (int)(TimeBar.SourceTexture.Width * (float)((eachStageTime - timePassed) / eachStageTime)), TimeBar.SourceTexture.Height);//show the timebar width

            timeClick++;
            if (timeClick >= 60)
            {
                timePassed++;
                timeClick = 0;

                timePassed_min++;
                if (timePassed_min > theMaxTimeEach)//if each group time reached,then clear the moles;
                {

                    foreach (var sp in Moles)
                    {
                        if (sp.isVisible == true && sp.isAlive == true)
                        {
                            sp.isAlive = false;
                        }
                    }
                    isNext = true;
                    theMoleKilled = 0;
                    timePassed_min = 0;
                }
                if (timePassed > eachStageTime)//if each stage time reached,then reStart game;
                {
                    foreach (var sp in Moles)
                    {
                        if (sp.isVisible == true && sp.isAlive == true)
                        {
                            sp.isAlive = false;
                        }
                    }
                    timePassed = 0;
                    if (score >= target)//if reach the target
                    {
                        stage++;
                        if (stage - level > 10)
                        {
                            stage--;
                            GameOver.isVisible = true;
                            isGamePause = true;
                            Game.IsMouseVisible = true;
                            return;
                        }

                        theMoleKilled = 0;
                        timePassed_min = 0;

                        target += 2;

                        score = 0;

                        Continue.isVisible = true;

                    }
                    else
                    {

                        // GameOver.isVisible = true;
                        rePlay.isVisible = true;

                    }
                    isGamePause = true;
                }
            }

        }
        //the mole Update
        private void moleUpdate()
        {
            int a = 0;
            if (mouseLeftButtonClick())
            {
                foreach (var sp in Moles)
                {
                    if (sp.isVisible == true && sp.isAlive == true)
                    {
                        if (new Rectangle((int)sp.position.X - 10, (int)sp.position.Y - 10, (int)sp.SourceTexture.Width / sp.spriteSizeX, (int)sp.SourceTexture.Height - 20).Contains(mousePosition))
                        {
                            sp.isAlive = false;
                            theMoleKilled++;
                            if (sp.isHit == true)
                            {
                                score++;

                            }
                            else
                            {



                                foreach (var s in Moles)
                                {
                                    if (s.isVisible == true && s.isAlive == true)
                                    {
                                        s.isAlive = false;
                                    }
                                }
                                isNext = true;
                                timePassed_min = 0;
                                theMoleKilled = 0;
                            }
                            if (theMoleKilled >= theMoleCount)
                            {
                                foreach (var bb in Bombs)
                                {
                                    bb.isAlive = false;
                                }
                                isNext = true;
                                timePassed_min = 0;
                                theMoleKilled = 0;

                            }

                        }
                    }
                }
            }

            #region prepare next grounp Moles and UFOs
            if (isNext == true)
            {
                if (allTheBombDisappear())
                {
                    Random rd = new Random();
                    List<int> RandomNumber = new List<int>();
                    List<int> RandomBombPosition = new List<int>();

                    theMoleCount = rd.Next(theMaxMoleNumber - 2, theMaxMoleNumber);
                    theBombCount = rd.Next(1, theMaxBombCount);
                    isNext = false;

                    while (true)
                    {
                        while (true)
                        {
                            a = rd.Next(0, holeList_X * holeList_Y);
                            if (!RandomNumber.Contains(a))
                            {
                                RandomNumber.Add(a);
                                break;
                            }
                        }
                        if (RandomNumber.Count >= theMoleCount) break;
                    }
                    if (stage >= 2)// (theMoleCount <= 3)
                    {
                        while (true)
                        {
                            while (true)
                            {
                                a = rd.Next(0, holeList_X * holeList_Y);
                                if (!RandomNumber.Contains(a))
                                {
                                    RandomBombPosition.Add(a);
                                    break;
                                }
                            }
                            if (RandomBombPosition.Count >= theBombCount) break;
                        }
                        for (int i = 0; i < theMaxBombCount; i++)
                        {
                            if (Bombs[i].isVisible == false && Bombs[i].isAlive == true)//&& Bombs[i].isAlive ==true
                            {
                                Bombs[i].isVisible = true;
                                Bombs[i].position = MolePositions[RandomBombPosition[i]] + BombsPositionFixed;
                                if (i >= theBombCount - 1) break;
                            }

                        }

                    }
                    for (int i = 0; i < holeList_X * holeList_Y; i++)
                    {
                        if (Moles[i].isVisible == false && Moles[i].isAlive == true)
                        {

                            Moles[i].isVisible = true;
                            Moles[i].position = MolePositions[RandomNumber[mole_iCount]];
                            mole_iCount++;
                            if (mole_iCount >= theMoleCount)
                            {
                                mole_iCount = 0;
                                return;
                            }
                        }
                    }
                }
            }
            #endregion


        }
        //is all the bomb and bombEffect is unVisible?
        private bool allTheBombDisappear()
        {
            foreach (var bb in Moles)
            {
                if (bb.isVisible == true)
                {
                    return false;
                }
            }
            return true;
        }
        //the hammer update
        private void hammerUpdate()
        {
            if (background.CollisionRect.Contains(mousePosition))
            {
                hammer.isVisible = true;
                Game.IsMouseVisible = false;
                hammer.position = new Vector2(mousePosition.X - 50, mousePosition.Y - 50);
                if (mouseLeftButtonClick())
                {
                    foreach (var sp in Moles)
                    {
                        if (sp.isVisible == true && new Rectangle((int)sp.position.X - 10, (int)sp.position.Y - 10, (int)sp.SourceTexture.Width / sp.spriteSizeX + 20, (int)sp.SourceTexture.Height + 20).Contains(mousePosition))
                        {
                            hammer.CurrentPositionX = 1;
                        }
                    }
                }
                if (hammer.CurrentPositionX == 1)
                {
                    hammer.iCount++;
                    if (hammer.iCount >= 5)
                    {
                        hammer.iCount = 0;
                        hammer.CurrentPositionX = 0;
                    }
                }
            }
            else
            {
                hammer.isVisible = false;
                Game.IsMouseVisible = true;
            }

        }

        void DrawNumber(int number, Vector2 position, int number_Width, int number_Height)
        {
            if (number > 0)
            {
                List<int> numberList = new List<int>();
                while (number > 0)
                {
                    numberList.Add(number % 10);
                    number = number / 10;
                }
                position.X = position.X + number_Width / 2 * (numberList.Count - 1);
                for (int i = 0; i < numberList.Count; i++)
                {
                    spriteBatch.Draw(stageNumberTexture, new Rectangle((int)position.X - number_Width / 2 * i, (int)position.Y, number_Width, number_Height),
                        new Rectangle(numberList[i] * (stageNumberTexture.Width / 11), 0, stageNumberTexture.Width / 11, stageNumberTexture.Height), Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(stageNumberTexture, new Rectangle((int)position.X, (int)position.Y, number_Width, number_Height), new Rectangle(0, 0, stageNumberTexture.Width / 11, stageNumberTexture.Height), Color.White);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            switch (gamestate)
            {
                case GameState.start:
                    spriteBatch.Draw(texture, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(word, "Press mouse left button to start!", new Vector2(300, 300), Color.Green);
                    break;
                case GameState.play:
                    foreach (var sp in Sprites)
                    {
                        sp.scale = rate;


                    }
                    //StateBar.scale *= (float)768 / 1600;
                    //StateBar.position.X = -3;
                    //TimeBar.scale *= (float)768 / 1600;

                    foreach (var sp in Sprites)
                    {


                        if (sp.isVisible == true)
                        {
                            spriteBatch.Draw(sp.SourceTexture, sp.position * rate, sp.ShowingRect, Color.White, 0f, new Vector2(0, 0), sp.scale, SpriteEffects.None, 1f);
                        }
                    }
                    DrawString();
                    break;
                case GameState.gameOver:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public virtual void AddChild(sprite child)
        {
            Sprites.Add(child);
            sortChilds();
        }

        private void sortChilds()
        {
            Sprites.Sort(sortFunc);
        }

        int sortFunc(sprite a, sprite b)
        {
            if (a.Z > b.Z) return 1;
            else if (a.Z < b.Z) return -1;
            else return 0;
        }

        private bool mouseLeftButtonClick()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && pre_mouseState.LeftButton == ButtonState.Released) return true;
            return false;
        }

        private void DrawString()
        {
            DrawNumber(score, (StateBar.position + theScoreMargin_left_top) * rate, (int)(50 * rate.X), (int)(60 * rate.X));
            //spriteBatch.DrawString(word, score.ToString(), (StateBar.position + theScoreMargin_left_top)*rate, Color.Green);//draw the score
            DrawNumber((stage - level), (StateBar.position + theStageMargin_left_top) * rate, (int)(50 * rate.X), (int)(60 * rate.X));
            //spriteBatch.DrawString(word, (stage - level).ToString(), (StateBar.position + theStageMargin_left_top)*rate, Color.Green);//draw the stage
            DrawNumber(target, (StateBar.position + theTargetMargin_left_top) * rate, (int)(50 * rate.X), (int)(60 * rate.X));
            //spriteBatch.DrawString(word, target.ToString(), (StateBar.position + theTargetMargin_left_top)*rate, Color.Green);//target: 
            //spriteBatch.DrawString(word, "isNext: " + isNext.ToString() + "   theMoleKilled:" + theMoleKilled + "   the moleCount and bombCount:" + theMoleCount + ":" + theBombCount + " theBoms{0].visible" + Bombs[0].isVisible.ToString(), new Vector2(200, 700), Color.Red);
            //+"theMoleKilled:"+theMoleKilled+"the moleCount:"+theMoleCount
            //if (flashFlag)
            // spriteBatch.Draw(Sprites[0].SourceTexture, new Vector2(200, 200), Color.White); 
        }

    }
}

