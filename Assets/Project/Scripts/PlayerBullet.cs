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
    public Quaternion direction;

    void Start()
    {
        bulletForce = 25.0f;
        direction = transform.rotation;
        bulletRigB = GetComponent<Rigidbody>();
        bulletSphereC = GetComponent<SphereCollider>();
        bulletRigB.AddForce(transform.right * bulletForce, ForceMode.VelocityChange);
        Invoke("SelfDestruct", 3.0f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SelfDestruct();
    }
}
