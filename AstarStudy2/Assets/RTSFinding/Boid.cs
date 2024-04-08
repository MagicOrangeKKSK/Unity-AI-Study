using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MovementState : int
{
    IDLE,  //��ֹ
    MOVING,  //�ƶ�
    ORGANIZING //��֯
}

public class Boid : MonoBehaviour
{
    public float movementSpeed = 1;
    public GameObject target;

    //��ǰ�˶�״̬
    public MovementState currentMovementState;
    public MovementState CurrentMovementState
    {
        get
        {
            return currentMovementState;
        }
        set
        {
            currentMovementState = value;
        }
    }

    public float coherencyWeight  = 0;//��Ԫ��˴�Ϸ����ǿ��
    //��Ԫ��˴˷����ǿ��
    public float separationWeight = 1;
    public float turnSpeed = 4.0f;

    private Vector3 relativePos;
    //�ۼ���Ϊ�еĵõ��Ĳٿ���
    private Vector3 coherency;
    //������Ϊ�еõ��Ĳٿ���
    private Vector3 separation;
    //Ⱥ����Ϊ�������ܲٿ���
    private Vector3 boidBehaviorForce;


    private List<Boid> boids;
    private CharacterController controller;
    private Seeker seeker;
    private Path path; //��ԪҪ�����·��
    //��ǰ·��
    private int currentWayPoint = 0;
    //������һ���ڵ�ľ���
    private float nextWayPoint = 1;
    //�ж��Ƿ�ִ����յ�
    private bool journeyComplete = true;
	private Vector3 pathDirection;
    private Vector3 center;  //С�ӵ�����
    private Vector3 steerForce;  //�ٿ���
    private Vector3 seekForce;  //����·��������
    private Vector3 driveForce = Vector3.zero; //�ٿ����͸������Ĺ�ͬ���ò���������
    private Vector3 newForward; //����

    //Ⱥ����Ϊ
    private float radius = 1;//�״�ɨ��뾶
    public int pingsPerSecond = 10; //ÿ��ɨ�輸��
    public float PingFrequency { get { return 1/pingsPerSecond; } }
    //�״�ɨ��  ������һ�β�
    public LayerMask radarLayers;
    //��¼�뾶�� ɨ�赽�ĵ�Ԫ
    private List<Boid> neighbors = new List<Boid>();
    private Collider[] detected;
    

    public void Start()
    {
        //��ȡ��ɫ������
        controller = GetComponent<CharacterController>();
        seeker = GetComponent<Seeker>();
        boids = FindObjectsOfType<Boid>().ToList();
        //���õ�ǰ�ƶ�״̬
        CurrentMovementState = MovementState.IDLE;
        StartCoroutine(StartTick(PingFrequency));
    }

    private IEnumerator StartTick(float freq)
    {
        yield return new WaitForSeconds(freq);
        RadarScan();
    }

    public void RadarScan()
    {
        neighbors.Clear();
        detected = Physics.OverlapSphere(transform.position,radius,radarLayers);
        foreach(Collider c in detected)
        {
            if(c.GetComponent<Boid>() != null && c.gameObject != gameObject)
            {
                neighbors.Add(c.GetComponent<Boid>());
            }
        }

        if(neighbors.Count == 0 && currentMovementState!=MovementState.MOVING &&
            currentMovementState == MovementState.ORGANIZING)
        {
            Debug.LogWarning(currentMovementState);
            CurrentMovementState = MovementState.IDLE;
        }
        StartCoroutine(StartTick(PingFrequency));
    }

    //����Seeker�� ����ӵ�Ԫ�ĵ�ǰλ�� ��Ŀ��λ�õ�·��
    public void CalculatePath()
    {
        Debug.Log("-------------------1");
        if(target == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }

        seeker.StartPath(transform.position, target
            .transform.position, OnPathComplete);
        journeyComplete = false;
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("-------------------2");
        if (p.error)
        {
            Debug.Log("Can't find path!");
            return;
        }

        path = p;

        currentWayPoint = 0;
        CurrentMovementState = MovementState.MOVING;
    }

    private void Update()
    {
        center = Vector3.zero;

        if (boids.Count > 0)
        {
            foreach (Boid b in boids)
            {
                center += b.transform.position;
            }

            center = center / boids.Count;
        }

        steerForce = Vector3.zero;
        seekForce = Vector3.zero;

        //û��� ���������ƶ�
        if (!journeyComplete && currentMovementState == MovementState.MOVING)
        {
            seekForce = FollowPath();
        }
        else
        {
            if (CurrentMovementState != MovementState.ORGANIZING && CurrentMovementState != MovementState.IDLE)
                CurrentMovementState = MovementState.ORGANIZING;
        }

        if (currentMovementState == MovementState.ORGANIZING)
        {
            steerForce = BoidBehaviors();
        }

        driveForce = steerForce + seekForce;
        transform.position += driveForce;
        Debug.Log(driveForce);
    }

    public Vector3 FollowPath()
    {
        if (path == null || currentWayPoint >= path.vectorPath.Count || target == null)
            return Vector3.zero;

        if (currentWayPoint >= path.vectorPath.Count || Vector3.Distance(transform.position, target.transform.position) < 0.2)
        {
            journeyComplete = true;
            return Vector3.zero;
        }

        pathDirection = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        pathDirection *= movementSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPoint)
        {
            currentWayPoint++;
        }

        return pathDirection;
    }

    public Vector3 BoidBehaviors()
    {
        Vector3 boidBehaviorForce;

        coherency = center - transform.position;
        separation = Vector3.zero;

        foreach (Boid b in neighbors)
        {
            if (b != this)
            {
                relativePos = (transform.position - b.transform.position);
                separation += relativePos / relativePos.sqrMagnitude;
            }
        }

        boidBehaviorForce = (coherency * coherencyWeight) + (separation * separationWeight);

        boidBehaviorForce.y = 0;

        return boidBehaviorForce;
    }
}
