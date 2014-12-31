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

    public class GameComponent_2 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        public bool gameOverFlag { get; set; }

        enum GameState { start, play, instruction, gameOver };
        GameState gamestate = GameState.start;

        MouseState mouseState, pre_mouseState;
        Point mousePosition;

        SpriteFont word;

        Vector2 exitPosition = new Vector2(1160, 140);
        //bool exitFlag = false;

        int gameOverShowTime = 5000;

        Vector2 questionPosition = new Vector2(320, 460);
        Vector2 answerPosition = new Vector2(950, 450);
        int offset_X = 150;
        int offset_Y = 120;
        //string questionString, answerA, answerB, answerC, answerD;
        int rightAnswerNumber = 0;
        bool[] enlargeFlag = { false, false, false, false };
        int choiceFlag = 0;

        int score = 0;

        Random rnd = new Random();

        //Texture2D[] texture_Element = new Texture2D[7];
        List<Texture2D> texture_Question = new List<Texture2D>();
        //List<Texture2D> texture_Answer = new List<Texture2D>();
        Texture2D texture_RightAnswer;
        Texture2D[] texture_Answer = new Texture2D[4];
        List<int> wrongAnswerList = new List<int>();
        int[] answerArray = new int[4];
        List<Point> wrongAnswerRandomPoint = new List<Point>();
        int randomPoint_X = 50;
        int randomPoint_Y = 50;
        float textureScale = 1.0f;
        int textureHalfLenght = 230;
        float[] textureScaleArray = { 0.8f, 0.82f, 0.84f, 0.86f, 0.88f, 0.9f, 0.92f, 0.94f, 0.96f, 1.0f };

        int choiceStopTime = 500;

        Texture2D texture_Frame;
        Texture2D texture_Instruction;

        protected const int SetQuestionUseTime = 30000;
        int questionUseTime = 30000;

        int stageNumber = 1;
        Texture2D stageNumberTexture;
        Texture2D exitTexture;

        public int level = 1;
        private int level_real = 1;
        List<int> level_ElementNumber = new List<int>{ 4, 5, 4, 5, 5, 5, 5, 5, 5, 5 };
        int totalLevel = 10;

        string basePath = "";

        Texture2D backgroundTexture;
        Texture2D counterTexture;
        Texture2D timeTexture;
        Texture2D startTexture;
        Vector2 startTexturePosition = new Vector2(400, 300);
        Texture2D instructionTexture;
        Texture2D nextTexture;
        Rectangle nextTextureRect = new Rectangle(1100, 650, 200, 100);

        public GameComponent_2(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {

            base.Initialize();
        }


        private bool CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之                
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之                
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组 
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录               
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    }
                    // 否则直接Copy文件 
                    else
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetBasePath()
        {
            string path = "";           

            #region MyRegion
            ////在GameComponent_2和GameComponent_3中，都写了这个复制文档的逻辑，
            ////这是因为程序启动时，先运行GameComponent_2和GameComponent_3的Load函数，所以无法只在Game1里面写一次
            //string name = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;  //取得当前方法命名空间
            //if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + Properties.Vision.Default.basePath))
            //{
            //    path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.Vision.Default.basePath + "\\";
            //}
            //else
            //{
            //    String resourcePath = System.AppDomain.CurrentDomain.BaseDirectory;
            //    string[] sArray = resourcePath.Split('\\');

            //    for (int i = 0; i < sArray.Length; i++)
            //    {
            //        if (sArray[i] == name)
            //        {
            //            resourcePath = sArray[0] + "\\";
            //            for (int j = 1; j <= i; j++)
            //            {
            //                resourcePath += sArray[j] + "\\";
            //            }
            //            if (Directory.Exists(resourcePath + "Resources"))
            //            {
            //                break;
            //            }
            //        }
            //    }

            //    if (CopyDir(resourcePath + "Resources", System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources"))
            //    {
            //        path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.Vision.Default.basePath + "\\";
            //    }
            //    else
            //    {
            //        path = resourcePath + Properties.Vision.Default.basePath + "\\";
            //    }
            //} 
            #endregion

            path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.Vision.Default.basePath + "\\";

            return path;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            basePath = GetBasePath();

            //texture = Game.Content.Load<Texture2D>("Texture\\texture_2");
            word = Game.Content.Load<SpriteFont>("Font\\word");

            texture_Frame = Game.Content.Load<Texture2D>("Texture\\element\\texture_Frame");
            texture_Instruction = Game.Content.Load<Texture2D>("Texture\\element\\instruction");

            stageNumberTexture = Game.Content.Load<Texture2D>("Texture\\stageNumber");
            exitTexture = Game.Content.Load<Texture2D>("Texture\\exit");
            counterTexture = Game.Content.Load<Texture2D>("Texture\\StateBar");
            backgroundTexture = Game.Content.Load<Texture2D>("Texture\\element\\bg_c");
            timeTexture = Game.Content.Load<Texture2D>("Texture\\TimeBar");
            startTexture = Game.Content.Load<Texture2D>("Texture\\moleGame\\GameStart");
            instructionTexture = Game.Content.Load<Texture2D>("Texture\\element\\layout-04");
            nextTexture = Game.Content.Load<Texture2D>("Texture\\element\\next");

            //GetQusetionAndAnswer();
            totalLevel = Properties.Vision.Default.totalLevel;
            string elementNumberString = Properties.Vision.Default.elementNumber;
            List<string> elementNumberList = elementNumberString.Split(';').ToList();
            if ((elementNumberList.Count > 0) && (!String.IsNullOrEmpty(elementNumberList[0])))
            {
                for (int i = 0; i < totalLevel; i++)
                {
                    if (i < elementNumberList.Count)
                    {
                        level_ElementNumber[i] = int.Parse(elementNumberList[i]);
                    }
                    else
                    {
                        level_ElementNumber[i] = int.Parse(elementNumberList[elementNumberList.Count - 1]);
                    }
                }
            }

            string textureScaleString = Properties.Vision.Default.textureScale;
            List<string> textureScaleList = textureScaleString.Split(';').ToList();
            if ((textureScaleList.Count > 0) && (!String.IsNullOrEmpty(textureScaleList[0])))
            {
                for (int i = 0; i < totalLevel; i++)
                {
                    if (i < textureScaleList.Count)
                    {
                        textureScaleArray[i] = float.Parse(textureScaleList[i]) / 100;
                    }
                    else
                    {
                        textureScaleArray[i] = float.Parse(textureScaleList[textureScaleList.Count - 1]) / 100;
                    }
                }
            }

            base.LoadContent();
        }


        List<string> GetTextureNameBySearchFile(string path, int kind)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> tempList = new List<string>();
            foreach (FileInfo fi in dir.GetFiles())
            {
                if (kind == 1)      //获取answer.png图片
                {
                    if (fi.Name == "answer.png")
                    {
                        tempList.Add(fi.Name);           //get textures name by search file
                    }
                }
                else                //获取answer.png之外的图片
                {
                    if (fi.Name != "answer.png")
                    {
                        tempList.Add(fi.Name);           //get textures name by search file
                    }
                }
            }
            if (tempList.Count <= 0)
            {
                //formMessage.Message_NOTexture();    //show message
                //Exit();
            }
            return tempList;
        }

        List<string> GetFileNameBySearchFile(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> tempList = new List<string>();
            foreach (DirectoryInfo dChild in dir.GetDirectories())
            {
                tempList.Add(dChild.Name);           //get file name by search file
            }
            if (tempList.Count <= 0)
            {
                //formMessage.Message_NOFile();    //show message
                //Exit();
            }
            return tempList;
        }


        void GetQusetionAndAnswer()
        {
            string texturePath = basePath + "\\level_" + level_real;
            if (!Directory.Exists(texturePath))
            {
                texturePath = basePath + "\\level_1";
            }

            List<string> allTextureList = GetFileNameBySearchFile(texturePath);
            List<int> randomList = new List<int>();
            for (int i = 0; i < allTextureList.Count; i++)
            {
                randomList.Add(i);
            }

            int temp = rnd.Next(0, allTextureList.Count);
            rightAnswerNumber = randomList[temp];
            randomList.Remove(randomList[temp]);
            List<string> answerTextureList = GetTextureNameBySearchFile(texturePath + "\\" + allTextureList[temp], 1);
            texture_RightAnswer = Texture2D.FromStream(GraphicsDevice, File.OpenRead(texturePath + "\\" + allTextureList[temp] + "\\" + answerTextureList[0]));
            allTextureList.Remove(allTextureList[temp]);

            texture_Question.Clear();
            wrongAnswerList.Clear();
            wrongAnswerRandomPoint.Clear();
            textureScale = textureScaleArray[level_real - 1];
            randomPoint_X = (int)((1 - textureScale) * 250);
            randomPoint_Y = (int)((1 - textureScale) * 250);
            int wrongAnswerNumber = level_ElementNumber[level_real - 1];
            for (int i = 0; i < wrongAnswerNumber; i++)
            {
                temp = rnd.Next(0, allTextureList.Count);
                wrongAnswerList.Add(randomList[temp]);
                randomList.Remove(randomList[temp]);
                List<string> questionTextureList = GetTextureNameBySearchFile(texturePath + "\\" + allTextureList[temp], 0);
                texture_Question.Add(Texture2D.FromStream(GraphicsDevice, 
                    File.OpenRead(texturePath + "\\" + allTextureList[temp] + "\\" + questionTextureList[rnd.Next(0, questionTextureList.Count)])));

                if (i < 3)
                {
                    List<string> dummyAnswerTextureList = GetTextureNameBySearchFile(texturePath + "\\" + allTextureList[temp], 1);
                    texture_Answer[i] = Texture2D.FromStream(GraphicsDevice,
                    File.OpenRead(texturePath + "\\" + allTextureList[temp] + "\\" + dummyAnswerTextureList[0]));
                }

                wrongAnswerRandomPoint.Add(new Point(rnd.Next(-randomPoint_X, randomPoint_X), rnd.Next(-randomPoint_Y, randomPoint_Y)));

                allTextureList.Remove(allTextureList[temp]);
            }

            answerArray[0] = wrongAnswerList[0];
            answerArray[1] = wrongAnswerList[1];
            answerArray[2] = wrongAnswerList[2];
            answerArray[3] = rightAnswerNumber;
            texture_Answer[3] = texture_RightAnswer;

            temp = rnd.Next(0, 4);
            int keepAnswer = answerArray[temp];
            answerArray[temp] = answerArray[3];
            answerArray[3] = keepAnswer;
            Texture2D keepTexture = texture_Answer[temp];
            texture_Answer[temp] = texture_Answer[3];
            texture_Answer[3] = keepTexture;

        }

        int GetScoreByTime(int time)
        {
            int maxScore = 10;
            int addScore = (time * maxScore) / SetQuestionUseTime;
            addScore += 1;

            return addScore;
        }

        void UpdateStage()
        {
            stageNumber = score / 100 + 1;

            level_real = level + stageNumber - 1;
            if (level_real > totalLevel)
            {
                level_real = totalLevel;
            }
        }

        void DrawNumber(int number, int x, int y, int number_Width, int number_Height)
        {
            if (number > 0)
            {
                List<int> numberList = new List<int>();
                while (number > 0)
                {
                    numberList.Add(number % 10);
                    number = number / 10;
                }
                x = x + number_Width / 2 * (numberList.Count - 1);
                for (int i = 0; i < numberList.Count; i++)
                {
                    spriteBatch.Draw(stageNumberTexture, new Rectangle(x - number_Width / 2 * i, y, number_Width, number_Height),
                        new Rectangle(numberList[i] * (stageNumberTexture.Width / 11), 0, stageNumberTexture.Width / 11, stageNumberTexture.Height), Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(stageNumberTexture, new Rectangle(x, y, number_Width, number_Height), new Rectangle(0, 0, stageNumberTexture.Width / 11, stageNumberTexture.Height), Color.White);
            }
        }


        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousePosition = new Point(mouseState.X, mouseState.Y);

            switch (gamestate)
            {
                case GameState.start:

                    if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                    {
                        gamestate = GameState.play;
                        score = 0;
                        questionUseTime = SetQuestionUseTime;
                        UpdateStage();
                        GetQusetionAndAnswer();
                    }

                    break;

                case GameState.play:

                    choiceFlag = 0;
                    for (int i = 0; i < enlargeFlag.Length; i++)
                    {
                        enlargeFlag[i] = false;
                    }

                    choiceStopTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (choiceStopTime <= 0)
                    {
                        choiceStopTime = 0;
                    }

                    if (questionUseTime > 0)
                    {
                        questionUseTime -= gameTime.ElapsedGameTime.Milliseconds;
                    }

                    if (rightAnswerNumber == -1)
                    {
                        questionUseTime = SetQuestionUseTime;
                        GetQusetionAndAnswer();
                    }
                    else if (choiceStopTime <= 0)
                    {
                        int size = 100;
                        int[] answerXPoint = new int[] { 1, -1, 1, -1 };
                        int[] answerYPoint = new int[] { 1, 1, -1, -1 };
                        Rectangle[] rect = new Rectangle[4];
                        for (int i = 0; i < 4; i++)
                        {
                            rect[i] = new Rectangle((int)answerPosition.X - answerXPoint[i] * offset_X - size, (int)answerPosition.Y - answerYPoint[i] * offset_Y - size,
                                size * 2, size * 2);
                        }

                        for (int i = 0; i < enlargeFlag.Length; i++)
                        {
                            if (rect[i].Contains(mousePosition))
                            {
                                enlargeFlag[i] = true;
                                choiceFlag = i + 1;
                                break;
                            }
                        }

                        if (choiceFlag != 0)
                        {
                            if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                            {
                                choiceStopTime = 500;       //当玩家按键进行一次选择后，要间隔0.5秒才能进行下一次选择

                                if ((rightAnswerNumber == answerArray[choiceFlag - 1]))           //make a right choice
                                {
                                    rightAnswerNumber = -1;

                                    score += GetScoreByTime(questionUseTime);
                                    UpdateStage();
                                }

                                //rightAnswerNumber = -1;     //无论是否答对，都切换到下一题
                                choiceFlag = 0;
                            }
                        }
                    }


                    Rectangle exitRect = new Rectangle((int)exitPosition.X - exitTexture.Width / 2, (int)exitPosition.Y - exitTexture.Height / 2,
                                                     exitTexture.Width, exitTexture.Height);
                    if (exitRect.Contains(mousePosition))
                    {
                        //exitFlag = true;
                        if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                        {
                            gamestate = GameState.gameOver;
                        }
                    }
                    else
                    {
                        //exitFlag = false;
                    }

                    if (nextTextureRect.Contains(mousePosition))
                    {
                        if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                        {
                            gamestate = GameState.instruction;
                        }
                    }

                    break;

                case GameState.instruction:

                    if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                    {
                        gamestate = GameState.play;
                    }

                    break;

                case GameState.gameOver:

                    //gameOverShowTime -= gameTime.ElapsedGameTime.Milliseconds;
                    //if (gameOverShowTime <= 0)
                    //{
                        gameOverShowTime = 5000;
                        gamestate = GameState.start;
                        gameOverFlag = true;

                        this.Enabled = false;
                        this.Visible = false;
                        this.Dispose();
                    //}

                    break;
            }

            pre_mouseState = mouseState;

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(230, 230, 230));

            spriteBatch.Begin();

            switch (gamestate)
            {
                case GameState.start:
                    
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    //spriteBatch.Draw(counterTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    //spriteBatch.Draw(timeTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    spriteBatch.Draw(counterTexture, new Vector2(16, 2), Color.White);
                    spriteBatch.Draw(timeTexture, new Vector2(281, 117), Color.White);
                    spriteBatch.Draw(startTexture, startTexturePosition, Color.White);
                    DrawNumber(score, 210, 48, 50, 60);
                    DrawNumber(stageNumber, 600, 48, 50, 60);

                    break;

                case GameState.play:
                    
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    //spriteBatch.Draw(counterTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    //spriteBatch.Draw(timeTexture, new Rectangle(0, 0, 1280, 768), Color.White);
                    spriteBatch.Draw(counterTexture, new Vector2(16, 2), Color.White);
                    spriteBatch.Draw(timeTexture, new Vector2(281, 117), Color.White);

                    //spriteBatch.Draw(texture_Instruction, new Vector2((1280 - texture_Instruction.Width) / 2, 60), Color.White);

                    //spriteBatch.Draw(texture_Frame, questionPosition - new Vector2(texture_Frame.Width / 2, texture_Frame.Height / 2), Color.White);

                    for (int i = 0; i < wrongAnswerList.Count; i++)
                    {
                        spriteBatch.Draw(texture_Question[i],
                            new Rectangle((int)questionPosition.X - (int)(textureScale * textureHalfLenght) + wrongAnswerRandomPoint[i].X, (int)questionPosition.Y - (int)(textureScale * textureHalfLenght) + wrongAnswerRandomPoint[i].Y, (int)(textureScale * textureHalfLenght * 2), (int)(textureScale * textureHalfLenght * 2)),
                            new Rectangle(0, 0, texture_Question[i].Width, texture_Question[i].Height), Color.White);
                    }

                    int smallSize = 100;
                    int bigSize = 150;
                    int[] answerXPoint = new int[] { 1, -1, 1, -1 };
                    int[] answerYPoint = new int[] { 1, 1, -1, -1 };
                    for (int i = 0; i < enlargeFlag.Length; i++)
                    {
                        if (enlargeFlag[i])
                        {
                            spriteBatch.Draw(texture_Answer[i], 
                                new Rectangle((int)answerPosition.X - answerXPoint[i] * offset_X - bigSize, (int)answerPosition.Y - answerYPoint[i] * offset_Y - bigSize, bigSize * 2, bigSize * 2),
                                new Rectangle(0, 0, texture_Answer[i].Width, texture_Answer[i].Height), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(texture_Answer[i],
                                new Rectangle((int)answerPosition.X - answerXPoint[i] * offset_X - smallSize, (int)answerPosition.Y - answerYPoint[i] * offset_Y - smallSize, smallSize * 2, smallSize * 2),
                                new Rectangle(0, 0, texture_Answer[i].Width, texture_Answer[i].Height), Color.White);
                        }
                    }

                    //if (exitFlag)       //exit button
                    //{
                    //    spriteBatch.Draw(exitTexture, exitPosition - new Vector2(exitTexture.Width, exitTexture.Height) * 0.65f,
                    //        new Rectangle(0, 0, exitTexture.Width, exitTexture.Height), Color.White, 0, Vector2.Zero, 1.3f, SpriteEffects.None, 1);
                    //}
                    //else
                    //{
                    //    spriteBatch.Draw(exitTexture, exitPosition - new Vector2(exitTexture.Width, exitTexture.Height) / 2, Color.White);
                    //}
                    spriteBatch.Draw(nextTexture, nextTextureRect, new Rectangle(0, 0, nextTexture.Width, nextTexture.Height), Color.White);
                    
                    DrawNumber(score, 210, 48, 50, 60);
                    DrawNumber(stageNumber, 600, 48, 50, 60);

                    break;

                case GameState.instruction:

                    spriteBatch.Draw(instructionTexture, new Rectangle(0, 0, 1280, 768), Color.White);

                    break;

                case GameState.gameOver:

                    //spriteBatch.Draw(texture, new Vector2(200, 200), Color.White);
                    spriteBatch.DrawString(word, "The game will exit,after   seconds", new Vector2(500, 500), Color.Blue);
                    spriteBatch.DrawString(word, " " + gameOverShowTime / 1000, new Vector2(500 + word.MeasureString("The game will exit,after").X, 500), Color.Red);

                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
