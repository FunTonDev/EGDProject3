using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer_FOV : MonoBehaviour
{
    bool fullRanged;
    float radius;

    public bool GetFullRanged() { return fullRanged; }
    public float GetRadius() { return radius; }

    // Update is called once per frame
    void Start()
    {
        fullRanged = this.GetComponent<PlatformerEnemy>().GetFullRanged(); ;
        radius = this.GetComponent<PlatformerEnemy>().GetRange(); ;
    }

    public bool FindVisibleTargets()
    {
        bool foundTarget = false;

        if (fullRanged)
        {
            Collider[] TargetsInRad = Physics.OverlapSphere(this.transform.position, radius);
            for (int i = 0; i < TargetsInRad.Length; i++)
            {
                if (TargetsInRad[i].tag == "Player")
                {
                    //Debug.Log("Player in Sphere");

                    Transform target = TargetsInRad[i].transform;
                    Vector3 dirToTarget = (target.position - this.transform.position).normalized;
                    float distToTarget = Vector3.Distance(this.transform.position, target.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(this.transform.position, dirToTarget, out hitInfo, distToTarget))
                    {
                        if (hitInfo.collider.tag == "Player")
                        {
                            foundTarget = true;
                        }
                        break;
                    }
                }
            }
        }

        else
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(this.transform.position, transform.forward, out hitInfo, radius))
            {
                if (hitInfo.collider.tag == "Player")
                {
                    foundTarget = true;
                }
            }
        }

        return foundTarget;

    }

}
