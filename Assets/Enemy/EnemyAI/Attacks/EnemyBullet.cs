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

    private Rigidbody rb;
    private Transform playerPos;
    private Vector3 target;
    private Vector3 origin;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        origin = this.transform.position;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z);

        if(tracking && PlatformerBossAtk3)
        {
            
            Invoke("SelfDestruct", 3.5f);
            target.y = 0;
        }

        else if (tracking)
        {
            Invoke("SelfDestruct", 8.0f);
            target.y = 0;
        }

        else
        { Invoke("SelfDestruct", 4.0f);}

    }

    void Update()
    {
        /*
        if (PlatformerBossAtk3)
        {
            // Compute the next position, with arc added in
            float x0 = origin.x;
            float targetx = target.x + target.y;
            float dist = targetx - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, targetx, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(origin.y, target.y, (nextX - x0) / dist);
            //float baseY = Mathf.Lerp(origin.y, target.y, dist);
            float arcHeight = 2 * (nextX - x0) * (nextX - targetx) / (-0.25f * dist * dist);
            Vector3 nextPos = new Vector3(nextX, baseY + arcHeight, transform.position.z);

            // Rotate to face the next position, and then move there
            this.transform.rotation = LookAtTarget(nextPos - transform.position);
            this.transform.position = nextPos;                        
        }
        */

        if(tracking && PlatformerBossAtk3)
        {
            /*
            Vector3 dir = target - rb.position;
            dir.Normalize();
            Vector3.Cross(dir, this.transform.up);
            rb.velocity = transform.up * speed;
            */
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                        Quaternion.LookRotation(target - transform.position),
                                                1f * Time.deltaTime);
            this.transform.position += transform.forward * speed * Time.deltaTime;
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
        if (collision.gameObject.tag != "Boss")
        {
            if (collision.gameObject.name.Substring(0, 12) == "PlayerPrefab")
            {
                collision.gameObject.GetComponent<PlayerController>().HealthUpdate(-1);
            }
            SelfDestruct();
        }
                
    }
}
