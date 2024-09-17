using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceDefenders.Classes
{
    // Responsible for drawing background
    public class DrawBackground : Drawing
    {
        private Texture2D background;
        private Random _randomGenerator;
        private string _currentBackground;
        private string[] _backgroundOptions;
        public DrawBackground(GraphicsDevice graphics, SpriteBatch spriteBatch) : base(graphics, spriteBatch)
        {
            _randomGenerator = new Random();
            _backgroundOptions = new string[]
            {

                "background",
                "gamestartbackground",
                "gamestartbackground1",
                "gamestartbackground2",
                "gamestartbackground3",
                "gamestartbackground4"
            };

            // Set the initial background when the class is instantiated
            _currentBackground = "background";
        }

        // Monogames pipeline function instead of my own content loader is used for demonstration purposes
        public void Draw(ContentManager content)
        {
            _graphicsDevice.Clear(Color.Black);
            background = content.Load<Texture2D>(_currentBackground);

            _spriteBatch.Draw(background, Vector2.Zero, Color.White);
        }
        // Randomises background when called
        public void RandomiseBackground(ContentManager content)
        {
            // Generate a random index to select a background
            int randomIndex = _randomGenerator.Next(0, _backgroundOptions.Length);
            _currentBackground = _backgroundOptions[randomIndex];
        }

        public void SetDefaultBackground(ContentManager content)
        {
            // Set the default background (change the filename as needed)
            _currentBackground = "background";
        }
    }
}
