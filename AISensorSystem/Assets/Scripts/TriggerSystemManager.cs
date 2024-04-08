using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystemManager : MonoBehaviour
{
    //初始化当前感知器列表
    List<Sensor> currentSensors = new List<Sensor>();
    //初始化当前触发器列表
    List<Trigger> currentTriggers = new List<Trigger>();

    //记录当前时刻需要被移除的感知器 例如感知器死亡 需要移除感知器时
    List<Sensor> sensorsToRemove;
    //记录当前实可需要被移除的触发器 例如触发器已过期时
    List<Trigger> triggersToRemove;

    private void Start()
    {
        sensorsToRemove = new List<Sensor>();
        triggersToRemove = new List<Trigger>();
    }

    private void UpdateTriggers()
    {
        foreach (Trigger trigger in currentTriggers)
        {
            if (trigger.toBeRemoved)
            {
                triggersToRemove.Add(trigger);
            }
            else
            {
                trigger.Updateme();
            }
        }

        foreach (Trigger trigger in triggersToRemove)
        {
            currentTriggers.Remove(trigger);
        }
    }

    private void TryTriggers()
    {
        foreach(Sensor sensor in currentSensors)
        {
            if(sensor.gameObject != null)
            {
                foreach(Trigger trigger in currentTriggers)
                {
                    trigger.Try(sensor);
                }
            }
            else
            {
                sensorsToRemove.Add(sensor);
            }
        }
    
    
        foreach(Sensor sensor in sensorsToRemove)
        {
            currentSensors.Remove(sensor);
        }

    }

    private void Update()
    {
        //更新所有触发器内部状态
        UpdateTriggers();
        //迭代所有感知器和触发器 做出相应的行为
        TryTriggers();
    }


    public void RegisterTrigger(Trigger trigger)
    {
        currentTriggers.Add(trigger);
    }


    public void RegisterSensor(Sensor sensor)
    {
        currentSensors.Add(sensor);
    }

}
