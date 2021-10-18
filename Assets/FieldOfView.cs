using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    float viewRadius = 3.0f;
    [Range(0, 360)]
    float viewAngle = 40.0f;

    public float GetViewRadius() { return viewRadius; }
    public float GetViewAngle() { return viewAngle; }

    public void Start()
    {
        viewRadius = this.GetComponent<ShooterEnemy>().GetAtkDist();
        viewAngle = this.GetComponent<ShooterEnemy>().GetAtkAngle();        
    }

    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDeg += this.transform.rotation.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }

}
