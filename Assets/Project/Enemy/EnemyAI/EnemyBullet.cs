using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private bool tracking;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Vector3 target;


    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);

        //Invoke("SelfDestruct", 3.0f);
    }

    void Update()
    {
        if (tracking)
        {
            Vector3.MoveTowards(this.transform.position, playerPos.position, speed * Time.deltaTime);
        }

        else
        {
            Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        }
        
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
