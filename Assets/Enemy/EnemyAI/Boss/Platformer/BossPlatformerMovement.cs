using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerMovement : MonoBehaviour
{
    [SerializeField] private bool isFlipped;       

    [SerializeField] protected float moveSpd;
    private List<Transform> pathNodes;
    private int currNode;

    private float axisLvl;

    // Start is called before the first frame update
    void Start()
    {
        isFlipped = false;
        axisLvl = GameObject.FindGameObjectWithTag("Player").transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoveSpd(float spd)
        { moveSpd = spd; }

    public void SetPathNodes(List<Transform> pts)
        { pathNodes = pts; currNode = 0;}

    //For regular Movement
    public virtual void MoveReg()
    {
        this.transform.position += this.transform.forward * moveSpd * Time.deltaTime;
        //this.transform.position = Vector3.MoveTowards(this.transform.position, Pos, moveSpd * Time.deltaTime);
        SetAxisLevel();
    }

    //For Movements related to Attacking
    public virtual void MoveAtk(Vector3 Pos)
    {
        transform.position = Vector3.MoveTowards(this.transform.position, Pos, moveSpd * Time.deltaTime);
        SetAxisLevel();
    }

    //Gets the transform of the current node
    public Vector3 PathFollow()
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

    public void SetAxisLevel()
    {
        Vector3 temp = this.transform.position;
        temp.z = axisLvl;

        this.transform.position = temp;
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
    }

    public void LookAtPlayer(Transform player)
    {
        //Vector3 flipped = this.transform.localScale;
        //flipped.z *= -1f;

        if((player.position.x < this.transform.position.x) && isFlipped)
        {
            //this.transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            //this.transform.eulerAngles = new Vector3(0, 270, 0);
            isFlipped = false;

            Debug.Log("Boss Flipped"); 
        }

        else if((player.position.x > this.transform.position.x) && !isFlipped)
        {
            //this.transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;

            Debug.Log("Boss Flipped");
        }
    }




}
