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
        //��������ҵľ���
        float dist = Vector3.Distance(npc.position, player.position);

        //�������ҵľ�����ڹ��������С��׷����� ���л���׷��ģʽ
        if(dist >= attackDistance && dist< chaseDistance)
        {
            Debug.Log("Switch to Chase state");
            npc.GetComponent<AIController>().SetTransition(Transition.SawPlayer);
        }
        //�������Ҿ��볬��׷����� ��ô�ص�Ѳ��״̬
        else if(dist >= chaseDistance)
        {
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<AIController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //�����λ������ΪĿ���
        destPos = player.position;

        //ת��Ŀ��
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //�����ӵ� ������ƶ���
        npc.GetComponent<AIController>().ShootBullet();
    }
}
