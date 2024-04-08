using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState  
{
    //字典 字典钟记录了 转换 - 状态 的信息
    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
    //状态编号ID
    protected FSMStateID stateID;
    public FSMStateID ID { get { return stateID; } }

    //下面是需要用到的  与各状态相关的变量
    protected Vector3 destPos;
    protected Transform[] waypoints;
    //转向的速度
    protected float curRotSpeed;
    //移动的速度
    protected float curSpeed;
    //AI角色与玩家的距离小于这个值的时候 开始追逐
    protected float chaseDistance = 40f;
    //AI角色与玩家小于这个距离时 开始攻击
    protected float attackDistance = 20f;
    //在巡逻过程中 如果AI角色与某个巡逻点的距离小于这个值 认为已经到达这个点
    protected float arriveDistance = 3f;

    public void AddTransition(Transition transition, FSMStateID id)
    {
        if (map.ContainsKey(transition))
        {
            Debug.LogWarning("FSMState ERROR: transition is already inside the map");
            return;
        }

        map.Add(transition, id);
        Debug.Log($"Added:{transition} with ID:{id}");
    }

    //从字典中删除一项
    public void DeleteTransition(Transition trans)
    {
        if(map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition passed was not on this State's List");
    }

    //查询字典 确认当前状态 发生转换
    public FSMStateID GetOutputState(Transition trans)
    {
        return map[trans];
    }

    //用来确定是否需要发生转换到其他状态 应该发生哪个转换
    public abstract void Reason(Transform player, Transform npc);

    //Act 定义了本状态的角色行为 例如移动  动画
    public abstract void Act(Transform player, Transform npc);

    public void FindNextPoint()
    {
        int rndIndex = Random.Range(0, waypoints.Length);
        Vector3 rndPosition = Vector3.zero; //可以用于扰动
        destPos = waypoints[rndIndex].position + rndPosition;
    }



}
