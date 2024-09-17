namespace SpaceDefenders
{
    public interface IHasHealth
    {

        void TakeDamage(int damage);
        void EntityDie();
        bool CheckHealthZero();

    }

}
