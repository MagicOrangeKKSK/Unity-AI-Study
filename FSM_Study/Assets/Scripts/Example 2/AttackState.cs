using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    public AttackState(Transform[] wp)
    {
        waypoints = wp;
        stateID = FSMStateID.Attacking;

        curRotSpeed = 12.0f;
        curSpeed = 100.0f;

        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //计算与玩家的距离
        float dist = Vector3.Distance(npc.position, player.position);

        //如果与玩家的距离大于攻击距离而小于追逐距离 则切换成追逐模式
        if(dist >= attackDistance && dist< chaseDistance)
        {
            Debug.Log("Switch to Chase state");
            npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
        }
        //如果与玩家距离超出追逐距离 那么回到巡逻状态
        else if(dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //将玩家位置设置为目标点
        destPos = player.position;

        //转向目标
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //发射子弹 播放设计动画
        npc.GetComponent<AIController>().ShootBullet();
    }
}
