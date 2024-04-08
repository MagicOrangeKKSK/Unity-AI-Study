using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFSM : FSM 
{
   public enum FSMState
    {
        Patrol , //Ѳ�� 
        Chase,   //׷��
        Attack,//����
        Dead,//����
    }

    //���忪ʼ׷����ҵľ���
    public float chaseDistance = 40f;
    //���忪ʼ������ҵľ���
    public float attackDistance = 20f;
    //����Ѳ�ߵ�С�����ֵʱ ��Ϊ�Ѿ�����Ѳ�ߵ�
    public float arriveDistance = 3f;

    //�ӵ������ɵ�
    public Transform bulletSpawnPoint;
    private CharacterController controller;

    //AI��ɫ�ĵ�ǰ״̬
    public FSMState curState;

    //AI��ɫ���ٶ�
    public float walkSpeed = 80f;
    public float runSpeed = 160f;


    //AI��ɫ��ת���ٶ�
    public float curRotSpeed = 6f;
    //�ӵ�Ԥ����
    public GameObject Bullet;

    //AI��ɫ�Ƿ�����
    private bool bDead;
    private int health;

    protected override void Initialize()
    {
        //���õ�ǰ״̬ΪѲ��
        curState = FSMState.Patrol;

        bDead = false;
        elapsedTime = 0f;
        shootRate = 0.5f;
        health = 100;

        pointList = GameObject.FindGameObjectsWithTag("PatrolPoints");

        controller = GetComponent<CharacterController>();
        //����

        //���ѡ��һ��Ѳ�ߵ�
        FindNextPoint();

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

        if (!playerTransform)
            Debug.LogError("û����ҽ�ɫ");
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
        //����Ѿ��ִ�Ѳ�ߵ� ��ôѰ����һ��Ѳ�ߵ�
        if (Vector3.Distance(transform.position, destPos) <= arriveDistance)
        {
            FindNextPoint();
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance)
        {
            //�������Ҿ��� �������Ҿ���Ͻ� ���л�Ϊ׷��״̬
            curState = FSMState.Chase;
        }

        //��Ŀ���ת��
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //��ǰ�ƶ�
        controller.SimpleMove(transform.forward * Time.deltaTime * walkSpeed);
        //�������߶���
    }


    protected void UpdateChaseState()
    {
        //��Ŀ��λ������Ϊ��ҵ�λ��
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

        //��Ŀ���ת��
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //��ǰ�ƶ�
        controller.SimpleMove(transform.forward * Time.deltaTime * runSpeed);
        //���ű��ܶ���
    }

    //���¹���״̬
    protected void UpdateAttackState()
    {
        Quaternion targetRotation;
        //����Ŀ���
        destPos = playerTransform.position;
        //�������ҵľ���
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        //����ڹ���������׷�����֮�� ��ת��Ϊ׷��״̬
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

        //ת��Ŀ���
        targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //�����ӵ�
        ShootBullet();

        //�����������
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
        Debug.Log("�ҵ���һ��");
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }





}
