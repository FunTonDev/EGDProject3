using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{

    //[SerializeField] private NavMeshAgent agent;

    [SerializeField] private bool ranged;     //Melee or ranged attacked
    [SerializeField] private bool follow;     //For ranged enemies, if true chase down player to shoot, else stay in place/path when attack
    [SerializeField] private bool sentry;     //Doesn't move from initial spot

    [SerializeField] private bool PlayerInSightRange, PlayerInAtkRange;
     private bool retreatMode;

    [SerializeField] private float detctRadius;      //Radius of their detection Circle
    [SerializeField] private float AtkDist;     //Distance of their attack
    [SerializeField] private float RetreatDist; //Distance of how close enemy can be to player
    [SerializeField] private float RotSpd;      //How fast enemy turns
    [SerializeField] private float AtkAngle = 60.0f;

    [SerializeField] private float timeBtwAtk;        //Time left till next attack
    [SerializeField] private float StartTimeBtwAtk;   //Starting time till next attack

    [SerializeField] private GameObject AttackObj;
    //[SerializeField] private LayerMask layerMask;

    public float GetDetctRadius() { return detctRadius; }
    public float GetAtkDist() { return AtkDist; }
    public float GetAtkAngle() { return AtkAngle; }



    public override void ClassUpdate()
    { 
        PlayerInSightRange = PlayerInDetectionRange();
        PlayerInAtkRange = PlayerInAttackVision();
        //retreatMode = PlayerInRetreatRange();

        //If enemy should be attacking the player
        if(PlayerInAtkRange)
            { AttackPlayer();}

        //If enemy should be chasing the player
        else if(PlayerInSightRange && !PlayerInAtkRange)
            { ChasePlayer();}


        //If enemy isn't chasing or attacking player
        else if(!PlayerInSightRange && !PlayerInAtkRange)
            { Patroling();}
        
        //Cooldown attack
        timeBtwAtk -= Time.deltaTime;
    }

    //Follow path, or stay still if there is no path
    private void Patroling()
    {
        transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);

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
        transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
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
            //base.Move(PathFollow());
            NavAgent.SetDestination(PathFollow());
        }

        //Enemy follows player to chase him down
        else if (follow)
        {
            this.transform.LookAt(player.transform.position);
            //base.Move(player.transform.position);
            NavAgent.SetDestination(player.transform.position);
        }

    }

    private void AttackPlayer()
    {
        Attack();

        transform.position = new Vector3(this.transform.position.x, 0.5f, this.transform.position.z);
        //For enemies that don't follow the player when attack, continuing to stay on a path while firing
        if (pathNodes.Count != 0 && !follow)
        {
            this.transform.LookAt(player.transform.position);
            //base.Move(PathFollow());
            NavAgent.SetDestination(PathFollow());
        }

        //If sentry enemy, contine to rotate towards player
        else if (sentry)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                   Quaternion.LookRotation(player.transform.position - this.transform.position),
                                           RotSpd * Time.deltaTime);
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

                //Instantiate(Projectile, this.transform.position, Quaternion.identity);                    
            }

            //Continue to move towards player to hit them
            else
            {
                Debug.Log("Melee attacked called");
                Move(Vector3.zero);
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

    public bool PlayerInRetreatRange()
    {
        bool retreat = false;

        if (Vector3.Distance(this.transform.position, player.transform.position) <= RetreatDist)
        {
            //Debug.Log("Player in Retreat Range");
            retreat = true;
        }

        return retreat;
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
        }
    }


}
