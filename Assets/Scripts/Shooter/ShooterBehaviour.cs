using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private GameObject _ball;

    private Rigidbody2D _rb; 
    
    private float ROTATIONLIMIT = 155;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
       // transform.rotation = Quaternion.Euler(0, 0,Mathf.Clamp(transform.rotation.z, -RIGHTANGLE, RIGHTANGLE));
    }

    private void Update()
    {
        RotateShooter();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Shoot Ball
            Debug.Log("Shooting Ball");
            ShootBall();
        }
    }

    private void RotateShooter()
    {
        //Access Raw Input
        var xInput = Input.GetAxisRaw("Horizontal");
        var turretPos = transform.eulerAngles.z;

        //Compute Turret Rotation
        var lookDir = turretPos - xInput * _rotationSpeed;
        var clampedRotation = Mathf.Clamp(lookDir, 25, ROTATIONLIMIT );
        
        _rb.MoveRotation(clampedRotation);
    }

    private void ShootBall()
    {
        GameObject ball = Instantiate(_ball, _shootOrigin.position, _shootOrigin.rotation);
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        
        ballRb.AddForce(_shootOrigin.up * _shootSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other != null)
        //     rb.velocity = new Vector2(0f,0f);
    }
}
