using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : Collectible
{
    [SerializeField] float pushSpeed = 20f;
    [SerializeField] Vector3 torque = new Vector3(2.5f, 2f, 3f);
    [SerializeField] float delay = 3f;

    protected override void PickedUpBy(GameObject picker)
    {
        // base.PickedUpBy(picker);
        Movement.enabled = false;
        GetComponent<Collider>().enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(((transform.position - picker.transform.position).normalized + (Vector3.up * 0.5f)) * pushSpeed, ForceMode.VelocityChange);
        rb.AddTorque(torque, ForceMode.VelocityChange);

        Invoke(nameof(DestroySelf), delay);
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
