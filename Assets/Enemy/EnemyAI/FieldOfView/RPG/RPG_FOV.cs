using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used to help enemy spot player in overworld along with give a representation of its sight range and path
public class RPG_FOV : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreMask;
    private List<Transform> pathNodes;
    private int pathNodesCount = 0;
    private int range;

    public int GetRange() { return range; }
    public int GetPathNodeCount() { return pathNodesCount; }
    public Vector3 GetPathNodePos(int index) { return pathNodes[index].position; }

    // Start is called before the first frame update
    void Start()
    {
        range = this.GetComponent<RPG_Enemy>().GetTileVisionRange();
        pathNodesCount = this.GetComponent<RPG_Enemy>().GetPathNodeCount();
        pathNodes = new List<Transform>();
        for (int i = 0; i < pathNodesCount; i++)
        {
            pathNodes.Add(this.GetComponent<RPG_Enemy>().GetPathNode(i));
        }

    }

    public bool FindVisibleTarget()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, range, ~ignoreMask))
        {
            Debug.Log("RPG Raycast hit " + hitInfo.collider.tag);
            if (hitInfo.collider.tag == "Player")
            {
                Debug.Log("Player spotted!");
                return true;
            }
        }

        return false;
    }


}
