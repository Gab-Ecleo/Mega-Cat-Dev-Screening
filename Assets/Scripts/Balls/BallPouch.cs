using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class BallPouch : MonoBehaviour
{
    private static BallPouch _instance;
    public static BallPouch Instance => _instance;
    
    [SerializeField] private List<GameObject> _pouch;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
        
        LoadBallPouch();
    }

    private void Update()
    {
        if (_pouch.Count == 0) 
            GameEvents.ON_GAME_CLEAR?.Invoke();
    }

    public void CheckColors()
    {
        if (_pouch == null) return;

        foreach (var ball in _pouch)
        {
            BallProperties ballProp = ball.GetComponent<BallProperties>();

            switch (ballProp.BallColor)
            {
                case ColorCoding.Red:
                    ShooterBehaviour.hasRed = true;
                    break;
                case ColorCoding.Blue:
                    ShooterBehaviour.hasBlue = true;
                    break;
                case ColorCoding.Yellow:
                    ShooterBehaviour.hasYellow = true;
                    break;
                default:
                    ShooterBehaviour.hasRed = false;
                    ShooterBehaviour.hasYellow = false;
                    ShooterBehaviour.hasBlue = false;
                    break;
            }
        }
    }
    
    private void LoadBallPouch()
    {
        _pouch.Clear();
        foreach (CircleCollider2D ballObj in gameObject.GetComponentsInChildren<CircleCollider2D>())
        {
            _pouch.Add(ballObj.gameObject);
        }
    }

    public void AddBall(GameObject ball)
    {
        _pouch.Add(ball);    
    }
    
    public void RemoveBall(GameObject ball)
    {
        _pouch.Remove(ball);
    }

    public bool ContainsBall(GameObject ball)
    {
        return _pouch.Contains(ball);
    }
}
