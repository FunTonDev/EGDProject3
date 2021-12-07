using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    Animator BossAnim;
    bool PlayerEntered;

    // Start is called before the first frame update
    void Start()
    {
        BossAnim = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>();
        PlayerEntered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !PlayerEntered)
        {
            BossAnim.SetBool("Start", true);
            PlayerEntered = true;
            Debug.Log("Boss Fight Start!");
        }
    }
}
