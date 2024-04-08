using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRespawning : Trigger
{
    //有一些游戏对象，在被一个实体触发之后，会保持一段时间的非活动状态
    //例如一些角色可以捡起地上的血包，当血包被捡起来后 会在一定时间内处于非活动状态
    //之后又重新变为激活的  可以被再次捡起

    //两次活跃之间的间隔时间
    protected int numUpdatesBetweenRespawns;
    //距离下次再生需要等待的时间
    protected int numUpdatesRemainingUntilRespawn;

    protected bool isActive;

    protected void SetActive()
    {
        isActive = true; 
    }

    protected void SetInactive()
    {
        isActive = false;
    }

    protected void Deactivate()
    {
        SetInactive();
        numUpdatesRemainingUntilRespawn = numUpdatesBetweenRespawns;
    }

    public override void Updateme()
    {
        numUpdatesRemainingUntilRespawn--;
        if(numUpdatesRemainingUntilRespawn<0 && !isActive)
        {
            SetActive();
        }
    }

    protected void Start()
    {
        isActive = true;
        base.Start();
    }

    private void Update()
    {
        
    }
}

