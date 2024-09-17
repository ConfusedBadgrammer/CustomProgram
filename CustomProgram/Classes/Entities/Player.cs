using Microsoft.Xna.Framework;

namespace SpaceDefenders
{
    public class Player : Entity, IHasHealth, IMoveable
    {
        private int _health;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private float _maxSpeed;
        private float _fireRateDelay;
        private float _fuel;
        
        private float _maxSpeedBonus;
        private float _fireRateBonus;

        private float _fuelBonus;
        private float _fuelBonusCap;
        private float _fuelRecoveryCooldown;

        private int _speedBonusCount;
        private int _fireRateBonusCount;

        private int _speedBoost;
        private int _fireRateBoost;
        private bool _IsBoosted;

        public Player()
        {
            // Assign initialisation values
            Position = GameConstants.PlayerSpawnPosition;
            Width = GameConstants.PlayerWidth;
            Height = GameConstants.PlayerHeight;
            Team = GameConstants.isAlly;
            DrawingType = DrawingType.Player;
            _health = GameConstants.PlayerInitialHealth;
            _velocity = new Vector2(0, 0);

            // Base values
            _maxSpeed = 300; // Increasing this value raises the maximum speed.
            _fireRateDelay = 17; // Increasing this value results in a slower firing rate.
            _fuel = 100;    // A higher value gives you more fuel capacity.

            // Variables used to increment player stats
            _fireRateBonus = 0;
            _fuelBonus = 0;
            _maxSpeedBonus = 0;

            // Variables used to increase player stats on "Boost"
            _IsBoosted = false;
            _speedBoost = 0;
            _fireRateBoost = 0;

            // Variables used to count the bonuses picked up
            _speedBonusCount = 0;
            _fireRateBonusCount = 0;

        }

        public bool IsBoosted
        {
            get
            { 
                return _IsBoosted; 
            }

            set
            {
                _IsBoosted = value;
            }
        }
        public int SpeedBonusCount
        {
            get 
            { 
                return _speedBonusCount; 
            }
        }

        public int FireRateBonusCount
        {
            get
            {
                return _fireRateBonusCount;
            }
        }

        public Vector2 Acceleration
        {
            get
            {
                return _acceleration;
            }

            set
            {
                _acceleration = value;
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
        public float FireRateDelay
        {
            get
            {
                return (_fireRateDelay - (_fireRateBonus + _fireRateBoost));
            }
        }

        public float Fuel
        {
            get
            {
                return (_fuel + _fuelBonus);
            }
        }

        public override void Update(float elapsedSeconds)
        {
            ToggleBoost(elapsedSeconds);  // Due to Monogame only able to handle 1 input, we continously check if the boost is toggled in update
            return;
        }

        public void MoveEntity(float elapsedSeconds)
        {
            float accelerationFactor = 0.1f;
            float decelerationFactor = 2.5f;
            //Acceleration
            _velocity += _acceleration * accelerationFactor;

            if (_velocity.Length() > (_maxSpeed + (_maxSpeedBonus + _speedBoost)))
            {
                _velocity.Normalize();
                _velocity *= _maxSpeed;
            }
            //Deceleration
            if (Acceleration == Vector2.Zero)
            {
                _velocity -= _velocity * decelerationFactor * elapsedSeconds;
            }
        }

        public void PowerUp(PowerUpType powerUpType)
        {
            int maxSpeedIncrease;
            int cappedSpeedBonus = 200;
            if (_maxSpeedBonus < cappedSpeedBonus)
            {
                maxSpeedIncrease = 20;
            }
            else
            {
                maxSpeedIncrease = 0;
            }

            float fireRateIncrease;
            float cappedfireRateIncrease = 10;
            if (_fireRateBonus < cappedfireRateIncrease)
            {
                fireRateIncrease = 1f;
            }
            else
            {
                fireRateIncrease = 0f;
            }

            int fuelIncrease;
            int cappedFuelBonus = 100;
            if (_fuelBonusCap < cappedFuelBonus)
            {
                fuelIncrease = 10;
            }
            else
            {
                fuelIncrease = 0;
            }

            int healthIncrease;
            int cappedHealth = 5;
            if (_health < cappedHealth)
            {
                healthIncrease = 1;
            }
            else
            {
                healthIncrease = 0;
            }

            switch (powerUpType)
            {
                case PowerUpType.Speed:
                    _maxSpeedBonus += maxSpeedIncrease;
                    if (_speedBonusCount < 10)
                    {
                        _speedBonusCount++;
                    }
                    break;

                case PowerUpType.FireRate:
                    _fireRateBonus += fireRateIncrease;
                    if (_fireRateBonusCount < 10)
                    {
                        _fireRateBonusCount++;
                    }
                    break;
                case PowerUpType.Fuel:
                    _fuelBonus += fuelIncrease;
                    _fuelBonusCap += fuelIncrease; // Increase the fuel bonus cap by 10 only when you pick up the fuel powerup.
                    break;
                case PowerUpType.Health:
                    _health += healthIncrease;
                    break;
                default:
                    break;
            }
        }



        public void ToggleBoost(float elapsedSeconds)
        {
            if (_IsBoosted && (_fuel + _fuelBonus > 0))
            {
                _speedBoost = 200; // Adjust this value to control the boost speed increase.
                _fireRateBoost = 5; // You can adjust this value for the fire rate increase if needed.

                if (_fuelBonus > 0)
                {
                    _fuelBonus -= 40f * elapsedSeconds; // Adjust this value to control the boost fuel consumption.
                }

                if (_fuelBonus <= 0 && _fuel > 0)
                {
                    _fuel -= 40f * elapsedSeconds; // Adjust this value to control the base fuel consumption.
                    _fuelRecoveryCooldown = 0.5f; // Set a cooldown time for fuel recovery after reaching 0.
                }
            }
            else
            {
                _speedBoost = 0;
                _fireRateBoost = 0;

                if (_fuelRecoveryCooldown > 0)
                {
                    _fuelRecoveryCooldown -= elapsedSeconds; // Decrease the cooldown timer.
                }
                else
                {
                    if (_fuelBonus < _fuelBonusCap)
                    {
                        _fuelBonus += 75f * elapsedSeconds; // Adjust this value to control the boost fuel recovery rate.

                        if (_fuelBonus > _fuelBonusCap)
                        {
                            _fuelBonus = _fuelBonusCap; // Cap the fuel bonus to the maximum of 10 powerups.
                        }
                    }
                    if (_fuelBonus <= _fuelBonusCap && _fuel < 100)
                    {
                        _fuel += 50f * elapsedSeconds; // Adjust this value to control the base fuel recovery rate.
                    }
                }
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
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

        public void EntityDie()
        {
            EntityReset();
        }
        public void EntityReset()
        {
            Position = GameConstants.PlayerSpawnPosition;
            _health = GameConstants.PlayerInitialHealth;
            _velocity = Vector2.Zero;

            _fireRateBonus = 0;
            _fuelBonus = 0;
            _maxSpeedBonus = 0;

            _speedBonusCount = 0;
            _fireRateBonusCount = 0;
        }
    }
}
