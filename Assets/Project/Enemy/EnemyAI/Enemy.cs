
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string type;                     //Name (Ex: TestBox)

    public float health;
    public float atkDamage;
    public float moveSpd;

    public int detectionLvl;                // 0,1,2

    public bool platformer, shooter, rpg;

    public Rigidbody rgbdy;

    public List<Transform> pathNodes;       
    public int currNode = 0;


    // Start is called before the first frame update
    void Start()
    {
        Introduction();
        rgbdy = this.GetComponent<Rigidbody>();
    }

    
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    

    public virtual void Introduction()
    {
        string genre = "";
        if (platformer) { genre += "Platformer"; }
        else if (shooter) { genre += "Shooter"; }
        else if (rpg) { genre += "RPG"; }


        Debug.Log(string.Format("Indroducing {0} enemy with health of {1} in {2} genre.", type, health, genre));
    }
    
    //Gets the transform of the current node
    public Vector3 PathFollow()
    {
        Vector3 newPos = Vector3.zero;

        if (1 < Vector3.Distance(this.transform.position, pathNodes[this.currNode].GetComponent<Transform>().position))
        { newPos = pathNodes[this.currNode].GetComponent<Transform>().position; }

        else
        {
            this.currNode = (this.currNode + 1) % pathNodes.Count;
            newPos = pathNodes[this.currNode].GetComponent<Transform>().position;
        }

        return newPos;
    }


    public virtual void Move()
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, PathFollow(), moveSpd * Time.deltaTime);
        if (platformer)
            { temp.z = -1; }

        transform.position = temp;
        //rgbdy.AddForce(Vector3.MoveTowards(transform.position, PathFollow(), moveSpd * Time.deltaTime));
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(string.Format("Enemy {0} took {1} damage", type, damage));

        if (health <= 0)
            { Death(); }
    }

    public void Death()
    {
        Debug.Log(string.Format("Enemy {0} destroyed", type));
        Destroy(this.gameObject);
    }
}
