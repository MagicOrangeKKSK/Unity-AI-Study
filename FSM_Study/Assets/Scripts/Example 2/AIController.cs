using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : AdvancedFSM
{
    public GameObject Bullet;//�ӵ�����Ϸ����
    public Transform bulletSpawnPoint; //�ӵ������ɵ�
    private int health; //AI��ɫ������ֵ

    //��ʼ��AI��ɫ��FSM
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
            //��ʼ����״̬��
        }
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        //��������ϴ��ӵ�������ȥ��ʱ��
        elapsedTime += Time.deltaTime;
    }

    protected override void FSMFixedUpdate()
    {
        //���õ�ǰ״̬��Reason����  ȷ����ǰ������ת��
        CurrentState.Reason(playerTransform, transform);
        //���õ�ǰ״̬��Act
        CurrentState.Act(playerTransform, transform);
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    private void ConstructFSM()
    {
        //�ҵ�
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
