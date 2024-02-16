using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static int _currentScene;
    private bool _displayFPS;
    
    private bool _gameSound;
    private bool _menuSound;
    private bool _gameOverSound;
    private float _heroSpeed;
    private float _heroJumpForce;
    private float _heroStartHealth;
    private int _respawnTimeSeconds;
    private int _weaponDelay;
    private int _maxBullets;
    private int _bulletCount;
    private bool _weaponDisabled;
    private int _difficulty;
    private bool _keyCollected;
    private int _gameScore;
    private int _liveCount;
    private int _collectedFuels;
    private float _bulletSpeed;
    public GameObject _mainCamera;

    private bool _bulletPositionOne;
    private bool _bulletPositionTwo;
    private bool _bulletPositionThree;
    private bool _bulletPositionFour;
    private bool _bulletRandomPosition;

    private Vector2 _v2BulletPositionOne;
    private Vector2 _v2BulletPositionTwo;
    private Vector2 _v2BulletPositionThree;
    private Vector2 _v2BulletPositionFour;

    #region Damage & Benefits

    private float FuelCrateBenefitEasy;
    private float FuelCrateBenefitNormal;
    private float FuelCrateBenefitHard;
    private int PointCrateBenefitEasy;
    private int PointCrateBenefitNormal;
    private int PointCrateBenefitHard;
    private int LiveCrateBenefitEasy;
    private int LiveCrateBenefitNormal;
    private int LiveCrateBenefitHard;
    private float LaserHitDamageEasy;
    private float LaserHitDamageNormal;
    private float LaserHitDamageHard;
    private float LaserStayDamageEasy;
    private float LaserStayDamageNormal;
    private float LaserStayDamageHard;
    private float GunHitDamageEasy;
    private float GunHitDamageNormal;
    private float GunHitDamageHard;
    private float FuelBenefitEasy;
    private float FuelBenefitNormal;
    private float FuelBenefitHard;

    private int LiveCrateReqEasy;
    private int SecretKeyReqEasy;
    private int LiveCrateReqNormal;
    private int SecretKeyReqNormal;
    private int FuelCrateReqHard;
    private int LiveCrateReqHard;
    private int SecretKeyReqHard;

    #endregion

    private AudioSource MenuSound;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Instance._gameSound = true;
            Instance._menuSound = true;
            Instance._gameOverSound = true;
            Instance._heroSpeed = 7f;
            Instance._heroJumpForce = 7.3f;
            Instance._heroStartHealth = 100f;
            Instance._respawnTimeSeconds = 5;
            Instance._weaponDelay = 3;
            Instance._maxBullets = 1;
            Instance._weaponDisabled = false;
            Instance._difficulty = 1;
            Instance._keyCollected = false;
            Instance._gameScore = 0;
            Instance._collectedFuels = 0;
            Instance._mainCamera = GameObject.FindWithTag("MainCamera");
            Instance.FuelCrateBenefitEasy = 40f;
            Instance.FuelCrateBenefitNormal = 20f;
            Instance.FuelCrateBenefitHard = 10f;
            Instance.PointCrateBenefitEasy = 500;
            Instance.PointCrateBenefitNormal = 250;
            Instance.PointCrateBenefitHard = 100;
            Instance.LiveCrateBenefitEasy = 4;
            Instance.LiveCrateBenefitNormal = 2;
            Instance.LiveCrateBenefitHard = 1;
            Instance.LaserHitDamageEasy = 25f;
            Instance.LaserHitDamageNormal = 40f;
            Instance.LaserHitDamageHard = 50f;
            Instance.LaserStayDamageEasy = 1f;
            Instance.LaserStayDamageNormal = 5f;
            Instance.LaserStayDamageHard = 10f;
            Instance.GunHitDamageEasy = 15f;
            Instance.GunHitDamageNormal = 25f;
            Instance.GunHitDamageHard = 45f;
            Instance.FuelBenefitEasy = 30f;
            Instance.FuelBenefitNormal = 20f;
            Instance.FuelBenefitHard = 10f;
            Instance._liveCount = 3;
            Instance._bulletSpeed = 0.8f;
            Instance._bulletPositionOne = true;
            Instance._bulletPositionTwo = true;
            Instance._bulletPositionThree = false;
            Instance._bulletPositionFour = false;
            Instance._bulletRandomPosition = true;
            Instance._v2BulletPositionOne = new Vector2(-3.99f, -0.25f);
            Instance._v2BulletPositionTwo = new Vector2(-0.49f, -0.79f);
            Instance._v2BulletPositionThree = new Vector2(-6.34f, -0.7f);
            Instance._v2BulletPositionFour = new Vector2(-6f, -1f);
            Instance.LiveCrateReqEasy = 1500;
            Instance.SecretKeyReqEasy = 5500;
            Instance.LiveCrateReqNormal = 2500;
            Instance.SecretKeyReqNormal = 7500;
            Instance.FuelCrateReqHard = 2500;
            Instance.LiveCrateReqHard = 7500;
            Instance.SecretKeyReqHard = 10000;
            Instance._displayFPS = false;
#if PC
#if DEVELOPMENT
            Instance._displayFPS = true;
#endif
#endif
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
        if (Instance._mainCamera != null && Instance._menuSound)
        {
            PlayMenuMusic();
        }
        else
        {
            StopMenuMusic();
        }
#if DEVELOPMENT
        Debug.Log($"Current value of _gameSound is: {Instance._gameSound}");
        Debug.Log($"Current value of _menuSound is: {Instance._menuSound}");
        Debug.Log($"Current value of _gameOverSound is: {Instance._gameOverSound}");
#endif
    }

    #region Crate Requirements

    public int GetFuelCrateRequirementsForDifficulty(int diff)
    {
        if (diff == 3)
            return Instance.FuelCrateReqHard;
        else
            return 0;
    }
    
    public int GetLiveCrateRequirementsForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.LiveCrateReqEasy;
        if (diff == 2)
            return Instance.LiveCrateReqNormal;
        if (diff == 3)
            return Instance.LiveCrateReqHard;
        return 0;
    }

    public int GetSecretKeyRequirementsForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.SecretKeyReqEasy;
        if (diff == 2)
            return Instance.SecretKeyReqNormal;
        if (diff == 3)
            return Instance.SecretKeyReqHard;
        return 0;
    }

    #endregion

    public void UpdateBulletPositionOneBoolValue(bool newValue)
        => Instance._bulletPositionTwo = newValue;

    public void UpdateBulletPositionTwoBoolValue(bool newValue)
        => Instance._bulletPositionTwo = newValue;

    public void UpdateBulletPositionThreeBoolValue(bool newValue)
        => Instance._bulletPositionThree = newValue;

    public void UpdateBulletPositionFourBoolValue(bool newValue)
        => Instance._bulletPositionFour = newValue;

    public void UpdateBulletRandomPositionBoolValue(bool newValue)
        => Instance._bulletRandomPosition = newValue;

    public bool GetBulletPositionOneBoolValue()
        => Instance._bulletPositionOne;

    public bool GetBulletPositionTwoBoolValue()
        => Instance._bulletPositionTwo;

    public bool GetBulletPositionThreeBoolValue()
        => Instance._bulletPositionThree;

    public bool GetBulletPositionFourBoolValue()
        => Instance._bulletPositionFour;

    public bool GetBulletRandomPositionBoolValue()
        => Instance._bulletRandomPosition;

    public Vector2 GetGunBulletPositionOne()
        => Instance._v2BulletPositionOne;

    public Vector2 GetGunBulletPositionTwo()
        => Instance._v2BulletPositionTwo;

    public Vector2 GetGunBulletPositionThree()
        => Instance._v2BulletPositionThree;

    public Vector2 GetGunBulletPositionFour()
        => Instance._v2BulletPositionFour;

    #region Damage & Benefits Getter/Setter

    public float GetFuelCrateBenefitForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.FuelCrateBenefitEasy;
        if (diff == 2)
            return Instance.FuelCrateBenefitNormal;
        if (diff == 3)
            return Instance.FuelCrateBenefitHard;
        return 0f;
    }

    public int GetPointCrateBenefitForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.PointCrateBenefitEasy;
        if (diff == 2)
            return Instance.PointCrateBenefitNormal;
        if (diff == 3)
            return Instance.PointCrateBenefitHard;
        return 0;
    }

    public int GetLiveCrateBenefitForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.LiveCrateBenefitEasy;
        if (diff == 2)
            return Instance.LiveCrateBenefitNormal;
        if (diff == 3)
            return Instance.LiveCrateBenefitHard;
        return 0;
    }

    public float GetLaserHitDamageForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.LaserHitDamageEasy;
        if (diff == 2)
            return Instance.LaserHitDamageNormal;
        if (diff == 3)
            return Instance.LaserHitDamageHard;
        return 0f;
    }

    public float GetLaserStayDamageForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.LaserStayDamageEasy;
        if (diff == 2)
            return Instance.LaserStayDamageNormal;
        if (diff == 3)
            return Instance.LaserStayDamageHard;
        return 0f;
    }

    public float GetGunHitDamageForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.GunHitDamageEasy;
        if (diff == 2)
            return Instance.GunHitDamageNormal;
        if (diff == 3)
            return Instance.GunHitDamageHard;
        return 0f;
    }

    public float GetFuelBenefitForDifficulty(int diff)
    {
        if (diff == 1)
            return Instance.FuelBenefitEasy;
        if (diff == 2)
            return Instance.FuelBenefitNormal;
        if (diff == 3)
            return Instance.FuelBenefitHard;
        return 0f;
    }
    
    //Setter
    public void UpdateBenefitForFuelCanForDifficulty(int diff, float newValue)
    {
        if (diff == 1)
            Instance.FuelBenefitEasy = newValue;
        if (diff == 2)
            Instance.FuelBenefitNormal = newValue;
        if (diff == 3)
            Instance.FuelBenefitHard = newValue;
    }

    public void UpdateBenefitForFuelCanCrateForDifficulty(int diff, float newValue)
    {
        if (diff == 1)
            Instance.FuelCrateBenefitEasy = newValue;
        if (diff == 2)
            Instance.FuelCrateBenefitNormal = newValue;
        if (diff == 3)
            Instance.FuelCrateBenefitHard = newValue;
    }

    public void UpdateBenefitForPointCrateForDifficulty(int diff, int newValue)
    {
        if (diff == 1)
            Instance.PointCrateBenefitEasy = newValue;
        if (diff == 2)
            Instance.PointCrateBenefitNormal = newValue;
        if (diff == 3)
            Instance.PointCrateBenefitHard = newValue;
    }

    public void UpdateBenefitForLiveCrateForDifficulty(int diff, int newValue)
    {
        if (diff == 1)
            Instance.LiveCrateBenefitEasy = newValue;
        if (diff == 2)
            Instance.LiveCrateBenefitNormal = newValue;
        if (diff == 3)
            Instance.LiveCrateBenefitHard = newValue;
    }

    public void UpdateDamageOfLaserHitForDifficulty(int diff, float newValue)
    {
        if (diff == 1)
            Instance.LaserHitDamageEasy = newValue;
        if (diff == 2)
            Instance.LaserHitDamageNormal = newValue;
        if (diff == 3)
            Instance.LaserHitDamageHard = newValue;
    }

    public void UpdateDamageOfLaserStayForDifficulty(int diff, float newValue)
    {
        if (diff == 1)
            Instance.LaserStayDamageEasy = newValue;
        if (diff == 2)
            Instance.LaserStayDamageNormal = newValue;
        if (diff == 3)
            Instance.LaserStayDamageHard = newValue;
    }

    public void UpdateDamageOfGunHitForDifficulty(int diff, float newValue)
    {
        if (diff == 1)
            Instance.GunHitDamageEasy = newValue;
        if (diff == 2)
            Instance.GunHitDamageNormal = newValue;
        if (diff == 3)
            Instance.GunHitDamageHard = newValue;
    }

    #endregion


    public void UpdateBulletSpeed(float newValue)
        => Instance._bulletSpeed = newValue;
    public float GetBulletSpeed()
        => Instance._bulletSpeed;
    
    public void UpdateLiveCountForDifficulty(int diff)
    {
        if (diff == 1)
            Instance._liveCount = 3;
        if (diff == 2)
            Instance._liveCount = 2;
        if (diff == 3)
            Instance._liveCount = 1;
    }

    public int GetLiveCountForDifficulty(int diff)
        => Instance._liveCount;
    
    public void UpdateFuelCount(int newScore)
        => Instance._collectedFuels = newScore;

    public int GetFuelCount()
        => Instance._collectedFuels;
    public void ResetFuelCount()
        => Instance._collectedFuels = 0;
    public int GetGameScore()
        => Instance._gameScore;
    
    public void UpdateGameScore(int newScore)
        => Instance._gameScore = newScore;
    
    public void ResetGameScore()
        => Instance._gameScore = 0;

    public bool IsSecretKeyCollected()
        => Instance._keyCollected;
    public void UpdateSecretKeyCollected(bool state)
        => Instance._keyCollected = state;
    public int GetDifficulty()
        => Instance._difficulty;

    public void UpdateDifficulty(int newDiff)
    {
        Instance._difficulty = newDiff;
        UpdateLiveCountForDifficulty(newDiff);
        UpdateBulletPositionForDifficulty(newDiff);
    }

    public void UpdateBulletPositionForDifficulty(int newDiff)
    {
        switch (newDiff)
        {
            case 1:
                Instance._bulletPositionThree = false;
                Instance._bulletPositionFour = false;
                break;
            case 2:
                Instance._bulletPositionThree = true;
                Instance._bulletPositionFour = false;
                break;
            case 3:
                Instance._bulletPositionThree = true;
                Instance._bulletPositionFour = true;
                break;
        }
        Instance._bulletPositionOne = true;
        Instance._bulletPositionTwo = true;
        Instance._bulletRandomPosition = true;
    }

    public void UpdateWeaponStatus(bool newStatus)
        => Instance._weaponDisabled = newStatus;

    public bool GetWeaponStatus()
        => Instance._weaponDisabled;

    public int GetBulletCount()
        => Instance._bulletCount;

    public void ResetBulletCount()
    {
        Instance._bulletCount = 0;
        UpdateWeaponStatus(false);
    }

    private void ResetBulletLocation(int prefab)
    {
        GunScript gunScript = FindObjectOfType<GunScript>();
        gunScript.ResetBulletCoordinates(prefab);
    }

    public void IncrementBulletCount()
    {
        Instance._bulletCount++;
        if(Instance._bulletCount >= Instance.GetMaxBullets())
            UpdateWeaponStatus(true);
    }
    
    public int GetMaxBullets()
        => Instance._maxBullets;

    public void UpdateMaxBullets(int newMax)
        => Instance._maxBullets = newMax;

    public int GetRespawnTime()
        => Instance._respawnTimeSeconds;

    public int GetWeaponDelay()
        => Instance._weaponDelay;

    public float GetHeroJumpForce()
        => Instance._heroJumpForce;

    public float UpdateHeroJumpForce(float newValue)
        => Instance._heroJumpForce = newValue;
    
    public float GetHeroStartHealth()
        => Instance._heroStartHealth;

    public void UpdateHeroStartHealth(float newValue)
        => Instance._heroStartHealth = newValue;
    
    public float GetHeroRunSpeed()
        => Instance._heroSpeed;

    public void UpdateHeroRunSpeed(float newValue)
        => Instance._heroSpeed = newValue;
    
    public void UpdateMenuSoundBoolValue(bool newValue)
        => Instance._menuSound = newValue;

    public bool GetMenuSoundBoolValue()
        => Instance._menuSound;

    public void UpdateGameSoundBoolValue(bool newValue)
        => Instance._gameSound = newValue;
    
    public bool GetGameSoundBoolValue()
        => Instance._gameSound;

    public void UpdateGameOverSoundBoolValue(bool newValue)
        => Instance._gameOverSound = newValue;

    public bool GetGameOverSoundBoolValue()
        => Instance._gameOverSound;

    public void UpdateMainCamera(GameObject camera)
        => Instance._mainCamera = camera;
    
    public GameObject GetMainCamera()
        => Instance._mainCamera;

    public bool GetDisplayFpsBoolValue()
        => Instance._displayFPS;

    public void UpdateDisplayFpsBoolValue(bool newValue)
        => Instance._displayFPS = newValue;
    
    public void PlayMenuMusic()
    {
        Instance.MenuSound = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        Instance.MenuSound.loop = true;
        Instance.MenuSound.playOnAwake = true;
        Instance.MenuSound.Play();
    }

    public void StopMenuMusic()
    {
        if(Instance.MenuSound != null)
            Instance.MenuSound.Stop();
    }

    
    /// <summary>
    /// 0 - Title Scene
    /// 1 - Option Scene
    /// 2 - Game Scene
    /// 3 - End Scene
    /// </summary>
    
    public void LoadHomeScreen()
    {
#if PC
        SceneManager.LoadScene(0);
#endif
#if MOBILE
        SceneManager.LoadScene(0);
#endif
#if DEVELOPMENT
        Debug.Log("Load Home Scene");
#endif
    }

    public void LoadOptionsScreen()
    {
#if PC
        SceneManager.LoadScene(1);
#endif
#if MOBILE
        SceneManager.LoadScene(1);
#endif
#if DEVELOPMENT
        Debug.Log("Load Option Scene");
#endif
    }

    public void LoadDevScreen()
    {
#if PC
#if DEVELOPMENT
        SceneManager.LoadScene(2);
#endif
#endif
    }

    public void LoadHelpScreen()
    {
#if PC
        SceneManager.LoadScene(3);
#endif
#if MOBILE
        SceneManager.LoadScene(2);
#endif
    }
    
    public void LoadGameScreen()
    {
#if PC
        SceneManager.LoadScene(4);
#endif
#if MOBILE
        SceneManager.LoadScene(3);
#endif
        ResetGameScore();
        ResetFuelCount();
#if DEVELOPMENT
        Debug.Log("Load Game Scene");
#endif
    }

    public void LoadGameOverScreen()
    {
#if PC
        SceneManager.LoadScene(5);
#endif
#if MOBILE
        SceneManager.LoadScene(4);
#endif
#if DEVELOPMENT
        Debug.Log("Load Game Over Scene");
#endif
    }

    public void LoadSecretLevelScreen()
    {
        if (Instance._keyCollected)
        {
#if PC
            SceneManager.LoadScene(6);
#endif
#if MOBILE
            SceneManager.LoadScene(5);
#endif
        }
    }

    public void LoadSourceCodeHyperlink()
    {
        Application.OpenURL("https://github.com/michaelfrankl/FWPM-GD-HER");
    }
}
