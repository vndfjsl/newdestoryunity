using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth; // 초기체력
    public float health { get; protected set; } // * private 박으면 상속받아도못씀
    public bool dead { get; protected set; }
    public event Action OnDeath;

    protected virtual void OnEnable() // * PoolManager 쓸거라 start 대신 onEnable
    {
        dead = false;
        health = initHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        health -= damage;
        if(health <= 0 && !dead) // * 안디졌으면
        {
            Die(); // 디져요
        }
    }

    public virtual void RestoreHealth(float value) // * 이것도 만드는게 좋을것같음.
    {
        if (dead) return; // * 초당 체력재생 방지
        health += value;
    }

    public virtual void Die()
    {
        if (OnDeath != null)
            OnDeath();
        dead = true;
    }
}
