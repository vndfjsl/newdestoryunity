using System.Collections;
using TMPro;
using UnityEngine;

// ���� ���¸� ������ enum
public enum State
{
    Ready, Empty, Reloading
}

public class Gun : MonoBehaviour
{
    public State state { get; private set; } // ���� ����

    public Transform firePosition; // �Ѿ� �߻���ġ
    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ����Ʈ
    public ParticleSystem shellEjectEffect; // ź�� ������ ����Ʈ
    public float bulletLineEffectTime = 0.03f; // ���η����� �׷����� �ð�

    public LineRenderer bulletLineRenderer;
    public float damage = 25f; // ���� ������
    public float fireDistance = 50f; // ���� ��Ÿ�
    public int magCapacity = 10; // ���� źâ�� źȯ ����
    public int magAmmo; // ���� ������ źȯ��
    public float timeBetFire = 0.12f; // źȯ�߻� �����̽ð�
    public float reloadTime = 1.0f; // ������ �ҿ�ð�
    public float lastFireTime; // ������ �ѹ߻�ð�

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
        // �Ѿ˹߻� ���, ���ǰ˻�= Shot ����
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
