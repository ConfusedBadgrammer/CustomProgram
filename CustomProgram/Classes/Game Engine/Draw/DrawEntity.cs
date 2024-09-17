using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
// DrawEntity V.2, Implements the use of PRELOADING textures instead of loading in each time draw is called (Allows for reusability of a single preloaded texture) - Removed lag issue
// Contains overloaded Draw and Animation functions (rotation, scale)
namespace SpaceDefenders
{
    public class DrawEntity : Drawing
    {
        private Dictionary<DrawingType, Texture2D> preloadedTextures = new Dictionary<DrawingType, Texture2D>();
        public DrawEntity(GraphicsDevice graphics, SpriteBatch spriteBatch) : base(graphics, spriteBatch)
        {
            PreloadTextures();
        }
        // For each drawingtype in DrawingType Enum we query our Resouce Manager for the directory, open the file using file stream, and assign the texture to the specific drawType
            // Allows all entities to use one texture associated with the specific drawingType
        public void PreloadTextures()
        {
            foreach (DrawingType drawingType in Enum.GetValues(typeof(DrawingType)))
            {
                string imagePath = _resourceManager.QueryDirectory(drawingType);
                using FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read);
                Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);
                preloadedTextures[drawingType] = texture;
            }
        }

        // Draw texture
        public void Draw(DrawingType drawingType, Vector2 entityPosition)
        {
            if (preloadedTextures.TryGetValue(drawingType, out Texture2D texture))
            {
                _spriteBatch.Draw(texture, entityPosition, Color.White);
            }
        }

        // Draw texture with scale
        public void Draw(DrawingType drawingType, Vector2 entityPosition, float scale)
        {
            if (preloadedTextures.TryGetValue(drawingType, out Texture2D texture))
            {
                Rectangle scaledImage = ImageScaler(entityPosition, texture.Width, texture.Height, scale);
                _spriteBatch.Draw(texture, scaledImage, Color.White);
            }
        }

        // Draw with rotation and scale
        public void Draw(DrawingType drawingType, Vector2 entityPosition, float rotationAngle, float scale)
        {
            if (preloadedTextures.TryGetValue(drawingType, out Texture2D texture))
            {
                Rectangle scaledImage = ImageScaler(entityPosition, texture.Width, texture.Height, scale);
                scaledImage.X += 35;
                scaledImage.Y += 25;
                Vector2 rotationOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                _spriteBatch.Draw(texture, scaledImage, null, Color.White, rotationAngle, rotationOrigin, SpriteEffects.None, 0f);
            }
        }

        // Draw Animation
        public void AnimateEntity(Animation animation, Vector2 position, float scale)
        {
            if (preloadedTextures.TryGetValue(animation.DrawingType, out Texture2D _spriteSheet))
            {
                Rectangle subImage = animation.CalculateSpriteSubImage();
                Rectangle scaledSubImage = ImageScaler(position, subImage.Width, subImage.Height, scale);
                _spriteBatch.Draw(_spriteSheet, scaledSubImage, subImage, Color.White);
            }
        }
        // Draw Animation with rotation
        public void AnimateEntity(Animation animation, Vector2 position, float scale, float rotation)
        {
            if (preloadedTextures.TryGetValue(animation.DrawingType, out Texture2D _spriteSheet))
            {
                Rectangle subImage = animation.CalculateSpriteSubImage();
                Rectangle scaledSubImage = ImageScaler(position, subImage.Width, subImage.Height, scale);
                _spriteBatch.Draw(_spriteSheet, scaledSubImage, subImage, Color.White, rotation, new Vector2(0,0), SpriteEffects.None, 0.5f);
            }
        }

        // Draw Rectangle (solid)
        public void DrawRectangle(float xPos, float yPos, float width, float height, Color colour)
        {
            // Uses MonoGame.Extended NuGet package for shape functions
            _spriteBatch.DrawRectangle(xPos, yPos, width, height, colour, 2);
        }

        // Draw rectangle with 1 pixel texture - Monogame lacks the ability to draw a hollow rectangle with opacity
        public void DrawSolidRectangle(int x, int y, int width, int height, Color colour, float opacity)
        {
            Texture2D pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { colour });

            _spriteBatch.Draw(pixelTexture, new Rectangle(x, y, width, height), colour * opacity);
        }

        // Used to scale the image before drawing - Used by the draw/animation functions which scale
        public static Rectangle ImageScaler(Vector2 position, int width, int height, float scale)
        {
            Rectangle subImage = new(
                (int)position.X,
                (int)position.Y,
                (int)(width * scale),
                (int)(height * scale)
            );
            return subImage;
        }
    }
}
