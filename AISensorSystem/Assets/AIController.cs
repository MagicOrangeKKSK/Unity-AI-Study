using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : AIPath
{
    public int health;
    public float arriveDistance = 1.0f;
    public Transform patrolWayPoints;
    //Ŀ���Ԥ����
    public GameObject targetPrefab;
    //����ֹͣ׷�� ��ʼ����ľ���
    public float shootingDistance = 7.0f;
    public float chasingDistance = 8.0f;

    private Animator animator;

    private Blackboard bb;

    private int wayPointIndex = 0;
    private Vector3 personalLastSighting;
    private Vector3 previousSighting;
    private Vector3[] wayPoints;

    private SenseMemory memory;

    public enum FSMState
    {
        Patrolling =0,
        Chasing,
        Shooting,
    }
    [SerializeField]
    private FSMState state;

    private void Start()
    {
        health = 30;
        animator = GetComponentInChildren<Animator>();
        bb = FindObjectOfType<Blackboard>();
        personalLastSighting = bb.resetPosition;
        previousSighting = bb.resetPosition;

        memory = GetComponent<SenseMemory>();
        GameObject newTarget = Instantiate(targetPrefab, transform.position, transform.rotation);
        target = newTarget.transform;

        state = FSMState.Patrolling;
        wayPoints = new Vector3[patrolWayPoints.childCount];
        int c = 0;
        foreach (Transform child in patrolWayPoints)
        {
            wayPoints[c] = child.position;
            c++;
        }

        target.position = wayPoints[0];

        base.Start();

    }

    protected override void Update()
    {
        if(bb.playerLastPosition != previousSighting)
        {
            personalLastSighting = bb.playerLastPosition;
        }

        switch (state)
        {
            case FSMState.Patrolling:
                Patrolling();
                break;
            case FSMState.Chasing:
                Chasing();
                break;
            case FSMState.Shooting:
                Shooting();
                break;
        }

        previousSighting = bb.playerLastPosition;

    }

    bool CanSeePlayer()
    {
        //�����һ��ڼ����� ��ô��Ϊ���Կ������
        if (memory != null)
            return memory.FindInList();
        return false;
    }

    void Shooting()
    {
        state = FSMState.Shooting;
        animator.SetTrigger("StandingFire");

        if(personalLastSighting == bb.resetPosition)
        {
            state = FSMState.Patrolling;
        }

        if(personalLastSighting != previousSighting && Vector3.Distance(transform.position, previousSighting) > chasingDistance)
        {
            state = FSMState.Chasing;
        }
    }


    void Chasing()
    {
        state = FSMState.Chasing;
        target.position = personalLastSighting;

        if(Vector3.Distance(transform.position,target.position)<shootingDistance && CanSeePlayer())
        {
            state = FSMState.Shooting;
        }
        else
        {
            base.Update();
        }

        animator.SetTrigger("Run");
    }

    void Patrolling()
    {
        state = FSMState.Patrolling;

        if (Vector3.Distance(transform.position, target.position) < 3)
        {
            if(wayPointIndex== wayPoints.Length - 1)
            {
                wayPointIndex = 0;
                target.position = wayPoints[wayPointIndex];
            }
            else
            {
                wayPointIndex++;
                target.position = wayPoints[wayPointIndex];
            }
        }
         
            base.Update();
       

        animator.SetTrigger("Walk");
        //Debug.Log(personalLastSighting+" "+bb.resetPosition);
        if (personalLastSighting != bb.resetPosition)
        {
            state = FSMState.Chasing;
        }
    }
}
