using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundSensor))]
public class DrawHearRegion : Editor
{
    private float radius;

    private void OnSceneGUI()
    {
        SoundSensor myTarget = (SoundSensor)target;
        radius = myTarget.hearingDistance;

        Handles.color = new Color(0, 0.8f, 0.4f, 0.2f);
        Handles.DrawSolidDisc(myTarget.transform.position, myTarget.transform.up, radius);

        Handles.color = new Color(0, 1, 1, 0.1f);
    }
}
