using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerAttack : MonoBehaviour
{
    [SerializeField] List<Transform> Atk1MovePattern;


    [SerializeField] private GameObject Atk1Obj;
    [SerializeField] private GameObject Atk3Obj;
        
    BossPlatformerMovement BossMovement;

    //Used to keep track of variables in during an attacking phase
    [SerializeField] List<int> AtksForPhase;
    [SerializeField] int AtkIndex = 0;    
    bool NewAtkPhase;
    bool attacking;

    //Used to help object manuver for specific patterns during attacks
    bool attack1;
    bool attack2;
    bool attack3;

    int AtkPart;
    int numOfAtks;
    Vector3 Destination;
    bool moving;
    bool charging;
    

    public void ResetAtkforPhase() { AtksForPhase = new List<int>(); }
    public void AddAtkforPhase(int i) { AtksForPhase.Add(i); }
    public void SetAtkIndex(int index) { AtkIndex = index; }
    public void SetAttacking(bool b) { attacking = b; }
    public void SetNewAtkPhase(bool b) { attacking = b; }

    public int GetAtkForPhase(int index) { return AtksForPhase[index]; }
    public int GetAtkIndex() { return AtkIndex; }
    public bool GetAttacking() {return attacking; }
    public bool GetNewAtkPhase() { return NewAtkPhase; }





    // Start is called before the first frame update
    void Start()
    {
        //Attack1();
        //Attack3();

        ArcThrow();

        NewAtkPhase = true;
        attacking = false;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        moving = false; ;
        charging = false;

        AtkPart = 0;

        BossMovement = this.gameObject.GetComponent<BossPlatformerMovement>();


    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            if (attack1)
            {
                //Perform during Atk1 part 1
                if (moving && AtkPart == 1)
                {
                    //Debug.Log(string.Format("BossPos: {0}, Destination: {1}, PathFollow{2}", this.transform.position, Destination, BossMovement.PathFollow()));
                    //Boss still hasn't reached destination
                    if(Destination == BossMovement.PathFollow())
                    {
                        BossMovement.LookAtPos(Destination);
                        BossMovement.MoveReg();
                    }

                    //Move onto next part
                    else { Atk1_P2(); }
                }

                //Perform during Atk1 part 2
                else if (AtkPart == 2)
                {
                    if (numOfAtks > 0)
                    {
                        //Boss still hasn't reached destination
                        if (Destination == BossMovement.PathFollow() && moving)
                            { BossMovement.MoveAtk(Destination); }

                        //Boss has reached destination and needs to start charging attack
                        else if (!charging)
                            { StartCoroutine("Atk1_P2_Charging");}
                    }

                    //Move onto next part
                    else { AtkPart = 3; }
                    
                }

                //End Atk
                else if(AtkPart == 3)
                    { StartCoroutine("Atk1_End"); }
            }

            if (attack3)
            {
                //Perform during Atk3 part 1
                if (moving && AtkPart == 1)
                {
                    //Debug.Log(string.Format("BossPos: {0}, Destination: {1}, PathFollow{2}", this.transform.position, Destination, BossMovement.PathFollow()));
                    //Boss still hasn't reached destination
                    if (Destination == BossMovement.PathFollow())
                    {
                        BossMovement.LookAtPos(Destination);
                        BossMovement.MoveReg();
                    }

                    //Move onto next part
                    else { Atk1_P2(); }
                }

                //Perform during Atk3 part 2
                else if (AtkPart == 2)
                {
                    if (numOfAtks > 0)
                    {
                        //Boss has reached destination and needs to start charging attack
                        if (!charging)
                        { StartCoroutine("Atk3_P2_Charging"); }
                    }

                    //Move onto next part
                    else { AtkPart = 3; }
                }

                //End Atk
                else if (AtkPart == 3)
                { StartCoroutine("Atk3_End"); }

            }

        }        
    }


    //Shoot projectiles in all directions
    void OmniShot()
    {
        int numOfShots = 8;
        float angle = 45f;
        for (int i = 0; i < numOfShots; i++)
        {
            Quaternion rotation = this.transform.rotation;
            Quaternion rotationMod = Quaternion.AngleAxis(angle * i, this.transform.forward);
            Vector3 dir = rotation * rotationMod * Vector3.forward;
            Debug.DrawRay(this.transform.position, dir, Color.red);

            Vector3 spawnPos = this.transform.position + dir;
            Instantiate(Atk1Obj, spawnPos, Quaternion.LookRotation(dir));
        }
    }

    public void Attack1()
    {
        //StartCoroutine("Atk1Pattern");

        Debug.Log("Boss Attack1() called");
        attacking = true;
        attack1 = true;
        
        Atk1_P1();
    }    

    //Figure out which side to start attack and move towards it
    public void Atk1_P1()
    {
        //Debug.Log("Boss Atk1_P1() called");

        AtkPart = 1;
        bool left = true;
        Vector3 firstNode = Atk1MovePattern[0].position;

        //If Boss is closer to right side
        if (Vector3.Distance(this.transform.position, Atk1MovePattern[0].position) > Vector3.Distance(this.transform.position, Atk1MovePattern[4].position))
        { left = false; firstNode = Atk1MovePattern[4].position; }
          
        BossMovement.SetPathNodes(Atk1MovePattern, left);
        BossMovement.LookAtPos(firstNode);
        //Debug.Log("firstNode = " + firstNode);
        //Debug.Log("left is " + left);

        Destination = firstNode;
        moving = true;                
    }

    //Move to first node
    public void Atk1_P2()
    {
        //Debug.Log("Boss Atk1_P2 called");
        Destination = BossMovement.PathFollow();
        AtkPart = 2;
        moving = true;

        //Look at other end
        Vector3 lastNode = Atk1MovePattern[4].position;       
        if (BossMovement.PathFollow() == Atk1MovePattern[3].position)
            { lastNode = Atk1MovePattern[0].position;}
        BossMovement.LookAtPos(lastNode);

        //Allow boss to float for movement in the air
        BossMovement.SetGravity(false);

        numOfAtks = 3; //Num of nodes enemy must vist to shoot its omni attack             
    }

    //For attack 
    IEnumerator Atk1_P2_Charging()
    {
        moving = false;
        charging = true;

        Debug.Log("Boss Atk1 Charging...");
        yield return new WaitForSeconds(2);
        OmniShot();
        Debug.Log("Boss Atk1 Fired!");
        yield return new WaitForSeconds(1);

        moving = true;
        charging = false;
        numOfAtks--;

        Destination = BossMovement.PathFollow();
    }

    //Reset Gravity for boss and end attack along with providing cooldown for enemy
    IEnumerator Atk1_End()
    {
        attack1 = false;

        yield return new WaitForSeconds(1);
       
        BossMovement.SetGravity(true);

        yield return new WaitForSeconds(2);

        attacking = false;
        
        Debug.Log("Boss Atk1 Finished");       
    } 


    //perform a dash that does damage on contact 
    public void Attack2()
    {
        StartCoroutine("Atk2Pattern");
    }

    IEnumerator Atk2Pattern()
    {
        attacking = true;

        yield return new WaitForSeconds(5);

        attacking = false;

        Debug.Log("Boss Atk2 Finished");
    }

    //Launch slime projectiles in an arc towards player
    void ArcThrow()
    {
        Quaternion rotation = this.transform.rotation;
        Quaternion rotationMod = Quaternion.AngleAxis(45f, this.transform.forward);
        Vector3 dir = rotation * rotationMod * Vector3.forward;

        Vector3 spawnPos = this.transform.position + dir;
        spawnPos.y += 1;
        Instantiate(Atk3Obj, spawnPos, Quaternion.LookRotation(dir));
    }

    public void Attack3()
    {
        Debug.Log("Boss Attack3() called");
        attacking = true;
        attack3 = true;

        Atk3_P1();
    }

    //Figure out which side to start attack and move towards it
    public void Atk3_P1()
    {
        AtkPart = 1;
        bool left = true;
        Vector3 firstNode = Atk1MovePattern[0].position;

        Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        //When leftside > PlayerPos.x > bossPos.x  
        if ((this.transform.position.x < PlayerPos.x) && PlayerPos.x < (Atk1MovePattern[0].position.x - 5))
        { left = false; firstNode = Atk1MovePattern[4].position; }

        BossMovement.SetPathNodes(Atk1MovePattern, left);
        //BossMovement.LookAtPos(PlayerPos);
        BossMovement.LookAtPos(firstNode);

        Destination = firstNode;
        moving = true;
    }

    //Set values for attack
    public void Atk3_P2()
    {
        moving = false;        
        AtkPart = 2;
        numOfAtks = 3; //Num of times enemy shoots projectile
    }

    //For attack 
    IEnumerator Atk3_P2_Charging()
    {
        moving = false;
        charging = true;

        Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        BossMovement.LookAtPos(PlayerPos);

        Debug.Log("Boss Atk3 Charging...");
        yield return new WaitForSeconds(1);
        ArcThrow();
        Debug.Log("Boss Atk3 Fired!");
        yield return new WaitForSeconds(3);

        charging = false;
        numOfAtks--;
    }

    //End attack
    IEnumerator Atk3_End()
    {
        attack3 = false;

        yield return new WaitForSeconds(1);

        BossMovement.SetGravity(true);

        yield return new WaitForSeconds(2);

        attacking = false;

        Debug.Log("Boss Atk3 Finished");
    }
}
