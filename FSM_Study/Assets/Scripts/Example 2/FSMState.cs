using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState  
{
    //�ֵ� �ֵ��Ӽ�¼�� ת�� - ״̬ ����Ϣ
    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
    //״̬���ID
    protected FSMStateID stateID;
    public FSMStateID ID { get { return stateID; } }

    //��������Ҫ�õ���  ���״̬��صı���
    protected Vector3 destPos;
    protected Transform[] waypoints;
    //ת����ٶ�
    protected float curRotSpeed;
    //�ƶ����ٶ�
    protected float curSpeed;
    //AI��ɫ����ҵľ���С�����ֵ��ʱ�� ��ʼ׷��
    protected float chaseDistance = 40f;
    //AI��ɫ�����С���������ʱ ��ʼ����
    protected float attackDistance = 20f;
    //��Ѳ�߹����� ���AI��ɫ��ĳ��Ѳ�ߵ�ľ���С�����ֵ ��Ϊ�Ѿ����������
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

    //���ֵ���ɾ��һ��
    public void DeleteTransition(Transition trans)
    {
        if(map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition passed was not on this State's List");
    }

    //��ѯ�ֵ� ȷ�ϵ�ǰ״̬ ����ת��
    public FSMStateID GetOutputState(Transition trans)
    {
        return map[trans];
    }

    //����ȷ���Ƿ���Ҫ����ת��������״̬ Ӧ�÷����ĸ�ת��
    public abstract void Reason(Transform player, Transform npc);

    //Act �����˱�״̬�Ľ�ɫ��Ϊ �����ƶ�  ����
    public abstract void Act(Transform player, Transform npc);

    public void FindNextPoint()
    {
        int rndIndex = Random.Range(0, waypoints.Length);
        Vector3 rndPosition = Vector3.zero; //���������Ŷ�
        destPos = waypoints[rndIndex].position + rndPosition;
    }



}
