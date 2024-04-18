using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorCoding
{
    Red,
    Blue,
    Yellow
}

public class BallProperties : MonoBehaviour
{
    public ColorCoding BallColor;

    private SpriteRenderer _ballSprite;

    private void Awake()
    {
        _ballSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetBallColor();
    }

    private void SetBallColor()
    {
        var color = Color.white;

        switch (BallColor)
        {
            case ColorCoding.Red:
                color = Color.red;
                break;
            case ColorCoding.Blue:
                color = Color.blue;
                break;
            case ColorCoding.Yellow:
                color = Color.yellow;
                break;
        }

        _ballSprite.color = color;
    }
}



