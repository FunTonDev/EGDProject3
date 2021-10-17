using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayView : MonoBehaviour
{
    int numOfRays = 5;
    float angle = 40.0f;
    float dist = 3.0f;

    private void Start()
    {
        angle = this.GetComponent<ShooterEnemy>().GetAtkAngle();
        dist = this.GetComponent<ShooterEnemy>().GetAtkDist();
    }

    private void Update()
    {
        for (int i = 0; i < numOfRays; i++)
        {
            Quaternion rotation = this.transform.rotation;
            Quaternion rotationMod = Quaternion.AngleAxis((i / ((float)numOfRays - 1)) * angle * 2 - angle, this.transform.up);
            Vector3 dir = rotation * rotationMod * Vector3.forward;

            Debug.DrawRay(this.transform.position, dir * dist, Color.red);

        }

    }
    
}
