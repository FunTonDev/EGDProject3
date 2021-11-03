using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformerMovement : MonoBehaviour
{
    [SerializeField] private bool facingLeft;       //Direction Enemy is facing

    [SerializeField] protected float moveSpd;
    private List<Transform> pathNodes;
    private int currNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoveSpd(float spd)
        { moveSpd = spd; }

    public void SetPathNodes(List<Transform> pts)
        { pathNodes = pts; currNode = 0;}

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
}
