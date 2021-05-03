using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Statestudy
{
    Ready,
    Empty,
    Reloading
}

public class PlayersGun : MonoBehaviour
{
    // 복사!
    public State state { get; private set; }
    public Transform firePosition;
    public ParticleSystem muzzleFlash; //총구화염 이펙트
    public ParticleSystem shellEjectEffect; // 탄피 배출 이펙트
    public float bulletLineEffectTime = 0.03f; //라인렌더러가 그려지는 시간 

    public LineRenderer bulletLineRenderer;
    public float damage = 25; //총의 데미지
    public float fireDistance = 50f; //총의 사거리
    public int magCapacity = 10; //탄창의 용량
    public int magAmmo; //현재 총의 남은 탄환
    public float timeBetFire = 0.12f; //총알 발사 간격
    public float reloadTime = 1.0f; //재장전 시간
    public float lastFireTime; //마지막으로 총을 발사한 시간

    [Header("Audio Clips")]
    public AudioClip reloadSound;
    public AudioClip fireSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            
            Shot(); // 호출
        }
    }

    private void Shot()
    {
        audioSource.clip = fireSound;
        audioSource.Play();

        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        if(Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {
            IDamagedable target = hit.transform.GetComponent<IDamagedable>();
            if(target != null)
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
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlash.Play();
        shellEjectEffect.Play();
        bulletLineRenderer.SetPosition(1,
           bulletLineRenderer.transform.InverseTransformPoint(hitPosition));
        bulletLineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(bulletLineEffectTime);
        bulletLineRenderer
            .gameObject.SetActive(false);
    }

    public bool Reload()
    {
        if (state == State.Reloading || magAmmo >= magCapacity)
        {
            return false;
        }
        
        StartCoroutine(ReloadRoutine());
        return true;
    }

    public IEnumerator ReloadRoutine()
    {
        audioSource.clip = reloadSound;
        audioSource.Play();

        state = State.Reloading;
        yield return new WaitForSeconds(reloadTime);
        magAmmo = magCapacity;
        state = State.Ready;
    }

    private void PlaySound()
    {
        #region 예전에만든거
        /*
         
         public void PlaySound(string idle)
    {
        switch(idle)
        {
            case "use":
                    audioSource.clip = useSound;
                break;

            case "gameover":
                    audioSource.clip = gameoverSound;
                break;

            case "jump":
                    audioSource.clip = jumpSound;
                break;

            case "walk":
                if (audioSource.isPlaying == false)
                {
                    audioSource.clip = walkSound;
                }
                break;

            case "waterdamage":
                    audioSource.clip = waterDamageSound;
                break;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
         */

        #endregion
    }
}
