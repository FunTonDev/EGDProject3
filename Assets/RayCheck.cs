using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCheck : MonoBehaviour
{
    public bool rayCheck(int rayNum, float angle, float rcDist, string tag)
    {
        bool hit = false;
        for (int i = 0; i < rayNum; i++)
        {
            Quaternion rotation = this.transform.rotation;
            Quaternion rotationMod = Quaternion.AngleAxis((i / ((float)rayNum - 1)) * angle * 2 - angle, this.transform.up);
            Vector3 dir = rotation * rotationMod * Vector3.forward;

            //Ray ray = new Ray(this.transform.position, dir);

            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position, dir, out hitInfo, rcDist))
            {

                if (hitInfo.collider.tag == tag)
                {
                    Debug.Log("Enemy Raycast hit Player");
                    hit = true;
                    break;
                }

                if (hitInfo.collider.tag == "Wall") Debug.Log("Enemy Raycast hit Wall");

            }
        }

        return hit;
    }
}
