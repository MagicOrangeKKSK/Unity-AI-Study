using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //角色的状态
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
            //随机移动
        }
        else if(currentState == State.Attack)
        {
            //射击
        }
        else if(currentState == State.Melee)
        {
            //格斗
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
