using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{

    public ChaseState(Transform[] wp)
    {
        waypoints = wp;
        //状态编号
        stateID = FSMStateID.Chasing;

        //设置转向速度与移动速度
        curRotSpeed = 6.0f;
        curSpeed = 160.0f;

        //从巡逻点数组中随机选择一个 
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        destPos = player.position;

        //检查与玩家的距离
        //如果小于攻击距离 那么转移到攻击状态
        float dist = Vector3.Distance(npc.position, destPos);

        //Debug.LogError($"dist:{dist} attackDistance:{attackDistance}");
        if (dist <= attackDistance)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<AIController>().SetTransition(Transition.ReachPlayer);
        }
        //如果与玩家距离超过追逐距离 那么回到巡逻状态
        else if (dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol State");
            npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //将玩家位置设置为目标点
        destPos = player.position;
        //转向目标点
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        CharacterController controller = npc.GetComponent<CharacterController>();
        controller.SimpleMove(npc.transform.forward * Time.deltaTime * curSpeed);

        //播放奔跑动画

    }

}
