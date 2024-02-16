using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Text _scoreUI;
    public Text _collectFuelUI;
    private void Awake()
    {
        GameManager.Instance.UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
        if (GameManager.Instance.GetGameOverSoundBoolValue())
        {
            AudioSource Sound = GameManager.Instance._mainCamera.GetComponent<AudioSource>();
            Sound.loop = true;
            Sound.Play();
        }
#if DEVELOPMENT
        Debug.Log($"Updated Camera --> {GameManager.Instance._mainCamera}");
#endif
    }

    private void Start()
    {
        GetCurrentScoreAndDisplay();
        GetCurrentCollectiblesAndDisplay();
    }

    private void GetCurrentCollectiblesAndDisplay()
    {
        _collectFuelUI.text = string.Empty + GameManager.Instance.GetFuelCount();
    }

    private void GetCurrentScoreAndDisplay()
    {
        _scoreUI.text = string.Empty + GameManager.Instance.GetGameScore() + "M";
    }

    public void StartNewGame()
        => GameManager.Instance.LoadGameScreen();

    public void BackToMenu()
        => GameManager.Instance.LoadHomeScreen();
}
