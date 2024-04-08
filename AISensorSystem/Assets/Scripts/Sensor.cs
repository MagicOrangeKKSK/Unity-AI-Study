using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    protected TriggerSystemManager manager;

    public enum SensorType
    {
        sight,
        sound,
        health
    }

    public SensorType sensorType;

    private void Awake()
    {
        manager = FindObjectOfType<TriggerSystemManager>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    //֪ͨ
    public virtual void Notify(Trigger trigger)
    {

    }
}
