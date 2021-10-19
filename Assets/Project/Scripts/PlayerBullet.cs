using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody bulletRigB;
    [SerializeField] private SphereCollider bulletSphereC;

    [Header("Variables")]
    public float bulletForce;

    void Start()
    {
        bulletForce = 25.0f;
        bulletRigB = GetComponent<Rigidbody>();
        bulletSphereC = GetComponent<SphereCollider>();
        bulletRigB.AddForce(transform.forward * bulletForce, ForceMode.VelocityChange);
        Invoke("SelfDestruct", 3.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            SelfDestruct();
        }
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
