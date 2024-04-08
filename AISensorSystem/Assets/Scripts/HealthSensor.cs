using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSensor : Sensor
{
    private void Start()
    {
        sensorType = SensorType.health;
        manager.RegisterSensor(this);
    }

    public override void Notify(Trigger trigger)
    {
        
    }

}
