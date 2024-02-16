using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class RandomizerScript : MonoBehaviour
{
    public GameObject _prefab1;

    public GameObject _prefab2;

    public GameObject _prefab3;

    private Random rnd;

    private void Awake()
    {
        rnd = new Random();
    }

    /// <summary>
    /// Method to randomize the start prefab with requirements
    /// Sensor1 must be disabled
    /// Weapon or laser (not both) (higher difficulty but not at the beginning)
    /// </summary>
    public void RandomizePrefab(int prefab)
    {
        ObservableCollection<GameObject> _gameObjects = GetAllChildElementsOfPrefab(prefab);
        RandomizeBackground(prefab, _gameObjects);
        RandomizeCollectibles(prefab, _gameObjects);
        RandomizeCrates(prefab);
        RandomizeObstacles(prefab, _gameObjects);
    }

    
    /// <summary>
    /// Method to randomize background only
    /// include (all tagged with "Random")
    /// - Enviro2_Cover
    /// - Obj_ScreenOff
    /// - Obj_LightsOut
    /// </summary>
    /// <param name="prefab"></param>
    private void RandomizeBackground(int prefab, ObservableCollection<GameObject> _objects)
    {
        List<GameObject> _bkparts = new List<GameObject>();
        if(prefab == 1)
            _bkparts = _objects.Where(i => i.gameObject.tag == "Random").ToList();
        if(prefab == 2)
            _bkparts = _objects.Where(i => i.gameObject.tag == "Random2").ToList();
        if(prefab == 3)
            _bkparts = _objects.Where(i => i.gameObject.tag == "Random3").ToList();
        
        foreach (GameObject value in _bkparts)
        {
            int random = rnd.Next(1, 100);
            if (random <= 49)
                value.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            else
                value.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Method to randomize collectibles only
    /// include
    /// - FuelCan
    /// - Scret Key (but not added yet)
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="_objects"></param>
    private void RandomizeCollectibles(int prefab, ObservableCollection<GameObject> _objects)
    {
        GameObject _fuelCan = null;
        if (prefab == 1)
            _fuelCan = _objects.Where(i => i.gameObject.tag == "Prefab1_FuelCan").First();
        if (prefab == 2)
            _fuelCan = _objects.Where(i => i.gameObject.tag == "Prefab2_FuelCan").First();
        if (prefab == 3)
            _fuelCan = _objects.Where(i => i.gameObject.tag == "Prefab3_FuelCan").First();

        int random = rnd.Next(1, 100);
#if DEVELOPMENT
        Debug.Log($"Rnd (FuelCan) with = {random}");
#endif
        if (random <= 49)
        {
            _fuelCan.SetActive(false);
            //Randomize position
            int rndPosition = rnd.Next(1, 60);
            if(rndPosition is >= 1 and <= 10)
                _fuelCan.transform.localPosition = new Vector3(-10.93f, -2.23f, 1.206474f);
            if(rndPosition is > 10 and <= 20)
                _fuelCan.transform.localPosition = new Vector3(-10.93f, -0.45f, 1.206474f);
            if(rndPosition is > 20 and <= 30)
                _fuelCan.transform.localPosition = new Vector3(-3.2f, -2.23f, 1.206474f);
            if(rndPosition is > 30 and <= 40)
                _fuelCan.transform.localPosition = new Vector3(-3.2f, -0.45f, 1.206474f);
            if(rndPosition is > 40 and <= 50)
                _fuelCan.transform.localPosition = new Vector3(8.7f, -2.23f, 1.206474f);
            if(rndPosition is > 50 and <= 60)
                _fuelCan.transform.localPosition = new Vector3(8.7f, -0.45f, 1.206474f);
        }
        else
            _fuelCan.SetActive(true);

    }
    
    /// <summary>
    /// Method to disable all crates
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void DisableAllCrates()
    {
        List<GameObject> _crateList1 = GetAllCratesOfPrefab(1);
        List<GameObject> _crateList2 = GetAllCratesOfPrefab(2);
        List<GameObject> _crateList3 = GetAllCratesOfPrefab(3);
        
        foreach (GameObject item in _crateList1)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in _crateList2)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in _crateList3)
        {
            item.SetActive(false);
        }
        
        
#if DEBUG
        Debug.Log($"All crates are disabled");
#endif
    }

    
    /// <summary>
    /// Method to disable all crate for a give prefab
    /// </summary>
    /// <param name="prefab"></param>
    public void DisableAllCratesForSpecificPrefab(int prefab)
    {
        List<GameObject> _crates = GetAllCratesOfPrefab(prefab);
        foreach (GameObject crate in _crates)
        {
            crate.SetActive(false);
        }
    }
    
    /// <summary>
    /// Method to randomize crates
    /// </summary>
    /// <param name="prefab"></param>

    private void RandomizeCrates(int prefab)
    {
        List<GameObject> _crateList = GetAllCratesOfPrefab(prefab);
        GameObject _fuelCrate = null;
        GameObject _liveCrate = null;
        GameObject _pointCrate = null;
        GameObject _secretKeyCrate = null;

        if (prefab == 1)
        {
            _fuelCrate = _crateList.Where(i => i.gameObject.tag == "CrateFuel1").First();
            _liveCrate = _crateList.Where(i => i.gameObject.tag == "CrateLive1").First();
            _pointCrate = _crateList.Where(i => i.gameObject.tag == "CratePoint1").First();
            _secretKeyCrate = _crateList.Where(i => i.gameObject.tag == "CrateSecretKey1").First();
        }
        if (prefab == 2)
        {
            _fuelCrate = _crateList.Where(i => i.gameObject.tag == "CrateFuel2").First();
            _liveCrate = _crateList.Where(i => i.gameObject.tag == "CrateLive2").First();
            _pointCrate = _crateList.Where(i => i.gameObject.tag == "CratePoint2").First();
            _secretKeyCrate = _crateList.Where(i => i.gameObject.tag == "CrateSecretKey2").First();
        }
        if (prefab == 3)
        {
            _fuelCrate = _crateList.Where(i => i.gameObject.tag == "CrateFuel3").First();
            _liveCrate = _crateList.Where(i => i.gameObject.tag == "CrateLive3").First();
            _pointCrate = _crateList.Where(i => i.gameObject.tag == "CratePoint3").First();
            _secretKeyCrate = _crateList.Where(i => i.gameObject.tag == "CrateSecretKey3").First();
        }

        Vector3 positionOne = new Vector3(-5.11343527f, -2.59000015f, 1.2064743f);
        Vector3 positionTwo = new Vector3(5.80999994f, -2.55999994f, 1.2064743f);
        Vector3 positionThree = new Vector3(1.87f, -2.55999994f, 1.2064743f);
        Vector3 positionFour = new Vector3(7.94000006f, -2.61000013f, 1.2064743f);
        
        int difficulty = GameManager.Instance.GetDifficulty();
        GameObject _winner = null;
        switch (difficulty)
        {
            case 1:
                _winner = SpawnCrateForDifficulty(difficulty, _fuelCrate, _liveCrate, _pointCrate, _secretKeyCrate);
                break;
            case 2:
                _winner = SpawnCrateForDifficulty(difficulty, _fuelCrate, _liveCrate, _pointCrate, _secretKeyCrate);
                break;
            case 3:
                _winner = SpawnCrateForDifficulty(difficulty, _fuelCrate, _liveCrate, _pointCrate, _secretKeyCrate);
                if (_winner == null)
                    return;
                break;
        }

        if (GameManager.Instance.IsSecretKeyCollected() && _winner.gameObject.tag == "CrateSecretKey1" ||
            GameManager.Instance.IsSecretKeyCollected() && _winner.gameObject.tag == "CrateSecretKey2" ||
            GameManager.Instance.IsSecretKeyCollected() && _winner.gameObject.tag == "CrateSecretKey3")
            return;

        int randomPosition = rnd.Next(1, 4);
        switch (randomPosition)
        {
            case 1:
                _winner.transform.localPosition = positionOne;
                break;
            case 2:
                _winner.transform.localPosition = positionTwo;
                break;
            case 3:
                _winner.transform.localPosition = positionThree;
                break;
            case 4:
                _winner.transform.localPosition = positionFour;
                break;
        }
        _winner.SetActive(true);
    }
    

    /// <summary>
    /// Method for spawning a crate with difficulty settings
    /// </summary>
    /// <param name="difficulty"></param>
    /// <param name="_fuelCrate"></param>
    /// <param name="_liveCrate"></param>
    /// <param name="_pointCrate"></param>
    /// <param name="_secretKeyCrate"></param>
    private GameObject SpawnCrateForDifficulty(int difficulty, GameObject _fuelCrate, GameObject _liveCrate, GameObject _pointCrate, GameObject _secretKeyCrate)
    {
        GameObject _winner = null;
        bool _fuelCrateUnlocked = false;
        bool _liveCrateUnlocked = false;
        bool _pointCrateUnlocked = false;
        bool _secretKeyCrateUnlocked = false;
        //only (one) crate at the same time
        //----------------------------------------------------------------------------
        //if difficulty is on easy (1), then _fuelCrate & _pointCrate spawn more often (50/50)
        //--> _liveCrate needs 500 Points to unlock (40/60)
        //--> _secretKeyCrate needs 1500 Points to unlock (35/65)
        //----------------------------------------------------------------------------
        //if difficulty is on normal (2), then _fuelCrate & _pointCrate spawn more often (60/40)
        //--> _liveCrate needs 1500 Points to unlock (30/70)
        //--> _secretKeyCrate needs 2000 Points to unlock (25/75)
        //----------------------------------------------------------------------------
        //if difficulty is on hard (3), then _pointCrate spawn sometimes (40/60)
        //--> _fuelCrate needs 1500 Points to unlock (25/75)
        //--> _liveCrate needs 2500 Points to unlock (10/90)
        //--> _secretKeyCrate needs 5500 Points to unlock (5/95)
        //----------------------------------------------------------------------------
    
        switch (difficulty)
        {
            case 1:
                _fuelCrateUnlocked = true;
                _pointCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetLiveCrateRequirementsForDifficulty(difficulty))
                    _liveCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetSecretKeyRequirementsForDifficulty(difficulty))
                    _secretKeyCrateUnlocked = true;
                break;
            case 2:
                _fuelCrateUnlocked = true;
                _pointCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetLiveCrateRequirementsForDifficulty(difficulty))
                    _liveCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetSecretKeyRequirementsForDifficulty(difficulty))
                    _secretKeyCrateUnlocked = true;
                break;
            case 3:
                _pointCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetFuelCrateRequirementsForDifficulty(difficulty))
                    _fuelCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetLiveCrateRequirementsForDifficulty(difficulty))
                    _liveCrateUnlocked = true;
                if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetSecretKeyRequirementsForDifficulty(difficulty))
                    _secretKeyCrateUnlocked = true;
                break;
        }

        if (difficulty == 1)
        {
            if (_fuelCrateUnlocked && _pointCrateUnlocked && !_liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.5, _pointCrate, 0.5);
            }
            else if (_fuelCrateUnlocked && _pointCrateUnlocked && _liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.5, _pointCrate, 0.5, _liveCrate, 0.4);
            }

            else if (_fuelCrateUnlocked && _pointCrateUnlocked && _liveCrateUnlocked && _secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.5, _pointCrate, 0.5, _liveCrate, 0.4,
                    _secretKeyCrate, 0.35);
            }
        }

        else if (difficulty == 2)
        {
            if (_fuelCrateUnlocked && _pointCrateUnlocked && !_liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.6, _pointCrate, 0.4);
            }

            else if (_fuelCrateUnlocked && _pointCrateUnlocked && _liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.6, _pointCrate, 0.4, _liveCrate, 0.3);
            }

            else if (_fuelCrateUnlocked && _pointCrateUnlocked && _liveCrateUnlocked && _secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_fuelCrate, 0.6, _pointCrate, 0.4, _liveCrate, 0.3,
                    _secretKeyCrate, 0.25);
            }
        }

        else if (difficulty == 3)
        {
            if (_pointCrateUnlocked && !_fuelCrateUnlocked && !_liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_pointCrate, 0.4, null, 0.6);
            }

            else if (_pointCrateUnlocked && _fuelCrateUnlocked && !_liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_pointCrate, 0.4, _fuelCrate, 0.25);
            }
            else if (_pointCrateUnlocked && _fuelCrateUnlocked && _liveCrateUnlocked && !_secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_pointCrate, 0.4, _fuelCrate, 0.25, _liveCrate, 0.10);
            }
            else if (_pointCrateUnlocked && _fuelCrateUnlocked && _liveCrateUnlocked && _secretKeyCrateUnlocked)
            {
                _winner = CalculateSpawnItem(_pointCrate, 0.4, _fuelCrate, 0.25,
                    _liveCrate, 0.10, _secretKeyCrate, 0.05);
            }
        }
        return _winner;
    }
    
    /// <summary>
    /// Method to calculate the spawn rate for two items
    /// </summary>
    /// <param name="_itemOne"></param>
    /// <param name="_itemValueOne"></param>
    /// <param name="_itemTwo"></param>
    /// <param name="_itemValueTwo"></param>
    /// <returns></returns>
    private GameObject CalculateSpawnItem(GameObject _itemOne, double _itemValueOne, GameObject _itemTwo, 
        double _itemValueTwo)
    {
        GameObject _winner = null;
        double _totalRate = _itemValueOne + _itemValueTwo;
        _itemValueOne /= _totalRate;
        _itemValueTwo /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemValueOne)
            return _itemOne;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo))
            return _itemTwo;
        return _winner;
    }

    /// <summary>
    /// Method to calculate the spawn rate for three items
    /// </summary>
    /// <param name="_itemOne"></param>
    /// <param name="_itemValueOne"></param>
    /// <param name="_itemTwo"></param>
    /// <param name="_itemValueTwo"></param>
    /// <param name="_itemThree"></param>
    /// <param name="_itemValueThree"></param>
    /// <returns></returns>
    private GameObject CalculateSpawnItem(GameObject _itemOne, double _itemValueOne, GameObject _itemTwo, 
        double _itemValueTwo, GameObject _itemThree, double _itemValueThree)
    {
        GameObject _winner = null;
        double _totalRate = _itemValueOne + _itemValueTwo + _itemValueThree;
        _itemValueOne /= _totalRate;
        _itemValueTwo /= _totalRate;
        _itemValueThree /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemValueOne)
            return _itemOne;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo))
            return _itemTwo;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo + _itemValueThree))
            return _itemThree;
        return _winner;
    }

    
    /// <summary>
    /// Method to calculate the spawn rate for four items
    /// </summary>
    /// <param name="_itemOne"></param>
    /// <param name="_itemValueOne"></param>
    /// <param name="_itemTwo"></param>
    /// <param name="_itemValueTwo"></param>
    /// <param name="_itemThree"></param>
    /// <param name="_itemValueThree"></param>
    /// <param name="_itemFour"></param>
    /// <param name="_itemValueFour"></param>
    /// <returns></returns>
    private GameObject CalculateSpawnItem(GameObject _itemOne, double _itemValueOne, GameObject _itemTwo, 
        double _itemValueTwo, GameObject _itemThree, double _itemValueThree, GameObject _itemFour, double _itemValueFour)
    {
        GameObject _winner = null;
        double _totalRate = _itemValueOne + _itemValueTwo + _itemValueThree + _itemValueFour;
        _itemValueOne /= _totalRate;
        _itemValueTwo /= _totalRate;
        _itemValueThree /= _totalRate;
        _itemValueFour /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemValueOne)
            return _itemOne;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo))
            return _itemTwo;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo + _itemValueThree))
            return _itemThree;
        else if (_spawnRate < (_itemValueOne + _itemValueTwo + _itemValueThree + _itemValueFour))
            return _itemFour;
        return _winner;
    }
    

    /// <summary>
    /// Method to randomize obstacles (Gun & Laser)
    /// constraints
    /// - only one active at a prefab (Laser or Gun 50/50 chance like everything else)
    /// - if the gun is activated the light plate must be activated
    /// </summary>
    /// <param name="i"></param>
    /// <param name="gameObjects"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void RandomizeObstacles(int prefab, ObservableCollection<GameObject> _objects)
    {
        List<GameObject> _laserParts = GetAllSpecificLaserParts(prefab, _objects);
        List<GameObject> _gunParts = GetAllSpecificGunParts(prefab, _objects);

        if (GameManager.Instance.IsSecretKeyCollected())
        {
            //fix laser despawn
            DisabledAllGivenObjects(_laserParts);
            //still need to randomize the weapon
            int rndWeapon = rnd.Next(1, 20);
            if (rndWeapon is >= 1 and <= 10)
                DisableAllSpecificGunParts(prefab, _gunParts);
            else
                ActivateAllSpecificGunParts(prefab, _gunParts);
            return;
        }
        int random = rnd.Next(1, 100);
#if DEVELOPMENT
        Debug.Log($"Rnd (Obstacles) with = {random}");
#endif
        //Laser is the winner
        if (random <= 49)
        {
            DisableAllSpecificGunParts(prefab, _gunParts);
            //TODO: Difficulty change --> seperated Laser position
            GameObject _laserUp = _laserParts.Where(i => i.gameObject.name == "LaserUp").First();
            GameObject _laserDown = _laserParts.Where(i => i.gameObject.name == "LaserDown").First();
            int rndPosition = rnd.Next(1, 40);
            if (rndPosition is > 1 and <= 14)
            {
                _laserUp.transform.localPosition = new Vector3(-7.87f, -2.44f, 1.206474f);
                _laserDown.transform.localPosition = new Vector3(_laserUp.transform.localPosition.x,
                    _laserUp.transform.localPosition.y - 0.64f, _laserUp.transform.localPosition.z);
            }
            else if (rndPosition is > 14 and <= 27)
            {
                _laserUp.transform.localPosition = new Vector3(-0.6234355f, -2.44f, 1.206474f);
                _laserDown.transform.localPosition = new Vector3(_laserUp.transform.localPosition.x,
                    _laserUp.transform.localPosition.y - 0.64f, _laserUp.transform.localPosition.z);
            }
            else if (rndPosition is > 27 and <= 40)
            {
                _laserUp.transform.localPosition = new Vector3(5.33f, -2.44f, 1.206474f);
                _laserDown.transform.localPosition = new Vector3(_laserUp.transform.localPosition.x,
                    _laserUp.transform.localPosition.y - 0.64f, _laserUp.transform.localPosition.z);
            }
            _laserUp.SetActive(true);
            _laserDown.SetActive(true);
        }
        //Gun is the winner
        else
        {
            DisabledAllGivenObjects(_laserParts);
            ActivateAllSpecificGunParts(prefab, _gunParts);
        }
    }

    /// <summary>
    /// Method to activate all related gun objects
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="_parts"></param>
    private void ActivateAllSpecificGunParts(int prefab, List<GameObject> _parts)
    {
        GameObject _gunCharge = null;
        GameObject _gunFire = null;
        GameObject _gunMount = null;
        GameObject _gunLight = null;
        GameObject _gunPlate = null;
        GameObject _gunSensor = null;
        if (prefab == 1)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor1").First();
        }
        if (prefab == 2)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor2").First();
        }
        if (prefab == 3)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor3").First();
        }
        
        _gunCharge.SetActive(true);
        _gunCharge.GetComponent<Animator>().enabled = true;
        _gunFire.SetActive(false);
        _gunFire.GetComponent<Animator>().enabled = true;
        _gunMount.SetActive(true);
        _gunLight.SetActive(true);
        _gunLight.GetComponent<Animator>().enabled = true;
        _gunPlate.SetActive(true);
        _gunPlate.GetComponent<SpriteRenderer>().enabled = false;
        _gunSensor.SetActive(true);
    }

    /// <summary>
    /// Method to deactivate all related gun objects
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="_parts"></param>
    private void DisableAllSpecificGunParts(int prefab, List<GameObject> _parts)
    {
         GameObject _gunCharge = null;
        GameObject _gunFire = null;
        GameObject _gunMount = null;
        GameObject _gunLight = null;
        GameObject _gunPlate = null;
        GameObject _gunSensor = null;
        if (prefab == 1)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab1_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor1").First();
        }
        if (prefab == 2)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab2_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor2").First();
        }
        if (prefab == 3)
        {
            _gunCharge = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First();
            _gunFire = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First();
            _gunMount = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponMount").First();
            _gunLight = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponLight").First();
            _gunPlate = _parts.Where(i => i.gameObject.tag == "Prefab3_WeaponPlate").First();
            _gunSensor = _parts.Where(i => i.gameObject.tag == "GunSensor3").First();
        }
        
        _gunCharge.SetActive(true);
        _gunCharge.GetComponent<Animator>().enabled = false;
        _gunFire.SetActive(false);
        _gunMount.SetActive(true);
        _gunLight.SetActive(true);
        _gunLight.GetComponent<Animator>().enabled = false;
        _gunPlate.SetActive(true);
        _gunPlate.GetComponent<SpriteRenderer>().enabled = true;
        _gunSensor.SetActive(false);
    }
    
    /// <summary>
    /// Method to return a list with the searched items
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private List<GameObject> GetAllSpecificLaserParts(int prefab, ObservableCollection<GameObject> _parts)
    {
        List<GameObject> _laserParts = new List<GameObject>();
        if (prefab == 1)
            _laserParts = _parts.Where(i => i.gameObject.tag == "Prefab1_Laser").ToList();
        if (prefab == 2)
            _laserParts = _parts.Where(i => i.gameObject.tag == "Prefab2_Laser").ToList();
        if (prefab == 3)
            _laserParts = _parts.Where(i => i.gameObject.tag == "Prefab3_Laser").ToList();

        return _laserParts;
    }
    
    /// <summary>
    /// Method to return a list with the searched items
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private List<GameObject> GetAllSpecificGunParts(int prefab, ObservableCollection<GameObject> _parts)
    {
        List<GameObject> _gunParts = new List<GameObject>();
        if (prefab == 1)
        {
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab1_WeaponMount").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab1_WeaponLight").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab1_WeaponPlate").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "GunSensor1").First());
        }
        if (prefab == 2)
        {
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab2_WeaponMount").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab2_WeaponLight").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab2_WeaponPlate").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "GunSensor2").First());
        }
        if (prefab == 3)
        {
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab3_WeaponMount").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab3_WeaponLight").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "Prefab3_WeaponPlate").First());
            _gunParts.Add(_parts.Where(i => i.gameObject.tag == "GunSensor3").First());
        }

        return _gunParts;
    }

    /// <summary>
    /// Method to disable every list item
    /// </summary>
    /// <param name="_objects"></param>
    private void DisabledAllGivenObjects(List<GameObject> _objects)
    {
        foreach (GameObject value in _objects)
        {
            value.SetActive(false);
        }
    }

    /// <summary>
    /// Method to disable the laser on start
    /// </summary>
    public void DisableLaserOnStartup()
    {
        DisabledAllGivenObjects(GetAllSpecificLaserParts(1, GetAllChildElementsOfPrefab(1)));
        DisabledAllGivenObjects(GetAllSpecificLaserParts(2, GetAllChildElementsOfPrefab(2)));
        DisabledAllGivenObjects(GetAllSpecificLaserParts(3, GetAllChildElementsOfPrefab(3)));
    }
    
    /// <summary>
    /// Method to get all child elements of a parent prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ObservableCollection<GameObject> GetAllChildElementsOfPrefab(int prefab)
    {
        ObservableCollection<GameObject> childs = new ObservableCollection<GameObject>();

        GameObject child = null;
        if (prefab == 1)
            child = _prefab1;
        else if (prefab == 2)
            child = _prefab2;
        else if (prefab == 3)
            child = _prefab3;

        foreach (Transform var in child.transform)
        {
            childs.Add(var.gameObject);
        }
        return childs;
    }

    /// <summary>
    /// Method to get all crate objects of a prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private List<GameObject> GetAllCratesOfPrefab(int prefab)
    {
        List<GameObject> _crates = new List<GameObject>();
        GameObject cratePrefab = null;
        if (prefab == 1)
            cratePrefab = _prefab1;
        if (prefab == 2)
            cratePrefab = _prefab2;
        if (prefab == 3)
            cratePrefab = _prefab3;

        foreach (Transform child in cratePrefab.transform)
        {
            if (prefab == 1)
            {
                if(child.gameObject.tag == "CrateFuel1")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateLive1")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CratePoint1")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateSecretKey1")
                    _crates.Add(child.gameObject);
            }

            if (prefab == 2)
            {
                if(child.gameObject.tag == "CrateFuel2")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateLive2")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CratePoint2")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateSecretKey2")
                    _crates.Add(child.gameObject);
            }

            if (prefab == 3)
            {
                if(child.gameObject.tag == "CrateFuel3")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateLive3")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CratePoint3")
                    _crates.Add(child.gameObject);
                if(child.gameObject.tag == "CrateSecretKey3")
                    _crates.Add(child.gameObject);
            }
        }
        return _crates;
    }
}
