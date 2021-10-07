using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody bulletRigB;
    [SerializeField] private SphereCollider bulletSphereC;

    [Header("Variables")]
    public Vector3 direction;
    public float speed;

    void Start()
    {
        bulletRigB = GetComponent<Rigidbody>();
        bulletSphereC = GetComponent<SphereCollider>();
        Invoke("SelfDestruct", 3.0f);
    }

    void FixedUpdate()
    {
        bulletRigB.AddForce(transform.right, ForceMode.Force);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
