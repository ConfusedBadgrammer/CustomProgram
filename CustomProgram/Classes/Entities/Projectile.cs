using Microsoft.Xna.Framework;
using System;

namespace SpaceDefenders
{
    public class Projectile : Entity, IMoveable, IObjectPoolable
    {
        private readonly int _damage;
        private Vector2 _velocity;
        private bool _isActive;
        private readonly Random _randomGenerator;
        public Projectile()
        {
            _isActive = false;
            Width = 2;
            Height = 5;
            Team = GameConstants.isAlly;
            DrawingType = DrawingType.Projectile;
            _damage = 1;

            _randomGenerator = new Random();
        }

        public int Damage
        {
            get
            {
                return _damage;
            }
        }

        public bool IsActive
        {
            get
            {
                return (_isActive);
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

        public void MoveEntity(float elapsedSeconds)
        {
            Position += _velocity * elapsedSeconds;
        }

        public void ActivateEntity()
        {
            int fireDeviation = _randomGenerator.Next(-40, 40);
            _velocity = new Vector2(fireDeviation, -900);
            _isActive = true;
        }

        public void DeactivateEntity()
        {
            _isActive = false;
            Position = new Vector2(-1000, -1000);
            _velocity = Vector2.Zero;
        }
    }
}
