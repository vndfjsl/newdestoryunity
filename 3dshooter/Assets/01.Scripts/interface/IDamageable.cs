using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable // �ǰ��� ���� �������̽�, livingEntity�� ������ְ� livingEntity�� �߻���Ŭ������ ��
{
    //�������̽��� �ż���� �� public
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal);
}
