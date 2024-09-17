using System.IO;

namespace SpaceDefenders
{
    // Used to save Score - can be expanded to save levels passed, upgrades etc. in the future
    public static class SaveAndLoad
    {
        private const string HighScoreFileName = "highscore.txt";

        public static int LoadHighScore()
        {
            int highScore = 0;

            if (File.Exists(HighScoreFileName))
            {
                string highScoreText = File.ReadAllText(HighScoreFileName);
                if (int.TryParse(highScoreText, out highScore))
                {
                    return highScore;
                }
            }

            return highScore;
        }

        public static void SaveHighScore(int highScore)
        {
            File.WriteAllText(HighScoreFileName, highScore.ToString());
        }
    }
}
