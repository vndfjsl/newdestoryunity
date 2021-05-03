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
    // ����!
    public State state { get; private set; }
    public Transform firePosition;
    public ParticleSystem muzzleFlash; //�ѱ�ȭ�� ����Ʈ
    public ParticleSystem shellEjectEffect; // ź�� ���� ����Ʈ
    public float bulletLineEffectTime = 0.03f; //���η������� �׷����� �ð� 

    public LineRenderer bulletLineRenderer;
    public float damage = 25; //���� ������
    public float fireDistance = 50f; //���� ��Ÿ�
    public int magCapacity = 10; //źâ�� �뷮
    public int magAmmo; //���� ���� ���� źȯ
    public float timeBetFire = 0.12f; //�Ѿ� �߻� ����
    public float reloadTime = 1.0f; //������ �ð�
    public float lastFireTime; //���������� ���� �߻��� �ð�

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
            
            Shot(); // ȣ��
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
        #region �����������
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
