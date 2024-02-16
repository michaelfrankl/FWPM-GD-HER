using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class HeroScript : MonoBehaviour
{
    public Joystick _joystick;
    private static bool _isGamePaused;
    private static bool _isDying;
    private static bool _isCrawling;
    private static bool _isRespawning;
    private static bool _gameOver;
    private DateTime _respawnStartTime;
    private DateTime _gameOverAnimationTime;
    
    //GameObjects for activate/disable
    public List<GameObject> _objects;
    public Text _fpsUI;
    public GameObject _fpsBackground;
    
    public float heroSpeed;
    private bool onGround;
    private bool _stayOnLaser;
    private Animator anim;
    //private static int health;
    private int _transition = 1;
    float deltaTime = 0.0f;
    
    public Text _scoreUI;
    private int _scoreValue;
    private int _fuelCountValue;
    private Vector3 _startPosition = new Vector3(-5f, -1.25f, 0f);
    public Text _fuelCount;

    public Text _liveUI;
    private static int _lives;
    public ProgressBar _healthBar;
    
    //Sounds
    private AudioSource GameSound;
    private AudioSource JumpSound;
    private AudioSource FuelCollectSound;
    private AudioSource FuelDestroySound;
    private AudioSource LaserDamageSound;
    private AudioSource LaserShotSound;
    private AudioSource BulletHitSound;
    private AudioSource HeroScreamSound;
    private AudioSource RespawnSound;
    private AudioSource SecretKeyCollectSound;
    private AudioSource CratePointCollectSound;
    private AudioSource CrateLiveCollectSound;

    private void Awake()
    {
        GameManager.Instance.UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
#if DEVELOPMENT
        Debug.Log($"Updated Camera --> {GameManager.Instance._mainCamera}");
#endif
        if (GameManager.Instance.GetGameSoundBoolValue())
        {
            GameSound = GameManager.Instance._mainCamera.GetComponent<AudioSource>();
            GameSound.loop = true;
            GameSound.playOnAwake = true;
            GameSound.Play();
        }

        if (GameManager.Instance.IsSecretKeyCollected())
        {
            DisplaySecretKeyToUI();
            FindObjectOfType<RandomizerScript>().DisableLaserOnStartup();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
#if PC
#if DEVELOPMENT
        if (GameManager.Instance.GetDisplayFpsBoolValue())
        {
            _fpsUI.gameObject.SetActive(true);
        }
#endif
        
#endif
        _isDying = false;
        _lives = GameManager.Instance.GetLiveCountForDifficulty(GameManager.Instance.GetDifficulty());
        _liveUI.text = string.Empty + _lives;
        _scoreValue = GameManager.Instance.GetGameScore();
        _scoreUI.text = string.Empty+_scoreValue;
        _fuelCountValue = GameManager.Instance.GetFuelCount();
        _fuelCount.text = string.Empty+_fuelCountValue;
        _healthBar.BarValue = GameManager.Instance.GetHeroStartHealth();
        heroSpeed = GameManager.Instance.GetHeroRunSpeed();
        onGround = true;
        anim = GetComponent<Animator>();
        anim.SetInteger("Transition", 1);

#if DEVELOPMENT
        Debug.Log($"Current speed of Hero: {heroSpeed.ToString()}");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        ControlHero();
        
        if(_isRespawning && DateTime.Now > (_respawnStartTime.AddSeconds(GameManager.Instance.GetRespawnTime())))
        {
            _isRespawning = false;
            _isDying = false;
            
#if DEVELOPMENT
            Debug.Log($"Respawntime is over");
#endif
        }

        if (_gameOver && DateTime.Now >= _gameOverAnimationTime.AddSeconds(3))
        {
            _gameOver = false;
            GameManager.Instance.LoadGameOverScreen();
        }
#if PC
#if DEVELOPMENT
        if (GameManager.Instance.GetDisplayFpsBoolValue())
        {
            UpdateFpsCounter();
        }
#endif     
#endif

        if (_stayOnLaser)
        {
            DecreaseHealth(2);
        }
    }

    /// <summary>
    /// Method to calculate the fps and display it
    /// </summary>
    /// <param name="_fpsUI"></param>
    private void UpdateFpsCounter()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float ms = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string txt = string.Format("{0:0.0} ms / {1:0} fps", ms, fps);
        _fpsUI.text = txt;
    }

    /// <summary>
    /// Method to switch the controls for the hero
    /// </summary>
    private void ControlHero()
    {
        if(_gameOver)
            return;
#if PC
        //For Looping it needs GetKey
        //For a single press and unpress GetKeyDown
        if (Input.GetKey("right") && !_isGamePaused && !_isDying && !_isCrawling)
        {
            HerRunRight();
        }
        else if (Input.GetKey("left") && !_isGamePaused && !_isDying && !_isCrawling)
        {
            HeroRunLeft();
        }

        else if (Input.GetKeyDown("up") && onGround && !_isGamePaused && !_isDying && !_isCrawling)
        {
            HeroJump();
        }
        else if (Input.GetKey("down") && onGround && !_isGamePaused && !_isDying)
        {
            _isCrawling = true;
            HeroCrawl();
        }
        else if (Input.GetKeyUp("down"))
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            _isCrawling = false;
        }
        
        //Release key
        else if (Input.GetKeyUp("right") || Input.GetKeyUp("left") && !_isCrawling)
        {
            HeroStandStill();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
#endif
#if MOBILE
        //run right
        if (_joystick.Horizontal >= .2f && !_isCrawling)
        {
            HerRunRight();
        }
        //run left
        else if (_joystick.Horizontal <= -.2f && !_isCrawling)
        {
            HeroRunLeft();
        }
        else
        {
            HeroStandStill();
        }
        float vertical = _joystick.Vertical;
        if (vertical >= .5f && !_isCrawling)
        {
            if(onGround)
                HeroJump();
        }

        if (vertical <= -.5f)
        {
            if (onGround)
            {
                _isCrawling = true;
                HeroCrawl();
            }
        }

        if (vertical > -.5f)
        {
            _isCrawling = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
#endif
    }
    
    /// <summary>
    /// Method to move the hero to the left
    /// </summary>
    private void HeroRunLeft()
    {
        transform.position = new Vector3(transform.position.x - heroSpeed * Time.deltaTime, transform.position.y,
            transform.position.z);
        transform.localScale = new Vector3(-1f, 1f, 1f);
        _transition = 2;
        if(!_isRespawning)
            anim.SetInteger("Transition", 2);
        else
            anim.SetInteger("Transition", 6);
    }

    /// <summary>
    /// Method to move the hero to the right
    /// </summary>
    private void HerRunRight()
    {
        transform.position = new Vector3(transform.position.x + heroSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(1f, 1f, 1f);
        _transition = 2;
        if(!_isRespawning)
            anim.SetInteger("Transition", 2);
        else
            anim.SetInteger("Transition", 6);
            
        if (transform.position.x > _startPosition.x)
        {
            if ((transform.position.x - _startPosition.x) >= 5f)
            {
                //float delta = (transform.position.x - _startPosition.x);
                _scoreValue += 5;
                GameManager.Instance.UpdateGameScore(_scoreValue);
                _scoreUI.text = string.Empty+_scoreValue;
                _startPosition.x = transform.position.x;
#if DEVELOPMENT
                //Debug.Log($"Reached a delta of {delta.ToString()}");
                //Debug.Log($"New Start Position is: {_startPosition.x.ToString()}");
#endif
            }
        }
    }

    /// <summary>
    /// Method to let the hero jump once
    /// </summary>
    private void HeroJump()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 jumpForce = new Vector2(0f, GameManager.Instance.GetHeroJumpForce());
        rb.AddForce(jumpForce, ForceMode2D.Impulse);
#if DEVELOPMENT
        Debug.Log($"Current Force Value: {GameManager.Instance.GetHeroJumpForce()}");
#endif
        if(!_isRespawning)
            anim.SetTrigger("Jump");
        else
            anim.SetTrigger("RespawnJump");
        PlayJumpSound();
            
        //_transition = 3;
        //anim.SetInteger("Transition", 3);
        onGround = false;
    }

    /// <summary>
    /// Method to let the hero crawl
    /// to avoid a bullet
    /// </summary>
    private void HeroCrawl()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        if(!_isRespawning)
            anim.SetInteger("Transition", 8);
        else
            anim.SetInteger("Transition", 9);
    }

    /// <summary>
    /// Method to trigger the stand animation
    /// </summary>
    private void HeroStandStill()
    {
        _transition = 1;
        if(!_isRespawning)
            anim.SetInteger("Transition", 1);
        else
            anim.SetInteger("Transition", 5);
    }
    
    /// <summary>
    /// Method to resume the game after stop and disable/enable all
    /// specific objects
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _isGamePaused = false;
        if (GameManager.Instance.GetGameSoundBoolValue())
            GameSound.UnPause();
        
        foreach (GameObject item in _objects)
        {
            if(item.tag == "PlayIcon")
                item.SetActive(false);
            if(item.tag == "PauseIcon")
                item.SetActive(true);
            if(item.tag == "PauseBackground")
                item.SetActive(false);
            if(item.tag == "PauseUI")
                item.SetActive(false);
            if(item.tag == "BtnBackMenu")
                item.SetActive(false);
            if(item.tag == "BtnSecretLevel" && GameManager.Instance.IsSecretKeyCollected())
                item.SetActive(false);
#if DEVELOPMENT
            if(item.tag == "BtnDev")
                item.SetActive(false);
#endif
#if MOBILE
            _joystick.gameObject.SetActive(true);
#endif
        }
    }

    
    /// <summary>
    /// Method to pause the game
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0f;
        _isGamePaused = true;
        if(GameManager.Instance.GetGameSoundBoolValue())
            GameSound.Pause();

        foreach (GameObject item in _objects)
        {
            if(item.tag == "PlayIcon")
                item.SetActive(true);
            if(item.tag == "PauseIcon")
                item.SetActive(false);
            if(item.tag == "PauseBackground")
                item.SetActive(true);
            if(item.tag == "PauseUI")
                item.SetActive(true);
            if(item.tag == "BtnBackMenu")
                item.SetActive(true);
            if(item.tag == "BtnSecretLevel" && GameManager.Instance.IsSecretKeyCollected())
                item.SetActive(true);
#if DEVELOPMENT
            if(item.tag == "BtnDev")
                item.SetActive(true);
#endif
#if MOBILE
            _joystick.gameObject.SetActive(false);
#endif
        }
    }

    /// <summary>
    /// Method to handle the pause button and
    /// switch between resume and pause
    /// </summary>
    public void PauseButtonPressed()
    {
        if(!_isGamePaused)
            PauseGame();
        else
            ResumeGame();
    }
    
    public float GetHealth()
        => _healthBar.BarValue;

    private void IncreaseFuelCount()
    {
        _fuelCountValue++;
        _fuelCount.text = string.Empty+_fuelCountValue;
        GameManager.Instance.UpdateFuelCount(_fuelCountValue);
    }

    /// <summary>
    /// Method to increase the live counter
    /// with difficulty states
    /// 1 = Easy = 4
    /// 2 = Normal = 2
    /// 3 = Hard = 1
    /// </summary>
    private void AddLiveToHero()
    {
        _lives += GameManager.Instance.GetLiveCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _liveUI.text = string.Empty + _lives;
        PlayHeroCrateLiveCollectSound();
    }
    
    /// <summary>
    /// Method to add points to hero
    /// with difficulty states
    /// 1 = Easy = 500
    /// 2 = Normal = 250
    /// 3 = Hard = 100
    /// </summary>
    private void AddPointsToHero()
    {
        _scoreValue += GameManager.Instance.GetPointCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        _scoreUI.text = string.Empty + _scoreValue;
        PlayHeroCratePointCollectSound();
    }

    /// <summary>
    /// Method to add the secret key
    /// </summary>
    private void AddSecretKeyToHero()
    {
        GameManager.Instance.UpdateSecretKeyCollected(true);
        PlayHeroCrateSecretKeyCollectSound();
        DisplaySecretKeyToUI();
    }
    
    private void DisplaySecretKeyToUI()
    {
        GameObject _keyAnimation = _objects.Where(i => i.gameObject.tag == "DisplayKey").First();
        _keyAnimation.SetActive(true);
    }

    /// <summary>
    /// Method to increase the health for a normal fuelCan
    /// </summary>
    public void IncreaseHealth()
    {
        _healthBar.BarValue += GameManager.Instance.GetFuelBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        if (_healthBar.BarValue > 100f)
        {
            _healthBar.BarValue = 100f;
            return;
        }
#if DEVELOPMENT
        Debug.Log($"Current health: {_healthBar.BarValue.ToString()}");
#endif
        PlayFuelCollectSound();
        IncreaseFuelCount();
    }

    /// <summary>
    /// Method for increase the health of the hero
    /// with a crate with difficulty states
    /// 1 = Easy
    /// 2 = Normal
    /// 3 = Hard
    /// </summary>
    private void IncreaseHealthWithCrate()
    {
        _healthBar.BarValue +=
            GameManager.Instance.GetFuelCrateBenefitForDifficulty(GameManager.Instance.GetDifficulty());
        if (_healthBar.BarValue > 100f)
            _healthBar.BarValue = 100f;
        
        PlayFuelCollectSound();
        IncreaseFuelCount();
    }

    /// <summary>
    /// Method do decrease the health of the hero
    /// with specific states
    /// </summary>
    /// <param name="type"></param>
    public void DecreaseHealth(int type)
    {
        if (_isDying || _isRespawning)
            return;
        
        float damage = DamageType(type);
        //Death and more
        if (GetHealth() - damage <= 0f)
        {
            if (_lives - 1 <= 0)
            {
                _lives--;
                _liveUI.text = string.Empty + _lives;
                _isDying = true;
                _healthBar.BarValue = 0f;
                anim.SetInteger("Transition", 4);
                PlayHeroScreamSound();
                _gameOver = true;
                _gameOverAnimationTime = DateTime.Now;
#if DEVELOPMENT
                Debug.Log("Gameover!");
#endif
            }
            else
            {
                _isDying = true;
                _healthBar.BarValue = 0f;
                _lives--;
                _liveUI.text = string.Empty + _lives;
                PlayHeroRespawnSound();
                anim.SetInteger("Transition", 5);
                _healthBar.BarValue = 100f;
                _respawnStartTime = DateTime.Now;
                _isRespawning = true;
                _isDying = false;
            }
        }
        else
        {
            if (type == 2)
            {
                _stayOnLaser = true;
                _healthBar.BarValue -= damage;
                return;
            }
            _healthBar.BarValue -= damage;
            if(type == 1)
                PlayLaserDamageSound();
            anim.SetTrigger("HeroHit");
            
#if DEVELOPMENT
            Debug.Log($"Decrease live of Hero with {damage.ToString()}");
#endif
        }
    }
    
    

    /// <summary>
    /// Method to define the different damages
    /// the hero will get
    /// TODO: Add difficulty difference
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private float DamageType(int type)
    {
        // 1 - Laser hit
        // 2 - Stay on laser
        // 3 - Bullet hit
        float damage = 0f;
        
        switch (type)
        {
            case 1:
                damage = GameManager.Instance.GetLaserHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
                break;
            case 2:
                damage = GameManager.Instance.GetLaserStayDamageForDifficulty(GameManager.Instance.GetDifficulty());
                damage *= Time.deltaTime;
                break;
            case 3:
                damage = GameManager.Instance.GetGunHitDamageForDifficulty(GameManager.Instance.GetDifficulty());
                break;
        }
        return damage;
    }

    /// <summary>
    /// Method to disable the _stayOnLaser bool
    /// if the hero exit the trigger of the laser
    /// </summary>
    public void DisableStayOnLaser()
    {
        _stayOnLaser = false;
    }

    /// <summary>
    /// Trigger when the hero collied with the ground
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        onGround = true;
        _transition = 1;
        anim.SetInteger("Transition", 1);
    }

    /// <summary>
    /// Method to get the crate item
    /// </summary>
    /// <param name="type"></param>
    /// 1 = Fuel
    /// 2 = Live
    /// 3 = Points
    /// 4 = Secret Key
    public void GetCrateBenefit(int type)
    {
        switch (type)
        {
            case 1:
                IncreaseHealthWithCrate();
                break;
            case 2:
                AddLiveToHero();
                break;
            case 3:
                AddPointsToHero();
                break;
            case 4:
                AddSecretKeyToHero();
                break;
        }
    }

    #region Sounds

    /// <summary>
    /// Sound section, all attached to
    /// -> CameraFollower --> Sounds
    /// </summary>
    private void PlayJumpSound()
    {
        JumpSound = GameObject.FindWithTag("JumpSound").GetComponent<AudioSource>();
        JumpSound.PlayOneShot(JumpSound.clip);
    }

    private void PlayFuelCollectSound()
    {
        FuelCollectSound = GameObject.FindWithTag("FuelCollectSound").GetComponent<AudioSource>();
        FuelCollectSound.PlayOneShot(FuelCollectSound.clip);
    }

    private void PlayHeroRespawnSound()
    {
        RespawnSound = GameObject.FindWithTag("RespawnSound").GetComponent<AudioSource>();
        RespawnSound.PlayOneShot(RespawnSound.clip);
    }

    public void PlayFuelDestroySound()
    {
        FuelDestroySound = GameObject.FindWithTag("FuelDestroySound").GetComponent<AudioSource>();
        FuelDestroySound.PlayOneShot(FuelDestroySound.clip);
    }

    public void PlayLaserDamageSound()
    {
        LaserDamageSound = GameObject.FindWithTag("LaserDamageSound").GetComponent<AudioSource>();
        LaserDamageSound.PlayOneShot(LaserDamageSound.clip);
    }

    public void PlayLaserShotSound()
    {
        LaserShotSound = GameObject.FindWithTag("LaserShotSound").GetComponent<AudioSource>();
        LaserShotSound.PlayOneShot(LaserShotSound.clip);
    }

    public void PlayBulletHitSound()
    {
        BulletHitSound = GameObject.FindWithTag("BulletHitSound").GetComponent<AudioSource>();
        BulletHitSound.PlayOneShot(BulletHitSound.clip);
    }

    private void PlayHeroScreamSound()
    {
        HeroScreamSound = GameObject.FindWithTag("HeroScreamSound").GetComponent<AudioSource>();
        HeroScreamSound.PlayOneShot(HeroScreamSound.clip);
    }

    private void PlayHeroCrateLiveCollectSound()
    {
        CrateLiveCollectSound = GameObject.FindWithTag("CrateLiveSound").GetComponent<AudioSource>();
        CrateLiveCollectSound.PlayOneShot(CrateLiveCollectSound.clip);
    }

    private void PlayHeroCratePointCollectSound()
    {
        CratePointCollectSound = GameObject.FindWithTag("CratePointSound").GetComponent<AudioSource>();
        CratePointCollectSound.PlayOneShot(CratePointCollectSound.clip);
    }

    private void PlayHeroCrateSecretKeyCollectSound()
    {
        SecretKeyCollectSound = GameObject.FindWithTag("CrateSecretKeySound").GetComponent<AudioSource>();
        SecretKeyCollectSound.PlayOneShot(SecretKeyCollectSound.clip);
    }

    #endregion
}
