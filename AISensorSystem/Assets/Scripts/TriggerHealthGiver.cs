using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHealthGiver : TriggerRespawning
{
    //Ѫ�������� �̳�TriggerRespawning��

    //ÿ�λ�Ѫ����
    public int healthGiven = 10;
    public override void Try(Sensor sensor)
    {
        if (isActive && isTouchingTrigger(sensor))
        {
            AIController controller = sensor.GetComponent<AIController>();
            if (controller != null)
            {
                controller.health += healthGiven;
                Debug.Log("��ǰ����ֵ:" + controller.health);
                GetComponent<Renderer>().material.color = Color.green;
                StartCoroutine(TurnColorBack());
                sensor.Notify(this);
            }
            else
            {
                Debug.Log("û�ҵ�����ֵ�ű�");
            }

            Deactivate();
        }
    }

    IEnumerator TurnColorBack()
    {
        yield return new WaitForSeconds(3);
        GetComponent<Renderer>().material.color = Color.black;
    }

    protected override bool isTouchingTrigger(Sensor sensor)
    {
        GameObject g = sensor.gameObject;
        if(sensor.sensorType == Sensor.SensorType.health)
        {
            if (Vector3.Distance(transform.position, gameObject.transform.position) < radius)
            {
                return true;
            }
        }

        return false;
    }

    private void Start()
    {
        numUpdatesBetweenRespawns = 6000;
        base.Start();
        manager.RegisterTrigger(this);
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
