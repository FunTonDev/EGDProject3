using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private bool tracking;

    //[SerializeField] private bool shooter;
    //[SerializeField] private bool PlatformerEnemy;
    [SerializeField] private bool PlatformerBossAtk3;

    private Transform playerPos;
    private Vector3 target;
    private Vector3 origin;

    //For the slime projectile attack for the PlatformerBoss
    private float arcHeight = 1;
    private float x1;
    private float targetX;
    private float dist;
    private float nextX;
    private float baseY;
    private float height;


    void Start()
    {
        origin = this.transform.position;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);

        if (tracking || PlatformerBossAtk3)
        {
            Invoke("SelfDestruct", 8.0f);
            target.y = 0;
        }

        else
        {
            Invoke("SelfDestruct", 4.0f);
        }

    }

    void Update()
    {
        if (PlatformerBossAtk3)
        {
            // Compute the next position, with arc added in
            float x0 = origin.x;
            float x1 = target.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(origin.y, target.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

            // Rotate to face the next position, and then move there
            this.transform.rotation = LookAtTarget(nextPos - transform.position);
            this.transform.position = nextPos;


        }


        else if (tracking)
        {            
            transform.position = Vector3.MoveTowards(this.transform.position, playerPos.position, speed * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }

    public static Quaternion LookAtTarget(Vector3 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
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
