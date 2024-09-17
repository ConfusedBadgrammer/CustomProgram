using System.Numerics;

namespace SpaceDefenders
{
    public class Hit : Entity, IObjectPoolable
    {
        private bool _isActive;
        private Animation _hitAnimation;
        public Hit()
        {
            _hitAnimation = new Animation(DrawingType.Hit, 12, 6, 512, 512, 4);
            _isActive = false;
            DrawingType = DrawingType.Hit;

            //If scale is needed
            Width = 50;
            Height = 50;
        }

        public Animation Animation
        {
            get
            {
                return _hitAnimation;
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
            _hitAnimation.UpdateFrames();
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
