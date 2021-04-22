using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right,
            360, fow.GetViewRadius());
        
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right,
            360, fow.GetViewRadiusNear());
        
        Vector3 viewAngleA = fow.DirFromAngle(fow.GetViewAngle() / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(-fow.GetViewAngle() / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.GetViewRadius());
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.GetViewRadius());

        Handles.color = Color.red;
        Transform player = fow.GetTarget();
        if(player != null)
        {
            Handles.DrawLine(fow.transform.position, player.position);
        }
    }
}
