using System;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParentPouch;

    [SerializeField] private Vector2 gridPos;
    [SerializeField] private float gridSize;

    private Rigidbody2D rb;
    private BallPouch _ballPouch;
    private BallProperties _ballProp;

    [SerializeField] private bool isProjectile;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _ballProp = GetComponent<BallProperties>();
        _ballPouch = BallPouch.Instance;
        
        ParentPouch = GameObject.Find("BallPouch");
        isProjectile = true;
    }

    private void FixedUpdate()
    {
        if (!isProjectile)
            SnapToGrid();
    }

    private void OnDestroy()
    {
        _ballPouch.RemoveBall(gameObject);
        CheckNeigboringBalls();
    }

    private void SnapToGrid()
    {
        var currentPos = transform.position - (Vector3)gridPos;
        var nearestRow = Mathf.RoundToInt(currentPos.x / gridSize);
        var nearestColumn = Mathf.RoundToInt(currentPos.y / gridSize);

        var newPos = new Vector2(nearestRow * gridSize + gridPos.x, nearestColumn * gridSize + gridPos.y);

        transform.position = newPos;
    }

    private void CheckNeigboringBalls()
    {
        Collider2D[] nBall = Physics2D.OverlapCircleAll(transform.position, 1f);
        
        foreach (var ball in nBall)
        {
            if (ball.gameObject.tag == "Bounceable" || ball.gameObject.tag == "Obstacle") return;

            //var ballCol = ball.gameObject.GetComponent<BallProperties>().BallColor;

            if (ball.gameObject.tag == gameObject.tag)
            {
                Debug.Log("Same Color");
                Destroy(ball.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("is colliding");
        if (other.gameObject.tag == "Bounceable") return;

        if (!_ballPouch.ContainsBall(gameObject))
            _ballPouch.AddBall(gameObject);
        
        rb.velocity = Vector3.zero;

        //rb.velocity = Vector3.zero;
        gameObject.transform.parent = ParentPouch.transform;

        if (other.gameObject.CompareTag(gameObject.tag))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
        isProjectile = false;
        GameEvents.ON_BALL_COLLISSION?.Invoke();
    }
}
