using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    #region Variables

    public float timer;

    [SerializeField] private TextMeshProUGUI timerTextBox;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _pauseScreen;
    
    private float _remainingTime;
    private bool _isGameOver;
    
    #endregion

    #region UnityMethods

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        GameEvents.ON_GAME_CLEAR += LevelClear;
        GameEvents.ON_GAME_LOSE += LevelLost;
        
        Time.timeScale = 1;
    }

    private void Start()
    {
        _isGameOver = false;
        ResetTimer();
    }

    private void Update()
    {
        RunTimer();

        if (_isGameOver && Input.anyKeyDown)
            ReturnToTitle();
        
        if (!_isGameOver && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

    }

    private void OnDestroy()
    {
        GameEvents.ON_GAME_CLEAR -= LevelClear;
        GameEvents.ON_GAME_LOSE -= LevelLost;
    }

    #endregion
    

    private void LevelClear()
    {
        Debug.Log("Level Cleared!");
        Time.timeScale = 0;
        _winScreen.SetActive(true);
        _isGameOver = true;
    }

    private void LevelLost()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        _loseScreen.SetActive(true);
        _isGameOver = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseScreen.SetActive(true);
    }
    
    public void ResumeGame()
    {
        _pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    private void RunTimer()
    {
        {
            if (_remainingTime > 0)
                _remainingTime -= Time.deltaTime;
            else if (_remainingTime < 0)
            {
                ResetTimer();
                GameEvents.ON_TIMER_END?.Invoke();
            }
        
            int minutes = Mathf.FloorToInt(_remainingTime / 60);
            int seconds = Mathf.FloorToInt(_remainingTime % 60);
            timerTextBox.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }
    }

    private void ResetTimer()
    {
        _remainingTime = timer;
    }
}
