using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.GetViewRadius());


        Vector3 ViewAngleA = fow.DirFromAngle(-fow.GetAtkAngle() / 2, false);
        Vector3 ViewAngleB = fow.DirFromAngle(fow.GetAtkAngle() / 2, false);

        Handles.color = Color.red;
        Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngleA * fow.GetAtkRadius());
        Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngleB * fow.GetAtkRadius());

    }
}
