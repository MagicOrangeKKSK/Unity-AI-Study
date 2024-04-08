using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Transform _t;
    private float input_x;
    private float input_y;
    public float antiBunny = 0.75f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _speed = 1;
    public float gravity = 20;
    private float rotateAngle;
    private float targetAngle;
    private float currentAngle;
    private float yVelocity = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _t = transform;

        currentAngle = targetAngle = HorizontalAngle(transform.forward);
    }

    private void Update()
    {
        rotateAngle = 0f;
        if(Input.GetKey(KeyCode.Q))
            rotateAngle = -1 * Time.deltaTime * 50f;
        if (Input.GetKey(KeyCode.E))
            rotateAngle = 1 * Time.deltaTime * 50f;

        targetAngle += rotateAngle;
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref yVelocity, 0.3f);
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);

        float input_modifier = (input_x != 0.0f && input_y != 0.0f) ? 0.7071f : 1.0f;

        input_x = 0;
        input_y = 0;
        if (Input.GetKey(KeyCode.A))
            input_x = -1;
        if (Input.GetKey(KeyCode.D))
            input_x = 1;
        if (Input.GetKey(KeyCode.W))
            input_y = 1;
        if (Input.GetKey(KeyCode.S))
            input_y = -1;



        _velocity = new Vector3(input_x * input_modifier, -antiBunny, input_y * input_modifier);
        _velocity = _t.TransformDirection(_velocity) * _speed;
        _velocity.y -= gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);

    }

    private float HorizontalAngle(Vector3 direction)
    {
        float num = Mathf.Atan2(direction.x, direction.z) * 57.295778f; //Mathf.Rad2Deg;
        if (num < 0f)
            num += 360f;
        return num;
    }

}
