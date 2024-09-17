using System.Collections.Generic;

namespace SpaceDefenders
{
    public class ObjectPool
    {
        private List<Enemy> _enemyPool;
        private List<Projectile> _projectilePool;
        private List<Explosion> _explosionPool;
        private List<Hit> _hitPool;
        private List<PowerUp> _powerUpsPool;
        private List<Thrusters> _thrustersPool; 

        public ObjectPool()
        {
            CreatePools();
        }

        public List<Enemy> EnemyPool
        {
            get
            {
                return _enemyPool;
            }
        }

        public List<Projectile> ProjectilePool
        {
            get
            {
                return _projectilePool;
            }
        }

        public List<Explosion> ExplosionPool
        {
            get
            {
                return _explosionPool;
            }
        }

        public List<Hit> HitPool
        {
            get
            {
                return _hitPool;
            }
        }

        public List<PowerUp> PowerUpsPool
        {
            get
            {
                return _powerUpsPool;
            }
        }

        public List<Thrusters> ThrustersPool
        {
            get
            {
                return _thrustersPool;
            }
        }

        // Creates a pool for each objectpoolable Entity type (List of entities)
        public void CreatePools()
        {
            _enemyPool = new List<Enemy>();
            for (int i = 0; i < GameConstants.MaxEnemies; i++)
            {
                Enemy enemy = new();
                _enemyPool.Add(enemy);
            }

            _projectilePool = new List<Projectile>();
            for (int i = 0; i < GameConstants.MaxProjectiles; i++)
            {
                Projectile projectile = new();
                _projectilePool.Add(projectile);
            }

            _powerUpsPool = new List<PowerUp>();
            for (int i = 0; i <= GameConstants.MaxPowerUps; i++)
            {
                PowerUp powerUps = new PowerUp();
                _powerUpsPool.Add(powerUps);
            }

            _explosionPool = new List<Explosion>();
            for (int i = 0; i < GameConstants.MaxExplosion; i++)
            {
                Explosion explosion = new();
                _explosionPool.Add(explosion);
            }

            _hitPool = new List<Hit>();
            for (int i = 0; i < GameConstants.MaxHit; i++)
            {
                Hit hit = new Hit();
                _hitPool.Add(hit);
            }

            _thrustersPool = new List<Thrusters>();
            for (int i = 0; i < GameConstants.MaxThrusters; i++)
            {
                Thrusters thrusters = new Thrusters();
                _thrustersPool.Add(thrusters);
            }
        }

        // Use of templates or generics to determine the entity needed to spawn
        public T SpawnEntity<T>() where T : Entity
        {
            if (typeof(T) == typeof(Enemy))
            {
                foreach (Enemy enemy in _enemyPool)
                {
                    if (enemy.IsActive == false)
                    {
                        enemy.ActivateEntity();
                        return (T)(Entity)enemy;
                    }
                }
            }

            if (typeof(T) == typeof(Projectile))
            {
                foreach (Projectile projectile in _projectilePool)
                {
                    if (projectile.IsActive == false)
                    {
                        projectile.ActivateEntity();
                        return (T)(Entity)projectile;
                    }
                }
            }

            if (typeof(T) == typeof(PowerUp))
            {
                foreach (PowerUp powerup in _powerUpsPool)
                {
                    if (powerup.IsActive == false)
                    {
                        powerup.ActivateEntity();
                        return (T)(Entity)powerup;
                    }
                }
            }

            if (typeof(T) == typeof(Explosion))
            {
                foreach (Explosion explosion in _explosionPool)
                {
                    if (explosion.IsActive == false)
                    {
                        explosion.ActivateEntity();
                        return (T)(Entity)explosion;
                    }
                }
            }

            if (typeof(T) == typeof(Hit))
            {
                foreach (Hit hit in _hitPool)
                {
                    if (hit.IsActive == false)
                    {
                        hit.ActivateEntity();
                        return (T)(Entity)hit;
                    }
                }
            }

            if (typeof(T) == typeof(Thrusters))
            {
                foreach (Thrusters thrusters in _thrustersPool)
                {
                    if (thrusters.IsActive == false)
                    {
                        thrusters.ActivateEntity();
                        return (T)(Entity)thrusters;
                    }
                }
            }

            return null;
        }


        // Use of Templates to find the first IsActive entity and return as a list
        public List<T> FindActive<T>() where T : Entity
        {
            List<T> activeList = new();

			//typeof(T).Equals(typeof(Enemy)) checks if the T is Enemy - if i only pass in enemy, projectile etc
			//typeof(Enemy).IsSubclassOf(typeof(T)) checks if T is a parent of Enemy, this covers the case where T is Entity 

			if (typeof(T).Equals(typeof(Enemy)) || typeof(Enemy).IsSubclassOf(typeof(T)))
			{
				foreach (Enemy enemy in _enemyPool)
				{
					if (enemy.IsActive == true)
					{
						activeList.Add((T)(Entity)enemy);   //cast then cast again (C# cannot cast directly and needs to be cast to parent class)
					}
				}
			}
				
            if (typeof(T).Equals(typeof(Projectile)) || typeof(Projectile).IsSubclassOf(typeof(T)))
            {
                foreach (Projectile projectile in _projectilePool)
                {
                    if (projectile.IsActive == true)
                    {
                        activeList.Add((T)(Entity)projectile);
                    }
                }
            }

            if (typeof(T).Equals(typeof(PowerUp)) || typeof(PowerUp).IsSubclassOf(typeof(T)))
            {
                foreach (PowerUp powerup in _powerUpsPool)
                {
                    if (powerup.IsActive == true)
                    {
                        activeList.Add((T)(Entity)powerup);
                    }
                }
            }

            if (typeof(T).Equals(typeof(Explosion)) || typeof(Explosion).IsSubclassOf(typeof(T)))
            {
                foreach (Explosion explosion in _explosionPool)
                {
                    if (explosion.IsActive == true)
                    {
                        activeList.Add((T)(Entity)explosion);
                    }
                }
            }


            if (typeof(T).Equals(typeof(Hit)) || typeof(Hit).IsSubclassOf(typeof(T)))
            {
                foreach (Hit hit in _hitPool)
                {
                    if (hit.IsActive == true)
                    {
                        activeList.Add((T)(Entity)hit);
                    }
                }
            }

            if (typeof(T).Equals(typeof(Thrusters)) || typeof(Thrusters).IsSubclassOf(typeof(T)))
            {
                foreach (Thrusters thrusters in _thrustersPool)
                {
                    if (thrusters.IsActive == true)
                    {
                        activeList.Add((T)(Entity)thrusters);
                    }
                }
            }

            //Add in additional objects

            return activeList;
        }

        // Deactivate ALL entities (cast to IObjectPoolable to run DeactivateEntity on all Entities)
        public void DeactivateAll()
        {
            foreach (Entity entity in FindActive<Entity>())
            {
                IObjectPoolable objectPoolableEntity = entity as IObjectPoolable;
                if (objectPoolableEntity != null)
                {
                    objectPoolableEntity.DeactivateEntity();
                }
            }
        }

    }
}
