using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    #region Variables

    [Header("Properties")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootSpeed;
    
    [Header("Origin Points")]
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private Transform _ballPrevOrigin;
    
    [Header("Prefabs/Projectiles")]
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _loadedBall;
    [SerializeField] private GameObject _ballPrev;

    private Rigidbody2D _rb;
    private Rigidbody2D _ballRb;

    private float MINANGLE = 25;
    private float MAXANGLE = 155;
    private bool _ballLoaded;
    private bool _nextBallShown;
    
    #endregion

    #region UnityMethods
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        GameEvents.ON_BALL_COLLISSION += ShowNextBall;
    }

    private void Start()
    {
        ShowNextBall();
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

    private void OnDestroy()
    {
        GameEvents.ON_BALL_COLLISSION -= ShowNextBall;
    }

    #endregion

    #region Methods

    private void RotateShooter()
    {
        //Access Raw Input
        var xInput = Input.GetAxisRaw("Horizontal");
        var turretPos = transform.eulerAngles.z;

        //Compute Turret Rotation
        var lookDir = turretPos - xInput * _rotationSpeed;
        var clampedRotation = Mathf.Clamp(lookDir, MINANGLE, MAXANGLE );
        
        _rb.MoveRotation(clampedRotation);
    }

    private void ShootBall()
    {
        _ballRb.AddForce(_shootOrigin.up * _shootSpeed, ForceMode2D.Impulse);
        _ballLoaded = false;
    }
    
    //TODO
    //Load the turret via a Loading Function, this function is called at the start and recalled every time the ball collides
    private void LoadBall()
    {
        if (_ballLoaded) return;

        _loadedBall = _ballPrev;
        _loadedBall.transform.position = _shootOrigin.position;
        _ballRb = _loadedBall.GetComponent<Rigidbody2D>();
        
        ShowNextBall();
        _ballLoaded = true;
        _nextBallShown = false;
    }
    //Show next upcoming bullet before Loading it
    private void ShowNextBall()
    {
        if (_nextBallShown) return;
        
        Debug.Log("Showing Next Ball");
        
        _ballPrev = Instantiate(_ball, _ballPrevOrigin.position, _shootOrigin.rotation);
        _nextBallShown = true;
        
        LoadBall();
    }
    //Make a randomizer as a Temporary bullet choosing system

    #endregion
    
}
