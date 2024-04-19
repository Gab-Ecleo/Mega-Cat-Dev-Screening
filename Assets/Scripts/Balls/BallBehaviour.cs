using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParentPouch;

    [SerializeField] private Vector2 _gridPos;
    [SerializeField] private float _gridSize;

    [SerializeField] private float _hitBoxRange;
    [SerializeField] private LayerMask _layerMask;

    private Rigidbody2D _rb;
    private BallPouch _ballPouch;

    [SerializeField] private bool isProjectile;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ballPouch = BallPouch.Instance;
        
        ParentPouch = GameObject.Find("BallPouch");
    }

    private void FixedUpdate()
    {
        if (!isProjectile)
        {
            SnapToGrid();
            CheckAttachedBall();
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Object Destroyed");
        _ballPouch.RemoveBall(gameObject);
        CheckNeigboringBalls();
    }

    private void SnapToGrid()
    {
        var currentPos = transform.position - (Vector3)_gridPos;
        var nearestRow = Mathf.RoundToInt(currentPos.x / _gridSize);
        var nearestColumn = Mathf.RoundToInt(currentPos.y / _gridSize);

        var newPos = new Vector2(nearestRow * _gridSize + _gridPos.x, nearestColumn * _gridSize + _gridPos.y);

        transform.position = newPos;
    }

    private void CheckAttachedBall()
    {
        RaycastHit2D hitScan = Physics2D.Raycast(transform.position, Vector2.up, _hitBoxRange, _layerMask);

        if(hitScan.collider != null) return;
            Destroy(gameObject);
    }

    private void CheckNeigboringBalls()
    {
        Collider2D[] nBall = Physics2D.OverlapCircleAll(transform.position, 1f);
        
        foreach (var ball in nBall)
        {
            if (ball.gameObject.tag == "Bounceable" || ball.gameObject.tag == "Obstacle") return;
            
            if (ball.gameObject.tag == gameObject.tag)
            {
                Destroy(ball.gameObject);
            }
        }
    }

    private void MoveBallsDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_ballPouch.ContainsBall(gameObject))
            _ballPouch.AddBall(gameObject);
        
        if (other.gameObject.tag == "Bounceable" || !isProjectile) return;
        
        _rb.velocity = Vector3.zero;
        
        gameObject.transform.parent = ParentPouch.transform;

        if (other.gameObject.CompareTag(gameObject.tag))
        {
            Debug.Log("Destroying Object");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
        isProjectile = false;
        GameEvents.ON_BALL_COLLISSION?.Invoke();
    }
}
