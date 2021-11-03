using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody bulletRigB;
    [SerializeField] private SphereCollider bulletSphereC;

    [Header("Variables")]
    public float bulletForce;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag != collision.gameObject.tag)
        {
            SelfDestruct();
            if (gameObject.tag == "Player" && collision.gameObject.tag == "Enemy")
            {
                
            }
            else if (gameObject.tag == "Enemy" && collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().HealthUpdate(-1.0f);
            }
        }
    }

    /*============================================================================
     * GAMEPLAY UPDATE METHODS
     ============================================================================*/
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void Init(string sourceTag, float sourceForce = 25.0f)
    {
        bulletRigB = GetComponent<Rigidbody>();
        bulletSphereC = GetComponent<SphereCollider>();
        gameObject.tag = sourceTag;
        bulletForce = sourceForce;
        bulletRigB.AddForce(transform.forward * bulletForce, ForceMode.VelocityChange);
        Invoke("SelfDestruct", 3.0f);
    }
}
