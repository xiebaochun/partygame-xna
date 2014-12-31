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
using System.Xml;

namespace TouchGame
{

    public class GameComponent_3 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //static const String SORT_QUESTION_LOCATION ="SortQuestion\\";
        SpriteBatch spriteBatch;
        public bool gameOverFlag { get; set; }

        enum GameState { start, play, gameOver };
        GameState gamestate = GameState.start;

        MouseState mouseState, pre_mouseState;
        Point mousePosition;

        SpriteFont word;
        //Texture2D[] cardTexture = new Texture2D[3];
        //Texture2D texture_bgFrame;

        Vector2 exitPosition = new Vector2(1160, 140);
        //bool exitFlag = false;
        float exitButton_scale = 0.5f;

        int gameOverShowTime = 5000;

        List<int> cardList = new List<int>();
        List<int> cardTextureList = new List<int>();
        int cardList_X = 6;
        int cardList_Y = 3;

        Vector2 startPosition = new Vector2(450, 240);
        int offset_X = 130;
        int offset_Y = 120;

        bool choiceFlag = false;
        //bool answerChoiceFlag = false;
        bool updateCardFlag = false;

        //int targetFlag = 0;

        int score = 0;

        Random rnd = new Random();


        string basePath = "";
        string questionFilePath = "";
        List<string> questionList = new List<string>();
        int questionKind;
        Texture2D chineseQuestionTexture;
        Vector2 questionTexturePosition = new Vector2(110, 260);
        List<Texture2D> answerTextureList = new List<Texture2D>();
        List<Texture2D> dummyAnswerTextureList = new List<Texture2D>();

        int max_choose_answer = 13;
        int min_choose_answer = 8;
        int no_of_answer = 3;
        //int set_max_choose_answer = 13;
        //int set_min_choose_answer = 8;
        //int set_no_of_answer = 3;

        //List<int> keepAnswerList = new List<int>();
        //List<int> keepAnswerTextureList = new List<int>();
        //Vector2 answerStartPosition = new Vector2(100, 625);
        //int answerOffset = 200;

        Texture2D confirmTexture;
        Vector2 confirmTexturePosition = new Vector2(1080, 680);
        bool confirmFlag = false;
        float confirmButton_scale = 0.2f;

        Texture2D clearTexture;
        Vector2 clearTexturePosition = new Vector2(600, 680);
        bool clearFlag = false;
        float clearButton_scale = 0.2f;

        Texture2D answerShowTexture;
        Vector2 answerShowTexturePosition = new Vector2(570, 250);
        int answerShowTime = 0;
        bool answerJudgeFlag = false;

        Texture2D answer_frameTexture;
        Texture2D correctTexture;
        //Vector2 correctTexturePosition = new Vector2(100, 350);
        bool correctFlag = false;

        int stageNumber = 1;
        Texture2D stageNumberTexture;
        Texture2D exitTexture;

        int pickMethod = 0;
        Point pickPosition;
        Point pickPositionOffset = new Point(10, 10);
        int pickKeepTime = 100;

        public int level = 1;
        private int level_real = 1;

        Texture2D backgroundTexture;
        Texture2D counterTexture;
        Texture2D timeTexture;
        const float reference_lenght = 120;     //问题图片的预设大小
        int questionNumber = 1;
        Texture2D questionTexture;
        List<Texture2D> questionTextureList = new List<Texture2D>();      //用来存放单个字符图片
        Texture2D startTexture;
        Vector2 startTexturePosition = new Vector2(400, 300);

        public GameComponent_3(Game game)
            : base(game)
        {

        }


        public override void Initialize()
        {

            base.Initialize();
        }


        List<string> GetTextureNameBySearchFile(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> tempList = new List<string>();
            foreach (FileInfo fi in dir.GetFiles())
            {
                tempList.Add(fi.Name);           //get textures name by search file
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
                //if ("txt".Equals(dChild.Extension))
                //{
                    tempList.Add(dChild.Name);           //get file name by search file
                //}
            }
            if (tempList.Count <= 0)
            {
                //formMessage.Message_NOFile();    //show message
                //Exit();
            }
            return tempList;
        }

        Texture2D GetQuestionTexture(string path)
        {
            StringFormatImg outitext = new StringFormatImg("没有找到问题图片", "宋体", 14, System.Drawing.Color.Black);
            Texture2D questionTexture_temp = Texture2D.FromStream(Game.GraphicsDevice, outitext.Out2D());
            

            bool png_Flag = false;
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> tempList = new List<string>();
            foreach (FileInfo fi in dir.GetFiles())
            {
                tempList.Add(fi.Name);           //get textures name by search file

                if ((".png".Equals(fi.Extension)) || (".PNG".Equals(fi.Extension)))    //判断是否有图片类型的问题
                {
                    tempList[0] = fi.Name;
                    png_Flag = true;
                }
            }
            //if (png_Flag)
            //{
            //    questionTexture_temp = Texture2D.FromStream(GraphicsDevice, File.OpenRead(path + "\\" + tempList[0]));
            //}
            //else
            //{
            //    string qString = File.ReadAllText(path + "\\" + tempList[0], System.Text.Encoding.UTF8);
            //    StringFormatImg outitext = new StringFormatImg(qString, "宋体", 14, System.Drawing.Color.Pink);
            //    questionTexture_temp = Texture2D.FromStream(Game.GraphicsDevice, outitext.Out2D());       //将需要显示的中文，转换为一张图片
            //}

            if (png_Flag)
            {
                questionTexture_temp = Texture2D.FromStream(GraphicsDevice, File.OpenRead(path + "\\" + tempList[0]));
            }
            else
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path + "\\" + tempList[0]);

                XmlNode root = xmldoc.SelectSingleNode("Node");
                string qString = root["question"].InnerText;

                questionTextureList.Clear();
                for (int i = 0; i < qString.Length; i++)        //把单个字符做一张图片，保存到图片链表中
                {
                    outitext = new StringFormatImg(qString[i].ToString(), "宋体", 30, System.Drawing.Color.Black);
                    questionTextureList.Add(Texture2D.FromStream(Game.GraphicsDevice, outitext.Out2D()));
                }                
                outitext = new StringFormatImg(qString, "宋体", 14, System.Drawing.Color.Pink);
                questionTexture_temp = Texture2D.FromStream(Game.GraphicsDevice, outitext.Out2D());       //把整个字符串做成一张图片

                XmlNodeList levelNodeList = root.SelectSingleNode("levels").ChildNodes;

                //level_real = 8;

                if (int.Parse(levelNodeList[levelNodeList.Count - 1].Attributes["id"].Value) <= level_real)     //当前难度，超过了玩家设定的最大难度
                {
                    no_of_answer = int.Parse(levelNodeList[levelNodeList.Count - 1]["answerNumber"].InnerText);
                    max_choose_answer = int.Parse(levelNodeList[levelNodeList.Count - 1]["maxNumber"].InnerText);
                    min_choose_answer = int.Parse(levelNodeList[levelNodeList.Count - 1]["minNumber"].InnerText);
                }
                else                                                                                            //当前难度，还在玩家设定的难度范围以内的情况
                {
                    for (int i = 0; i < levelNodeList.Count; i++)
                    {
                        if (int.Parse(levelNodeList[i].Attributes["id"].Value) == level_real)
                        {
                            no_of_answer = int.Parse(levelNodeList[i]["answerNumber"].InnerText);
                            max_choose_answer = int.Parse(levelNodeList[i]["maxNumber"].InnerText);
                            min_choose_answer = int.Parse(levelNodeList[i]["minNumber"].InnerText);
                            break;
                        }
                        else if (int.Parse(levelNodeList[i].Attributes["id"].Value) > level_real)
                        {
                            no_of_answer = int.Parse(levelNodeList[i - 1]["answerNumber"].InnerText);
                            max_choose_answer = int.Parse(levelNodeList[i - 1]["maxNumber"].InnerText);
                            min_choose_answer = int.Parse(levelNodeList[i - 1]["minNumber"].InnerText);
                            break;
                        }
                    }
                }

                if (root["kind"].InnerText == "sort")
                {
                    questionKind = 1;
                }
                else
                {
                    questionKind = 0;
                }

            }

            return questionTexture_temp;
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
            //if (Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + Properties.DragAndDrop.Default.basePath))
            //{
            //    path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.DragAndDrop.Default.basePath + "\\";
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
            //        path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.DragAndDrop.Default.basePath + "\\";
            //    }
            //    else
            //    {
            //        path = resourcePath + Properties.DragAndDrop.Default.basePath + "\\";
            //    }
            //} 
            #endregion

            path = System.AppDomain.CurrentDomain.BaseDirectory + Properties.DragAndDrop.Default.basePath + "\\";

            return path;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            basePath = GetBasePath();
            questionFilePath = Properties.DragAndDrop.Default.questionFilePath + "\\";

            answerShowTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(basePath + Properties.DragAndDrop.Default.otherTextureFilePath + "\\answerShow.png"));
            confirmTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(basePath + Properties.DragAndDrop.Default.otherTextureFilePath + "\\confirm.png"));
            correctTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(basePath + Properties.DragAndDrop.Default.otherTextureFilePath + "\\correct.png"));
            clearTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(basePath + Properties.DragAndDrop.Default.otherTextureFilePath + "\\clear.png"));
            answer_frameTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead(basePath + Properties.DragAndDrop.Default.otherTextureFilePath + "\\answer_frame.png"));


            stageNumberTexture = Game.Content.Load<Texture2D>("Texture\\stageNumber");
            exitTexture = Game.Content.Load<Texture2D>("Texture\\exit");
            counterTexture = Game.Content.Load<Texture2D>("Texture\\StateBar");
            backgroundTexture = Game.Content.Load<Texture2D>("Texture\\card\\bg_b");
            timeTexture = Game.Content.Load<Texture2D>("Texture\\TimeBar");
            questionTexture = Game.Content.Load<Texture2D>("Texture\\card\\question");
            startTexture = Game.Content.Load<Texture2D>("Texture\\moleGame\\GameStart");

            word = Game.Content.Load<SpriteFont>("Font\\word");

            for (int i = 0; i < cardList_X * cardList_Y; i++)
            {
                cardList.Add(0);
                cardTextureList.Add(0);
            }
            //UpdateCardList();

            base.LoadContent();
        }


        void UpdateStage()
        {
            stageNumber = score / 10 + 1;

            level_real = level + stageNumber - 1;
            if (level_real > 10)
            {
                level_real = 10;
            }

            //if (questionKind == 0)
            //{
            //    if (level_real <= 5)
            //    {
            //        no_of_answer = level_real;
            //        max_choose_answer = 10 + no_of_answer;
            //        min_choose_answer = 5 + no_of_answer;
            //    }
            //    else
            //    {
            //        no_of_answer = set_no_of_answer;
            //        max_choose_answer = set_max_choose_answer;
            //        min_choose_answer = set_min_choose_answer;
            //    }
            //    answerOffset = 1000 / no_of_answer;
            //}
            //else //if (questionKind == 1)
            //{
            //    if (level_real <= 4)
            //    {
            //        no_of_answer = level_real + 1;
            //        max_choose_answer = no_of_answer;
            //        min_choose_answer = no_of_answer;
            //    }
            //    else
            //    {
            //        no_of_answer = 5;
            //        max_choose_answer = no_of_answer;
            //        min_choose_answer = no_of_answer;
            //    }
            //    answerOffset = 1000 / no_of_answer;
            //}

        }

        string GetQuestionPath()
        {
            string questionString = "";
            string questionPath = "";
            questionPath = basePath + questionFilePath;
            if (questionList.Count <= 0)
            {
                questionList = GetFileNameBySearchFile(questionPath);
            }

            int temp = rnd.Next(0, questionList.Count);
            questionString = questionList[temp];
            questionList.Remove(questionList[temp]);
            questionString = questionPath + questionString;

            return questionString;
        }

        void UpdateCardList()
        {
            string questionFileString = GetQuestionPath();

            UpdateStage();       //update stageNumber,and change the difficulty.

            chineseQuestionTexture = GetQuestionTexture(questionFileString);

            answerTextureList.Clear();
            dummyAnswerTextureList.Clear();

            // get answer texture list
            List<string> allAnswerTextureList = GetTextureNameBySearchFile(questionFileString + "\\answer");
            List<int> sortTextureList = new List<int>();
            List<int> randomTextureList = new List<int>();
            for (int i = 0; i < allAnswerTextureList.Count; i++)
            {
                randomTextureList.Add(1);   //初始时，所有图片都是没有使用过的
            }
            if (no_of_answer > allAnswerTextureList.Count)
            {
                no_of_answer = allAnswerTextureList.Count;  //防止有人把答案数设置成比图片数还要多的情况
            }
            for (int i = 0; i < no_of_answer; i++)
            {
                int temp = rnd.Next(0, randomTextureList.Count);
                while (randomTextureList[temp] == 0)    //直到选出没有使用过的图片，保证每张图片只被选中一次
                {
                    temp = rnd.Next(0, randomTextureList.Count);
                }
                answerTextureList.Add(Texture2D.FromStream(GraphicsDevice, File.OpenRead(questionFileString + "\\answer\\" + allAnswerTextureList[temp])));
                randomTextureList[temp] = 0;

                if (questionKind == 1)   //需要保证选取图片的顺序性
                {
                    sortTextureList.Add(temp);
                }

            }
            if (questionKind == 1)      //将图片排序
            {
                for (int i = 0; i < sortTextureList.Count; i++)
                {
                    for (int j = i + 1; j < sortTextureList.Count; j++)
                    {
                        if (sortTextureList[i] > sortTextureList[j])
                        {
                            int temp = sortTextureList[i];
                            Texture2D textureTemp = answerTextureList[i];
                            sortTextureList[i] = sortTextureList[j];
                            answerTextureList[i] = answerTextureList[j];
                            sortTextureList[j] = temp;
                            answerTextureList[j] = textureTemp;
                        }
                    }
                }
            }

            //get dummy answer texture list
            //if (questionKind == 0)
            //{
                int dummyAnswerNumber = rnd.Next(min_choose_answer, max_choose_answer + 1) - no_of_answer;
                List<string> allDummyAnswerTextureList = GetTextureNameBySearchFile(questionFileString + "\\dummyAnswer");
                if (dummyAnswerNumber > allDummyAnswerTextureList.Count)
                {
                    dummyAnswerNumber = allDummyAnswerTextureList.Count;  //防止有人把答案数设置成比图片数还要多的情况
                }
                for (int i = 0; i < dummyAnswerNumber; i++)
                {
                    int temp = rnd.Next(0, allDummyAnswerTextureList.Count);
                    dummyAnswerTextureList.Add(Texture2D.FromStream(GraphicsDevice, File.OpenRead(questionFileString + "\\dummyAnswer\\" + allDummyAnswerTextureList[temp])));
                    allDummyAnswerTextureList.Remove(allDummyAnswerTextureList[temp]);
                }
            //}

            List<int> randomList = new List<int>();
            List<int> randomAnswerList = new List<int>();
            List<int> randomDummyAnswerList = new List<int>();
            for (int i = 0; i < cardList_X * cardList_Y; i++)
            {
                if (i < answerTextureList.Count)
                {
                    randomList.Add(1);
                    randomAnswerList.Add(i);
                }
                else if (i < (answerTextureList.Count + dummyAnswerTextureList.Count))
                {
                    randomList.Add(2);
                    randomDummyAnswerList.Add(i - answerTextureList.Count);
                }
                else
                {
                    randomList.Add(0);
                }
                cardList[i] = 0;
                cardTextureList[i] = 0;
            }

            for (int i = 0; i < cardList_X * cardList_Y; i++)
            {
                int temp = rnd.Next(0, randomList.Count);
                cardList[i] = randomList[temp];
                if (randomList[temp] == 1)
                {
                    int temp2 = rnd.Next(0, randomAnswerList.Count);
                    cardTextureList[i] = randomAnswerList[temp2] + 1;
                    randomAnswerList.Remove(randomAnswerList[temp2]);
                }
                else if (randomList[temp] == 2)
                {
                    int temp2 = rnd.Next(0, randomDummyAnswerList.Count);
                    cardTextureList[i] = randomDummyAnswerList[temp2] + 1;
                    randomDummyAnswerList.Remove(randomDummyAnswerList[temp2]);
                }
                randomList.Remove(randomList[temp]);
            }

        }

        void DrawNumber(int number, int x, int y,int number_Width,int number_Height)
        {
            if (number > 0)
            {
                List<int> numberList = new List<int>();
                while (number > 0)
                {
                    numberList.Add(number % 10);
                    number = number / 10;
                }
                x = x + number_Width/2 * (numberList.Count - 1);
                for (int i = 0; i < numberList.Count; i++)
                {
                    spriteBatch.Draw(stageNumberTexture, new Rectangle(x - number_Width/2 * i, y, number_Width, number_Height),
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
                        UpdateCardList();
                    }

                    break;

                case GameState.play:

                    #region Exit button
                    Rectangle exitRect = new Rectangle((int)(exitPosition.X - exitTexture.Width * exitButton_scale),
                                                        (int)(exitPosition.Y - exitTexture.Height * exitButton_scale),
                                                     (int)(exitTexture.Width * exitButton_scale * 2), (int)(exitTexture.Height * exitButton_scale * 2));
                    if (exitRect.Contains(mousePosition) && (choiceFlag == false))
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
                    #endregion

                    #region Clear button
                    Rectangle clearRect = new Rectangle((int)(clearTexturePosition.X - clearTexture.Width * clearButton_scale),
                        (int)(clearTexturePosition.Y - clearTexture.Height * clearButton_scale),
                        (int)(clearTexture.Width * clearButton_scale * 2), (int)(clearTexture.Height * clearButton_scale * 2));
                    if ((clearRect.Contains(mousePosition)) && (questionKind == 0))
                    {
                        clearFlag = true;
                        if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                        {
                            for (int i = 0; i < cardList_X * cardList_Y; i++)
                            {
                                cardList[i] = cardList[i] % 10;
                            }
                        }
                    }
                    else
                    {
                        clearFlag = false;
                    }
                    #endregion

                    #region Confirm button
                    Rectangle confirmRect = new Rectangle((int)(confirmTexturePosition.X - confirmTexture.Width * confirmButton_scale), 
                        (int)(confirmTexturePosition.Y - confirmTexture.Height * confirmButton_scale),
                        (int)(confirmTexture.Width * confirmButton_scale * 2), (int)(confirmTexture.Height * confirmButton_scale * 2));
                    if ((confirmRect.Contains(mousePosition)) && (answerShowTime <= 0))
                    {
                        confirmFlag = true;
                        if ((mouseState.LeftButton == ButtonState.Released) && (pre_mouseState.LeftButton == ButtonState.Pressed))
                        {
                            updateCardFlag = true;
                            if (updateCardFlag)
                            {
                                answerJudgeFlag = true;
                                if (questionKind == 0)          //简单问题的结果判断
                                {
                                    int answerTemp = 0;
                                    for (int i = 0; i < cardList_X * cardList_Y; i++)
                                    {
                                        if (cardList[i] > 100)
                                        {
                                            if (cardList[i] % 10 == 1)
                                            {
                                                answerTemp++;
                                            }
                                            else
                                            {
                                                answerTemp = 0;
                                                break;
                                            }
                                        }
                                    }
                                    if (answerTemp < no_of_answer)
                                    {
                                        answerJudgeFlag = false;
                                        updateCardFlag = false;         //回答错误，不需要重新抽卡
                                        correctFlag = true;             //显示修改答案提示图片
                                        
                                        for (int i = 0; i < cardList_X * cardList_Y; i++)
                                        {
                                            cardList[i] = cardList[i] % 10;
                                        }
                                    }
                                    //for (int i = 0; i < no_of_answer; i++)
                                    //{
                                    //    if (cardList[i] != 1)
                                    //    {
                                    //        answerJudgeFlag = false;
                                    //        updateCardFlag = false;         //回答错误，不需要重新抽卡
                                    //        correctFlag = true;             //显示修改答案提示图片
                                    //        break;
                                    //    }
                                    //}
                                }
                                else //if (questionKind == 1)   //排序问题的结果判断
                                {
                                    for (int i = 0; i < no_of_answer - 1; i++)
                                    {
                                        if ((cardTextureList[i] > cardTextureList[i + 1]) || (cardList[i] != 1))
                                        {
                                            answerJudgeFlag = false;
                                            updateCardFlag = false;         //回答错误，不需要重新抽卡
                                            correctFlag = true;             //显示修改答案提示图片
                                            break;
                                        }
                                    }
                                    if (cardList[no_of_answer - 1] != 1)      //上面的for循环无法检查到最后这个点
                                    {
                                        answerJudgeFlag = false;
                                        updateCardFlag = false;         //回答错误，不需要重新抽卡
                                        correctFlag = true;             //显示修改答案提示图片
                                    }
                                }
                                if (answerJudgeFlag)
                                {
                                    score++;
                                }
                                answerShowTime = 3000;              //提示玩家答对或答错的图片显示时长
                            }
                        }
                    }
                    else
                    {
                        confirmFlag = false;
                    }
                    #endregion

                    #region Update card
                    if ((choiceFlag) && (questionKind == 1))
                    {
                        if ((mouseState.LeftButton == ButtonState.Released) && (pickMethod == 88))
                        {
                            if ((mousePosition.X > (pickPosition.X - pickPositionOffset.X)) && (mousePosition.X < (pickPosition.X + pickPositionOffset.X))
                             && (mousePosition.Y > (pickPosition.Y - pickPositionOffset.Y)) && (mousePosition.Y < (pickPosition.Y + pickPositionOffset.Y)))
                            {
                                pickMethod = 1;     //如果松开鼠标的位置，离拾取的位置几乎一样，那么玩家是在使用方式二
                            }
                            else
                            {
                                pickMethod = 0;
                            }
                        }

                        #region 鼠标已经选定了卡片的情况
                        if ((mouseState.LeftButton == ButtonState.Released && pickMethod == 0)
                            || (mouseState.LeftButton == ButtonState.Pressed && pickMethod == 1))
                        {
                            pickMethod = 89;
                            pickPosition = new Point(0, 0);

                            //if (choiceFlag)
                            //{
                                #region 放下问题区卡片
                                bool changeFlag = false;
                                int sourcePosition = 0;
                                int targetPosition = 0;
                                for (int i = 0; i < cardList_X * cardList_Y; i++)
                                {
                                    Rectangle rect = new Rectangle((int)startPosition.X + (i % cardList_X) * offset_X, (int)startPosition.Y + (i / cardList_X) * offset_Y, offset_X, offset_Y);
                                    if (rect.Contains(mousePosition))
                                    {
                                        targetPosition = i;
                                        changeFlag = true;      //找到了符合要求的区域，才需要调换卡片位置
                                    }
                                    if (cardList[i] > 100)
                                    {
                                        sourcePosition = i;
                                    }
                                }
                                if (changeFlag)
                                {
                                    int temp = cardList[sourcePosition] % 10;
                                    cardList[sourcePosition] = cardList[targetPosition];
                                    cardList[targetPosition] = temp;
                                    int cardTexture_temp = cardTextureList[sourcePosition];
                                    cardTextureList[sourcePosition] = cardTextureList[targetPosition];
                                    cardTextureList[targetPosition] = cardTexture_temp;
                                }
                                #endregion
                            //}

                            choiceFlag = false;
                            for (int i = 0; i < cardList_X * cardList_Y; i++)
                            {
                                cardList[i] = cardList[i] % 10;
                            }
                        }
                        #endregion
                    }
                    else if ((answerShowTime <= 0) && (correctFlag == false))
                    {
                        #region 鼠标没有选定卡片的情况
                        for (int i = 0; i < cardList_X * cardList_Y; i++)
                        {
                            if (cardList[i] != 0)
                            {
                                Rectangle rect = new Rectangle((int)startPosition.X + (i % cardList_X) * offset_X, (int)startPosition.Y + (i / cardList_X) * offset_Y, (int)reference_lenght, (int)reference_lenght);
                                if (rect.Contains(mousePosition))
                                {
                                    if (cardList[i] < 10)
                                    {
                                        cardList[i] = cardList[i] % 10 + 10;
                                    }
                                    if ((pre_mouseState.LeftButton == ButtonState.Released) && (mouseState.LeftButton == ButtonState.Pressed) && 
                                        ((pickMethod == 0) || (questionKind == 0)))
                                    {
                                        if (questionKind == 1)
                                        {
                                            choiceFlag = true;
                                            pickMethod = 88;
                                            pickPosition = mousePosition;       //记录下拾取坐标
                                            cardList[i] = cardList[i] % 10 + 100;
                                        }
                                        else if (questionKind == 0)
                                        {
                                            if (cardList[i] < 100)
                                            {
                                                cardList[i] = cardList[i] % 10 + 100;
                                            }
                                            else
                                            {
                                                cardList[i] = cardList[i] % 10;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if ((cardList[i] < 100) || (questionKind == 1))
                                    {
                                        cardList[i] = cardList[i] % 10;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    if (pickMethod == 89)
                    {
                        pickKeepTime -= gameTime.ElapsedGameTime.Milliseconds;
                        if (pickKeepTime <= 0)
                        {
                            pickKeepTime = 100;
                            pickMethod = 0;
                        }
                    }

                    if (answerShowTime > 0)
                    {
                        answerShowTime -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else if ((answerShowTime <= 0) && correctFlag && (mouseState.LeftButton == ButtonState.Pressed))
                    {
                        correctFlag = false;        //按下鼠标左键，结束修改答案提示图片的显示
                    }

                    if ((updateCardFlag) && (answerShowTime <= 0))
                    {
                        updateCardFlag = false;
                        UpdateCardList();
                        questionNumber++;
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

        //reference： 希望获得的图片最长边像素大小
        private float GetTextureScale(Texture2D texture, float reference)
        {
            float scale = 1;
            if (texture.Width >= texture.Height)
            {
                scale = reference / (float)texture.Width;
                return scale;
            }
            else //if (texture.Width < texture.Height)
            {
                scale = reference / (float)texture.Height;
                return scale;
            }
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

                    //if (exitFlag)       //exit button
                    //{
                    //    spriteBatch.Draw(exitTexture, exitPosition - new Vector2(exitTexture.Width, exitTexture.Height) * exitButton_scale*1.25f,
                    //        new Rectangle(0, 0, exitTexture.Width, exitTexture.Height), Color.White, 0, Vector2.Zero, exitButton_scale*2*1.25f, SpriteEffects.None, 1);
                    //}
                    //else
                    //{
                    //    spriteBatch.Draw(exitTexture, exitPosition - new Vector2(exitTexture.Width, exitTexture.Height) * exitButton_scale,
                    //        new Rectangle(0, 0, exitTexture.Width, exitTexture.Height), Color.White, 0, Vector2.Zero, exitButton_scale * 2, SpriteEffects.None, 1);
                    //}
                    if (clearFlag)    //clear button
                    {
                        spriteBatch.Draw(clearTexture, clearTexturePosition - new Vector2(clearTexture.Width, clearTexture.Height) * clearButton_scale * 1.25f,
                            new Rectangle(0, 0, clearTexture.Width, clearTexture.Height), Color.White, 0, Vector2.Zero, clearButton_scale * 2 * 1.25f, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.Draw(clearTexture, clearTexturePosition - new Vector2(clearTexture.Width, clearTexture.Height) * clearButton_scale,
                            new Rectangle(0, 0, clearTexture.Width, clearTexture.Height), Color.White, 0, Vector2.Zero, clearButton_scale * 2, SpriteEffects.None, 1);
                    }
                    if (confirmFlag)    //confirm button
                    {
                        spriteBatch.Draw(confirmTexture, confirmTexturePosition - new Vector2(confirmTexture.Width, confirmTexture.Height) * confirmButton_scale * 1.25f,
                            new Rectangle(0, 0, confirmTexture.Width, confirmTexture.Height), Color.White, 0, Vector2.Zero, confirmButton_scale * 2 * 1.25f, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.Draw(confirmTexture, confirmTexturePosition - new Vector2(confirmTexture.Width, confirmTexture.Height) * confirmButton_scale,
                            new Rectangle(0, 0, confirmTexture.Width, confirmTexture.Height), Color.White, 0, Vector2.Zero, confirmButton_scale * 2, SpriteEffects.None, 1);
                    }


                    #region draw card
                    for (int i = 0; i < cardList_X * cardList_Y; i++)
                    {
                        if (questionKind == 1)
                        {
                            if (i < no_of_answer)
                            {
                                int texture_ScaleMore = 5;
                                float texture_Scale = GetTextureScale(answer_frameTexture, reference_lenght + texture_ScaleMore * 2);
                                spriteBatch.Draw(answer_frameTexture,
                                    startPosition + new Vector2((i % cardList_X) * offset_X - texture_ScaleMore, (i / cardList_X) * offset_Y - texture_ScaleMore),
                                    new Rectangle(0, 0, answer_frameTexture.Width, answer_frameTexture.Height),
                                    Color.White, 0, Vector2.Zero, texture_Scale, SpriteEffects.None, 1);
                            }
                        }
                        if ((cardList[i] < 10) && (cardList[i] > 0))
                        {
                            if (cardList[i] == 1)
                            {
                                float texture_Scale = GetTextureScale(answerTextureList[cardTextureList[i] - 1], reference_lenght);
                                spriteBatch.Draw(answerTextureList[cardTextureList[i] - 1], 
                                    startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y),
                                    new Rectangle(0, 0, answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height),
                                    Color.White, 0, Vector2.Zero, texture_Scale, SpriteEffects.None, 1);
                            }
                            else if (cardList[i] == 2)
                            {
                                float texture_Scale = GetTextureScale(dummyAnswerTextureList[cardTextureList[i] - 1], reference_lenght);
                                spriteBatch.Draw(dummyAnswerTextureList[cardTextureList[i] - 1], 
                                    startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y),
                                    new Rectangle(0, 0, dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height),
                                    Color.White, 0, Vector2.Zero, texture_Scale, SpriteEffects.None, 1);
                            }
                            //spriteBatch.Draw(cardTexture[cardList[i] % 10 - 1], startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y), Color.White);
                        }
                    }
                    for (int i = 0; i < cardList_X * cardList_Y; i++)
                    {
                        if ((cardList[i] < 100) && (cardList[i] >= 10))
                        {
                            if ((cardList[i] % 10) == 1)
                            {
                                float texture_Scale = GetTextureScale(answerTextureList[cardTextureList[i] - 1], reference_lenght);
                                spriteBatch.Draw(answerTextureList[cardTextureList[i] - 1],
                                    startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y)
                                    - new Vector2(answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height) * texture_Scale / 4,
                                    new Rectangle(0, 0, answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height),
                                    Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                            }
                            else if ((cardList[i] % 10) == 2)
                            {
                                float texture_Scale = GetTextureScale(dummyAnswerTextureList[cardTextureList[i] - 1], reference_lenght);
                                spriteBatch.Draw(dummyAnswerTextureList[cardTextureList[i] - 1],
                                    startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y)
                                    - new Vector2(dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height) * texture_Scale / 4,
                                    new Rectangle(0, 0, dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height),
                                    Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                            }
                            //spriteBatch.Draw(cardTexture[cardList[i] % 10 - 1], startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y) - new Vector2(cardTexture[cardList[i] % 10 - 1].Width, cardTexture[cardList[i] % 10 - 1].Height) / 2, new Rectangle(0, 0, cardTexture[cardList[i] % 10 - 1].Width, cardTexture[cardList[i] % 10 - 1].Height), Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1);
                        }
                    }
                    for (int i = 0; i < cardList_X * cardList_Y; i++)       //in order to draw the choice card on the top
                    {
                        if (cardList[i] > 100)
                        {
                            if (questionKind == 1)
                            {
                                if ((cardList[i] % 10) == 1)
                                {
                                    float texture_Scale = GetTextureScale(answerTextureList[cardTextureList[i] - 1], reference_lenght);
                                    spriteBatch.Draw(answerTextureList[cardTextureList[i] - 1],
                                        new Vector2(mousePosition.X, mousePosition.Y)
                                        - new Vector2(answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height) * texture_Scale * 0.75f,
                                        new Rectangle(0, 0, answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height),
                                        Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                                }
                                else if ((cardList[i] % 10) == 2)
                                {
                                    float texture_Scale = GetTextureScale(dummyAnswerTextureList[cardTextureList[i] - 1], reference_lenght);
                                    spriteBatch.Draw(dummyAnswerTextureList[cardTextureList[i] - 1],
                                        new Vector2(mousePosition.X, mousePosition.Y)
                                        - new Vector2(dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height) * texture_Scale * 0.75f,
                                        new Rectangle(0, 0, dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height),
                                        Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                                }
                            }
                            else //if (questionKind == 0)
                            {
                                if ((cardList[i] % 10) == 1)
                                {
                                    float texture_Scale = GetTextureScale(answerTextureList[cardTextureList[i] - 1], reference_lenght);
                                    spriteBatch.Draw(answerTextureList[cardTextureList[i] - 1],
                                        startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y)
                                        - new Vector2(answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height) * texture_Scale / 4,
                                        new Rectangle(0, 0, answerTextureList[cardTextureList[i] - 1].Width, answerTextureList[cardTextureList[i] - 1].Height),
                                        Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                                }
                                else if ((cardList[i] % 10) == 2)
                                {
                                    float texture_Scale = GetTextureScale(dummyAnswerTextureList[cardTextureList[i] - 1], reference_lenght);
                                    spriteBatch.Draw(dummyAnswerTextureList[cardTextureList[i] - 1],
                                        startPosition + new Vector2((i % cardList_X) * offset_X, (i / cardList_X) * offset_Y)
                                        - new Vector2(dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height) * texture_Scale / 4,
                                        new Rectangle(0, 0, dummyAnswerTextureList[cardTextureList[i] - 1].Width, dummyAnswerTextureList[cardTextureList[i] - 1].Height),
                                        Color.White, 0, Vector2.Zero, texture_Scale * 1.5f, SpriteEffects.None, 1);
                                }
                            }
                            //spriteBatch.Draw(cardTexture[cardList[i] % 10 - 1], new Vector2(mousePosition.X, mousePosition.Y) - new Vector2(cardTexture[cardList[i] % 10 - 1].Width, cardTexture[cardList[i] % 10 - 1].Height), new Rectangle(0, 0, cardTexture[cardList[i] % 10 - 1].Width, cardTexture[cardList[i] % 10 - 1].Height), Color.White, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 1);                            
                        }
                    }

                    #endregion

                    DrawNumber(score, 210, 48, 50, 60);
                    DrawNumber(stageNumber, 600, 48, 50, 60);

                    spriteBatch.Draw(questionTexture, new Rectangle((int)questionTexturePosition.X, (int)questionTexturePosition.Y, 130, 75), new Rectangle(0, 0, questionTexture.Width, questionTexture.Height), Color.White);
                    DrawNumber(questionNumber, (int)(questionTexturePosition.X + 115), (int)questionTexturePosition.Y - 20, 100, 120);
                    //spriteBatch.Draw(chineseQuestionTexture, questionTexturePosition, Color.Black);    //draw question
                    for (int i = 0; i < questionTextureList.Count; i++)
                    {
                        //每行7个字符
                        int lenght = 7;
                        spriteBatch.Draw(questionTextureList[i], questionTexturePosition + new Vector2(-60 + 45 * (i % lenght), 100 + 40 * (i / lenght)), Color.Black);
                    }


                    if (answerShowTime > 0)
                    {
                        int temp_width = 300;
                        int temp_height = temp_width * (answerShowTexture.Height / answerShowTexture.Width) / 2;
                        if (answerJudgeFlag)
                        {
                            spriteBatch.Draw(answerShowTexture, new Rectangle((int)answerShowTexturePosition.X, (int)answerShowTexturePosition.Y, temp_width, temp_height), new Rectangle(0, 0, answerShowTexture.Width, answerShowTexture.Height / 2), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(answerShowTexture, new Rectangle((int)answerShowTexturePosition.X, (int)answerShowTexturePosition.Y, temp_width, temp_height), new Rectangle(0, answerShowTexture.Height / 2, answerShowTexture.Width, answerShowTexture.Height / 2), Color.White);
                        }
                    }
                    else //if (answerShowTime <= 0)
                    {
                        if (correctFlag)
                            spriteBatch.Draw(correctTexture, answerShowTexturePosition - new Vector2((correctTexture.Width - answerShowTexture.Width) / 2, 0), Color.White);
                    }

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
