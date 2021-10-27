using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{

    //[SerializeField] private NavMeshAgent agent;

    [SerializeField] private bool ranged;    //Melee or ranged attacked
    [SerializeField] private bool follow;   //For ranged enemies, if true chase down player to shoot, else stay in place/path when attack
    [SerializeField] private bool sentry;  //Doesn't move from initial spot
    private bool agroMode;                //Enemy detection radius expands after player enters it visions or attacks enemy

    [SerializeField] private bool PlayerInSightRange, PlayerInAtkRange, PlayerInBufferRange;

    private float detctRadius;                        //Radius of their detection Circle
    private float AtkDist;                           //Distance of attack
    [SerializeField] private float mindetctRadius;  //minRadius of their detection Circle
    [SerializeField] private float maxdetctRadius; //maxRadius of their detection Circle    
    [SerializeField] private float minAtkDist;    //minDistance of attack
    [SerializeField] private float maxAtkDist;   //maxDistance of attack
    [SerializeField] private float BufferDist;  //Distance of how close enemy can be to player
    [SerializeField] private float RotSpd;     //How fast enemy turns
    [SerializeField] private float AtkAngle;  //Angle of their cone of vision for attacking
    [SerializeField] private float rotAngle; //How far enemy can turn

    private float timeBtwAtk;        //Time left till next attack
    [SerializeField] private float StartTimeBtwAtk;   //Starting time till next attack

    [SerializeField] private GameObject AttackObj;

    public float GetDetctRadius() { return detctRadius; }
    public float GetAtkDist() { return AtkDist; }
    public float GetAtkAngle() { return AtkAngle; }
    public float GetBufferDist() { return BufferDist; }



    public override void ClassUpdate()
    {

        if (agroMode)
        {
            detctRadius = maxdetctRadius;
            AtkDist = maxAtkDist;
        }

        else
        {
            detctRadius = mindetctRadius;
            AtkDist = minAtkDist;
        }

        PlayerInSightRange = PlayerInDetectionRange();
        PlayerInAtkRange = PlayerInAttackVision();
        PlayerInBufferRange = PlayerInBufferCircle();
               

        //If enemy should be attacking the player
        if (PlayerInAtkRange)
        {
            agroMode = true;
            AttackPlayer();
        }

        //If enemy should be chasing the player
        else if(PlayerInSightRange && !PlayerInAtkRange && !PlayerInBufferRange)
        {
            agroMode = true;
            ChasePlayer();
        }


        //If enemy isn't chasing or attacking player
        else if(!PlayerInSightRange && !PlayerInAtkRange)
        {
            agroMode = false;
            Patroling();
        }

        else if(PlayerInBufferRange)
        {
            
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                            Quaternion.LookRotation(player.transform.position - this.transform.position),
                                                        6.0f * Time.deltaTime);          

        }

        base.SetAxisLevel();

        //Cooldown attack
        timeBtwAtk -= Time.deltaTime;
    }

    //Follow path, or stay still if there is no path
    private void Patroling()
    {
        //transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);

        //Have Sentry enemy rotate
        if (sentry)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                        Quaternion.LookRotation(this.transform.right),
                                                0.5f * Time.deltaTime);
            
        }

        //Follow path if one given
        else if (pathNodes.Count != 0)
        {
            Vector3 newPos = PathFollow();
            this.transform.LookAt(new Vector3(newPos.x, this.transform.position.y, newPos.z));
            NavAgent.SetDestination(newPos);
            //base.Move(newPos);
        }
    }

    private void ChasePlayer()
    {
        //transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
        //Sentry stays in place but rotates to look at Player's current position
        if (sentry)
        {
            //agent.SetDestination(player.transform.position); //For Potential NavMesh
            transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                   Quaternion.LookRotation(player.transform.position - this.transform.position),
                                           RotSpd * Time.deltaTime);
        }

        //Enemy continues on set path, but looks at player during so
        else if (!follow)
        {
            this.transform.LookAt(player.transform.position);
            NavAgent.SetDestination(PathFollow());
        }

        //Enemy follows player to chase him down if they are not within buffer dist
        else if (follow)
        {
            this.transform.LookAt(player.transform.position);
            NavAgent.SetDestination(player.transform.position);
        }

    }

    private void AttackPlayer()
    {
        Attack();


        NavAgent.SetDestination(this.transform.position);
        //transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);

        //For enemies that don't follow the player when attack, continuing to stay on a path while firing
        if (pathNodes.Count != 0 && !follow)
        {
            this.transform.LookAt(player.transform.position);
            NavAgent.SetDestination(PathFollow());
        }

        //If sentry enemy, contine to rotate towards player
        else if (sentry)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                   Quaternion.LookRotation(player.transform.position - this.transform.position),
                                           RotSpd * Time.deltaTime);
        }

        else if(follow && !PlayerInBufferRange)
        {
            this.transform.LookAt(player.transform.position);
            NavAgent.SetDestination(player.transform.position);
        }

    }


    public void Attack()
    {
        //If enemy attack cooldown is done
        if(timeBtwAtk <= 0)
        {
            if (ranged)
            {
                Debug.Log("Ranged attacked called");

                Vector3 spawnPos = this.transform.position + (this.transform.forward * 1);
                spawnPos.y = 0.5f;
                Instantiate(AttackObj, spawnPos, this.transform.rotation);                    
            }

            //Melee attack
            else
            {
                Debug.Log("Melee attacked called");

                Vector3 spawnPos = this.transform.position + (this.transform.forward * 0.75f);
                spawnPos.y = 0.5f;
                Instantiate(AttackObj, spawnPos, this.transform.rotation);            
            }

            timeBtwAtk = StartTimeBtwAtk;
        }        
    }

    //Perform a check to see if player is within the enemy range
    public bool PlayerInDetectionRange()
    {
        bool playerFound = false;

        if(Vector3.Distance(this.transform.position, player.transform.position) <= detctRadius)
        {
            //Debug.Log("Player in Detection Range");
            playerFound = true;
        }

        return playerFound;
    }

    //Perform cone check with the FieldOfView script see if player is within the enemy's vision
    public bool PlayerInAttackVision()
    {
        return this.GetComponent<FieldOfView>().FindVisibleTargets();

    }

    public bool PlayerInBufferCircle()
    {
        bool stop = false;

        if (Vector3.Distance(this.transform.position, player.transform.position) <= BufferDist)
        {
            //Debug.Log("Player in Retreat Range");
            stop = true;
        }

        return stop;
    }

    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(2);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Player and enemy collider
        if (collision.collider.tag == "Player")
        {
            //If melee enemy, player takes damage
            if (!ranged)
            {

            }

        }

        //Bullet hit enemy, enemy takes damage
        else if(collision.collider.tag == "Bullet")
        {
            base.TakeDamage(5);
            agroMode = true;
            this.transform.LookAt(player.transform.position);
        }
    }


}