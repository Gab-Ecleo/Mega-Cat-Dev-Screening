using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bounceable") return;

        rb.velocity = new Vector2(0f, 0f);

        
        if (!other.gameObject.CompareTag("ColoredBall")) return;
        
        var colBallColor = other?.gameObject.GetComponent<BallProperties>().BallColor;
        var ballColor = GetComponent<BallProperties>().BallColor;

        if (colBallColor.Equals(ballColor))
        {
            Destroy(gameObject);
            //Check If there's a same color next to this ball and destroy that as well
        } 
            
    }
}
