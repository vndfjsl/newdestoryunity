using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagealbe
{
    public float hp = 200f;
    public GameObject bloodEffect;
    public void OnDamage(float damage, Vector3 point, Vector3 hitNormal)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            Die();
        }
        GameObject blood = Instantiate(bloodEffect, transform.position, Quaternion.LookRotation(hitNormal));
        Destroy(blood, 1f);
        // 피격시 출혈효과와 hp감소, hp 0이하면 사라지게
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
