using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [field: SerializeField]
    public bool IsPaused { get; private set; }

    public event Action OnGameOver;
    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) return;
            StartGame();
    }

    public void StartGame()
    {
        if(IsPaused || Time.timeScale == 0) ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused) return;
        GameUpdate();
    }

    void GameUpdate()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void GameOver(bool isVictory)
    {
        if (isVictory)
        {
            
        }
        
        PauseGame();
        OnGameOver?.Invoke();
    }

}
