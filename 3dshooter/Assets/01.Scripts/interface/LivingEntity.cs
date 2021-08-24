using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float initHealth; // �ʱ�ü��
    public float health { get; protected set; } // * private ������ ��ӹ޾Ƶ�����
    public bool dead { get; protected set; }
    public event Action OnDeath;

    protected virtual void OnEnable() // * PoolManager ���Ŷ� start ��� onEnable
    {
        dead = false;
        health = initHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        health -= damage;
        if(health <= 0 && !dead) // * �ȵ�������
        {
            Die(); // ������
        }
    }

    public virtual void RestoreHealth(float value) // * �̰͵� ����°� �����Ͱ���.
    {
        if (dead) return; // * �ʴ� ü����� ����
        health += value;
    }

    public virtual void Die()
    {
        if (OnDeath != null)
            OnDeath();
        dead = true;
    }
}
