using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
// TODO: Read in values
namespace SpaceDefenders
{
    // Responsible for loading in the location configFile (a json file which contains a key and pair(string and string)
    // Reponsible for returning directories based on drawingType - Used only by resource manager
    public class EntityDirectoryDictionary
    {
        private Dictionary<DrawingType, string> drawingDirectories = new Dictionary<DrawingType, string>();

        public EntityDirectoryDictionary()
        {
            LoadDirectoryMapping();
        }

        public string GetDirectory(DrawingType drawingType)
        {
            if (drawingDirectories.ContainsKey(drawingType))
            {
                return drawingDirectories[drawingType];
            }
            else
            {
                throw new ArgumentException($"Drawing type {drawingType} is not found");
            }
        }
        // Loops through each key-value pair in directoryMapping (.json file) and attempts to parse kvp.Key (string) into Enum value type DrawingType
        // If successful, assign the Key to the Key Value
        private void LoadDirectoryMapping()
        {
            // Config file for directories to entities
            string configFile = "JSON/drawingDirectories.json";

            // If the file exists run:
                // The json file at location configFile is read
                // the json file is deserialized as <string, string> or KEY and PAIR into a dictionary and named directoryMapping
                // For each of the KEY and PAIR in the directoryMapping, attempt to parse the string kvp.Key to each Enum, if Enum == kvp.Key
                    // Assocaite the Key.Value to the drawType (the directory to the Key.Value)
            if (File.Exists(configFile))
            {
                string json = File.ReadAllText(configFile);
                Dictionary<string, string> directoryMapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                foreach (KeyValuePair<string, string> kvp in directoryMapping)
                {
                    if (Enum.TryParse(kvp.Key, out DrawingType drawingType))
                    {
                        drawingDirectories[drawingType] = kvp.Value;
                    }
                }
            }
        }
    }
}
