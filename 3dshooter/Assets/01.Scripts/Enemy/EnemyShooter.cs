using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Gun gun;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("fire");
    private readonly int hashReload = Animator.StringToHash("reload");

    // �� �߻� ���¸� �˷��ִ� ������
    public bool isFire = false;
    public bool isReload = false;
    private float nextFire = 0.0f;
    private WaitForSeconds wsReload;
    private Transform playerTr;
    private readonly float damping = 10f; // �������� �ѱ� ȸ���ӵ��� ������

    private void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        anim = GetComponent<Animator>();
        wsReload = new WaitForSeconds(gun.reloadTime); // ���� ������ Ÿ���� gun���� �޾ƿ�
    }

    void Start()
    {
        playerTr = GameManager.instance.playerTR;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFire && !isReload)
        {
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position);
            // transform.rotation = rot // ������
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

            if(Time.time >= nextFire)
            {
                Fire(); // Ÿ�� �����̰� �����Ǿ����� �߻�
                nextFire = Time.time + gun.timeBetFire + Random.Range(0f, 0.3f);
            }
        }
    }

    private void Fire()
    {
        anim.SetTrigger(hashFire);
        gun.Fire(); // �˾Ƽ�����
        if(gun.magAmmo <= 0) // �Ѿ� �ٴ޸�
        {
            gun.Reload(); // ���ε�
            isReload = true;
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        anim.SetTrigger(hashReload);
        yield return wsReload;
        isReload = false;
    }
}
