using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    private Gun gun;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("fire");
    private readonly int hashReload = Animator.StringToHash("reload");

    // 총 발사 상태를 알려주는 변수들
    public bool isFire = false;
    public bool isReload = false;
    private float nextFire = 0.0f;
    private WaitForSeconds wsReload;
    private Transform playerTr;
    private readonly float damping = 10f; // 빠를수록 총구 회전속도가 빨라짐

    private void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        anim = GetComponent<Animator>();
        wsReload = new WaitForSeconds(gun.reloadTime); // 총의 재장전 타임을 gun에서 받아옴
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
            // transform.rotation = rot // 에임핵
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);

            if(Time.time >= nextFire)
            {
                Fire(); // 타임 딜레이가 만족되었으면 발사
                nextFire = Time.time + gun.timeBetFire + Random.Range(0f, 0.3f);
            }
        }
    }

    private void Fire()
    {
        anim.SetTrigger(hashFire);
        gun.Fire(); // 알아서해줌
        if(gun.magAmmo <= 0) // 총알 다달면
        {
            gun.Reload(); // 리로드
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
