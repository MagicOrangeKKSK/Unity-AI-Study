using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : AdvancedFSM
{
    public GameObject Bullet;//子弹的游戏对象
    public Transform bulletSpawnPoint; //子弹的生成点
    private int health; //AI角色的生命值

    //初始化AI角色的FSM
    protected override void Initialize()
    {
        health = 100;

        elapsedTime = 0f;
        shootRate = 0.1f;

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
        {
            Debug.LogError("Player doesn't exist.. Please add one with Tag named 'Player'");
            //开始构造状态机
        }
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        //计算距离上次子弹发射后过去的时间
        elapsedTime += Time.deltaTime;
    }

    protected override void FSMFixedUpdate()
    {
        //调用当前状态的Reason方法  确定当前发生的转换
        CurrentState.Reason(playerTransform, transform);
        //调用当前状态的Act
        CurrentState.Act(playerTransform, transform);
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    private void ConstructFSM()
    {
        //找到
        pointList = GameObject.FindGameObjectsWithTag("PatrolPoints");
        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;

        foreach(GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }

        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            health -= 50;
            if(health <= 0)
            {
                SetTransition(Transition.NoHealth);
            }
        }
    }

    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            GameObject bulletObj = Instantiate(Bullet, bulletSpawnPoint.transform.position, transform.rotation);
            bulletObj.GetComponent<Bullet>().Go();
            elapsedTime = 0;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 20f);
        Gizmos.DrawWireSphere(transform.position, 40f);
        Gizmos.DrawWireSphere(transform.position, 3f);

    }
}
