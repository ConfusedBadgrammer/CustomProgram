using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefenders
{
    // MonoGame objects necessary for all drawing children to use
    // Implementation of my own drawing methods (More code based)
    // Mention the implementation of own drawing functions rather than using Monogame's content pipeline 
    public abstract class Drawing
    {
        protected ResourceManager _resourceManager;
        protected GraphicsDevice _graphicsDevice;
        protected SpriteBatch _spriteBatch;

        public Drawing(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _resourceManager = new ResourceManager();
        }
    }
}
