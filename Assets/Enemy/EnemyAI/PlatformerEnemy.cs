using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerEnemy : Enemy
{
    [Header("Platformer Enemey Class")]

    [SerializeField] private bool facingLeft;       //Direction Enemy is facing
    [SerializeField] private bool vertical;        //Is their path vertical
    [SerializeField] private bool ranged;         //If true, enemy can perform ranged attacks
    [SerializeField] private bool fullRanged;    //If true, enemy can perform attack in any direction
    [SerializeField] private float range;       //Dist of their attack range
    private bool PlayerInRange;
    private bool boss;

    [SerializeField] private float StartTimeBtwAtk;   //Starting time till next attack
    private float timeBtwAtk;                       //Time left till next attack

    [SerializeField] private GameObject AttackObj;

    private SpriteRenderer sprite;

    //For the FOV Script
    public bool GetFullRanged() { return fullRanged; }
    public float GetRange() { return range; }


    public override void ClassStart()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (facingLeft)
        { Face(true); }

        else
        { Face(false); }
    }

    public override void ClassUpdate()
    {
        PlayerInRange = PlayerInSight();
        

        if (PlayerInRange && ranged)
        {
            //Look at player wheen shooting
            if (vertical)
            { LookAtPos(player.transform.position);}
            
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

            base.Move(base.PathFollow());

            //Moving Right
            if (this.transform.position.x < base.PathFollow().x)
            {
                Face(false);
            }            

            //Moving left
            else
            {
                Face(true);
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
            //Debug.Log("Enemy performed attack");
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

                Quaternion rotation = Quaternion.LookRotation(dirToTarget);

                spawnPos += dirToTarget * 1;                               
                Instantiate(AttackObj, spawnPos, rotation);

            }

            timeBtwAtk = StartTimeBtwAtk;
        }
    }

    bool PlayerInSight()
    {
        return this.GetComponent<Platformer_FOV>().FindVisibleTargets();
    }

    
    void Face(bool faceLeft)
    {
        if (faceLeft)
        {
            //Debug.Log("Enemy facing left");
            this.transform.eulerAngles = new Vector3(0, 270, 0);
            facingLeft = true;
        }

        else
        {
            //Debug.Log("Eneemy Moving right");
            this.transform.eulerAngles = new Vector3(0, 90, 0);
            facingLeft = false;
        }
    }

    public void LookAtPos(Vector3 pos)
    {
        //Vector3 flipped = this.transform.localScale;
        //flipped.z *= -1f;

        if ((pos.x < this.transform.position.x) && !facingLeft)
        {
            //this.transform.localScale = flipped;
            Face(false);

            //Debug.Log("Boss Flipped"); 
        }

        else if ((pos.x > this.transform.position.x) && facingLeft)
        {
            //this.transform.localScale = flipped;
            Face(true);

            //Debug.Log("Boss Flipped");
        }
    }


        private void OnCollisionEnter(Collision collision)
    {
/*        //Checks to see if 
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
*/
        //Flip enemy direction
        //Make sure enemy didn't hit ground
        //else if(rgbdy.velocity.y <= 0.01f && collision.collider.tag != "Ground")
        if (collision.collider.tag != "Ground" && rgbdy.velocity.y < 1f)
        {                
            //Debug.Log(string.Format("{0} flipped direction", this.name));

            facingLeft = !facingLeft;   
                
            if(pathNodes.Count != 0)
                { this.currNode = (this.currNode + 1) % pathNodes.Count;}

            else
                {this.transform.eulerAngles += new Vector3(0, 180, 0);}

        }
        

    }



}
