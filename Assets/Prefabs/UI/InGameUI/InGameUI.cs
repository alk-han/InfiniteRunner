using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private UISwitcher uiSwitcher;
    [SerializeField] private Transform inGameUI;
    [SerializeField] private Transform pauseUI;
    [SerializeField] private Transform gameOverUI;
    private GameManager gameManager;

    private void Start()
    {
        ScoreController scoreController = FindObjectOfType<ScoreController>();
        if (scoreController != null)
        {
            scoreController.OnScoreChanged += UpdateScoreText;
        }

        gameManager = Utils.GetGameManager();
        gameManager.OnGameOver += OnGameOver;
    }


    private void UpdateScoreText(int newValue)
    {
        scoreText.SetText($"Score: {newValue}");
    }


    public void SignalPause(bool IsGamePaused)
    {
        if (IsGamePaused)
        {
            uiSwitcher.SetActiveUI(pauseUI);
        }
        else
        {
            uiSwitcher.SetActiveUI(inGameUI);            
        }
    }


    public void ResumeGame()
    {
        Utils.GetGameManager().PauseGame(false);
        uiSwitcher.SetActiveUI(inGameUI);
    }


    public void BackToMainMenu()
    {
        Utils.GetGameManager().BackToMainMenu();
    }


    public void RestartCurrentLevel()
    {
        Utils.GetGameManager().RestartCurrentLevel();
    }


    public void OnGameOver()
    {
        uiSwitcher.SetActiveUI(gameOverUI);
    }


    private void OnDisable()
    {
        gameManager.OnGameOver -= OnGameOver;
    }
}
