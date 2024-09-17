using Newtonsoft.Json;
using System;
using System.IO;
using System.Numerics;
// James recommends reading in the value
// TODO:  Read in the values
namespace SpaceDefenders
{
    public static class GameConstants
    {
        // Accessible to all classes in the program
        public static int ScreenWidth { get; private set; }
        public static int ScreenHeight { get; private set; }
        public static string GameName { get; private set; }
        public static int MaxProjectiles { get; private set; }
        public static int MaxEnemies { get; private set; }
        public static int MaxPowerUps { get; private set; }
        public static int MaxExplosion { get; private set; }
        public static int MaxHit { get; private set; }
        public static int MaxThrusters { get; private set; }
        public static bool isAlly { get; private set; }
        public static bool isEnemy { get; private set; }
        public static bool isNeutral { get; private set; }
        public static int PlayerWidth { get; private set; }
        public static int PlayerHeight { get; private set; }
        public static int PlayerInitialHealth { get; private set; }
        public static Vector2 PlayerSpawnPosition { get; private set; }

        static GameConstants()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            // Loads .json file, deserialises the json file to type Configuration then assigns the values to the public static variables
            try
            {
                string configFile = "JSON/gameconfig.json";

                if (File.Exists(configFile))
                {
                    string json = File.ReadAllText(configFile);
                    Configuration config = JsonConvert.DeserializeObject<Configuration>(json);

                    if (config != null)
                    {
                        ScreenWidth = config.ScreenWidth;
                        ScreenHeight = config.ScreenHeight;
                        GameName = config.GameName;
                        MaxProjectiles = config.MaxProjectiles;
                        MaxEnemies = config.MaxEnemies;
                        MaxPowerUps = config.MaxPowerUps;
                        MaxExplosion = config.MaxExplosion;
                        MaxHit = config.MaxHit;
                        MaxThrusters = config.MaxThrusters;
                        isAlly = config.isAlly;
                        isEnemy = config.isEnemy;
                        isNeutral = config.isNeutral;
                        PlayerWidth = config.PlayerWidth;
                        PlayerHeight = config.PlayerHeight;
                        PlayerInitialHealth = config.PlayerInitialHealth;
                        PlayerSpawnPosition = config.PlayerSpawnPosition;
                    }
                }
                else
                {
                    throw new FileNotFoundException("Configuration file not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                throw;
            }
        }

        private class Configuration
        {
            // Inaccessible, Using a seperate class allows us to match property names and easier mapping of JSON keys to class properties during deserialisation
            public int ScreenWidth { get; set; }
            public int ScreenHeight { get; set; }
            public string GameName { get; set; }
            public int MaxProjectiles { get; set; }
            public int MaxEnemies { get; set; }
            public int MaxPowerUps { get; set; }
            public int MaxExplosion { get; set; }
            public int MaxHit { get; set; }
            public int MaxThrusters { get; set; }
            public bool isAlly { get; set; }
            public bool isEnemy { get; set; }
            public bool isNeutral { get; set; }
            public int PlayerWidth { get; set; }
            public int PlayerHeight { get; set; }
            public int PlayerInitialHealth { get; set; }
            public Vector2 PlayerSpawnPosition { get; set;}
        }
    }
}
