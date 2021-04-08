using System.Collections;
using TMPro;
using UnityEngine;

// 총의 상태를 저장할 enum
public enum State
{
    Ready, Empty, Reloading
}

public class Gun : MonoBehaviour
{
    public State state { get; private set; } // 현재 상태

    public Transform firePosition; // 총알 발사위치
    public ParticleSystem muzzleFlashEffect; // 총구 화염 이펙트
    public ParticleSystem shellEjectEffect; // 탄피 나오는 이펙트
    public float bulletLineEffectTime = 0.03f; // 라인렌더러 그려지는 시간

    public LineRenderer bulletLineRenderer;
    public float damage = 25f; // 총의 데미지
    public float fireDistance = 50f; // 총의 사거리
    public int magCapacity = 10; // 총의 탄창의 탄환 갯수
    public int magAmmo; // 현재 장점된 탄환수
    public float timeBetFire = 0.12f; // 탄환발사 딜레이시간
    public float reloadTime = 1.0f; // 재장전 소요시간
    public float lastFireTime; // 마지막 총발사시간

    private void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
        // 총알발사 명령, 조건검사= Shot 실행
    }

    private void Shot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {
            IDamagealbe target = hit.transform.GetComponent<IDamagealbe>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if (magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        bulletLineRenderer.SetPosition(
            1,
            bulletLineRenderer.transform.InverseTransformPoint(hitPosition));
        bulletLineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(bulletLineEffectTime);
        
        bulletLineRenderer.gameObject.SetActive(false);
    }

    public bool Reload()
    {
        if(state == State.Reloading || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    public IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }
}
