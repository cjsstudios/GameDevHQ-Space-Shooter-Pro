﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //VARS FLOAT
    [SerializeField]
    private float _speed = 3.5f;        //Movement speed

    [SerializeField]
    private GameObject _laserPrefab;    //Set @Inspector

    [SerializeField]
    private float _fireRate = 0.5f;     //Fire Rate   *Override @Inspector
    private float _canFire = 0.0f;      //Var to hold new time to cross-check fire rate

    [SerializeField]
    private int _lives = 3;             //Player lives

    SpawnManager _spawnManager;         //<SpawnManager> ref

    //Triple Shot
    [SerializeField]
    private bool _isTripleShotActive = false;           //3shot active toggle
    [SerializeField]
    private GameObject _tripleShotPrefab;               //Set @Inspector
    [SerializeField]
    private float _coolDownTimeTripleShot = 5.0f;       //3shot cool down time
    //Speed Boost
    [SerializeField]
    private bool _isSpeedBoostActive = false;           //SpeedBoost active toggle
    [SerializeField]
    private float _powerupSpeed = 8.5f;                 //Player-Speed when SpeedBoost active ~~WAS MULTIPLIER~~
    [SerializeField]
    private float _coolDownSpeedBoost = 5.0f;           //SpeedBoost cooldown time
    //Shield
    [SerializeField]
    private UnityEngine.GameObject _shieldPrefab;       //Set @Inspector
    [SerializeField]
    private bool _isShieldActive = false;               //Shield active toggle
    [SerializeField]
    private float _coolDownShieldPowerup = 5.0f;        //Shield cooldown time
    [SerializeField]
    private UnityEngine.GameObject _shieldVisualsPrefab;//Set @Inspector
    [SerializeField]
    private UnityEngine.GameObject _rightEngine;        //Set @Inspector
    [SerializeField]
    private UnityEngine.GameObject _leftEngine;         //Set @Inspector
    [SerializeField]
    private UnityEngine.GameObject[] _bothEngines = new UnityEngine.GameObject[1];  //Array of engines for random   //Set @Inspector

    [SerializeField]
    public int _score = 0;              //Score

    private UIManager _uiManager;       //<UIManager> ref

    //var to store audio clip
    //private AudioSource _audioLaser;  //*NOT IN USE
    [SerializeField]
    private AudioClip _laserSoundClip;  //<AudioClip> ref of Laser sound  //Set @Inspector 
    [SerializeField]
    private AudioSource _audioSource;   //<AudioSource> ref

    private SoundFX _explosionSoundFX;  //<SoundFX> ref of explosion sound

    

    //START
    //Move player to start pos
    //Get:Set <SpawnManger> <UIManager> <AudioSource> <SoundFX>
    //Deactive engine damage visuals
    //Set engines to array
    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0f, -2.12f, 0f);                               //Player spawn pos
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();  //<SpawnManager> ref
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();               //<UIManager> ref
        _audioSource = GetComponent<AudioSource>();                                     //<AudioSource> ref
        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();   //<SoundFX> ref

        //NULL CHECKS: 
        //<SpawnManger>
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        //<UIManager>
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        //<AudioSource>
        if (_audioSource == null)
        { Debug.LogError("_audioSource on Player is NULL"); }
        else { _audioSource.clip = _laserSoundClip; }                                   //Set <AudioSource>.clip to <_laserSoundClip> ref

        // _audioLaser = GameObject.Find("Laser_SFX").GetComponent<AudioSource>();      //*NOT IN USE Set <AudioSource> to [Laser_SFX]<AudioSource>
        //if (_audioLaser == null) { Debug.LogError("_audioLaser is NULL"); }           //*NOT IN USE Null-check

        //<SoundFX>
        if (_explosionSoundFX == null) { Debug.LogError("_explosionSoundFX is NULL"); }

        _rightEngine.SetActive(false);  //Deactivate right engine on start
        _leftEngine.SetActive(false);   //Deactivate left engine on start
        _bothEngines[0] = _rightEngine; //Set array[0] to right engine
        _bothEngines[1] = _leftEngine;  //Set array[1] to left engine
    }

    //UPDATE
    //Calculate player movement
    //Shoot laser on spacebar
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space))    
        { 
            if (Time.time > _canFire)
            {
                //Debug.Log("Shoot Time: " + Time.time);
                //Debug.Log("***Is GameTime " + Time.time + " > " + _canFire + " CanFire Time");
                FireLaser();
            }
            else { Debug.Log("TOO EARLY TO SHOOT"); }   //Player shoot attempt too early
        }
    }

    //MOVEMENT
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    //Set x-input
        float verticalInput = Input.GetAxis("Vertical");        //Set y-input

        //Time.deltaTime = per second in Update frame / meters per second
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);                   //**NOT IN USE //Move left and right
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);                        //**NOT IN USE //Move up and down
        //Another solution
        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);    //**NOT IN USE //Move all directions
        //Another solution (Tutorial)
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //If SpeedBoost is active then multiply by power-up speed boost..
        //..otherwise speed is normal
        if (_isSpeedBoostActive == true)
        {
            transform.Translate(direction * _powerupSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }        
        //only 1 line of code with multiplier, just set multiplier to 1 when off

        //Move Horizontal & Verticle with y-clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0f);   //Clamps the y-cord (Prevent: ship from moving up pass clamp y-cords, up&down)
        //Same As^
        /* //Move Horizontal & Verticle with y-boundries
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0f);
        }
        */

        //Movement screen wrap
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0f);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0f);
        }
    }

    //SHOOT
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;           //Set new time for fire rate
        //Debug.Log("Can Fire Time: " + _canFire);
        //Debug.Log("MathCheck: FireRate = (" + _fireRate + ") <> GameTime(inSec) = " + Time.time + " <> Is FireRate + Time = " + _canFire);

        //Shoot triple-shot if active, otherwise shoot default laser
        if (_isTripleShotActive)    
        {
            Debug.Log("Triple Laser Shot by Player");
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Laser Shot by Player");
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.0f, 0f), Quaternion.identity);
            //Debug.Break();
        }

        //play laser audio clip
        _audioSource.Play();
        //_audioLaser.Play();   //**NOT IN USE
        
    }

    //DAMAGE
    //Player damage control
    //No damage if shields, otherwise -1 life
    //Update UI: Lives img
    //Activate engine damage on player damage
    //Out of lives stops enemy spawn, plays explosion soundfx, destroy self
    //Display 'Game Over'
    public void Damage()
    {
        if (_isShieldActive == true)                //If shield is active...
        {
            _isShieldActive = false;                //then Set isActive to false
            _shieldVisualsPrefab.SetActive(false);  //deactivate shield visual prefab
            return;                                 //EXIT Damage()
        }

        _lives -= 1;                                //If no shield, then subtract a life

        /*
        //Controlled Engines
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);            //**NOT IN USE //Set left engine active on lives(2)
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);           //**NOT IN USE //Set right engine active on lives(1)
        }
        */
        //Random Engines
        if (_lives == 2)                            //If player is damaged...
        {
            int random = Random.Range(0, 2);        //Set random engine (0 or 1) #Always -1 max for int
            Debug.Log("Random Engine: " + random);  
            _bothEngines[random].SetActive(true);   //Set chosen engine active
        }
        else if (_lives == 1)                       //If lives(1) on damage...
        {
            if (_bothEngines[0].activeSelf == true) //then If engine[0] is active..
            {
                _bothEngines[1].SetActive(true);    //Set engine[1] active
            }
            else
            {
                _bothEngines[0].SetActive(true);    //Otherwise set engine[0]active
            }
        }
        //Update UI to display lives img
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)                             //If lives is 0...
        {
            //Communicate with SpawnManager
            _spawnManager.OnPlayerDeath();          //Stop spawning enemies
            _uiManager.DisplayGameOver();           //Display Game Over text
            _explosionSoundFX.PlayExplosionSFX();   //Play explosion soundFx
            Destroy(this.gameObject);               //Destroy self
        }
    }

    //POWERUP: TRIPLESHOT ACTIVE
    //CB: <Powerup> @OnTriggerEnter2D
    public void TripleShotActive()
    {
        _isTripleShotActive = true;             //Set active
        StartCoroutine(TripleShotCoolDown());   //Begin cooldown
        
    }

    //COOLDOWN: TRIPLESHOT
    IEnumerator TripleShotCoolDown()
    {
        yield return new WaitForSeconds(_coolDownTimeTripleShot);
        _isTripleShotActive = false;            //Set inactive after cooldown timer
       
    }

    //POWERUP: SPEED BOOST ACTIVE
    //CB: <Powerup> @OnTriggerEnter2D
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;             //Set Active
        StartCoroutine(SpeedBoostCoolDown());   //Begin cooldown
        //set speed boost is active
        //start coroutine for cool down
    }

    //COOLDOWN: SPEED BOOST
    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(_coolDownSpeedBoost);
        _isSpeedBoostActive = false;            //Set not active //Runs after yield time

    }

    //POWERUP SHIELD
    //CB: <Powerup> @OnTriggerEnter2D
    public void ShieldPowerupActive()
    {
        _isShieldActive = true;                 //SET toggle check active
        _shieldVisualsPrefab.SetActive(true);   //SET shield prefab active to display shield
        //enable visualizer
        //start coroutine
    }

    //COOLDOWN: SHIELD
    //**NOT IN USE (changed with tutorial)
    //Shield deactivates on Damage()
    /*
    IEnumerator ShieldPowerupCoolDown()
    {
        yield return new WaitForSeconds(_coolDownShieldPowerup);
        _isShieldActive = false;
    }
    */

    //SCORE: ADD POINTS(int ~points set by call)
    public void AddScore(int points)
    {
        _score += points;                   //SET score
        _uiManager.UpdateScore(_score);     //Update UI score display
    }

 
    //method to add 10 to score
    //communicate with UI to update score
}
