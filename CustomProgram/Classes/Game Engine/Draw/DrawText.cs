using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefenders
{
    // Used to create an object capable of drawing text on the screen - font assignable with the use of Monogame's content pipeline
    public class DrawText : Drawing
    {
        private SpriteFont _font;

        // Loads the font needed (.xnb file generated using monogame's content pipeline)
        public DrawText(string font, ContentManager content, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) : base(graphicsDevice, spriteBatch)
        {
            _font = content.Load<SpriteFont>(font);
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;

        }

        // DrawString based on texts centre
        public void DrawString(string textToDraw, int xPos, int yPos, float scale)
        {
            Vector2 textPosition = new Vector2(xPos, yPos);
            Vector2 textMiddlePoint = _font.MeasureString(textToDraw) / 2;

            _spriteBatch.DrawString(_font, textToDraw, textPosition, Color.White, 0, textMiddlePoint, scale, SpriteEffects.None, 1.0f);
        }
    }
}
