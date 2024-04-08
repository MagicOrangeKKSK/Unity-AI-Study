using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    //玩家的transform组件
    protected Transform playerTransform;

    //下一个巡逻点或玩家的位置 取决于当前的状态
    protected Vector3 destPos;

    //巡逻点的数组
    protected GameObject[] pointList;

    protected float shootRate; //子弹射击速率
    protected float elapsedTime; //距离上一次射击的时间

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    private void Start()
    {
        Initialize(); //用于FSM初始化
    }

    private void Update()
    {
        FSMUpdate(); //每帧更新fsm
    }

    private void FixedUpdate()
    {
        //以固定的时间周期更新FSM
        FSMFixedUpdate();
    }


}
