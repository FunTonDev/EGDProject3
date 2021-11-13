
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Base Class Variables")]
    [SerializeField] protected string type;                     //Name (Ex: TestBox)

    [SerializeField] protected float health;
    [SerializeField] protected float atkDamage;
    [SerializeField] protected float moveSpd;
    [SerializeField] protected float armor;

    [SerializeField] protected int detectionLvl;                // 0,1,2
    protected float axisLevel;

    [Header("Genre")]
    [SerializeField] protected bool platformer;
    [SerializeField] protected bool shooter;
    [SerializeField] protected bool rpg;
    

    [SerializeField] protected List<Transform> pathNodes;
    protected int currNode = 0;

    protected Rigidbody rgbdy;
    protected GameObject player;

    protected NavMeshAgent NavAgent;    

    // Start is called before the first frame update
    void Start()
    {
        Introduction();
        ClassStart();

        rgbdy = this.GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        if (shooter || rpg )
        {
            axisLevel = player.transform.position.y;
            if (shooter)
            {
                NavAgent = GetComponent<NavMeshAgent>();
                NavAgent.speed = moveSpd;
            }
            
        }

        else
        {
            axisLevel = player.transform.position.z;
        }
    }


    // Update is called once per frame
    void Update()
    {
        ClassUpdate();

        if (shooter)
        {
            axisLevel = player.transform.position.y;
        }

        rgbdy.velocity = new Vector3(0, rgbdy.velocity.y, 0);

        /*
        else
        {
            axisLevel = player.transform.position.z;
        }
        */
    }
    

    //A start function for each specfic class to be define behaviours specific to them
    public virtual void ClassStart()
    {

    }

    //An update function for each specfic class to be define behaviours specific to them
    public virtual void ClassUpdate()
    {

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
    public virtual Vector3 PathFollow()
    {
        Vector3 newPos = this.transform.position;

        if (1 < Vector3.Distance(this.transform.position, pathNodes[this.currNode].GetComponent<Transform>().position))
        { newPos = pathNodes[this.currNode].GetComponent<Transform>().position; }

        else
        {
            this.currNode = (this.currNode + 1) % pathNodes.Count;
            newPos = pathNodes[this.currNode].GetComponent<Transform>().position;
        }

        return newPos;
    }

    public virtual void Move(Vector3 Pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, Pos, moveSpd * Time.deltaTime);
        SetAxisLevel();
    }

    public void SetAxisLevel()
    {
        Vector3 temp = this.transform.position;
        if (shooter || rpg)
        { temp.y = axisLevel; }

        else if (platformer)
        { temp.z = axisLevel; }
                
        this.transform.position = temp;
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
    }

    public float GetAtkDamage()
    {
        return atkDamage;
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float damage)
    {
        if(armor > 0)
        {
            armor -= (3 / 4) * damage;
            health -= (1 / 4) * damage;
            Debug.Log(string.Format("Enemy {0} took {1} damage", type, (1 / 4)* damage));
        }
        else
        {
            health -= damage;
            Debug.Log(string.Format("Enemy {0} took {1} damage", type, damage));
        }      
        
        if (health <= 0)
            { Death(); }

    }

    private void Death()
    {
        Debug.Log(string.Format("Enemy {0} destroyed", type));
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider coll)
    {
        if(coll.tag == "Volume")
        {
            TriggerVolume tV = coll.GetComponent<TriggerVolume>();
            if(tV.primaryGenre == States.GameGenre.Platformer)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, tV.transform.position.z);
                axisLevel = tV.transform.position.z;
            }

            else if (tV.primaryGenre == States.GameGenre.RPG)
            {
                this.transform.position = new Vector3(this.transform.position.x, tV.transform.position.y, this.transform.position.z);
                axisLevel = tV.transform.position.y;
            }

        }
    }
}
