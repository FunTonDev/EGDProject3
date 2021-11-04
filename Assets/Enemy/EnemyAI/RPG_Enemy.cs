using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Enemy : MonoBehaviour
{
    [SerializeField] private int TileVisionRange;   //How many tiles they can see in front of them
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckForPlayer()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, TileVisionRange))
                    {
            if (hitInfo.collider.tag == "Player")
            {
                Debug.Log("Player spotted!");
                return true;
            }
        }

        return false;
    }
}
