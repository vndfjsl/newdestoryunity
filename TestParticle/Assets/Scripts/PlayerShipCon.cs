using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipCon : MonoBehaviour
{
    [SerializeField] float controlSpeed = 25f;
    [SerializeField] float xLimitRange = 7f;
    [SerializeField] float yLimitRange = 7f;

    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 2f;
    [SerializeField] float controlRollFactor = -20f;

    float xAxisVal, yAxisVal;

    public ParticleSystem[] lasers;
    void Update()
    {
        // 이동 관련 코드
        xAxisVal = Input.GetAxis("Horizontal");
        yAxisVal = Input.GetAxis("Vertical");

        float xOffset = xAxisVal * Time.deltaTime * controlSpeed;
        float xPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(xPos, -xLimitRange, xLimitRange);

        float yOffset = yAxisVal * Time.deltaTime * controlSpeed;
        float yPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(yPos, -yLimitRange, yLimitRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);

        // 회전 관련 코드
        float pitchDueToPos = transform.localPosition.y * positionPitchFactor;
        float pitchDueToCotrolAxis = yAxisVal * controlPitchFactor;

        float pitch = pitchDueToPos + pitchDueToCotrolAxis;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xAxisVal * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        SetLaser(Input.GetMouseButton(0));

    }

    private void SetLaser(bool isActive)
    {
        foreach(ParticleSystem p in lasers)
        {
            var emission = p.emission;
            emission.enabled = isActive;
        }
    }
}
