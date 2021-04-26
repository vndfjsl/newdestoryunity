using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float hp = 100f;
    void Start()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        hp -= 10;
        if(hp <= 0){
            Die();
        }
    }

    private void Die(){
        //StartCoroutine(Respawn());
        Invoke("Respawn", 5f);
        gameObject.SetActive(false);
    }

    private void Respawn()
    {
        hp = 100;
        gameObject.SetActive(true);
    }
}
