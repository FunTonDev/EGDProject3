using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Platformer_FOV))]
public class Platformer_FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        Platformer_FOV fow = (Platformer_FOV)target;
        //View circle

        //Creates vertical circle around enemy
        if (fow.GetFullRanged())
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.GetRadius());
        }

        else
        {
            Debug.DrawRay(fow.transform.position, fow.transform.forward* fow.GetRadius(), Color.red);
        }
       
    }
}