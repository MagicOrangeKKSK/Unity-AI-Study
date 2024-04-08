using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float input_X;
    private float input_Y;

    public float _speed = 1;
    private int health;

    private void Start()
    {
        health = 100;

    }
    private void Update()
    {
        input_X = Input.GetAxis("Horizontal");
        input_Y = Input.GetAxis("Vertical");

        transform.position += new Vector3(input_X, 0, input_Y) * _speed * Time.deltaTime;
    }
}
