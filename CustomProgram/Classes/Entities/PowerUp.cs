using Microsoft.Xna.Framework;
using System;


namespace SpaceDefenders
{
    public class PowerUp : Entity, IMoveable, IObjectPoolable
    {
        private Vector2 _velocity;
        private bool _isActive;
        private PowerUpType _type;
        private Random _randomGenerator;
        public PowerUp()
        {
            Width = 30;
            Height = 25;
            Team = GameConstants.isNeutral;

            _isActive = false;
            _randomGenerator = new Random();
        }

        public PowerUpType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }
        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }

            set
            {
                _velocity = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
            }
        }

        public override void Update(float elapsedSeconds)
        {
            MoveEntity(elapsedSeconds);
        }

        public void MoveEntity(float elaspedSeconds)
        {
            Position += _velocity * elaspedSeconds;
        }

        public void ActivateEntity()
        {
            _isActive = true;
            int randomxPos = _randomGenerator.Next(10, GameConstants.ScreenWidth - 10);
            int VelocityY = 50;
            _velocity = new Vector2(0, VelocityY);
            //Re-Roll power up type by randomly selecting a PowerUpType without using arrays
            _type = (PowerUpType)_randomGenerator.Next(Enum.GetNames(typeof(PowerUpType)).Length);
            Position = new Vector2(randomxPos, 1);
            DrawingType = SetDrawingType();
        }

        public void DeactivateEntity()
        {
            _isActive = false;
            _velocity = Vector2.Zero;
            Position = new Vector2(-1000, -1000);
        }

        public DrawingType SetDrawingType()
        {
            switch (_type)
            {
                case PowerUpType.FireRate:
                    DrawingType = DrawingType.PowerUpFireRate;
                    break;
                case PowerUpType.Health:
                    DrawingType = DrawingType.PowerUpHealth;
                    break;
                case PowerUpType.Fuel:
                    DrawingType = DrawingType.PowerUpFuel;
                    break;
                case PowerUpType.Speed:
                    DrawingType = DrawingType.PowerUpSpeed;
                    break;
                default:
                    DrawingType = DrawingType.PowerUpSpeed; // Default to a value
                    break;
            }
            return DrawingType;
        }

    }
}
