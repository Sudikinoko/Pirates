using UnityEngine;

public interface IHittable
{
    public void TakeDamage(GameObject damageDealer, float damage);
    public void Slow(GameObject damageDealer, float percent);
    public void Heal(float heal);

}
