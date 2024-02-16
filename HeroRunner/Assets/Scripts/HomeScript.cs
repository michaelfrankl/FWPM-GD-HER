using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    public Button _devBtn;
    public Button _scretLvlBtn;
    public Text _infoUI;

    private DateTime _displayTime;
    private bool _displayMessage;
    void Start()
    {
        _infoUI.gameObject.SetActive(false);
        _scretLvlBtn.gameObject.SetActive(true);
        _devBtn.enabled = false;
        _devBtn.gameObject.SetActive(false);
        if (GameManager.Instance != null)
        {
#if DEVELOPMENT
            _devBtn.enabled = true;
            _devBtn.gameObject.SetActive(true);
#endif
        }
    }

    private void Update()
    {
        if (_displayMessage && DateTime.Now >= (_displayTime.AddSeconds(10)))
        {
            _displayMessage = false;
            _infoUI.gameObject.SetActive(false);
#if DEVELOPMENT
            Debug.Log($"Disable Text at: {DateTime.Now}");
#endif
        }
    }

    public void CheckSecretLevelStatus()
    {
        if(_displayMessage)
            return;
        if (GameManager.Instance.IsSecretKeyCollected()) 
            GameManager.Instance.LoadSecretLevelScreen();
        else
        {
            _displayTime = DateTime.Now;
            _displayMessage = true;
            _infoUI.gameObject.SetActive(true);
#if DEVELOPMENT
            Debug.Log($"Set Text to disable at: {_displayTime}");
#endif
        }
    }

    public void LoadDevScene()
        => GameManager.Instance.LoadDevScreen();
}
