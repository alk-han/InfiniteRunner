using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action OnGameOver;
    [SerializeField] int mainMenuSceneIdx = 0;
    [SerializeField] int firstSceneIdx = 1;
    private bool isGamePaused = false;
    private bool isGameOver = false;
    private Player player;
    private SpeedController speedController;


    private void Awake()
    {
        player = FindObjectOfType<Player>();
        speedController = GetComponent<SpeedController>();
    }


    public void LoadFirstLevel()
    {
        LoadScene(firstSceneIdx);
    }


    public void GameOver()
    {
        StartCoroutine(nameof(GameOverRoutine));
    }


    private IEnumerator GameOverRoutine()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        // player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.PlayHitEffect();
        yield return new WaitForSeconds(.3f);
        PauseGame(true);
        isGameOver = true;
        OnGameOver?.Invoke();
    }


    public bool IsGameOver()
    {
        return isGameOver;
    }


    public void PauseGame(bool pause)
    {
        if (isGameOver) return;

        isGamePaused = pause;

        if (pause)
        {
            Time.timeScale = 0;

            if (player)
                player.DisableGameplayInput();
        }
        else
        {
            Time.timeScale = 1;

            if (player)
                player.EnableGameplayInput();
        }
    }


    public void TogglePause()
    {
        if (IsGamePaused())
        {
            PauseGame(false);
        }
        else
        {
            PauseGame(true);
        }
    }


    public bool IsGamePaused()
    {
        // return Time.timeScale == 0;
        return isGamePaused;
    }


    public void BackToMainMenu()
    {
        LoadScene(mainMenuSceneIdx);
    }


    private void LoadScene(int sceneBuildIdx)
    {
        isGameOver = false;
        PauseGame(false);
        SceneManager.LoadScene(sceneBuildIdx);
    }


    public void RestartCurrentLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public SpeedController GetSpeedController()
    {
        return speedController;
    }
}
