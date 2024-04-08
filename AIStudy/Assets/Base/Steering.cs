using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : MonoBehaviour
{
    /// <summary>
    /// 操控力的权重
    /// </summary>
    public float Weight = 1;

    /// <summary>
    /// 计算操控力
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 Force()
    {
        return Vector3.zero;
    }
}
