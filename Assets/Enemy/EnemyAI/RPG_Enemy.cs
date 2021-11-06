using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Enemy : Enemy
{
    [SerializeField] private int TileVisionRange;   //How many tiles they can see in front of them

    
    [HideInInspector] public GameObject closestTile;

    private bool pathForward;

    public override void ClassStart()
    {
        pathForward = true;
    }

    public override void ClassUpdate()
    {
        Move(Vector3.zero);
    }

    public override Vector3 PathFollow()
    {
        int nodeDir = 0;

        if (pathForward)
        { nodeDir = 1; }

        else { nodeDir = -1; }

        Vector3 newPos = this.transform.position;

        //Continue towards node
        //if (.1 < Vector3.Distance(this.transform.position, pathNodes[this.currNode].GetComponent<Transform>().position))
        if(this.transform.position != pathNodes[this.currNode].GetComponent<Transform>().position)
        { newPos = pathNodes[this.currNode].GetComponent<Transform>().position; }

        //Find next node
        else
        {
            this.currNode = (this.currNode + nodeDir);

            if (currNode == -1 || currNode == pathNodes.Count)
            {
                pathForward = !pathForward;
                this.currNode = (this.currNode + nodeDir * -2);
            }

            newPos = pathNodes[this.currNode].GetComponent<Transform>().position;
        }

        return newPos;
    }

    public override void Move(Vector3 Pos)
    {
        //Enemy on a path
        if (pathNodes.Count != 0)
        {
            //Debug.Log(this.transform.eulerAngles);

            this.transform.LookAt(PathFollow());
            base.Move(PathFollow());
            /*
            this.transform.position += this.transform.forward * moveSpd * Time.deltaTime;
            base.SetAxisLevel();
            */
            
        }
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
