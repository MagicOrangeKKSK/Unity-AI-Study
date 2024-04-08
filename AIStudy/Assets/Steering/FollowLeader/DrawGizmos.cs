using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 显示领队行进路线前方的检测球
/// 如果跟随着进入检测球
/// 说明跟随着挡住了领队的路线
/// 需要暂时的给跟随着加上躲避行为
/// 好让它给领队让路
/// </summary>
public class DrawGizmos : MonoBehaviour
{
    public float evadeDistance;
    //领队前方的一个点
    private Vector3 center;
    private Vehicle vehicleScript;
    private float LEADER_BEHIND_DIST;

    private void Start()
    {
        vehicleScript = GetComponent<Vehicle>();
        LEADER_BEHIND_DIST = 2f;
    }

    private void Update()
    {
        center = transform.position + vehicleScript.Velocity.normalized * LEADER_BEHIND_DIST;
    }

    private void OnDrawGizmos()
    {
        //画一个领队前方的球
        Gizmos.DrawWireSphere(center, evadeDistance);
    }
}
