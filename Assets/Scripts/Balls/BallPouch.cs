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
    }

    private void Start()
    {
        LoadBallPouch();
    }

    //TODO
    //Iterate on each object checking their ball property color
    //set true if a color is found then skip the said color until it iterates to the whole list
    
    
    public void CheckColors()
    {
        if (_pouch == null) return;
        
        foreach (var ball in _pouch)
        {
            BallProperties _ballColor = ball.GetComponent<BallProperties>();

            switch (_ballColor.BallColor)
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
            }
        }
    }

    private void LoadBallPouch()
    {
        _pouch.Clear();
        foreach (BallProperties ballObj in gameObject.GetComponentsInChildren<BallProperties>())
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
}
