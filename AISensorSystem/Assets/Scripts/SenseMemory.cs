using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseMemory : MonoBehaviour
{
    //�Ƿ����б���
    private bool alreadyInList = false;

    //��������ʱ��
    public float memoryTime = 4f;

    //�����б�
    public List<MemoryItem> memoryList = new List<MemoryItem>();
    private List<MemoryItem> removeList = new List<MemoryItem>();

    public bool FindInList()
    {
        foreach(MemoryItem memory in memoryList)
        {
            if(memory.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public void AddToList(GameObject gameObject ,float type)
    {
        alreadyInList = false;
        foreach(MemoryItem memory in memoryList)
        {
            if(gameObject == memory.gameObject)
            {
                alreadyInList = true;
                memory.lastMemoryTime = Time.time; ;
                memory.memoryTimeLeft = memoryTime;
                if (type > memory.sensorType)
                {
                    memory.sensorType = type;
                }
                break;
            }
        }

        if (!alreadyInList)
        {
            MemoryItem newItem = new MemoryItem(gameObject,Time.time,memoryTime,type);
            memoryList.Add(newItem);
        }
    }

    private void Update()
    {
        removeList.Clear();
        foreach(MemoryItem memory in memoryList)
        {
            memory.memoryTimeLeft -= Time.deltaTime;
            if(memory.memoryTimeLeft < 0)
            {
                removeList.Add(memory);
            }
            else
            {
                if(memory.gameObject != null)
                {
                    Debug.DrawLine(transform.position,memory.gameObject.transform.position,Color.blue);
                }
            }
        }
        foreach (MemoryItem memory in removeList)
        {
            memoryList.Remove(memory);
        }
    }


}
public class MemoryItem
{
    public GameObject gameObject; //��֪������Ϸ����
    public float lastMemoryTime; //����ĸ�֪ʱ��
    public float memoryTimeLeft; //���������ڼ����е�ʱ��
    public float sensorType; //ͨ�����ַ�ʽ��֪���ĸö��� �Ӿ�Ϊ1 ����Ϊ0.6;
    public MemoryItem(GameObject gameObject, float time, float timeLeft, float type)
    {
        this.gameObject = gameObject;
        lastMemoryTime = time;
        memoryTimeLeft = timeLeft;
        sensorType = type;
    }

}
