using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSystemManager : MonoBehaviour
{
    //��ʼ����ǰ��֪���б�
    List<Sensor> currentSensors = new List<Sensor>();
    //��ʼ����ǰ�������б�
    List<Trigger> currentTriggers = new List<Trigger>();

    //��¼��ǰʱ����Ҫ���Ƴ��ĸ�֪�� �����֪������ ��Ҫ�Ƴ���֪��ʱ
    List<Sensor> sensorsToRemove;
    //��¼��ǰʵ����Ҫ���Ƴ��Ĵ����� ���紥�����ѹ���ʱ
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
        //�������д������ڲ�״̬
        UpdateTriggers();
        //�������и�֪���ʹ����� ������Ӧ����Ϊ
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
