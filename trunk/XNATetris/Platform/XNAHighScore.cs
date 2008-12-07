using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;

namespace XNATetris
{
    [Serializable]
    public struct HighScoreData
    {
        public string[] PlayerName;
        public int[] Score;

        public int Count;

        public HighScoreData(int count)
        {
            PlayerName = new string[count];
            Score = new int[count];
            Count = count;
        }
    }

    class XNAHighScore
    {

        public readonly string HighScoresFilename = "highscores.lst";

        private HighScoreData currentTopScores;
        public HighScoreData CurrentTopScores
        {
            get { return currentTopScores; }
            private set { currentTopScores = value; }
        }
        public XNAHighScore()
        {
            init(false);
        }

        private void init(bool forceNew)
        {
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, HighScoresFilename);

            // Check to see if the save exists
            if (!File.Exists(fullpath) || forceNew)
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                int top10 = 10;
                HighScoreData data = new HighScoreData(top10);
                for (int i = 0; i < top10; i++)
                {
                    data.PlayerName[i] = "NotValid";
                    data.Score[i] = -1;
                }

                SaveHighScores(data, HighScoresFilename);
            }
            else
            {
                CurrentTopScores = LoadHighScores(fullpath);
            }
        }

        public bool IsHighScore(String player, int score)
        {
            // Create the data to save
            HighScoreData data = LoadHighScores(HighScoresFilename);

            int scoreIndex = -1;
            for (int i = 0; i < data.Count; i++)
            {
                if (score > data.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //New high score found ... do swaps
                for (int i = data.Count - 1; i > scoreIndex; i--)
                {
                    data.PlayerName[i] = data.PlayerName[i - 1];
                    data.Score[i] = data.Score[i - 1];
                }

                data.PlayerName[scoreIndex] = player; //Retrieve User Name Here
                data.Score[scoreIndex] = score;

                CurrentTopScores = data;
                SaveHighScores(data, HighScoresFilename);

                return true;
            }

            return false;
        }
 
        private static void SaveHighScores(HighScoreData data, string filename)
        {
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Convert the object to XML data and put it in the stream
            XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
            // Open the file, creating it if necessary
            Stream stream = File.Open(fullpath, FileMode.Create, FileAccess.Write, FileShare.Write);
            serializer.Serialize(stream, data);
            stream.Close();
        }

        private static HighScoreData LoadHighScores(string filename)
        {
            HighScoreData data;

            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.Open,FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
        }

    }
}
