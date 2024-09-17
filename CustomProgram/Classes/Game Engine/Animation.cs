using Microsoft.Xna.Framework;

namespace SpaceDefenders
{
    public class Animation
    {
        private int _maxAnimationFrames;
        private int _ticksPerFrame;
        private int _currentAnimationFrame;
        private int _currentTickCount;
        private int _frameHeight;
        private int _frameWidth;
        private int _framesPerRow;
        private DrawingType _drawingType;

        public Animation(DrawingType drawingType, int maxAnimationFrames, int ticksPerFrame, int frameWidth, int frameHeight, int framesPerRow)
        {
            _drawingType = drawingType;
            _maxAnimationFrames = maxAnimationFrames;
            _ticksPerFrame = ticksPerFrame;
            _currentAnimationFrame = 0;
            _currentTickCount = 0;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _framesPerRow = framesPerRow;
        }

        public DrawingType DrawingType
        {
            get
            {
                return _drawingType;
            }
        }

        // Update the frames
        public bool UpdateFrames()
        {
            _currentTickCount++;
            if (_currentTickCount >= _ticksPerFrame)
            {
                _currentTickCount = 0;
                _currentAnimationFrame++;
                if (_currentAnimationFrame >= _maxAnimationFrames)
                {
                    _currentAnimationFrame = 0; // Reset to the first frame
                    return true;    // Animation finished
                }
            }
            return false;   // Animation not finished
        }

        // MonoGame lacks spritesheet methods, used to subimage a spritesheet
        public Rectangle CalculateSpriteSubImage()
        {
            int subImageX = (_currentAnimationFrame % _framesPerRow) * _frameWidth; // Calculate remainder of frames in the row (e.g. 7 % 4 = 3 or 11 % 4, we are in the third frame of the row)
            int subImageY = (_currentAnimationFrame / _framesPerRow) * _frameHeight; // Calculate the current row (because ints in C# always round down 3/4 = 0 and not 0.75, only when 4/4 we mov eon to the next row)
                                                                                            // if we have 5/4 it will still round to 1, allowing us to move onto the next row only when fully divisible, allowing us to set when to move to next row

            if (_currentAnimationFrame >= _maxAnimationFrames)
            {
                _currentAnimationFrame = 0; // Reset to the first frame
            }

            return new Rectangle(subImageX, subImageY, _frameWidth, _frameHeight);
        }
    }
}
