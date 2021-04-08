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
        // �ǰݽ� ����ȿ���� hp����, hp 0���ϸ� �������
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
