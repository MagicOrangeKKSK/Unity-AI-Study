using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition  //转换条件
{
    SawPlayer = 0,//看到玩家
    ReachPlayer , //接近玩家
    LostPlayer , //玩家离开视线
    NoHealth,//死亡

}

public enum FSMStateID  //状态
{
   Patrolling = 0,   //巡逻的状态编号0
   Chasing,          //追逐的状态编号1
   Attacking,        //攻击的状态编号2
   Dead,             //死亡的状态编号3
}

