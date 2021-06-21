using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public int shotCount = 8;
    public float shotSpeed = 50f;
    public Transform bossTrm; // ½î´Â°÷
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpellStart(shotCount, shotSpeed));
    }

    private void Update()
    {
    }





    // ÃÑ¾Ë ½î´Â count, °¢µµ angle

    private IEnumerator SpellStart(int count, float speed)
    {

        do
        {
            for (int i = 0; i < count; i++)
            {
                Rigidbody2D scr = Instantiate(bulletPrefab, bossTrm.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                float angle = 360 * i / count - 90;
                scr.gameObject.transform.Rotate(new Vector3(0f, 0f, angle));
                scr.AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * i / count),
                                         speed * Mathf.Sin(Mathf.PI * 2 * i / count)));
            }
            yield return new WaitForSeconds(1f);
        } while (true);
    }
}
