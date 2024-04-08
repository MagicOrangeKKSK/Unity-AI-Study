using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseMemory : MonoBehaviour
{
    //是否在列表中
    private bool alreadyInList = false;

    //记忆留存时间
    public float memoryTime = 4f;

    //记忆列表
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
    public GameObject gameObject; //感知到的游戏对象
    public float lastMemoryTime; //最近的感知时间
    public float memoryTimeLeft; //还能留存在记忆中的时间
    public float sensorType; //通过哪种方式感知到的该对象 视觉为1 听力为0.6;
    public MemoryItem(GameObject gameObject, float time, float timeLeft, float type)
    {
        this.gameObject = gameObject;
        lastMemoryTime = time;
        memoryTimeLeft = timeLeft;
        sensorType = type;
    }

}
