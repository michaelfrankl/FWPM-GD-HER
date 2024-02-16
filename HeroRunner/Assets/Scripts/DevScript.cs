using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevScript : MonoBehaviour
{
    public Toggle _fpsCounterTg;
    public Dropdown DiffDropDown;
    public Slider _heroHealthSlider;
    public Text _heroHealthValue;
    public Slider _heroSpeedSlider;
    public Text _heroSpeedValue;
    public Slider _heroJumpForceSlider;
    public Text _heroJumpForceValue;
    public Slider _crateFuelSlider;
    public Text _crateFuelValue;
    public Slider _cratePointSlider;
    public Text _cratePointValue;
    public Slider _crateLiveSlider;
    public Text _crateLiveValue;
    public Toggle _crateSecretKey;
    public Slider _laserHitSlider;
    public Text _laserHitValue;
    public Slider _laserStaySlider;
    public Text _laserStayValue;
    public Slider _fuelCanSlider;
    public Text _fuelCanValue;
    public Slider _gunBulletHitSlider;
    public Text _gunBulletHitValue;
    public Slider _gunBulletSpeedSlider;
    public Text _gunBulletSpeedValue;
    public Toggle _bulletPositionOneTg;
    public Toggle _bulletPositionTwoTg;
    public Toggle _bulletPositionThreeTg;
    public Toggle _bulletPositionFourTg;
    public Toggle _bulletPositionRandomTg;

    private void Awake()
    {
        if (GameManager.Instance.GetMenuSoundBoolValue())
        {
            GameManager.Instance.PlayMenuMusic();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
        InitializeDiffDropDown();
        InitializeSlider();
        InitializeBulletPositionToggle();
        InitializeLabels();
        DiffDropDown.onValueChanged.AddListener(OnDifficultValueChanged);
        DiffDropDown.value = (GameManager.Instance.GetDifficulty() - 1);
        _heroHealthSlider.onValueChanged.AddListener(OnHeroHealthSliderValueChanged);
        _heroSpeedSlider.onValueChanged.AddListener(OnHeroSpeedSliderValueChanged);
        _heroJumpForceSlider.onValueChanged.AddListener(OnHeroJumpForceSliderValueChanged);
        _crateFuelSlider.onValueChanged.AddListener(OnCrateFuelSliderValueChanged);
        _cratePointSlider.onValueChanged.AddListener(OnCratePointSliderValueChanged);
        _crateLiveSlider.onValueChanged.AddListener(OnCrateLiveSliderValueChanged);
        _crateSecretKey.isOn = GameManager.Instance.IsSecretKeyCollected();
        _laserHitSlider.onValueChanged.AddListener(OnLaserHitValueChanged);
        _laserStaySlider.onValueChanged.AddListener(OnLaserStayValueChanged);
        _fuelCanSlider.onValueChanged.AddListener(OnFuelCanValueChanged);
        _gunBulletHitSlider.onValueChanged.AddListener(OnGunBulletHitValueChanged);
        _gunBulletSpeedSlider.onValueChanged.AddListener(OnGunBulletSpeedValueChanged);
        _fpsCounterTg.isOn = GameManager.Instance.GetDisplayFpsBoolValue();
    }

    /// <summary>
    /// Method to initialize all Slider's default difficulty values
    /// </summary>
    private void InitializeSlider()
    {
        _heroHealthSlider.value = GameManager.Instance.GetHeroStartHealth();
        _heroSpeedSlider.value = GameManager.Instance.GetHeroRunSpeed();
        _heroJumpForceSlider.value = GameManager.Instance.GetHeroJumpForce();
        _crateFuelSlider.value =
            GameManager.Instance.GetFuelCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _cratePointSlider.value =
            GameManager.Instance.GetPointCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _crateLiveSlider.value =
            GameManager.Instance.GetLiveCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _laserHitSlider.value =
            GameManager.Instance.GetLaserHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
        _laserStaySlider.value =
            GameManager.Instance.GetLaserStayDamageForDifficulty(GameManager.Instance.GetDifficulty());
        _fuelCanSlider.value = GameManager.Instance.GetFuelBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _gunBulletHitSlider.value =
            GameManager.Instance.GetGunHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
        _gunBulletSpeedSlider.value = GameManager.Instance.GetBulletSpeed();
    }

    /// <summary>
    /// Method to initialize all Labels default difficulty values
    /// </summary>
    private void InitializeLabels()
    {
        _heroHealthValue.text = string.Empty + GameManager.Instance.GetHeroStartHealth();
        _heroSpeedValue.text = string.Empty + GameManager.Instance.GetHeroRunSpeed();
        _heroJumpForceValue.text = string.Empty + GameManager.Instance.GetHeroJumpForce();
        _crateFuelValue.text = string.Empty +
                               GameManager.Instance.GetFuelCrateBenefitForDifficulty(
                                   GameManager.Instance.GetDifficulty());
        _cratePointValue.text = string.Empty +
                                GameManager.Instance.GetPointCrateBenefitForDifficulty(GameManager.Instance
                                    .GetDifficulty());
        _crateLiveValue.text = string.Empty +
                               GameManager.Instance.GetLiveCrateBenefitForDifficulty(
                                   GameManager.Instance.GetDifficulty());
        _laserHitValue.text = string.Empty +
                              GameManager.Instance.GetLaserHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
        _laserStayValue.text = string.Empty +
                               GameManager.Instance.GetLaserStayDamageForDifficulty(
                                   GameManager.Instance.GetDifficulty());
        _fuelCanValue.text = string.Empty +
                             GameManager.Instance.GetFuelBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _gunBulletHitValue.text = string.Empty +
                                  GameManager.Instance.GetGunHitDamageForDifficulty(
                                      GameManager.Instance.GetDifficulty());
        _gunBulletSpeedValue.text = string.Empty + GameManager.Instance.GetBulletSpeed();
    }
    
    /// <summary>
    /// Method to initialize alle Bullet Position Toggles
    /// </summary>
    private void InitializeBulletPositionToggle()
    {
        _bulletPositionOneTg.isOn = GameManager.Instance.GetBulletPositionOneBoolValue();
        _bulletPositionTwoTg.isOn = GameManager.Instance.GetBulletPositionTwoBoolValue();
        _bulletPositionThreeTg.isOn = GameManager.Instance.GetBulletPositionThreeBoolValue();
        _bulletPositionFourTg.isOn = GameManager.Instance.GetBulletPositionFourBoolValue();
        _bulletPositionRandomTg.isOn = GameManager.Instance.GetBulletRandomPositionBoolValue();
    }

    #region Crate Slider

    /// <summary>
    /// Event listener method to update the start health of the hero
    /// </summary>
    /// <param name="newValue"></param>
    private void OnHeroHealthSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateHeroStartHealth(newValue);
        _heroHealthValue.text = string.Empty + GameManager.Instance.GetHeroStartHealth();
    }

    /// <summary>
    /// Event listener method to update the speed of the hero
    /// </summary>
    /// <param name="newValue"></param>
    private void OnHeroSpeedSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateHeroRunSpeed(newValue);
        _heroSpeedValue.text = string.Empty + GameManager.Instance.GetHeroRunSpeed();
    }
    
    /// <summary>
    /// Event listener method to update the jump force of the hero
    /// </summary>
    /// <param name="newValue"></param>
    private void OnHeroJumpForceSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateHeroJumpForce(newValue);
        _heroJumpForceValue.text = string.Empty + GameManager.Instance.GetHeroJumpForce();
    }

    /// <summary>
    /// Event listener method to update the value of the fuel crate
    /// </summary>
    /// <param name="newValue"></param>
    private void OnCrateFuelSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateBenefitForFuelCanCrateForDifficulty(GameManager.Instance.GetDifficulty(), newValue);
        _crateFuelValue.text = string.Empty +
                               GameManager.Instance.GetFuelCrateBenefitForDifficulty(
                                   GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Event listener method to update the value of the point crate
    /// </summary>
    /// <param name="newValue"></param>
    private void OnCratePointSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateBenefitForPointCrateForDifficulty(GameManager.Instance.GetDifficulty(),
            (int)newValue);
        _cratePointValue.text = string.Empty +
                                GameManager.Instance.GetPointCrateBenefitForDifficulty(
                                    GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Event listener method to update the value of the live crate
    /// </summary>
    /// <param name="newValue"></param>
    private void OnCrateLiveSliderValueChanged(float newValue)
    {
        GameManager.Instance.UpdateBenefitForLiveCrateForDifficulty(GameManager.Instance.GetDifficulty(),
            (int)newValue);
        _crateLiveValue.text = string.Empty +
                               GameManager.Instance.GetLiveCrateBenefitForDifficulty(
                                   GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Method for the secret key toggle to change the value
    /// </summary>
    public void OnSecretKeyCollectedChanged()
    {
        if (_crateSecretKey.isOn)
            GameManager.Instance.UpdateSecretKeyCollected(true);
        else
            GameManager.Instance.UpdateSecretKeyCollected(false);
    }

    #endregion

    /// <summary>
    /// Event listener method to update the value of the fuel can
    /// </summary>
    /// <param name="newValue"></param>
    private void OnFuelCanValueChanged(float newValue)
    {
        GameManager.Instance.UpdateBenefitForFuelCanForDifficulty(GameManager.Instance.GetDifficulty(), newValue);
        _fuelCanValue.text = string.Empty +
                             GameManager.Instance.GetFuelBenefitForDifficulty(GameManager.Instance.GetDifficulty());
    }

    #region Obstacle Slider

    /// <summary>
    /// Event listener method to update the damage of laser hit
    /// </summary>
    /// <param name="newValue"></param>
    private void OnLaserHitValueChanged(float newValue)
    {
        GameManager.Instance.UpdateDamageOfLaserHitForDifficulty(GameManager.Instance.GetDifficulty(), newValue);
        _laserHitValue.text = string.Empty +
                              GameManager.Instance.GetLaserHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Event listener method to update the damage of laser hit
    /// </summary>
    /// <param name="newValue"></param>
    private void OnLaserStayValueChanged(float newValue)
    {
        GameManager.Instance.UpdateDamageOfLaserStayForDifficulty(GameManager.Instance.GetDifficulty(), newValue);
        _laserStayValue.text = string.Empty +
                              GameManager.Instance.GetLaserStayDamageForDifficulty(GameManager.Instance
                                  .GetDifficulty());
    }

    /// <summary>
    /// Event listener method to update the damage of gun bullet hit
    /// </summary>
    /// <param name="newValue"></param>
    private void OnGunBulletHitValueChanged(float newValue)
    {
        GameManager.Instance.UpdateDamageOfGunHitForDifficulty(GameManager.Instance.GetDifficulty(), newValue);
        _gunBulletHitValue.text = string.Empty +
                                  GameManager.Instance.GetGunHitDamageForDifficulty(
                                      GameManager.Instance.GetDifficulty());
    }

    /// <summary>
    /// Event listener method to update the damage of gun bullet hit
    /// </summary>
    /// <param name="newValue"></param>
    private void OnGunBulletSpeedValueChanged(float newValue)
    {
        GameManager.Instance.UpdateBulletSpeed(newValue);
        _gunBulletSpeedValue.text = string.Empty + GameManager.Instance.GetBulletSpeed();
    }

#endregion
    
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
        InitializeBulletPositionToggle();
    }

    /// <summary>
    /// Method to change the value of the bullet position boolean
    /// </summary>
    public void OnBulletPositionOneBoolChanged()
    {
        if(_bulletPositionOneTg.isOn)
            GameManager.Instance.UpdateBulletPositionOneBoolValue(true);
        else
            GameManager.Instance.UpdateBulletPositionOneBoolValue(false);
    }
    
    /// <summary>
    /// Method to change the value of the bullet position boolean
    /// </summary>
    public void OnBulletPositionTwoBoolChanged()
    {
        if(_bulletPositionTwoTg.isOn)
            GameManager.Instance.UpdateBulletPositionTwoBoolValue(true);
        else
            GameManager.Instance.UpdateBulletPositionTwoBoolValue(false);
    }
    
    /// <summary>
    /// Method to change the value of the bullet position boolean
    /// </summary>
    public void OnBulletPositionThreeBoolChanged()
    {
        if(_bulletPositionThreeTg.isOn)
            GameManager.Instance.UpdateBulletPositionThreeBoolValue(true);
        else
            GameManager.Instance.UpdateBulletPositionThreeBoolValue(false);
    }
    
    /// <summary>
    /// Method to change the value of the bullet position boolean
    /// </summary>
    public void OnBulletPositionFourBoolChanged()
    {
        if(_bulletPositionFourTg.isOn)
            GameManager.Instance.UpdateBulletPositionFourBoolValue(true);
        else
            GameManager.Instance.UpdateBulletPositionFourBoolValue(false);
    }

    /// <summary>
    /// Method to change the value of the bullet random position boolean
    /// </summary>
    public void OnBulletRandomPositionBoolChanged()
    {
        if(_bulletPositionRandomTg.isOn)
            GameManager.Instance.UpdateBulletRandomPositionBoolValue(true);
        else
            GameManager.Instance.UpdateBulletRandomPositionBoolValue(false);
    }

    /// <summary>
    /// Method to change the value of the fps boolean
    /// </summary>
    public void OnFpsCounterBoolChanged()
    {
        if(_fpsCounterTg.isOn)
            GameManager.Instance.UpdateDisplayFpsBoolValue(true);
        else
            GameManager.Instance.UpdateDisplayFpsBoolValue(false);
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
}
