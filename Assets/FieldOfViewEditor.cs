using System.Collections;
using System.Collections.Generic;
using UnityEngine
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.GetViewRadius());
        Vector3 ViewAngleA = fow.DirFromAngle() - fow.GetViewAngle();
    }
}
