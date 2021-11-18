using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerAttack : MonoBehaviour
{
    [SerializeField] List<Transform> Atk1MovePattern;


    [SerializeField] private GameObject Atk1Obj;
    [SerializeField] private GameObject Atk3Obj;

    //For OmniShot
    int numOfShots = 8;
    int angle = 360;

    BossPlatformerMovement BossMovement;

    //Used to keep track of variables in during an attacking phase
    [SerializeField] List<int> AtksForPhase;
    [SerializeField] int AtkIndex = 0;    
    bool NewAtkPhase;
    bool attacking;
    bool attack1;
    bool attack2;
    bool attack3;

    int AtkPart;
    Vector3 Destination;
    bool moving;
    bool atDest;

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

        NewAtkPhase = true;
        attacking = false;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        moving = false; ;
        atDest = false;

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
                if (moving && AtkPart == 1)
                {
                    //Debug.Log(string.Format("BossPos: {0}, Destination: {1}, PathFollow{2}", this.transform.position, Destination, BossMovement.PathFollow()));
                    if(Destination == BossMovement.PathFollow())
                    {
                        BossMovement.LookAtPos(Destination);
                        BossMovement.MoveReg();
                    }

                    //Move onto next part
                    else { Atk1_P2(); moving = false; }
                }

                else if (AtkPart == 2)
                {
                    attacking = false;
                    attack1 = false;
                    Debug.Log("Boss Atk1 Finished");
                }
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
        atDest = false;
                
    }

    //Move to first node
    public void Atk1_P2()
    {
        //Debug.Log("Boss Atk1_P2 called");

        AtkPart = 2;
        //moving = true;

        //Look at other end
        Vector3 lastNode = Atk1MovePattern[4].position;       
        if (BossMovement.PathFollow() == Atk1MovePattern[3].position)
        {
            lastNode = Atk1MovePattern[0].position;
        }

        BossMovement.LookAtPos(lastNode);
        
    }
    

    
    IEnumerator Atk1Pattern()
    {
        attacking = true;

        //Figure out which side to start attack

        bool left = true;
        Vector3 firstNode = Atk1MovePattern[0].position;
        Vector3 lastNode = Atk1MovePattern[4].position;
        //If Boss is closer to right side
        if (Vector3.Distance(this.transform.position, Atk1MovePattern[0].position) > Vector3.Distance(this.transform.position, Atk1MovePattern[4].position))
        {left = false; firstNode = Atk1MovePattern[4].position; lastNode = Atk1MovePattern[0].position; }

        Debug.Log("firstNode = " + firstNode);
        BossMovement.LookAtPos(firstNode);
        BossMovement.SetPathNodes(Atk1MovePattern, left);


        Destination = BossMovement.PathFollow();
        moving = true;
        atDest = false;
        /*
        Vector2 dest = new Vector2(destV3.x, destV3.y);
        Vector2 BossPos = new Vector2(this.transform.position.x, this.transform.position.y);
        */

        /*
        while(destV3 == BossMovement.PathFollow())
        {
            BossMovement.MoveReg();
        }

        
        //Head to first node
        while (Vector2.Distance(BossPos, dest) > 0.2f)
        {
            BossMovement.MoveReg();
        }

        
        //Look at other side of screen
        BossMovement.LookAtPos(lastNode);

        /*
        //Allow boss to float for movement in the air
        BossMovement.SetGravity(false);

        int numOfAtks = 3;
        destV3 = BossMovement.PathFollow();
        dest= new Vector2(destV3.x, destV3.y);        
        BossPos = new Vector2(this.transform.position.x, this.transform.position.y);
        //Perform the 3 omni attacks at specific nodes
        while (true)
        {
            if(numOfAtks == 0)
            { break;}

            if(BossPos == dest)
            {
                Debug.Log("Boss perform  Omni-Attack");
                //OmniShot();
                yield return new WaitForSeconds(1);
                numOfAtks--;
            }

            else
            {
                BossMovement.MoveAtk(dest);
            }

            
            destV3 = BossMovement.PathFollow();
            dest = new Vector2(destV3.x, destV3.y);
            BossPos = new Vector2(this.transform.position.x, this.transform.position.y);
        }

        //Reset Gravity for boss and end attack
        BossMovement.SetGravity(false);                           
        
        */
        
        yield return new WaitForSeconds(1);

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
        Instantiate(Atk3Obj, spawnPos, Quaternion.LookRotation(dir));
    }

    public void Attack3()
    {
        StartCoroutine("Atk3Pattern");
    }

    IEnumerator Atk3Pattern()
    {
        attacking = true;

        yield return new WaitForSeconds(2);

        ArcThrow();

        yield return new WaitForSeconds(2);

        attacking = false;

        Debug.Log("Boss Atk3 Finished");
    }
}
