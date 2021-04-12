using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysHealth : MonoBehaviour, IDamagedable
{
    public float hp = 100f;
    public GameObject bloodEffect;

    public void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        hp -= damage;
        GameObject effect = Instantiate(bloodEffect, point, Quaternion.LookRotation(normal), transform);
        Destroy(effect, 1f);
        // ÇÇÆ¢±â±â
        if(hp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
