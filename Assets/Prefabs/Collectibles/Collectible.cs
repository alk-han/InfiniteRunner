using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : Spawnable
{
    [SerializeField] private int    scoreBoost;
    [SerializeField] private float  speedBoost;
    [SerializeField] private float  speedBoostDuration;

    private bool isAdjusted = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpeedController speedController = FindObjectOfType<SpeedController>();
            if (speedController != null && speedBoost != 0)
            {
                speedController.ChangeGlobalSpeed(speedBoost, speedBoostDuration);
            }

            ScoreController scoreController = FindObjectOfType<ScoreController>();
            if (scoreController != null && scoreBoost != 0)
            {
                scoreController.ChangeScore(scoreBoost);
            }

            PickedUpBy(other.gameObject);
        }

        if (other.CompareTag("Obstacle") && !isAdjusted)
        {
            Collider collider = other.GetComponent<Collider>();
            if (collider != null)
            {
                // transform.position = collider.bounds.center + (collider.bounds.extents.y + gameObject.GetComponent<Collider>().bounds.center.y) * Vector3.up;
                transform.position = collider.bounds.center + collider.bounds.extents.y * Vector3.up;
                isAdjusted = true;
            }
        }
    }


    protected virtual void PickedUpBy(GameObject picker)
    {
        Destroy(gameObject);
    }
}
