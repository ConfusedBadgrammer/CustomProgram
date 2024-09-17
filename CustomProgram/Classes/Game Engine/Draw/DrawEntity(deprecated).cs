//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using MonoGame.Extended;
//using System.IO;

//namespace SpaceDefenders
//{
//    // Overloaded Draw methods and Animate method
//    public class DrawEntity
//    {
//        // Contains overloaded draw functions
//        public DrawEntity(GraphicsDevice graphics, SpriteBatch spriteBatch) : base(graphics, spriteBatch) { }

//        public void Draw(DrawingType drawingType, Vector2 entityPosition)
//        {
//            string imagePath = _resourceManager.QueryDirectory(drawingType);
//            using FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read);
//            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);
//            _spriteBatch.Draw(texture, entityPosition, Color.White);
//        }

//        //Draw with scale
//        public void Draw(DrawingType drawingType, Vector2 entityPosition, float scale)
//        {
//            string imagePath = _resourceManager.QueryDirectory(drawingType);
//            using FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read);
//            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);

//            Rectangle scaledImage = ImageScaler(entityPosition, texture.Width, texture.Height, scale);

//            _spriteBatch.Draw(texture, scaledImage, Color.White);
//        }

//        // Draw with rotation per tick and scale
//        public void Draw(DrawingType drawingType, Vector2 entityPosition, float rotationAngle, float scale, GameTime gameTime)
//        {
//            string imagePath = _resourceManager.QueryDirectory(drawingType);
//            using FileStream fileStream = new(imagePath, FileMode.Open, FileAccess.Read);
//            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);

//            Rectangle scaledImage = ImageScaler(entityPosition, texture.Width, texture.Height, scale);
//            // Rotation draw messes with collision, offsets are added to fix
//            scaledImage.X += 35;
//            scaledImage.Y += 25;

//            Vector2 rotationOrigin = new(texture.Width / 2, texture.Height / 2);

//            _spriteBatch.Draw(texture, scaledImage, null, Color.White, rotationAngle, rotationOrigin, SpriteEffects.None, 0f);
//        }

//        // Animate the entities based of DrawingType
//        public void AnimateEntity(Animation animation, Vector2 position, float scale)
//        {
//            if (animation.DrawingType == DrawingType.Explosion)
//            {
//                Texture2D _spriteSheet = LoadSpritesheet(_resourceManager.QueryDirectory(animation.DrawingType));
//                Rectangle subImage = animation.CalculateSourceRectangle();
//                Rectangle scaledSubImage = ImageScaler(position, subImage.Width, subImage.Height, scale);

//                _spriteBatch.Draw(_spriteSheet, scaledSubImage, subImage, Color.White);
//            }

//            if (animation.DrawingType == DrawingType.Earth)
//            {
//                Texture2D _spriteSheet = LoadSpritesheet(_resourceManager.QueryDirectory(animation.DrawingType));
//                Rectangle subImage = animation.CalculateSourceRectangle();
//                Rectangle scaledSubImage = ImageScaler(position, subImage.Width, subImage.Height, scale);
//                Color transparentColor = new Color(255, 255, 255, 0); // Transparent color with alpha = 0

//                _spriteBatch.Draw(_spriteSheet, scaledSubImage, subImage, Color.White);
//            }
//        }

//        public void DrawRectangle(float xPos, float yPos, float width, float height, Color colour)
//        {
//            // Uses MonoGame.Extended NuGet package for shape functions
//            _spriteBatch.DrawRectangle(xPos, yPos, width, height, colour, 2);
//        }

//        // Load the file directory provided by resource manager
//        private Texture2D LoadSpritesheet(string directory)
//        {
//            using FileStream fileStream = new(directory, FileMode.Open, FileAccess.Read);
//            Texture2D _spriteSheet = Texture2D.FromStream(_graphicsDevice, fileStream);
//            return _spriteSheet;
//        }

//        // Used to scale the images in the draw functions
//        public static Rectangle ImageScaler(Vector2 position, int width, int height, float scale)
//        {
//            Rectangle subImage = new(
//            (int)position.X,
//            (int)position.Y,
//            (int)(width * scale),
//            (int)(height * scale)
//        );
//            return subImage;
//        }
//    }
//}
