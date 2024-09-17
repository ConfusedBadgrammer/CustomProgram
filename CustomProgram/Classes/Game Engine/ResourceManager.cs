namespace SpaceDefenders
{
    public class ResourceManager
    {
        private readonly SoundDirectoryDictionary _soundDictionary;
        private readonly EntityDirectoryDictionary _entityDictionary;
        public ResourceManager()
        {
             _soundDictionary = new SoundDirectoryDictionary();
             _entityDictionary = new EntityDirectoryDictionary();

        }
        // Returns directories for SoundTypes
        public string QueryDirectory(SoundType soundType)
        {
           string directory = _soundDictionary.GetDirectory(soundType);
           return directory;
        }
        // Returns directories for DrawingTypes
        public string QueryDirectory(DrawingType drawingType)
        {
            string directory = _entityDictionary.GetDirectory(drawingType);
            return directory;
        }

    }
}
