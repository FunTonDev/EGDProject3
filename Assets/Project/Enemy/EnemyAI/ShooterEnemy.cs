using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{

    //[SerializeField] private NavMeshAgent agent;

    [SerializeField] private bool ranged;     //Melee or ranged attacked
    [SerializeField] private bool follow;      //For ranged enemies, if true chase down player to shoot, else stay in place/path when attack

    [SerializeField] private bool chaseMode;
    [SerializeField] private bool attackMode;
    [SerializeField] private bool retreatMode;

    [SerializeField] private float radius;      //Radius of their detection Circle
    [SerializeField] private float AtkDist;     //Distance of their attack
    [SerializeField] private float RetreatDist; //Distance of how close enemy can be to player

    [SerializeField] private float timeBtwAtk;        //Time left till next attack
    [SerializeField] private float StartTimeBtwAtk;   //Starting time till next attack

    [SerializeField] private GameObject Projectile;    


    public override void ClassUpdate()
    {

        chaseMode = PlayerInDetectionRange();
        attackMode = PlayerInAttackRange();
        //retreatMode = PlayerInRetreatRange();

        //Check if enemy should attack player
        if (chaseMode)
        {
            //Attack
            if (attackMode)
            {
                Attack();
                
                //For enemies that don't follow the player when attack, either continuing to stay on a path or in place
                if (!follow)
                {
                    Move(Vector3.zero);
                }
            }

            //Move towards Player to reach attack distance
            else
            {
                Debug.Log("Moving towards Players");
                Move(Vector3.zero);
            }
            
        }

        //Either return to path or stand in place if still == true when there is no enemy
        else
        {
            //If Enemy doesn't have path, stays still
            Move(Vector3.zero);
        }
        
        //Cooldown attack
        timeBtwAtk -= Time.deltaTime;
    }


    public override void Move(Vector3 Pos)
    {
        //If Enemy must chase player
        if (chaseMode && follow)
        {
            //agent.SetDestination(player.transform.position); //For Potential NavMesh
            base.Move(player.transform.position);
        }

        //Follow path if one given
        else if (pathNodes.Count != 0 )
        {
            base.Move(PathFollow());
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

                Instantiate(Projectile, this.transform.position, Quaternion.identity);                    
            }

            //Continue to move towards player to hit them
            else
            {
                Debug.Log("Melee attacked called");
                Move(Vector3.zero);
            }
        }        
    }


    public bool PlayerInDetectionRange()
    {
        bool playerFound = false;

        if(Vector3.Distance(this.transform.position, player.transform.position) <= radius)
        {
            Debug.Log("Player in Detection Range");
            playerFound = true;
        }

        return playerFound;
    }


    public bool PlayerInAttackRange()
    {
        bool attack = false;

        if (Vector3.Distance(this.transform.position, player.transform.position) <= AtkDist)
        {
            Debug.Log("Player in Attack Range");
            attack = true;
        }

        return attack;
    }

    public bool PlayerInRetreatRange()
    {
        bool retreat = false;

        if (Vector3.Distance(this.transform.position, player.transform.position) <= RetreatDist)
        {
            Debug.Log("Player in Attack Range");
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
