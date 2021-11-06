using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Enemy : MonoBehaviour
{
    [SerializeField] private int TileVisionRange;   //How many tiles they can see in front of them

    [HideInInspector] public GameObject closestTile;


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

    /*
    public void RPGMoveUpdate()
    {
        float xClampVel = 0, yClampVel = 0;
        if (inputMan.inputX != 0)
        {
            playerRigB.AddForce(new Vector3(inputMan.inputX * xForce / 2, 0, 0), ForceMode.VelocityChange);
            xClampVel = (inputMan.inputX == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.x), 0, maxXVelocity) * Mathf.Sign(playerRigB.velocity.x);  //X move check
        }
        else if (inputMan.inputY != 0)
        {
            playerRigB.AddForce(new Vector3(0, 0, inputMan.inputY * yForce / 2), ForceMode.VelocityChange);
            yClampVel = (inputMan.inputY == 0) ? 0 : Mathf.Clamp(Mathf.Abs(playerRigB.velocity.z), 0, maxYVelocity) * Mathf.Sign(playerRigB.velocity.z);  //Y move check
        }
        else if (closestTile != null)
        {
            transform.position = closestTile.transform.position;//.new Vector3(position.x, transform.position.y, closestTile.transform.position.z);
        }
        playerRigB.velocity = new Vector3(xClampVel, playerRigB.velocity.y, yClampVel);
    }
    */

}
