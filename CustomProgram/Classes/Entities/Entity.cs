using Microsoft.Xna.Framework;

namespace SpaceDefenders
{
    public abstract class Entity
    {
        private Vector2 _position;
        private int _width;
        private int _height;
        private bool _team;
        private DrawingType _drawingType;

        //Entity properties
        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value; 
            }
        }

        public bool Team
        {
            get
            {
                return _team;
            }
           
            set
            {
                _team = value;
            }
        }

        public DrawingType DrawingType
        {
            get 
            {    
                return _drawingType; 
            }

            set
            { 
                _drawingType = value; 
            }
        }

        public abstract void Update(float elapsedSeconds);

    }
}
