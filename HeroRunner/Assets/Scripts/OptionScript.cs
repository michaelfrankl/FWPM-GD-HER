using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    public Toggle TgGameSound;
    public Toggle TgGameOverSound;
    public Toggle TgMenuSound;
    public Dropdown DiffDropDown;

    private void Awake()
    {
        InitializeDiffDropDown();
        if (GameManager.Instance.GetMenuSoundBoolValue())
        {
            GameManager.Instance.PlayMenuMusic();
        }
    }

    void Start()
    {
        GameManager.Instance.UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
        TgGameSound.isOn = GameManager.Instance.GetGameSoundBoolValue();
        TgGameOverSound.isOn = GameManager.Instance.GetGameOverSoundBoolValue();
        TgMenuSound.isOn = GameManager.Instance.GetMenuSoundBoolValue();
        DiffDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        DiffDropDown.value = (GameManager.Instance.GetDifficulty()-1);
    }

    private void OnDropdownValueChanged(int index)
    {
        string _currentValue = DiffDropDown.options[index].text;
        if(_currentValue == "Easy")
            GameManager.Instance.UpdateDifficulty(1);
        else if(_currentValue == "Normal")
            GameManager.Instance.UpdateDifficulty(2);
        else if(_currentValue == "Hard")
            GameManager.Instance.UpdateDifficulty(3);
    }
    
    public void UpdateGameSoundPreference()
    {
        if (TgGameSound.isOn)
        {
            GameManager.Instance.UpdateGameSoundBoolValue(true);
#if DEVELOPMENT
            Debug.Log("Activate Game Sound");
#endif
        }
        else
        {
            GameManager.Instance.UpdateGameSoundBoolValue(false);
#if DEVELOPMENT
            Debug.Log("Deactivate Game Sound");
#endif
        }
    }
    
    public void UpdateMenuSoundPreference()
    {
        if (TgMenuSound.isOn)
        {
            GameManager.Instance.UpdateMenuSoundBoolValue(true);
            AudioSource Sound = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
            Sound.loop = true;
            Sound.Play();
#if DEVELOPMENT
            Debug.Log("Activate Menu Sound");
#endif
        }
        else
        {
            GameManager.Instance.UpdateMenuSoundBoolValue(false);
            AudioSource Sound = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
            Sound.loop = false;
            Sound.playOnAwake = false;
            Sound.Stop();
            
#if DEVELOPMENT
            Debug.Log("Deactivate Menu Sound");
#endif
        }
    }
    
    public void UpdateGameOverSoundPreference()
    {
        if (TgGameOverSound.isOn)
        {
            GameManager.Instance.UpdateGameOverSoundBoolValue(true);
#if DEVELOPMENT
            Debug.Log("Activate Game Over Sound");
#endif
        }
        else
        {
            GameManager.Instance.UpdateGameOverSoundBoolValue(false);
#if DEVELOPMENT
            Debug.Log("Deactivate Game Over Sound");
#endif
        }
    }
    
    private void InitializeDiffDropDown()
    {
        List<Dropdown.OptionData> _values = new List<Dropdown.OptionData>
        {
            new Dropdown.OptionData("Easy"),
            new Dropdown.OptionData("Normal"),
            new Dropdown.OptionData("Hard")
        };
        
        DiffDropDown.AddOptions(_values);
    }
}
