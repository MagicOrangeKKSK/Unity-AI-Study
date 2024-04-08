using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSensor : Sensor
{
    public float hearingDistance = 30f;
    private AIController controller;

    //�ڰ���� 
    private Blackboard bb;
    private SenseMemory memoryScript; //�������

    private void Start()
    {
        controller = GetComponent<AIController>();
        sensorType = SensorType.sound;
        manager.RegisterSensor(this);

        bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
        memoryScript = GetComponent<SenseMemory>();
    }

    private void Update()
    {
        
    }

    public override void Notify(Trigger trigger)
    {
        if(memoryScript != null)
        {
            memoryScript.AddToList(trigger.gameObject, 0.6f);
        }

        bb.playerLastPosition = trigger.gameObject.transform.position;
        bb.lastSensedTime = Time.time;
        //controller.MoveToTarget(this.gameObject.transform.position);
    }
}
