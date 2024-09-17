using System.Numerics;

namespace SpaceDefenders
{
    public class Thrusters : Entity, IObjectPoolable
    {
        private bool _isActive;
        private Animation _thrustersAnimation;
        public Thrusters()
        {
            _thrustersAnimation = new Animation(DrawingType.Thrusters, 2, 6, 32, 32, 4);
            _isActive = false;
            DrawingType = DrawingType.Thrusters;

            //If scale is needed
            Width = 50;
            Height = 50;
        }

        public Animation Animation
        {
            get
            {
                return _thrustersAnimation;
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
            _thrustersAnimation.UpdateFrames();
        }


        public void ActivateEntity()
        {
            _isActive = true;
        }

        public void DeactivateEntity()
        {
            _isActive = false;
            Position = new Vector2(-1000, 1000);
        }

    }
}
