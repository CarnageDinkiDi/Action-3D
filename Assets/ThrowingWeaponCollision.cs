using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeaponCollision : MonoBehaviour
{
    Rigidbody rb;
    public ThrowingWeapon throwingWeapon;
    public Collider coll;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        throwingWeapon.CollisionOccured();
        rb.useGravity = false;
        rb.isKinematic = true;
        Constraints();
    }

    void Constraints()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    public void RemoveConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    public void NoCollision(int i)
    {
        if (i == 1) { coll.isTrigger = true; }
        else if (i == 0) { coll.isTrigger = false; }
    }
}
