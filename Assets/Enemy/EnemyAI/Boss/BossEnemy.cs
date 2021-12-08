
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private string type;                     //Name (Ex: TestBox)

    [SerializeField] private int phase;               //currPhase of the boss
    [SerializeField] public float health;      
    [SerializeField] private float armor;

    float axisLevel;

    private bool invincible;
    private bool vulnarable;

    protected Rigidbody rgbdy;

    public void SetInvincible(bool t) { invincible = t; }
    public void SetVulnarable(bool t) { vulnarable = t; }
    public bool GetInvincible() { return invincible; }
    public bool GetVulnarablity() { return vulnarable; }


    // Start is called before the first frame update
    void Start()
    {
        Introduction();
        rgbdy = this.GetComponent<Rigidbody>();

        invincible = false;
        vulnarable = false;

        rgbdy.useGravity = true;
                        
        axisLevel = GameObject.FindGameObjectWithTag("Player").transform.position.z;
    }


    // Update is called once per frame
    void Update()
    {
        SetAxisLevel();
    }
    
    public virtual void Introduction()
    {
        //Debug.Log(string.Format("Indroducing Platformer Boss Enemy with health of {1}.", type, health));
    }

    public void SetAxisLevel()
    {
        Vector3 temp = this.transform.position;
        temp.z = axisLevel;
               
        this.transform.position = temp;
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
    }

    public void TakeDamage(float damage)
    {
        if (vulnarable)
        {
            if (armor > 0)
            {
                armor -= (3 / 4) * damage;
                health -= (1 / 4) * damage;
                Debug.Log(string.Format("Platform Boss took {0} damage to armor and {1} damage to health", (3 / 4) * damage, (1 / 4) * damage));
            }
            else
            {
                health -= damage;
                Debug.Log(string.Format("Platform Boss took {0} damage", damage));
            }

            Debug.Log(string.Format("Platform Boss Current Health and Armor: {0} & {1}", health, armor));

            if (health <= 0)
            { Death(); }
        }
    }

    public void BecomeVulnarable()
    {
        StartCoroutine("VulnarableTimer");
    }

    IEnumerator VulnarableTimer()
    {
        vulnarable = true;
        Debug.LogError("Platformer Boss is now Vulnarable");
        yield return new WaitForSeconds(4.5f);

        vulnarable = false;
    }


    public void Death()
    {
        Debug.Log(string.Format("Enemy {0} destroyed", type));
        Destroy(this.gameObject);
    }

}
