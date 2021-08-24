using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable // 피격을 위한 인터페이스, livingEntity에 상속해주고 livingEntity는 추상형클래스가 됨
{
    //인터페이스의 매서드는 다 public
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal);
}
