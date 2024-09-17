using Microsoft.Xna.Framework;


namespace SpaceDefenders.Classes
{
    public class Earth : Entity, IHasHealth
    {
        private int _health;
        private Animation _rotatingEarthAnimation;

        public int Health
        {
            get
            {
                return _health;
            }

            set
            {
                _health = value;
            }
        }

        public Animation Animation
        {
            get
            {
                return _rotatingEarthAnimation;
            }
        }
        public Earth()
        {
            _rotatingEarthAnimation = new Animation(DrawingType.Earth, 6, 30, 322, 322, 6);
            _health = 5;
            Position = new Vector2(GameConstants.ScreenWidth/2 - 180, GameConstants.ScreenHeight - 100);

            DrawingType = DrawingType.Earth;
            Width = 275;
            Height = 275;
        }

        public override void Update(float elaspedSeconds)
        {
            _rotatingEarthAnimation.UpdateFrames();
            CheckHealthZero();
        }

        public bool CheckHealthZero()
        {
            if (_health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EntityDie(){}

        public void Reset()
        {
            _health = 5;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }
}
