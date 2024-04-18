using System;
using System.Collections.Generic;
using System.Net.WebSockets;
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
        GameEvents.ON_BALL_COLLISSION += ResetAmmoBank;
    }
    
    private void OnDestroy()
    {
        GameEvents.ON_BALL_COLLISSION -= ResetAmmoBank;
    }
    
    private void Start()
    {
        ResetAmmoBank();
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
        
        _ballPrev = RandomizeBall();

        if (!ColorExist(_ballPrev))
        {
            if (_ballPrev != null)
            {
                Debug.Log("This Function is being called");
                Destroy(_ballPrev);
            }
            _ballPrev = RandomizeBall();
        }
        
        LoadBall();
        showingNextBall = true;
    }

    //Load the turret, this function is called at the start and recalled every time the ball collides
    private void LoadBall()
    {
        if (ballLoaded) return;

        if (_ballPrev != null)
        {
            _loadedBall = _ballPrev;
            _loadedBall.transform.position = _shootOrigin.position;
            _ballRb = _loadedBall.GetComponent<Rigidbody2D>();
        
            ballLoaded = true;
            showingNextBall = false;
        }
        ShowNextBall();
    }

    //TODO
    //Make a randomizer as a Temporary bullet choosing system
    private void LoadAmmoBank()
    {
        _ammoBank.Clear();
        BallPouch.Instance.CheckColors();
        
        if (hasRed)
            _ammoBank.Add(_ballPrefab[0]);
        if (hasYellow)
            _ammoBank.Add(_ballPrefab[1]);
        if (hasBlue)
            _ammoBank.Add(_ballPrefab[2]);
        
        Debug.Log("Color List:");
        foreach (var ob in _ammoBank)
        {
            Debug.Log(ob);
        }

        hasRed = false;
        hasBlue = false;
        hasYellow = false;
    }

    private void ResetAmmoBank()
    {
        Debug.Log("Clearing Ammo Bank");
        LoadAmmoBank();
        LoadBall();
    }

    private GameObject RandomizeBall()
    {
        var ballIndex = Random.Range(0, _ammoBank.Count);
        if (ballIndex > _ammoBank.Count)
            ballIndex -= 1;

        return Instantiate(_ammoBank[ballIndex], _ballPrevOrigin.position, _shootOrigin.rotation);
    }

    private bool ColorExist(GameObject obj)
    {
        var cond = true;
        var lBallProp = obj.GetComponent<BallProperties>();

        foreach (GameObject ball in _ammoBank)
        {
            var ballProp = ball.GetComponent<BallProperties>();

            cond = lBallProp.BallColor == ballProp.BallColor;

            if (cond)
                break;
        }
        
        
        
        return cond;
    }
    #endregion
    
}
