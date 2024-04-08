using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : TriggerLimitedLifetime
{
    public override void Try(Sensor sensor)
    {
        if (isTouchingTrigger(sensor))
        {
            sensor.Notify(this);
        }
    }

    //�жϸ�֪���Ƿ���������������������������
    protected override bool isTouchingTrigger(Sensor sensor)
    {
        GameObject g = sensor.gameObject;
        if (sensor.sensorType == Sensor.SensorType.sound)
        {
            if (Vector3.Distance(transform.position, g.transform.position) < radius)
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        lifetime = 3;
        base.Start();
        manager.RegisterTrigger(this);
    }

    private void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
