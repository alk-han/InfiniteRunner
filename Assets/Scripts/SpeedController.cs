using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private float globalSpeed = 15f;

    // public delegate void OnGlobalSpeedChanged(float newSpeed);
    // public event OnGlobalSpeedChanged onGlobalSpeedChanged;
    public event Action<float> OnGlobalSpeedChanged;
    public event Action OnSpeedBoostActivated;
    public event Action OnSpeedBoostDeactivated;


    public void ChangeGlobalSpeed(float speedBoost, float duration)
    {
        globalSpeed += speedBoost;
        OnSpeedBoostActivated?.Invoke();
        SpeedChanged();
        StartCoroutine(RemoveSpeedBoost(speedBoost, duration));
    }


    public float GetGlobalSpeed()
    {
        return globalSpeed;
    }


    private IEnumerator RemoveSpeedBoost(float speedBoost, float duration)
    {
        yield return new WaitForSeconds(duration);
        globalSpeed -= speedBoost;
        OnSpeedBoostDeactivated?.Invoke();
        SpeedChanged();
    }


    private void SpeedChanged()
    {
        OnGlobalSpeedChanged?.Invoke(globalSpeed);
    }
}
