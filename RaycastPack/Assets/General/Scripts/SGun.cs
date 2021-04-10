using UnityEngine;
using UnityEngine.UI;

public class SGun : MonoBehaviour
{
    public float power = 10f;
    public float range = 100f;
    public Camera fpsCam;

    public ParticleSystem muzzleFlash; // ��¿� ��ƼŬ�ε�
    public GameObject impactEffect; // ��� ���ӿ�����Ʈ�ΰ�? ��: muzzle�� ��ƼŬ�̶�.
    public float impactForce = 60f;

    public float fireRate = 1f;
    private float nextTimeToFire = 0f;

    public LineRenderer lineLaser;
    public GameObject crosshair;

    public Image reloadimage;
    public bool bReload = false;
    private float reloadCurrent = 0f;

    void Update()
    {
        RaycastHit laserHit;
        Ray laserRay = new Ray(lineLaser.transform.position, lineLaser.transform.forward);
        if(Physics.Raycast(laserRay, out laserHit))
        {
            lineLaser.SetPosition(1, lineLaser.transform.InverseTransformPoint(laserHit.point));
            crosshair.transform.position = Camera.main.WorldToScreenPoint(laserHit.point);
            
        }

        if(Input.GetButton("Fire1") && Time.time > nextTimeToFire && bReload == false)
        {
            bReload = true;
            reloadCurrent = 0f;
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }

        if(bReload)
        {
            reloadCurrent += Time.deltaTime;
            float percent = reloadCurrent / fireRate;
            reloadimage.fillAmount = percent;
            if(percent >= 1f)
            {
                bReload = false;
                reloadimage.fillAmount = 0f;
            }
        }
    }

    private void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;

        if(Physics.Raycast(lineLaser.transform.position, lineLaser.transform.forward, out hit, range))
        {
            STargetBox targetb = hit.transform.GetComponent<STargetBox>();
            Debug.Log(hit.transform.name);

            if(targetb != null) // ��üũ
            {
                targetb.TakeDamage(power);
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            }

            GameObject impactObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // vector�� ���������� �������� normal

            Destroy(impactObject, 1f);

        }
    }
}
