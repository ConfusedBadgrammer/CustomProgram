using Newtonsoft.Json;
using SpaceDefenders;
using System;
using System.Collections.Generic;
using System.IO;

public class SoundDirectoryDictionary
{
    // Similar to that of DrawingType but for SoundTypes
    private Dictionary<SoundType, string> _soundDirectories = new Dictionary<SoundType, string>();

    public SoundDirectoryDictionary()
    {
        LoadDirectoryMapping();
    }

    public string GetDirectory(SoundType soundType)
    {
        if (_soundDirectories.ContainsKey(soundType))
        {
            return _soundDirectories[soundType];
        }
        else
        {
            throw new ArgumentException($"Sound type {soundType} is not found");
        }
    }

    private void LoadDirectoryMapping()
    {
        // Error handling for Demonstration purposes
        // Same as EntityDirectoryDictionary LoadDirectoryMapping
        try
        {
            string configFile = "JSON/soundDirectories.json";

            if (File.Exists(configFile))
            {
                string json = File.ReadAllText(configFile);
                Dictionary<string, string> directoryMapping = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                foreach (KeyValuePair<string, string> kvp in directoryMapping)
                {
                    if (Enum.TryParse(kvp.Key, out SoundType soundType))
                    {
                        _soundDirectories[soundType] = kvp.Value;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid SoundType: {kvp.Key}");
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Sound directory mapping file not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading sound directory mapping: {ex.Message}");
            throw;
        }
    }
}
