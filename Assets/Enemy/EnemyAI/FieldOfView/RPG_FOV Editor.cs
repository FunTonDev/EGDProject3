using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RPG_FOV))]
public class RPG_FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        RPG_FOV fow = (RPG_FOV)target;

        for (int i = 0; i < fow.GetPathNodeCount() - 1; i++)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(fow.GetPathNodePos(i), fow.GetPathNodePos(i+1));
        }

        Debug.DrawRay(fow.transform.position, fow.transform.forward * fow.GetRange(), Color.red);
    }
}
