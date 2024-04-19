using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    #region Variables

    public float timer;

    [SerializeField] private TextMeshProUGUI timerTextBox; 
    private float _remainingTime;
    
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
        ResetTimer();
    }

    private void Update()
    {
        RunTimer();
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
        //Add Win Screen
    }

    private void LevelLost()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        //Add Lose Screen
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
