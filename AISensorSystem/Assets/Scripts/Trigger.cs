using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    //保存管理中心对象
    protected TriggerSystemManager manager;
    //触发器的位置
    protected Vector3 position;
    //触发器半径
    public int radius;
    public bool toBeRemoved;
    //检查触发器是否能被感知器s感觉到  如果是 那么采取相应的行为
    public virtual void Try(Sensor sensor)
    {

    }

    //更新触发器的内部状态 例如声音触发器的剩余有效时间
    public virtual void Updateme()
    {

    }

    //检查触发器是否能被感知器s感觉到  
    protected virtual bool isTouchingTrigger(Sensor sensor)
    {
        return false;
    }

    private void Awake()
    {
        //查找管理器并保存
        manager = FindObjectOfType<TriggerSystemManager>();
    }

    protected void Start()
    {
        toBeRemoved = false;
    }

    private void Update()
    {
        
    }
}
