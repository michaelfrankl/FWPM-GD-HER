using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpScript : MonoBehaviour
{
    
    public Dropdown DiffDropDown;
    public Text FuelCrateUI;
    public Text PointCrateUI;
    public Text LiveCrateUI;
    public Text LaserHitUI;
    public Text LaserStayUI;
    public Text GunHitUI;
    public Text FuelBenefitUI;
    public GameObject FuelCrateTT;
    public Text FuelCrateTTT;
    public GameObject LiveCrateTT;
    public Text LiveCrateTTT;
    public GameObject SecretKeyTT;
    public Text SecretKeyTTT;

    
    void Start()
    {
        InitializeDiffDropDown();
        FuelCrateTTT.gameObject.SetActive(false);
        DiffDropDown.onValueChanged.AddListener(OnDifficultValueChanged);
        DiffDropDown.value = (GameManager.Instance.GetDifficulty() - 1);
        InitializeLabels();
        InitializeToolTipsForDifficulty(GameManager.Instance.GetDifficulty());
    }
    
    /// <summary>
    /// Method to update the difficulty if the value is changed
    /// </summary>
    /// <param name="index"></param>
    private void OnDifficultValueChanged(int index)
    {
        string _currentValue = DiffDropDown.options[index].text;
        if(_currentValue == "Easy")
            GameManager.Instance.UpdateDifficulty(1);
        else if(_currentValue == "Normal")
            GameManager.Instance.UpdateDifficulty(2);
        else if(_currentValue == "Hard")
            GameManager.Instance.UpdateDifficulty(3);
        UpdateAllUIElements();
    }

    /// <summary>
    /// Method to update all specific ui elements and values
    /// after the difficulty changed
    /// </summary>
    private void UpdateAllUIElements()
    {
        InitializeLabels();
        InitializeToolTipsForDifficulty(GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Method to disable all unnecessary tooltips for a specific difficulty
    /// </summary>
    /// <param name="diff"></param>
    private void InitializeToolTipsForDifficulty(int diff)
    {
        if (diff == 1)
        {
            FuelCrateTT.SetActive(false);
            LiveCrateTT.SetActive(true);
            SecretKeyTT.SetActive(true);
        }
        else if (diff == 2)
        {
            FuelCrateTT.SetActive(false);
            LiveCrateTT.SetActive(true);
            SecretKeyTT.SetActive(true);
        }
        else if (diff == 3)
        {
            FuelCrateTT.SetActive(true);
            LiveCrateTT.SetActive(true);
            SecretKeyTT.SetActive(true);
        }
    }


    /// <summary>
    /// Method to initialize/update all labels
    /// </summary>
    private void InitializeLabels()
    {
        FuelCrateUI.text =
            "+" + GameManager.Instance.GetFuelCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        PointCrateUI.text =
            "+" + GameManager.Instance.GetPointCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        LiveCrateUI.text =
            "+" + GameManager.Instance.GetLiveCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        LaserHitUI.text = "HIT: -" +
                          GameManager.Instance.GetLaserHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
        LaserStayUI.text = "STAY: -" +
                           GameManager.Instance.GetLaserStayDamageForDifficulty(
                               GameManager.Instance.GetDifficulty());
        GunHitUI.text = "HIT: -" +
                        GameManager.Instance.GetGunHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
        FuelBenefitUI.text =
            "+" + GameManager.Instance.GetFuelBenefitForDifficulty(GameManager.Instance.GetDifficulty());
    }
    
    /// <summary>
    /// Method to initialize the difficulties
    /// </summary>
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

    
    /// <summary>
    /// Method to handle the different tool tip objects
    /// </summary>
    /// <param name="_type"></param>
    public void ShowTypeOfToolTipText(ToolTipType _type)
    {
        switch (_type)
        {
            case ToolTipType.FuelCrate:
                ActivateFuelCrateToolTipForDifficulty(GameManager.Instance.GetDifficulty());
                break;
            case ToolTipType.LiveCrate:
                ActivateLiveCrateToolTipForDifficulty(GameManager.Instance.GetDifficulty());
                break;
            case ToolTipType.SecretKey:
                ActivateSecretKeyToolTipForDifficulty(GameManager.Instance.GetDifficulty());
                break;
        }
    }

    public void HideToolTip()
    {
        DisableAllRequirementsLabel();
    }

    /// <summary>
    /// Method to disable every tool tip label
    /// </summary>
    private void DisableAllRequirementsLabel()
    {
        FuelCrateTTT.gameObject.SetActive(false);
        LiveCrateTTT.gameObject.SetActive(false);
        SecretKeyTTT.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method to display the tooltip with specific difficulty
    /// requirements
    /// </summary>
    /// <param name="diff"></param>
    private void ActivateFuelCrateToolTipForDifficulty(int diff)
    {
        string prep = $"Unlock with {GameManager.Instance.GetFuelCrateRequirementsForDifficulty(diff)} Points and a";
        FuelCrateTTT.text = prep;
        
        if (diff == 3)
        {
            FuelCrateTTT.text += " 50% spawn rate!";
            FuelCrateTTT.gameObject.SetActive(true);
        }
        else
            throw new NotImplementedException($"Difficulty {diff} is not implemented!");

    }

    /// <summary>
    /// Method to display the tooltip with specific difficulty
    /// requirements
    /// </summary>
    private void ActivateLiveCrateToolTipForDifficulty(int diff)
    {
        string prep = $"Unlock with {GameManager.Instance.GetLiveCrateRequirementsForDifficulty(diff)} Points and a ";
        LiveCrateTTT.text = prep;
        
        if (diff == 1)
            LiveCrateTTT.text += "50%";
        if (diff == 2)
            LiveCrateTTT.text += "30%";
        if (diff == 3)
            LiveCrateTTT.text += "10%";

        LiveCrateTTT.text += " spawn rate!";
        LiveCrateTTT.gameObject.SetActive(true);
    }

    /// <summary>
    /// Method to display the tooltip with specific difficulty
    /// requirements
    /// </summary>
    private void ActivateSecretKeyToolTipForDifficulty(int diff)
    {
        string prep = $"Unlock with {GameManager.Instance.GetSecretKeyRequirementsForDifficulty(diff)} Points and a ";
        SecretKeyTTT.text = prep;
        
        if (diff == 1)
            SecretKeyTTT.text += "35%";
        if (diff == 2)
            SecretKeyTTT.text += "25%";
        if (diff == 3)
            SecretKeyTTT.text += "5%";

        SecretKeyTTT.text += " spawn rate!";
        SecretKeyTTT.gameObject.SetActive(true);
    }
}
