using UnityEngine;

public class STargetBox : MonoBehaviour
{
    public float hp = 50f;
    public GameObject destroyObject;
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(destroyObject, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
