using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //子弹的生命期
    public float LifeTime = 3f;
    //如果被子弹击中 减少的生命值
    public int damage = 50;
    //子弹出枪膛的速度
    public float beamVelocity = 100f;
    public Rigidbody rigidbody;

    public void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
    }

    public void Go()
    {
        rigidbody.AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * beamVelocity, ForceMode.Acceleration);
    }

    public void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
