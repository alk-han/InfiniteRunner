using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UISwitcher menuSwitcher;
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform howToPlayMenu;
    [SerializeField] private Transform leaderBoardMenu;


    public void StartGame()
    {
        Utils.GetGameManager().LoadFirstLevel();
    }


    public void BackToMainMenu()
    {
        menuSwitcher.SetActiveUI(mainMenu);
    }


    public void GoToHowToPlayMenu()
    {
        menuSwitcher.SetActiveUI(howToPlayMenu);
    }


    public void GoToLeaderBoardMenu()
    {
        menuSwitcher.SetActiveUI(leaderBoardMenu);
    }


    public void QuitGame()
    {
        Utils.GetGameManager().QuitGame();
    }
}
