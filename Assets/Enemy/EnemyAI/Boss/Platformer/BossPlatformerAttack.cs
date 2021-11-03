using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerAttack : MonoBehaviour
{
    [SerializeField] private GameObject AttackObj;

    int numOfShots = 8;
    int angle = 360;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Shoot projectiles in all directions
    public void Attack1()
    {
        int numOfShots = 8;
        float angle = 45f;
        for(int i = 0; i < numOfShots; i++)
        {
            Quaternion rotation = this.transform.rotation;
            Quaternion rotationMod = Quaternion.AngleAxis(angle * i, this.transform.forward);
            Vector3 dir = rotation * rotationMod * Vector3.forward;
            Debug.DrawRay(this.transform.position, dir, Color.red);

            Vector3 spawnPos = this.transform.position + dir;
            Instantiate(AttackObj, spawnPos, Quaternion.LookRotation(dir));
        }
    }

    //perform a dash that does damage on contact 
    public void Attack2()
    {

    }


    //Launch slime projectiles in an arc towards player
    public void Attack3()
    {

    }
}
