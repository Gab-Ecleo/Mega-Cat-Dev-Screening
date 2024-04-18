using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParentPouch;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        ParentPouch = GameObject.Find("BallPouch");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameEvents.ON_BALL_COLLISSION?.Invoke();

        if (other.gameObject.tag == "Bounceable") return;

        //rb.velocity = new Vector2(0f, 0f);
        if (rb != null)
            Destroy(rb);
        
        gameObject.transform.parent = ParentPouch.transform;

        if (!other.gameObject.CompareTag("ColoredBall")) return;
        
        var colBallColor = other.gameObject.GetComponent<BallProperties>().BallColor;
        var ballColor = GetComponent<BallProperties>().BallColor;

        if (colBallColor.Equals(ballColor))
        {
            BallPouch.Instance.RemoveBall(gameObject);
            Destroy(gameObject);
            //Check If there's a same color next to this ball and destroy that as well
        } 
            
    }
}
