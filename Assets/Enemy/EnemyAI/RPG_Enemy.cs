using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Enemy : Enemy
{
    [SerializeField] private int TileVisionRange;   //How many tiles they can see in front of them
    [SerializeField] private List<bool> directions;
    [SerializeField] private int minLookTime, maxLookTime; //How short and long an emeny can look in a specific direction
    

    [HideInInspector] public GameObject closestTile;

    private bool pathForward;
    private bool lookingInDir;

    public override void ClassStart()
    {
        pathForward = true;
        lookingInDir = false;
    }

    public override void ClassUpdate()
    {
        //When arriving at a node, looking in specific directions   
        if (this.transform.position == pathNodes[this.currNode].GetComponent<Transform>().position && !lookingInDir)
        {
            lookingInDir = true;
            StartCoroutine(LookInAllDir());            
        }

        else if(this.transform.position != pathNodes[this.currNode].GetComponent<Transform>().position)
        {
            Move(Vector3.zero);            
        }

        closestTile.GetComponent<GridUnit>().occupied = rgbdy.velocity.magnitude == 0;
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
        { newPos = pathNodes[this.currNode].GetComponent<Transform>().position;}

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
            this.transform.LookAt(PathFollow());
            base.Move(PathFollow());            
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

    void LookInDir(char dir)
    {
        //Look north/up
        if (dir == 'n' || dir == 'u')
        { this.transform.eulerAngles = new Vector3(0, 0, 0); }

        //look east/right
        else if (dir == 'e' || dir == 'r')
        { this.transform.eulerAngles = new Vector3(0, 90, 0); }

        //look west/left
        else if (dir == 'w' || dir == 'l')
        { this.transform.eulerAngles = new Vector3(0, -90, 0); }

        //look south/down
        else if (dir == 's' || dir == 'd')
        { this.transform.eulerAngles = new Vector3(0, 180, 0); }
    }

    float RandomTime()
    {
        return Random.RandomRange(minLookTime, maxLookTime);
    }


    IEnumerator LookInAllDir()
    {
        Debug.Log("LookInAllDir() called");


       
        //Can look north
        if (directions[0])
        {
            LookInDir('n');
            float sec = RandomTime();
            Debug.Log("Looking north for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }


        //Can look east
        if (directions[1])
        {
            LookInDir('e');
            float sec = RandomTime();
            Debug.Log("Looking east for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        //Can look south
        if (directions[2])
        {
            LookInDir('s');
            float sec = RandomTime();
            Debug.Log("Looking south for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        //Can look west
        if (directions[3])
        {
            LookInDir('w');
            float sec = RandomTime();
            Debug.Log("Looking west for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        Debug.Log("LookInAllDir() finished");
        lookingInDir = false;
        Move(Vector3.zero);
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
