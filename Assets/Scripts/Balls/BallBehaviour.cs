using System;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParentPouch;

    [SerializeField] private Vector2 gridPos;
    [SerializeField] private int gridSize;
    [SerializeField] private float gYOffset;
    [SerializeField] private float gXOffset;
    
    private Rigidbody2D rb;
    private BallPouch _ballPouch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _ballPouch = BallPouch.Instance;
        
        ParentPouch = GameObject.Find("BallPouch");
    }

    private void SnapToGrid()
    {
        var currentPos = transform.position - (Vector3)gridPos;
        var nearestRow = Mathf.RoundToInt(currentPos.x / gridSize);
        var nearestColumn = Mathf.RoundToInt(currentPos.y / gridSize);

        var newPos = new Vector2((nearestRow * gridSize + gridPos.x) - gXOffset, (nearestColumn * gridSize + gridPos.y) - gYOffset);

        transform.position = newPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bounceable") return;

        if (!_ballPouch.ContainsBall(gameObject))
            _ballPouch.AddBall(gameObject);
        
        if (rb != null)
            Destroy(rb);

        if (other.gameObject.CompareTag("ColoredBall"))
        {
            var colBallColor = other.gameObject.GetComponent<BallProperties>().BallColor;
            var ballColor = GetComponent<BallProperties>().BallColor;

            if (colBallColor.Equals(ballColor))
            {
                _ballPouch.RemoveBall(gameObject);
                Destroy(gameObject);
                //Check If there's a same color next to this ball and destroy that as well
            }
        }
        
        SnapToGrid();
        GameEvents.ON_BALL_COLLISSION?.Invoke();
    }
}
