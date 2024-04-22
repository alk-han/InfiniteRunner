using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Add SerializeField for all the properties that you need the instantiation to copy. (ArraySpawner)
    [SerializeField] private float   moveSpeed       = 20f;
    [SerializeField] private Vector3 moveDirection   = Vector3.forward;
    [SerializeField] private Vector3 destination;


    private void Start()
    {
        SpeedController speedController = FindObjectOfType<SpeedController>();
        if (speedController != null)
        {
            speedController.OnGlobalSpeedChanged += SetMoveSpeed;
            SetMoveSpeed(speedController.GetGlobalSpeed());
        }
    }


    private void Update()
    {
        // transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if (Vector3.Dot((destination - transform.position).normalized, moveDirection) < 0)
        {
            Destroy(gameObject);
        }
    }


    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }


    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }


    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }


    public void CopyFrom(Movement other)
    {
        SetMoveSpeed(other.moveSpeed);
        SetMoveDirection(other.moveDirection);
        SetDestination(other.destination);
    }
}
