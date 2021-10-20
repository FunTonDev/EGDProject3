using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private bool tracking;

    private Transform playerPos;
    private Vector3 target;


    void Start()
    {

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);

        if (tracking)
        {
            Invoke("SelfDestruct", 6.0f);
        }

        else
        {
            Invoke("SelfDestruct", 4.0f);
        }
        

        
    }

    void Update()
    {
       
        if (tracking)
        {            
            transform.position = Vector3.MoveTowards(this.transform.position, playerPos.position, speed * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            //transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        }
        
    }

    private void SelfDestruct()
    {
        //Debug.Log("Bullet SelfDestruct");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SelfDestruct();
    }
}
