using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame
{
    public class AstarAI : MonoBehaviour
    {
        //目标位置
        public Transform target;

        private Seeker seeker;
        private Path path;

        //角色的每秒速度
        public float speed = 30f;
        //当角色与导航节点小于这个距离时，角色便转向下一个节点
        public float nextWaypointDistance = 3f;
        //角色当前的导航点下标
        private int currentWaypoint = 0;

        private void Start()
        {
            seeker = GetComponent<Seeker>();
            seeker.pathCallback += OnPathComplete;

            Debug.Log(transform.position + " to " + target.position);
            seeker.StartPath(transform.position, target.position);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        private void FixedUpdate()
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                Debug.Log("寻路结束");
                return;
            }
            //求出前进方向
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;
            transform.position += dir;

            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        private void OnDisable()
        {
            seeker.pathCallback -= OnPathComplete;
        }
    }
}