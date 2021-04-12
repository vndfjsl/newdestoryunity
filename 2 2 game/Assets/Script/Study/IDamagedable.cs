using UnityEngine;

public interface IDamagedable
{
    void OnDamage(float damage, Vector3 point, Vector3 normal);
}
