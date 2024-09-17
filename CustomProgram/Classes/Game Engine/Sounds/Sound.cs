using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpaceDefenders
{
    public class Sound
    {
        private Dictionary<SoundType, SoundEffect> _preloadedSounds = new Dictionary<SoundType, SoundEffect>();
        private ResourceManager _resourceManager;

        public Sound()
        {
            _resourceManager = new ResourceManager();
            PreloadSounds();
        }
        // Foreach value in SoundType Enums file, Query the resource manager for directory, Use filestream to open file and assign to SoundEffect and assign SoundType Enum as Key and sound to Value.
        public void PreloadSounds()
        {
            foreach (SoundType soundType in Enum.GetValues(typeof(SoundType)))
            {
                string soundPath = _resourceManager.QueryDirectory(soundType);
                using FileStream fileStream = new(soundPath, FileMode.Open, FileAccess.Read);
                SoundEffect sound = SoundEffect.FromStream(fileStream);
                _preloadedSounds[soundType] = sound;
            }
        }
        // Play sound with volume control by passing in desired SoundType
        public void PlaySound(SoundType soundType, float volume)
        {
            if (_preloadedSounds.TryGetValue(soundType, out SoundEffect sound))
            {
                sound.Play(volume, 0f, 0f);
            }
        }
    }
}
