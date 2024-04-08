using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SenseMemory))]
public class DrawMemory : Editor
{
    private Vector3 knowPos;
    private float timeStamp;
    private float timeLeft;
    private float knowType;

    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = Color.blue;
        SenseMemory myTarget = (SenseMemory)target;

        foreach (var mi in myTarget.memoryList)
        {
            Handles.Label(mi.gameObject.transform.position +
                Vector3.up, "Position:" + mi.gameObject.transform.position.ToString() +
                "\nSensor Type:" + mi.sensorType.ToString() +
                "\nSense Time Stamp:" + mi.lastMemoryTime.ToString() +
                "\nMemory Time Left:" + mi.memoryTimeLeft.ToString(), style);

        }

        Handles.BeginGUI(new Rect(Screen.width - 100, Screen.height - 80, 90, 50));
    }
}
