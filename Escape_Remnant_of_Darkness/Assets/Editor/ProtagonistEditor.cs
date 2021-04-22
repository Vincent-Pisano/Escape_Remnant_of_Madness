using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ProtagonistScript))]
public class ProtagonistEditor : Editor
{
    private void OnSceneGUI()
    {
        ProtagonistScript _protagonistScript = (ProtagonistScript) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(_protagonistScript.transform.position, Vector3.forward, Vector3.right,
            360, _protagonistScript.GetViewRadius());

    }
}
