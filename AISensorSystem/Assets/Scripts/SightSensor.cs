using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : Sensor
{
    //定义这个AI角色的视觉范围
    public float fieldOfView = 45;
    //AI角色的视觉距离
    public float viewDistance = 100.0f;

     private AIController controller;

    //黑板对象 
    private Blackboard bb;
    private SenseMemory memoryScript; //记忆对象

    private void Start()
    {
        controller = GetComponent<AIController>();
        sensorType = SensorType.sight;
        manager.RegisterSensor(this);

        bb = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
        memoryScript = GetComponent<SenseMemory>();
    }

    private void Update()
    {
        
    }

    public override void Notify(Trigger trigger)
    {
        Debug.DrawLine(transform.position, trigger.transform.position, Color.red);
        if(trigger.tag == "Player")
        {
            bb.playerLastPosition = trigger.transform.position;
            bb.lastSensedTime = Time.time;
        }

        if(memoryScript != null)
        {
            //添加到记忆列表
            memoryScript.AddToList(trigger.gameObject, 1.0f);
        }

      // controller.MoveToTarget(trigger.gameObject.transform.position);
    }

    private void OnDrawGizmos()
    {
        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
        float fieldOfViewInRadians = fieldOfView * 3.14f / 180f;
        Vector3 leftRayPoint = transform.TransformPoint(new Vector3(viewDistance * Mathf.Sin(fieldOfViewInRadians), 0, viewDistance * Mathf.Cos(fieldOfViewInRadians)));
        Vector3 rightRayPoint = transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewInRadians), 0, viewDistance * Mathf.Cos(fieldOfViewInRadians)));
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), frontRayPoint + new Vector3(0, 1, 0), Color.green);
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), leftRayPoint + new Vector3(0, 1, 0), Color.green);
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), rightRayPoint + new Vector3(0, 1, 0), Color.green);

    }
}
