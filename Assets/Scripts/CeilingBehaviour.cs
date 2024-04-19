using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingBehaviour : MonoBehaviour
{
    private void Awake()
    {
        GameEvents.ON_TIMER_END += MoveCeilingDown;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0);
    }

    private void OnDestroy()
    {
        GameEvents.ON_TIMER_END -= MoveCeilingDown;
    }

    private void MoveCeilingDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

}
