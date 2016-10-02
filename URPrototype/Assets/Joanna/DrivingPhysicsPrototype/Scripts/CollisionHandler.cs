using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{

    Rigidbody shipRigidbody;
    Vector3 cachedReactionForce;

    // Use this for initialization
    void Start()
    {
        shipRigidbody = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = shipRigidbody.rotation;
        transform.rotation = rotation;
    }

    void OnCollisionEnter(Collision coll)
    {
        Vector3 temp = Vector3.zero;

        for (int i = 0; i < coll.contacts.Length; i++)
            temp += coll.contacts[i].normal;

        temp += shipRigidbody.velocity.normalized;
        temp /= coll.contacts.Length + 1;

        temp.y = 0;
        cachedReactionForce = temp;

        shipRigidbody.AddForce(cachedReactionForce * shipRigidbody.velocity.magnitude, ForceMode.VelocityChange);
        shipRigidbody.AddForce(-shipRigidbody.velocity * 2, ForceMode.Impulse);
    }

    void OnCollisionStay(Collision coll)
    {
        shipRigidbody.AddForce(cachedReactionForce, ForceMode.VelocityChange);
        shipRigidbody.AddForce(shipRigidbody.velocity * 0.6f, ForceMode.Impulse);
    }
}