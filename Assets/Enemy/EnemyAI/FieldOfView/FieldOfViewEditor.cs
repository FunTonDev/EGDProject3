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
        //View circle
        Handles.color = Color.white;
        Handles.DrawWireArc (fow.transform.position, Vector3.up, Vector3.forward, 360, fow.GetViewRadius());

        //Buffer circle
        Handles.color = Color.black;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.GetBufferRadius());

        //Attack cone
        Vector3 ViewAngle1A = fow.DirFromAngle(-fow.GetAtkAngle() / 2, false);
        Vector3 ViewAngle1B = fow.DirFromAngle(fow.GetAtkAngle() / 2, false);

        Handles.color = Color.red;
        Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngle1A * fow.GetAtkRadius());
        Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngle1B * fow.GetAtkRadius());

        if(fow.GetRotAngle() != 0 && fow.GetRotAngle() != 360)
        {
            Vector3 ViewAngle2A = fow.GetViewAngleA();
            Vector3 ViewAngle2B = fow.GetViewAngleB();

            Handles.color = Color.blue;
            Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngle2A * 1f);
            Handles.DrawLine(fow.transform.position, fow.transform.position + ViewAngle2B * 1f);
        }
    }
}
