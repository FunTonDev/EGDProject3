using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Enemy : Enemy
{
    [Header("RPG Enemey Class")]

    [SerializeField] private int TileVisionRange;               //How many tiles they can see in front of them

    [Header("Looking Around (directions index is north, east, south, and west)")]    
    [SerializeField] [Range(0.0f, 1.01f)] private float chanceToLook;   //Compared to random number from 0-1, if Random # is <= chanceToLook, look in dir  
    [SerializeField] private int minLookTime, maxLookTime;             //How short and long an emeny can look in a specific direction
    [SerializeField] private List<bool> directions;                   //Which directions the enemy can look in. Index 0-3 for north, east, south, west respectively
   
    [HideInInspector] public GameObject closestTile;

    private SaveFile SavingObject;
    private Vector3 plaPosition;


    [SerializeField] private bool looping;
    [SerializeField] private bool pathForward;
    private bool lookingInDir;
    private bool playerSpotted;
    private bool fought;

    [SerializeField] private int fightLvl;

    //For the FOV Script
    public int GetTileVisionRange() { return TileVisionRange; }
    public int GetPathNodeCount() { return pathNodes.Count; }
    public Transform GetPathNode(int index) {return pathNodes[index];}

    public override void ClassStart()
    {        
        lookingInDir = false;
        fought = false;

        SavingObject = GameObject.Find("PlayerPrefab").GetComponent<PlayerController>().so;

        if (looping)
        {
            if (1 < Random.Range(0, 2)) { pathForward = false; }
            else { pathForward = true; }
        }

        else { pathForward = true; }
    }

    public override void ClassUpdate()
    {
        playerSpotted = CheckForPlayer();

        if (!fought)
        {
            //Stop Coroutine
            if (playerSpotted && lookingInDir)
            {
                StopCoroutine("LookInAllDir");
                lookingInDir = false;
            }

            if (playerSpotted)
            {
                InitiateBattle();
            }

            //When arriving at a node, looking in specific directions   
            else if (this.transform.position == pathNodes[this.currNode].position && !lookingInDir)
            {
                lookingInDir = true;

                if (closestTile != null)
                { closestTile.GetComponent<GridUnit>().occupied = rgbdy.velocity.magnitude == 0; }

                StartCoroutine("LookInAllDir");
            }

            else if (this.transform.position != pathNodes[this.currNode].position && !lookingInDir)
            {
                Move(Vector3.zero);
            }
        }

        else
        {
            //base.Death(); 
        }

        
    }

    public override Vector3 PathFollow()
    {        
        Vector3 newPos = this.transform.position;

        //Continue towards node
        //if (.1 < Vector3.Distance(this.transform.position, pathNodes[this.currNode].GetComponent<Transform>().position))
        if(this.transform.position != pathNodes[this.currNode].GetComponent<Transform>().position)
        { newPos = pathNodes[this.currNode].GetComponent<Transform>().position;}
        
        //Find next node
        else if (looping)
        {
            int nodeDir = 0;
            if (pathForward) { nodeDir = 1; }
            else { nodeDir = -1; }

            this.currNode = (this.currNode + nodeDir);

            if (currNode == -1 && !pathForward)
            { this.currNode = pathNodes.Count-1; }

            else if (currNode == pathNodes.Count && pathForward)
            { this.currNode = 0; }

            newPos = pathNodes[this.currNode].GetComponent<Transform>().position;
        }
                
        else
        {
            int nodeDir = 0;

            if (pathForward) { nodeDir = 1; }
            else { nodeDir = -1; }

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
        return this.GetComponent<RPG_FOV>().FindVisibleTarget(); 
    }

    void LookInDir(char dir)
    {
        //Relative to player
        //Look forward
        if (dir == 'n' || dir == 'f')
        { this.transform.Rotate(0, 0, 0); }// this.transform.eulerAngles = new Vector3(0, 0, 0); }

        //look right
        else if (dir == 'r')
        { this.transform.Rotate(0, 90, 0); }//this.transform.eulerAngles = new Vector3(0, 90, 0); }

        //look left
        else if ( dir == 'l')
        { this.transform.Rotate(0, 90, 0); } //this.transform.eulerAngles = new Vector3(0, -90, 0); }

        //look behind
        else if ( dir == 'b')
        { this.transform.Rotate(0, 90, 0); } //this.transform.eulerAngles = new Vector3(0, 180, 0); }

        //Relative to Map
        //Look north
        if (dir == 'n' )
        { this.transform.eulerAngles = new Vector3(0, 0, 0); }

        //look east
        else if (dir == 'e')
        { this.transform.eulerAngles = new Vector3(0, 90, 0); }

        //look west
        else if (dir == 'w' )
        { this.transform.eulerAngles = new Vector3(0, -90, 0); }

        //look south
        else if (dir == 's')
        { this.transform.eulerAngles = new Vector3(0, 180, 0); }
    }

    float RandomTime()
    {
        return Random.Range(minLookTime, maxLookTime+1);
    }

    bool ChanceToLookInDir()
    {
        if(chanceToLook > Random.Range(0.0f, 1.0f))
        {return true;}

        return false;
    }


    IEnumerator LookInAllDir()
    {
        Debug.Log("LookInAllDir() called");
        /*
        Vector3 nextspot = PathFollow();
        if (currNode != 0 || currNode != pathNodes.Count)
        { this.transform.LookAt(nextspot); }
        */
       
        //Can look foward
        if (directions[0] && ChanceToLookInDir())
        {
            LookInDir('n');
            float sec = RandomTime();
            //Debug.Log("Looking north for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }


        //Can look right
        if (directions[1] && ChanceToLookInDir())
        {
            LookInDir('e');
            float sec = RandomTime();
            //Debug.Log("Looking east for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        //Can look behind
        if (directions[2] && ChanceToLookInDir())
        {
            LookInDir('s');
            float sec = RandomTime();
            //Debug.Log("Looking south for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        //Can look left
        if (directions[3] && ChanceToLookInDir())
        {
            LookInDir('w');
            float sec = RandomTime();
            //Debug.Log("Looking west for " + sec + " seconds");
            yield return new WaitForSeconds(sec);
        }

        Debug.Log("LookInAllDir() finished");
        lookingInDir = false;
        Move(Vector3.zero);
    }

    public void InitiateBattle()
    {
        if (!fought)
        {
            Debug.Log("Enemy Initiating Battle");

            fought = true;
            SavingObject.fightLevel = fightLvl;
            SavingObject.lastPosition = plaPosition; 
            TransitionManager tm = GameObject.Find("[MANAGER]").GetComponent<TransitionManager>();
            SaveManager.Save(SavingObject);
            tm.SceneSwitch("RPGBattle");
        }
                     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            plaPosition = collision.transform.position;
            InitiateBattle();
        }
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
