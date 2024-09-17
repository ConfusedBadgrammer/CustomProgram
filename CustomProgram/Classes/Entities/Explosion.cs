using System.Numerics;

namespace SpaceDefenders
{
    public class Explosion : Entity, IObjectPoolable
    {
        private bool _isActive;
        private Animation _explosionAnimation;
        public Explosion()
        {
            _explosionAnimation = new Animation(DrawingType.Explosion, 12, 12, 96, 96, 12);
            _isActive = false;
            DrawingType = DrawingType.Explosion;
        }

        public Animation Animation
        {
            get 
            {
                    return _explosionAnimation;
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
                _isActive= value;
            }
        }

        public override void Update(float elapsedSeconds)
        {
            _explosionAnimation.UpdateFrames();
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
