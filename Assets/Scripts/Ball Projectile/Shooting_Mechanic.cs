using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_Mechanic : MonoBehaviour
{
    [SerializeField] private float _shootSpeed;
    [SerializeField] private Transform _startPoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Shoot Ball
            Debug.Log("Shooting Ball");
            ShootBall();
        }
    }

    private void ShootBall()
    {
        rb.velocity = new Vector2(0, 1) * _shootSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null)
            rb.velocity = new Vector2(0f,0f);
    }
}
