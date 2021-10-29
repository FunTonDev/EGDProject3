using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerEnemy : Enemy
{

    [SerializeField] private bool facingLeft;       //Direction Enemy is facing
    [SerializeField] private bool ranged;          //If true, enemy can perform ranged attacks
    [SerializeField] private bool fullRanged;     //If true, enemy can perform attack in any direction
    [SerializeField] private float range;        //Dist of their attack range
    private bool PlayerInRange;                 

    [SerializeField] private float StartTimeBtwAtk;   //Starting time till next attack
    private float timeBtwAtk;                       //Time left till next attack

    [SerializeField] private GameObject AttackObj;


    public bool GetFullRanged() { return fullRanged; }
    public float GetRange() { return range; }


    public override void ClassUpdate()
    {
        PlayerInRange = PlayerInSight();
        

        if (PlayerInRange && ranged)
        {
            Attack();            
        }

       
        Move(Vector3.zero);

        //Cooldown attack
        timeBtwAtk -= Time.deltaTime;
    }


    /*
    public override void Introduction()
    {
        Debug.Log("I AM THE BOX GHOST OF THE 2D PlatformingTest");
    }
    */

    public override void Move(Vector3 Pos)
    {
        
        //Enemy on a path
        if (pathNodes.Count != 0)
        {
            //Debug.Log(this.transform.eulerAngles);

            base.Move(PathFollow());

            //Moving Right
            if (this.transform.position.x < base.PathFollow().x)
            {
                //Debug.Log("Eneemy Moving right");
                this.transform.eulerAngles = new Vector3(0, 90, 0);
                facingLeft = false;
            }            

            //Moving left
            else
            {
                //Debug.Log("Enemy Moving left");
                this.transform.eulerAngles = new Vector3(0, 270, 0);
                facingLeft = true;
            }
        }

        else
        {
            if (facingLeft)
            { this.transform.position += this.transform.forward * moveSpd * Time.deltaTime; }

            else
            {this.transform.position += this.transform.forward * moveSpd * Time.deltaTime; }           

        }
                
    }

    public void Attack()
    {
        //If enemy attack cooldown is done
        if (timeBtwAtk <= 0)
        {
            Debug.Log("Enemy performed attack");
            if (ranged)
            {
                Vector3 spawnPos = this.transform.position;
                Vector3 dirToTarget;
                //Debug.Log("Ranged attacked called");
                //Vector3 spawnPos = this.transform.position + (this.transform.forward * 1);
                if (fullRanged)
                {
                    dirToTarget = (player.transform.position - this.transform.position).normalized;
                }

                else
                {
                    dirToTarget = this.transform.forward;                   
                }

                spawnPos += dirToTarget * 1;
                               
                Instantiate(AttackObj, spawnPos, Quaternion.identity);

            }

            timeBtwAtk = StartTimeBtwAtk;
        }
    }

    bool PlayerInSight()
    {
        return this.GetComponent<Platformer_FOV>().FindVisibleTargets();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        //Checks to see if 
        if(collision.collider.tag == "Player")
        {
            float xDiff = Mathf.Abs(this.transform.position.x - collision.collider.transform.position.x);
            float yDiff = collision.collider.transform.position.y - this.transform.position.y;

            //Player jumped on enemy
            if((yDiff > 0.5f) && (collision.relativeVelocity.y < 0) )
            {
                Debug.Log(string.Format("Enemy {0} was jumped on by Player", type));

                this.TakeDamage(health);               

                //collision.rigidbody.AddForce(Vector3.up * 3); Give player bounce
            }

            //Player takes damage
            else if(xDiff > 0.55f)
            {
                Debug.Log(string.Format("Enemy {0} hit Player", type));
            }
        }

        //Flip enemy direction
        //Make sure enemy didn't hit ground
        else if(rgbdy.velocity.y <= 0.01f && collision.collider.tag != "Ground")
        {                
            Debug.Log(string.Format("{0} flipped direction", this.name));

            facingLeft = !facingLeft;   
                
            if(pathNodes.Count != 0)
                { this.currNode = (this.currNode + 1) % pathNodes.Count;}

            else
                {this.transform.eulerAngles += new Vector3(0, 180, 0);}

        }
        

    }



}
