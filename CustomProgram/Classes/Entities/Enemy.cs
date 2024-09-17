using Microsoft.Xna.Framework;
using System;

namespace SpaceDefenders
{
    public class Enemy : Entity, IHasHealth, IMoveable, IObjectPoolable
    {
        private int _health;
        private Vector2 _velocity;
        private bool _isActive;
        private Random _randomGenerator;
        private float _rotationAngle;
        private float _rotationSpeed;
        private bool _isBigEnemy;
        public Enemy()
        {
            Width = 60;
            Height = 30;
            Team = GameConstants.isEnemy;
            DrawingType = DrawingType.Enemy;

            _isActive = false;
            _health = 3;
            _randomGenerator = new Random();
            _rotationSpeed = MathHelper.ToRadians(_randomGenerator.Next(0, 360));
            _isBigEnemy = false;

        }

        public bool IsBigEnemy
        {
            get
            {
                return _isBigEnemy;
            }

            set
            {
                _isBigEnemy = value;
            }
        }

        public float RotationAngle
        {
            get
            {
                return _rotationAngle;
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

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public override void Update(float elapsedSeconds)
        {
            CheckHealthZero();
            MoveEntity(elapsedSeconds);
            _rotationAngle += _rotationSpeed * (float)elapsedSeconds;
            return;
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

        public void MoveEntity(float elaspedSeconds)
        {
            Position += _velocity * elaspedSeconds;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }

        public void EntityDie()
        {
            DeactivateEntity();
        }

        public void DeactivateEntity()
        {
            _isActive = false;
            _velocity = Vector2.Zero;
            _health = 3;
            Position = new Vector2(-1000, -1000);
            _isBigEnemy = false;
        }

        public void ActivateEntity()
        {
            int randomVelocityX = _randomGenerator.Next(50, 250);
            int randomVelocityY = _randomGenerator.Next(-30, 30);
            int randomxPos = _randomGenerator.Next(10, GameConstants.ScreenWidth - 10);
            _rotationAngle = _randomGenerator.Next(0, 360);
            _isActive = true;
            _velocity = new Vector2(randomVelocityY, randomVelocityX);
            Position = new Vector2(randomxPos, 1);
        }
    }
}
