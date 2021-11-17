using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigB;
    private SphereCollider bulletSphereC;

    /*============================================================================
     * DEFAULT UNITY METHODS
     ============================================================================*/
    private void OnTriggerEnter(Collider coll)
    {
        if (gameObject.tag != coll.gameObject.tag && coll.gameObject.tag != "Volume" && coll.gameObject.tag != "EnemyAttack")// && (gameObject.layer == coll.gameObject.layer || coll.gameObject.tag == "Player"))
        {
            SelfDestruct();
            if (gameObject.tag == "Player" && coll.gameObject.tag == "Enemy")
            {
                try
                {
                    PlatformerEnemy pScript = coll.GetComponent<PlatformerEnemy>();
                    pScript.TakeDamage(10);
                }
                catch(Exception e1)
                {
                    try
                    {
                        ShooterEnemy sScript = coll.GetComponent<ShooterEnemy>();
                        sScript.TakeDamage(10);
                    }
                    catch (Exception e2)
                    {

                    }
                }
            }
            else if (gameObject.tag == "Enemy" && coll.gameObject.tag == "Player")
            {
                coll.gameObject.GetComponent<PlayerController>().HealthUpdate(-1.0f);
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
        bulletRigB.AddForce(transform.forward * sourceForce, ForceMode.VelocityChange);
        Invoke("SelfDestruct", 3.0f);
    }
}
