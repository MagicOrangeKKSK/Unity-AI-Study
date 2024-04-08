using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM 
{
   public enum FSMState
    {
        Patrol , //巡逻 
        Chase,   //追逐
        Attack,//攻击
        Dead,//死亡
    }

    //定义开始追逐玩家的距离
    public float chaseDistance = 40f;
    //定义开始攻击玩家的距离
    public float attackDistance = 20f;
    //距离巡逻点小于这个值时 认为已经到达巡逻点
    public float arriveDistance = 3f;

    //子弹的生成点
    public Transform bulletSpawnPoint;
    private CharacterController controller;

    //AI角色的当前状态
    public FSMState curState;

    //AI角色的速度
    public float walkSpeed = 80f;
    public float runSpeed = 160f;


    //AI角色的转向速度
    public float curRotSpeed = 6f;
    //子弹预制体
    public GameObject Bullet;

    //AI角色是否死亡
    private bool bDead;
    private int health;

    protected override void Initialize()
    {
        //设置当前状态为巡逻
        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0f;
        shootRate = 0.5f;
        health = 100;

        pointList = GameObject.FindGameObjectsWithTag("PatrolPoints");

        controller = GetComponent<CharacterController>();
        //动画

        //随机选择一个巡逻点
        FindNextPoint();

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            Debug.LogError("没有玩家角色");
    }

    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.Chase:
                UpdateChaseState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;
        }

        elapsedTime += Time.deltaTime;
        if (health <= 0)
            curState = FSMState.Dead;
    }

    protected void UpdatePatrolState()
    {
        //如果已经抵达巡逻点 那么寻找下一个巡逻点
        if (Vector3.Distance(transform.position, destPos) <= arriveDistance)
        {
            FindNextPoint();
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance)
        {
            //检查与玩家距离 如果与玩家距离较近 则切换为追逐状态
            curState = FSMState.Chase;
        }

        //向目标点转向
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //向前移动
        controller.SimpleMove(transform.forward * Time.deltaTime * walkSpeed);
        //播放行走动画
    }


    protected void UpdateChaseState()
    {
        //将目标位置设置为玩家的位置
        destPos = playerTransform.position;
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist <= attackDistance)
        {
            curState = FSMState.Attack;
        }
        else if(dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
        }

        //向目标点转向
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //向前移动
        controller.SimpleMove(transform.forward * Time.deltaTime * runSpeed);
        //播放奔跑动画
    }

    //更新攻击状态
    protected void UpdateAttackState()
    {
        Quaternion targetRotation;
        //设置目标点
        destPos = playerTransform.position;
        //检查与玩家的距离
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        //如果在攻击距离与追逐距离之间 则转换为追逐状态
        if(dist >= attackDistance && dist < chaseDistance)
        {
            curState = FSMState.Chase;
            return;
        }
        else if (dist >= chaseDistance)
        {
            curState = FSMState.Patrol;
            return;
        }

        //转向目标点
        targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //发射子弹
        ShootBullet();

        //播放射击动画
        //
    }

    private void ShootBullet()
    {
        if(elapsedTime >= shootRate)
        {
            GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
            bulletObj.GetComponent<Bullet>().Go();
            elapsedTime = 0f;
        }
    }


    protected void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }

    protected void FindNextPoint()
    {
        Debug.Log("找到下一个");
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }





}
