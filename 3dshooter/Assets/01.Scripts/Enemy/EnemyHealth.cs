using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    public float bloodEffectTime = 1.0f;
    private EnemyAI ai; // 적 사망처리를 위해서 AI를 가지고있어야함

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    public override void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        base.OnDamage(damage, point, normal);
        StartCoroutine(ShowBloodEffect(point, normal));
    }

    public override void Die()
    {
        base.Die(); // * 상속받은거 Die 그대로쓰고
        ai.SetDead(); // * 옵션 추가
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        GameObject effect = EffectManager.GetBloodEffect(); // * 피 가져와
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        effect.transform.rotation = rot;
        effect.transform.position = hitPosition;
        effect.SetActive(true);

        yield return new WaitForSeconds(bloodEffectTime);
        effect.gameObject.SetActive(false);
    }

    // * 다 중복임
    /* public float hp = 50f;
    public GameObject effect;

    public void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        hp -= damage;

        GameObject e = Instantiate(effect, point, Quaternion.LookRotation(normal));
        Destroy(e, 1.0f);

        if (hp <= 0)
        {
            Die(); //사망시 Die 호출
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    */
}
