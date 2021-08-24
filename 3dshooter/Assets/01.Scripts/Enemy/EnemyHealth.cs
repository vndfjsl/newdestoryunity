using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    public float bloodEffectTime = 1.0f;
    private EnemyAI ai; // �� ���ó���� ���ؼ� AI�� �������־����

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
        base.Die(); // * ��ӹ����� Die �״�ξ���
        ai.SetDead(); // * �ɼ� �߰�
    }

    private IEnumerator ShowBloodEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        GameObject effect = EffectManager.GetBloodEffect(); // * �� ������
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        effect.transform.rotation = rot;
        effect.transform.position = hitPosition;
        effect.SetActive(true);

        yield return new WaitForSeconds(bloodEffectTime);
        effect.gameObject.SetActive(false);
    }

    // * �� �ߺ���
    /* public float hp = 50f;
    public GameObject effect;

    public void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        hp -= damage;

        GameObject e = Instantiate(effect, point, Quaternion.LookRotation(normal));
        Destroy(e, 1.0f);

        if (hp <= 0)
        {
            Die(); //����� Die ȣ��
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    */
}
