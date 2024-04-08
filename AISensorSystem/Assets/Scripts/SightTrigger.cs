using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTrigger : Trigger
{
    public override void Try(Sensor sensor)
    {
        if (isTouchingTrigger(sensor))
        {
            sensor.Notify(this);
        }
    }

    protected override bool isTouchingTrigger(Sensor sensor)
    {
        GameObject g = sensor.gameObject;
        if(sensor.sensorType == Sensor.SensorType.sight)
        {
            Vector3 rayDirection = transform.position - g.transform.position;
            rayDirection.y = 0;
            if(Vector3.Angle(rayDirection,g.transform.forward) < (sensor as SightSensor).fieldOfView)
            {
                if(Physics.Raycast(g.transform.position + new Vector3(0,1,0),rayDirection,out RaycastHit hit, (sensor as SightSensor).viewDistance))
                {
                    if(hit.collider.gameObject == this.gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //更新触发器的内部信息 由于带着视觉触发器的AI角色可能是运动的 因此要不停更新这个触发器的位置
    public override void Updateme()
    {
        position = transform.position;
    }

    private void Start()
    {
        base.Start();
        manager.RegisterTrigger(this);
    }

    private void Update()
    {
        
    }
}
