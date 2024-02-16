using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public void GoBackToMenu()
    {
        FindObjectOfType<HeroScript>().ResumeGame();
        GameManager.Instance.LoadHomeScreen();
    }

    /// <summary>
    /// TODO: Create level
    /// </summary>
    public void LoadSecretLevel()
    {
        FindObjectOfType<HeroScript>().ResumeGame();
        GameManager.Instance.LoadSecretLevelScreen();
    }

    public void LoadDevSettingsScreen()
    {
        FindObjectOfType<HeroScript>().ResumeGame();
        GameManager.Instance.LoadDevScreen();
    }
}
