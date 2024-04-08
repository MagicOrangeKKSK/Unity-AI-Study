using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //��ɫ��״̬
    public enum State
    {
        Idle,
        Attack,
        Melee,
        Dodge,
        Search,
        Die,
    }


    private State currentState;

    private void Start()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        if(currentState == State.Idle)
        {
            //����ƶ�
        }
        else if(currentState == State.Attack)
        {
            //���
        }
        else if(currentState == State.Melee)
        {
            //��
        }
        //.....

        TriggerHandler();
    }
    int hp = 10;
    bool seeEnemy = false;
    void TriggerHandler()
    {
        if(hp < 0)
        {
            currentState = State.Idle;
        }
        else if (seeEnemy)
        {
            if((currentState == State.Idle) || (currentState == State.Search))
            {
                currentState = State.Attack;
            }
        }
        //...
    }
}
