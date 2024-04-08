using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MovementState : int
{
    IDLE,  //静止
    MOVING,  //移动
    ORGANIZING //组织
}

public class Boid : MonoBehaviour
{
    public float movementSpeed = 1;
    public GameObject target;

    //当前运动状态
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

    public float coherencyWeight  = 0;//单元间彼此戏引的强度
    //单元间彼此分离的强度
    public float separationWeight = 1;
    public float turnSpeed = 4.0f;

    private Vector3 relativePos;
    //聚集行为中的得到的操控力
    private Vector3 coherency;
    //分离行为中得到的操控力
    private Vector3 separation;
    //群组行为产生的总操控力
    private Vector3 boidBehaviorForce;


    private List<Boid> boids;
    private CharacterController controller;
    private Seeker seeker;
    private Path path; //单元要跟随的路径
    //当前路点
    private int currentWayPoint = 0;
    //距离下一个节点的距离
    private float nextWayPoint = 1;
    //判断是否抵达了终点
    private bool journeyComplete = true;
	private Vector3 pathDirection;
    private Vector3 center;  //小队的中心
    private Vector3 steerForce;  //操控力
    private Vector3 seekForce;  //跟随路径的向量
    private Vector3 driveForce = Vector3.zero; //操控力和跟随力的共同作用产生的向量
    private Vector3 newForward; //朝向

    //群组行为
    private float radius = 1;//雷达扫描半径
    public int pingsPerSecond = 10; //每秒扫描几次
    public float PingFrequency { get { return 1/pingsPerSecond; } }
    //雷达扫描  监视哪一次层
    public LayerMask radarLayers;
    //记录半径内 扫描到的单元
    private List<Boid> neighbors = new List<Boid>();
    private Collider[] detected;
    

    public void Start()
    {
        //获取角色控制器
        controller = GetComponent<CharacterController>();
        seeker = GetComponent<Seeker>();
        boids = FindObjectsOfType<Boid>().ToList();
        //设置当前移动状态
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

    //利用Seeker类 计算从单元的当前位置 到目标位置的路径
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

        //没完成 并且正在移动
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
