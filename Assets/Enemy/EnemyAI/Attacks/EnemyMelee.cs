using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private Vector3 hitBoxDim; //Dimensions for the hitbox
    [SerializeField] private float time;       //How long hitbox should be active

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = hitBoxDim;

        Invoke("SelfDestruct", time);
    }

    private void SelfDestruct()
    {
        Debug.Log("MeleeHitbox SelfDestruct");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            Debug.Log("MeleeHitbox Hit Player");
        }

        SelfDestruct();
    }
}
