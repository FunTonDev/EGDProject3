using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerAttack : MonoBehaviour
{
    [SerializeField] private GameObject Atk1Obj;
    [SerializeField] private GameObject Atk3Obj;

    int numOfShots = 8;
    int angle = 360;


    //Used to keep track of variables in during an attacking phase
    [SerializeField] List<int> AtksForPhase;
    [SerializeField] int AtkIndex = 0;
    bool attacking;
    bool NewAtkPhase;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
        StartCoroutine("Atk1Pattern");
    }


    IEnumerator Atk1Pattern()
    {
        attacking = true;

        yield return new WaitForSeconds(5);

        OmniShot();

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

        yield return new WaitForSeconds(5);

        ArcThrow();

        attacking = false;

        Debug.Log("Boss Atk3 Finished");
    }
}
