using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GunScript : MonoBehaviour
{
    public HeroScript _heroScript;
    private Vector3 _heroPosition;
    public GameObject _mainCameraFollower;
    public GameObject _mainCamera;

    private int _currentPrefab;
    private static int _bulletCount;
    private Random rnd;
    
    private bool _weaponCharging;
    private bool _weaponShooting;

    private bool _prefabOneActive;
    private bool _prefabOneLightActive;
    private bool _prefabOneWeaponActive;
    private bool _prefabTwoActive;
    private bool _prefabTwoLightActive;
    private bool _prefabTwoWeaponActive;
    private bool _prefabThreeActive;
    private bool _prefabThreeLightActive;
    private bool _prefabThreeWeaponActive;

    private DateTime _shootFired;

    private DateTime _hitTime;
    private DateTime _reloadTime;
    
    // Prefab1_GunBullet
    public List<GameObject> _bulletList;
    // Prefab1_WeaponCharge
    public List<GameObject> _weaponChargeList;
    // Prefab1_WeaponFire
    public List<GameObject> _weaponFireList;
    // Prefab1_WeaponMount
    public List<GameObject> _weaponMountList;
    // Prefab1_WeaponLight
    public List<GameObject> _weaponLightList;
    // Prefab1_WeaponPlate
    public List<GameObject> _weaponPlateList;
    // GunSensor
    public List<GameObject> _weaponSensorList;

    private Vector2 _targetPosition = new Vector2(0f, 5.884332f);
    private RaycastHit hit;
    // Start is called before the first frame update

    private void Awake()
    {
        rnd = new Random();
        _bulletCount = 0;
        _currentPrefab = 1;
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _reloadTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        SerachForObjects();

        if (DateTime.Now > _reloadTime && _weaponShooting)
        {
            _weaponShooting = false;
            DisableWeaponAnimation(_currentPrefab);
            ChargeWeapon(_currentPrefab);
        }
    }

    /// <summary>
    /// Method for finding every objects which is in the view of the camera
    /// </summary>
    private void SerachForObjects()
    {
        //Camera catch every object
        Vector2 camPos = Camera.main.transform.position;
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(camPos, size, 0);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "Prefab1")
                _prefabOneActive = true;
            if (hit.gameObject.tag == "Prefab1_WeaponLight" && hit.gameObject.GetComponent<Animator>().enabled)
                _prefabOneLightActive = true;
            if (hit.gameObject.tag == "GunSensor1")
                _prefabOneWeaponActive = true;
            if (hit.gameObject.tag == "Prefab2")
                _prefabTwoActive = true;
            if (hit.gameObject.tag == "Prefab2_WeaponLight" && hit.gameObject.GetComponent<Animator>().enabled)
                _prefabTwoLightActive = true;
            if (hit.gameObject.tag == "GunSensor2")
                _prefabTwoWeaponActive = true;
            if (hit.gameObject.tag == "Prefab3")
                _prefabThreeActive = true;
            if (hit.gameObject.tag == "Prefab3_WeaponLight" && hit.gameObject.GetComponent<Animator>().enabled)
                _prefabThreeLightActive = true;
            if (hit.gameObject.tag == "GunSensor3")
                _prefabThreeWeaponActive = true;
        }
        
        if (_weaponShooting && GameManager.Instance.GetBulletCount() >= GameManager.Instance.GetMaxBullets())
            return;
        
        if (_prefabOneActive && _prefabOneLightActive && _prefabOneWeaponActive)
        {
            _currentPrefab = 1;
            PlayerSpotted(_currentPrefab);
            _prefabOneActive = false;
            _prefabOneLightActive = false;
            _prefabOneWeaponActive = false;
#if DEVELOPMENT
            Debug.Log($"Player spotted at Prefab{_currentPrefab} || BulletCount: {GameManager.Instance.GetBulletCount()}");
#endif
        }
        if (_prefabTwoActive && _prefabTwoLightActive && _prefabTwoWeaponActive)
        {
            _currentPrefab = 2;
            PlayerSpotted(_currentPrefab);
            _prefabTwoActive = false;
            _prefabTwoLightActive = false;
            _prefabTwoWeaponActive = false;
#if DEVELOPMENT
            Debug.Log($"Player spotted at Prefab{_currentPrefab} || BulletCount: {GameManager.Instance.GetBulletCount()}");
#endif
        }
        if (_prefabThreeActive && _prefabThreeLightActive && _prefabThreeWeaponActive)
        {
            _currentPrefab = 3;
            PlayerSpotted(_currentPrefab);
            _prefabThreeActive = false;
            _prefabThreeLightActive = false;
            _prefabThreeWeaponActive = false;
#if DEVELOPMENT
            Debug.Log($"Player spotted at Prefab{_currentPrefab} || BulletCount: {GameManager.Instance.GetBulletCount()}");
#endif
        }
    }

    /// <summary>
    /// Method for trigger the gun and the bullet if the player enter the view
    /// </summary>
    /// <param name="prefab"></param>
    private void PlayerSpotted(int prefab)
    {
        DisableWeaponSensorForSelectedPrefab(prefab);
        FireWeaponAnimation(prefab);
        FireBullet(prefab);
        _reloadTime = DateTime.Now.AddSeconds(1);
        _weaponShooting = true;
    }

    /// <summary>
    /// Method to fire the bullet to a specific location
    /// </summary>
    private void FireBullet(int prefab)
    {
        GameManager.Instance.IncrementBulletCount();
        GameObject bullet = GetBulletForSelectedPrefab(prefab);
        bullet.SetActive(true);
        _heroScript.PlayLaserShotSound();
        Rigidbody2D _bulletRB = bullet.GetComponent<Rigidbody2D>();
        Vector2 bulletForce = RandomizeBulletPosition();
        _bulletRB.AddForce(bulletForce, ForceMode2D.Impulse);
        //_reloadTime = DateTime.Now.AddSeconds(1);
#if DEVELOPMENT
        Debug.Log($"Reload timer at :  {DateTime.Now}");
#endif
    }

    
    private Vector2 RandomizeBulletPosition()
    {
        Vector2 _rndPosition;
        if (GameManager.Instance.GetBulletPositionOneBoolValue() && !GameManager.Instance.GetBulletPositionTwoBoolValue()
                                                                 && !GameManager.Instance.GetBulletPositionThreeBoolValue() 
                                                                 && !GameManager.Instance.GetBulletPositionFourBoolValue()
           )
        {
            return CalculateBulletRandomPosition(GameManager.Instance.GetGunBulletPositionOne() * (GameManager.Instance.GetBulletSpeed()));
        }

        if (GameManager.Instance.GetBulletPositionOneBoolValue() && GameManager.Instance.GetBulletPositionTwoBoolValue()
                                                                 && !GameManager.Instance.GetBulletPositionThreeBoolValue() 
                                                                 && !GameManager.Instance.GetBulletPositionFourBoolValue())
        {
            return CalculateBulletRandomPosition(CalculateBulletPositionChance(0.5, 0.5));
        }

        if (GameManager.Instance.GetBulletPositionOneBoolValue() && GameManager.Instance.GetBulletPositionTwoBoolValue()
                                                                 && GameManager.Instance.GetBulletPositionThreeBoolValue() 
                                                                 && !GameManager.Instance.GetBulletPositionFourBoolValue())
        {
            return CalculateBulletRandomPosition(CalculateBulletPositionChance(0.5, 0.25, 0.25));
        }

        if (GameManager.Instance.GetBulletPositionOneBoolValue() && GameManager.Instance.GetBulletPositionTwoBoolValue()
                                                                 && GameManager.Instance.GetBulletPositionThreeBoolValue() 
                                                                 && GameManager.Instance.GetBulletPositionFourBoolValue())
        {
            return CalculateBulletRandomPosition(CalculateBulletPositionChance(0.5, 0.2, 0.2, 0.1));
        }

#if DEVELOPMENT
        Debug.Log($"Error at RandomizeBulletPosition() --> GunScript return default Position");
#endif
        return _rndPosition = new Vector2(-2.99f, -0.55f) * (GameManager.Instance.GetBulletSpeed());
    }

    /// <summary>
    /// Method to randomize again the bullet position
    /// to make it not predictable for the player
    /// fix position/random position
    /// 1 = Easy = 60/40
    /// 2 = Normal = 50/50
    /// 3 = Hard = 40/60
    /// </summary>
    /// <param name="currentWinner"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletRandomPosition(Vector2 currentWinner)
    {
        return CalculateBulletRandomPositionForDifficulty(GameManager.Instance.GetDifficulty(), currentWinner);
    }

    /// <summary>
    /// Method to randomize the bullet position winner
    /// with difficulty settings
    ///  1 = Easy = 60/40
    ///  2 = Normal = 50/50
    ///  3 = Hard = 40/60
    /// </summary>
    /// <param name="diff"></param>
    /// <param name="currentWinner"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletRandomPositionForDifficulty(int diff, Vector2 currentWinner)
    {
        //fixed range from -0.49f to -6f (X)
        float minX = -0.49f;
        float maxX = -6f;
        //fixed range from -0.25f to -1f (Y)
        float minY = -0.25f;
        float maxY = -1f;
        float rndmFloatX = (float)(rnd.NextDouble() * (maxX - minX) + minX);
        float rndmFloatY = (float)(rnd.NextDouble() * (maxY - minY) + minY);

        double randm = rnd.NextDouble();

        switch (diff)
        {
            case 1:
                if (randm is >= 0.0 and <= 0.60)
                    return currentWinner;
                break;
            case 2:
                if (randm is >= 0.0 and <= 0.5)
                    return currentWinner;
                break;
            case 3:
                if (randm is >= 0.0 and <= 0.4)
                    return currentWinner;
                break;
        }
#if DEVELOPMENT
        Debug.Log($"Random Bullet Position Target: X=({rndmFloatX}) | Y=({rndmFloatY})");
#endif
        
        return new Vector2(rndmFloatX, rndmFloatY) * GameManager.Instance.GetBulletSpeed();
    }
    
    /// <summary>
    /// Method to calculate the bullet position with given probabilities
    /// for two items
    /// </summary>
    /// <param name="_chanceOne"></param>
    /// <param name="_chanceTwo"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletPositionChance(double _chanceOne, double _chanceTwo)
    {
        Vector2 _winner;
        double _itemOne = _chanceOne;
        double _itemTwo = _chanceTwo;
        double _totalRate = _chanceOne + _chanceTwo;
        _itemOne /= _totalRate;
        _itemTwo /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemOne)
            return (GameManager.Instance.GetGunBulletPositionOne() * (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo))
            return (GameManager.Instance.GetGunBulletPositionTwo() *
                   (GameManager.Instance.GetBulletSpeed()));
        
        return _winner = new Vector2(-2.99f, -0.55f) * (GameManager.Instance.GetBulletSpeed());
    }
    
    /// <summary>
    /// Method to calculate the bullet position with given probabilities
    /// for three items
    /// </summary>
    /// <param name="_chanceOne"></param>
    /// <param name="_chanceTwo"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletPositionChance(double _chanceOne, double _chanceTwo, double _chanceThree)
    {
        Vector2 _winner;
        double _itemOne = _chanceOne;
        double _itemTwo = _chanceTwo;
        double _itemThree = _chanceThree;
        double _totalRate = _chanceOne + _chanceTwo + _chanceThree;
        _itemOne /= _totalRate;
        _itemTwo /= _totalRate;
        _itemThree /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemOne)
            return (GameManager.Instance.GetGunBulletPositionOne() * (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo))
            return (GameManager.Instance.GetGunBulletPositionTwo() *
                   (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo + _itemThree))
            return (GameManager.Instance.GetGunBulletPositionThree() *
                   (GameManager.Instance.GetBulletSpeed()));
        
        return _winner = new Vector2(-2.99f, -0.55f) * (GameManager.Instance.GetBulletSpeed());
    }
    
    /// <summary>
    /// Method to calculate the bullet position with given probabilities
    /// for four items
    /// </summary>
    /// <param name="_chanceOne"></param>
    /// <param name="_chanceTwo"></param>
    /// <returns></returns>
    private Vector2 CalculateBulletPositionChance(double _chanceOne, double _chanceTwo, double _chanceThree, double _chanceFour)
    {
        Vector2 _winner;
        double _itemOne = _chanceOne;
        double _itemTwo = _chanceTwo;
        double _itemThree = _chanceThree;
        double _itemFour = _chanceFour;
        double _totalRate = _chanceOne + _chanceTwo + _chanceThree + _chanceFour;
        _itemOne /= _totalRate;
        _itemTwo /= _totalRate;
        _itemThree /= _totalRate;
        _itemFour /= _totalRate;
        double _spawnRate = rnd.NextDouble();
        if (_spawnRate < _itemOne)
            return (GameManager.Instance.GetGunBulletPositionOne() * (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo))
            return (GameManager.Instance.GetGunBulletPositionTwo() *
                   (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo + _itemThree))
            return (GameManager.Instance.GetGunBulletPositionThree() *
                   (GameManager.Instance.GetBulletSpeed()));
        else if (_spawnRate < (_itemOne + _itemTwo + _itemThree + _itemFour))
            return (GameManager.Instance.GetGunBulletPositionFour() *
                   (GameManager.Instance.GetBulletSpeed()));
        
        return _winner = new Vector2(-2.99f, -0.55f) * (GameManager.Instance.GetBulletSpeed());
    }


    /// <summary>
    /// Method to disabled a specific weapon sensor
    /// </summary>
    /// <param name="prfab"></param>
    private void DisableWeaponSensorForSelectedPrefab(int prefab)
    {
        GameObject sensor = null;
        if (prefab == 1)
            sensor = _weaponSensorList.Where(i => i.gameObject.tag == "GunSensor1").First();
        if (prefab == 2)
            sensor = _weaponSensorList.Where(i => i.gameObject.tag == "GunSensor2").First();
        if (prefab == 3)
            sensor = _weaponSensorList.Where(i => i.gameObject.tag == "GunSensor3").First();
        sensor.SetActive(false);
    }

    /// <summary>
    /// Method to disable the current bullet
    /// </summary>
    /// <param name="prefab"></param>
    public void DisableBullet(int prefab)
    {
        GameObject bullet = GetBulletForSelectedPrefab(prefab);
        bullet.gameObject.SetActive(false);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
#if DEVELOPMENT
        Debug.Log($"Bullet mit dem Namen: {bullet.name}");
#endif
        _weaponShooting = false;
        _weaponCharging = false;
    }

    /// <summary>
    /// Method to disable the current Weapon light (only)
    /// </summary>
    /// <param name="prefab"></param>
    private void DisableWeaponLightAnimationForSelectedPrefab(int prefab)
    {
        foreach (GameObject value in _weaponLightList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponLight")
            {
                value.gameObject.GetComponent<Animator>().enabled = false;
                return;
            }

            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponLight")
            {
                value.gameObject.GetComponent<Animator>().enabled = false;
                return;
            }

            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponLight")
            {
                value.gameObject.GetComponent<Animator>().enabled = false;
                return;
            }
        }
    }

    /// <summary>
    /// Method to get all gun parts
    /// --> Light
    /// --> Mount
    /// --> Charge Animation
    /// --> Fire Animation
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ObservableCollection<GameObject> GetGunPartsForSelectedPrefab(int prefab)
    {
        ObservableCollection<GameObject> items = new ObservableCollection<GameObject>();
        foreach (GameObject value in _weaponLightList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponLight")
            {
                items.Add(value);
                break;
            }
            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponLight")
            {
                items.Add(value);
                break;
            }
            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponLight")
            {
                items.Add(value);
                break;
            }
        }
        foreach (GameObject value in _weaponMountList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponMount")
            {
                items.Add(value);
                break;
            }
            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponMount")
            {
                items.Add(value);
                break;
            }
            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponMount")
            {
                items.Add(value);
                break;
            }
        }
        foreach (GameObject value in _weaponChargeList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponCharge")
            {
                items.Add(value);
                break;
            }
            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponCharge")
            {
                items.Add(value);
                break;
            }
            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponCharge")
            {
                items.Add(value);
                break;
            }
        }
        foreach (GameObject value in _weaponFireList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponFire")
            {
                items.Add(value);
                break;
            }
            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponFire")
            {
                items.Add(value);
                break;
            }
            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponFire")
            {
                items.Add(value);
                break;
            }
        }
        return items;
    }

    /// <summary>
    /// Method to get the current bullet
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private GameObject GetBulletForSelectedPrefab(int prefab)
    {
        GameObject bullet = null;
        foreach (GameObject value in _bulletList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_GunBullet")
                return value;
            if (prefab == 2 && value.gameObject.tag == "Prefab2_GunBullet")
                return value;
            if (prefab == 3 && value.gameObject.tag == "Prefab3_GunBullet")
                return value;
        }
        return bullet;
    }
    
    /// <summary>
    /// Method to disable objects to simulate a charge animation
    /// </summary>
    /// <param name="prefab"></param>
    private void ChargeWeapon(int prefab)
    {
        ObservableCollection<GameObject> _gunParts = GetGunPartsForSelectedPrefab(prefab);
        
        GameObject _weaponLight = null;
        if(prefab == 1)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponLight").First();
        else if (prefab == 2)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponLight").First();
        else if (prefab == 3)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponLight").First();
        else
            new NotImplementedException("Wrong prefab integer!");

        _weaponLight.GetComponent<Animator>().enabled = false;
        
        GameObject _weaponFire = null;
        if(prefab == 1)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First();
        else if (prefab == 2)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First();
        else if (prefab == 3)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponFire.SetActive(false);
        
        GameObject _weaponCharge = null;
        if(prefab == 1)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First();
        else if (prefab == 2)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First();
        else if (prefab == 3)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponCharge.SetActive(true);
        _weaponCharge.GetComponent<Animator>().enabled = true;

    }

    /// <summary>
    /// Method to disable the current weapon and animation
    /// </summary>
    /// <param name="prefab"></param>
    private void DisableWeaponAnimation(int prefab)
    {
        ObservableCollection<GameObject> _gunParts = GetGunPartsForSelectedPrefab(prefab);
        
        GameObject _weaponFire = null;
        if(prefab == 1)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First();
        else if (prefab == 2)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First();
        else if (prefab == 3)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponFire.SetActive(false);
        
        GameObject _weaponCharge = null;
        if(prefab == 1)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First();
        else if (prefab == 2)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First();
        else if (prefab == 3)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponCharge.SetActive(true);
        _weaponCharge.GetComponent<Animator>().enabled = false;
        DisableWeaponPlateForSelectedPrefab(prefab);
    }

    /// <summary>
    /// Method to turn the light off from the plate the weapon attched to
    /// </summary>
    /// <param name="prefab"></param>
    private void DisableWeaponPlateForSelectedPrefab(int prefab)
    {
        foreach (GameObject value in _weaponPlateList)
        {
            if (prefab == 1 && value.gameObject.tag == "Prefab1_WeaponPlate")
            {
                value.GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
            if (prefab == 2 && value.gameObject.tag == "Prefab2_WeaponPlate")
            {
                value.GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
            if (prefab == 3 && value.gameObject.tag == "Prefab3_WeaponPlate")
            {
                value.GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
        }
    }

    /// <summary>
    /// Method to disable objects to simulate a fire animation
    /// </summary>
    /// <param name="prefab"></param>
    private void FireWeaponAnimation(int prefab)
    {
        ObservableCollection<GameObject> _gunParts = GetGunPartsForSelectedPrefab(prefab);
        
        GameObject _weaponLight = null;
        if(prefab == 1)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponLight").First();
        else if (prefab == 2)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponLight").First();
        else if (prefab == 3)
            _weaponLight = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponLight").First();
        else
            new NotImplementedException("Wrong prefab integer!");

        _weaponLight.GetComponent<Animator>().enabled = true;
        
        GameObject _weaponFire = null;
        if(prefab == 1)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponFire").First();
        else if (prefab == 2)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponFire").First();
        else if (prefab == 3)
            _weaponFire = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponFire").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponFire.SetActive(true);
        _weaponFire.GetComponent<Animator>().enabled = true;
        
        GameObject _weaponCharge = null;
        if(prefab == 1)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab1_WeaponCharge").First();
        else if (prefab == 2)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab2_WeaponCharge").First();
        else if (prefab == 3)
            _weaponCharge = _gunParts.Where(i => i.gameObject.tag == "Prefab3_WeaponCharge").First();
        else
            new NotImplementedException("Wrong prefab integer!");
        
        _weaponCharge.SetActive(false);
    }

    
    /// <summary>
    /// Method to reset all bullet coordinates
    /// </summary>
    /// <param name="prefab"></param>
    public void ResetAllBulletCoordinates()
    {
        foreach (GameObject bullet in _bulletList)
        {
            bullet.transform.localPosition = new Vector3(4.09f, 1.78f, 0f);
        }
    }

    /// <summary>
    /// Method to reset specific bullet coordinates
    /// </summary>
    /// <param name="prefab"></param>
    public void ResetBulletCoordinates(int prefab)
    {
        GameObject bullet = null;
        if (prefab == 1)
            bullet = _bulletList.Where(i => i.gameObject.tag == "Prefab1_GunBullet").First();
        else if (prefab == 2)
            bullet = _bulletList.Where(i => i.gameObject.tag == "Prefab2_GunBullet").First();
        else if (prefab == 3)
            bullet = _bulletList.Where(i => i.gameObject.tag == "Prefab3_GunBullet").First();
        bullet.transform.localPosition = new Vector3(4.09f, 1.78f, 0f);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    /// <summary>
    /// Method to draw a cube on the "scene" screen to locate the camera view field
    /// </summary>
    private void OnDrawGizmos()
    {
#if DEVELOPMENT
        Vector2 camPos = Camera.main.transform.position;
        Vector2 size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2);
        Gizmos.DrawWireCube(camPos, size);  
#endif
    }
    
}
