
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] protected string type;                     //Name (Ex: TestBox)

    [SerializeField] protected int phase;               //currPhase of the boss
    [SerializeField] protected List<float> health;      //List of the bosses health set at the begining of each of their phase
    [SerializeField] protected List<float> armor;       //List of the bosses armor set at the begining of each of their phase
    protected float currHealth;             
    protected float currArmor;
    
    protected Rigidbody rgbdy;


    // Start is called before the first frame update
    void Start()
    {
        Introduction();
        rgbdy = this.GetComponent<Rigidbody>();

        phase = 1;              
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void Introduction()
    {
        Debug.Log(string.Format("Indroducing Platformer Boss Enemy with health of {1}.", type, health));
    }

    void SetPhase()
    {
        currHealth = health[phase-1];
        currArmor = armor[phase - 1];
    }



    public void TakeDamage(float damage)
    {
        if (currArmor > 0)
        {
            currArmor -= (3 / 4) * damage;
            currHealth -= (1 / 4) * damage;
            Debug.Log(string.Format("Enemy {0} took {1} damage", type, (1 / 4) * damage));
        }
        else
        {
            currHealth -= damage;
            Debug.Log(string.Format("Enemy {0} took {1} damage", type, damage));
        }

        if (currHealth <= 0)
        { Death(); }

    }

    public void Death()
    {
        Debug.Log(string.Format("Enemy {0} destroyed", type));
        Destroy(this.gameObject);
    }
}
