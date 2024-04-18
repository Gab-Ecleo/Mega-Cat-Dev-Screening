using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ShooterBehaviour : MonoBehaviour
{
    #region Variables

    [Header("Properties")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootSpeed;
    
    [Header("Origin Points")]
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private Transform _ballPrevOrigin;
    
    [FormerlySerializedAs("_ball")]
    [Header("Prefabs/Projectiles")]
    [SerializeField] private List<GameObject> _ballPrefab;

    private Rigidbody2D _rb;
    private Rigidbody2D _ballRb;

    private float MINANGLE = 25;
    private float MAXANGLE = 155;
    private bool ballLoaded;
    private bool showingNextBall;

    public static bool hasRed;
    public static bool hasYellow;
    public static bool hasBlue;
    
    private GameObject _loadedBall;
    private GameObject _ballPrev;
    [SerializeField] private List<GameObject> _ammoBank;
    
    #endregion

    #region UnityMethods
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        GameEvents.ON_BALL_COLLISSION += LoadBall;
    }
    
    private void OnDestroy()
    {
        GameEvents.ON_BALL_COLLISSION -= LoadBall;
    }
    
    private void Start()
    {
        ShowNextBall();
    }

    private void Update()
    {
        RotateShooter();
        
        if (Input.GetButtonDown("Fire1"))
            ShootBall();
    }
    
    #endregion

    #region Methods

    private void ShootBall()
    {
        var shootDir = _shootOrigin.up * _shootSpeed;

        _ballRb.AddForce(shootDir, ForceMode2D.Impulse);
        BallPouch.Instance.AddBall(_loadedBall);
        
        ballLoaded = false;
    }
    
    private void RotateShooter()
    {
        //Access Raw Input
        var xInput = Input.GetAxisRaw("Horizontal");
        var turretAngle = transform.eulerAngles.z;

        //Compute Turret Rotation
        var lookDir = turretAngle - xInput * _rotationSpeed;
        var clampedRotation = Mathf.Clamp(lookDir, MINANGLE, MAXANGLE );
        
        _rb.MoveRotation(clampedRotation);
    }
    
    //Show next upcoming bullet before Loading it
    private void ShowNextBall()
    {
        if (showingNextBall) return;

        LoadAmmoBank();
        var ballIndex = Random.Range(0, _ammoBank.Count);
        if (ballIndex > _ammoBank.Count)
            ballIndex -= 1;
        
        _ballPrev = Instantiate(_ammoBank[ballIndex], _ballPrevOrigin.position, _shootOrigin.rotation);
        showingNextBall = true;
        
        _ammoBank.Clear();
        LoadBall();
    }

    //Load the turret, this function is called at the start and recalled every time the ball collides
    private void LoadBall()
    {
        if (ballLoaded) return;
        
        _loadedBall = _ballPrev;
        _loadedBall.transform.position = _shootOrigin.position;
        _ballRb = _loadedBall.GetComponent<Rigidbody2D>();
        
        ballLoaded = true;
        showingNextBall = false;
        ShowNextBall();
    }

    //TODO
    //Make a randomizer as a Temporary bullet choosing system
    private void LoadAmmoBank()
    {
        BallPouch.Instance.CheckColors();
        
        if (hasRed)
            _ammoBank.Add(_ballPrefab[0]);
        if (hasYellow)
            _ammoBank.Add(_ballPrefab[1]);
        if (hasBlue)
            _ammoBank.Add(_ballPrefab[2]);

        hasRed = false;
        hasBlue = false;
        hasYellow = false;
    }
    #endregion
    
}
