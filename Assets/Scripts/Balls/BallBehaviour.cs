using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ParentPouch;
    
    private Rigidbody2D rb;
    private BallPouch _ballPouch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _ballPouch = BallPouch.Instance;
        
        ParentPouch = GameObject.Find("BallPouch");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bounceable") return;

        if (!_ballPouch.ContainsBall(gameObject))
            _ballPouch.AddBall(gameObject);

        if (rb != null)
            Destroy(rb);
        
        gameObject.transform.parent = ParentPouch.transform;

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
        
        GameEvents.ON_BALL_COLLISSION?.Invoke();
    }
}
