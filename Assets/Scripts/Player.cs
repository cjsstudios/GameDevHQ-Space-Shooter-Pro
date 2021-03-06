﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //References - Managers
    SpawnManager _spawnManager;         //<SpawnManager> ref 
    private UIManager _uiManager;       //<UIManager> ref

    //References - Audio
    [SerializeField]
    private AudioSource _audioSource;   //<AudioSource> ref
    [SerializeField]
    private AudioClip _laserSoundClip;  //<AudioClip> ref of Laser sound  //Set @Inspector 
    private SoundFX _explosionSoundFX;  //<SoundFX> ref of explosion sound

    //Reference - UI
   

    //VARS - Gameplay
    [SerializeField]
    public int _score = 0;              //Score

    //VARS - Player Ship
    //Health - Lives
    [SerializeField]
    private int _lives = 3;             //Player lives
    
    //Movement
    [SerializeField]
    private float _speed = 3.5f;        //Movement speed
    //[SerializeField]
    //private int _direction;             //Direction of x, y input axis

    //Sprites
    [SerializeField]
    public Sprite _spr_shipTurn;                        //Current sprite passed to render
    [SerializeField]
    public Sprite[] _spr_ShipTurnLeft = new Sprite[9];  //Array of turn left sprites
    [SerializeField]
    public Sprite[] _spr_ShipTurnRight = new Sprite[9]; //Array of turn right sprites

    //Thruster - Colors
    private Color _colorThruster_On;
    private Color _colorSpeedBoost_On;
    private Color _colorBoostAndThruster_On;

    //Thruster - Fuel
    [SerializeField]
    private float _thrusterFuel;        //Amount of fuel for thruster
    [SerializeField]
    private bool _isRefueling;          //Refuel check, true is true

    //Weapons - Laser
    [SerializeField]
    private GameObject _laserPrefab;    //Set @Inspector    
    [SerializeField]
    private float _fireRate = 0.5f;     //Fire Rate   *Override @Inspector
    private float _canFire = 0.0f;      //Var to hold new time to cross-check fire rate
    [SerializeField]
    private int _ammoLaser = 15;
    [SerializeField]
    private int _ammoLaserMax = 15;
    [SerializeField]
    private bool _isReloadingAmmo;

    //Weapons - Space Mine
    [SerializeField]
    private bool _isSpaceMineActive = false;            //Set active toggle
    [SerializeField]
    private GameObject _spaceMinePrefab;                //Set @inspector
    [SerializeField]
    private int _mineSupply = 0;                            //mine supply count
    [SerializeField]
    private int _mineAmmoMax = 9;                       //Set max mine ammo

    //Weapons - Triple Shot
    [SerializeField]
    private bool _isTripleShotActive = false;           //3shot active toggle
    [SerializeField]
    private GameObject _tripleShotPrefab;               //Set @Inspector
    [SerializeField]
    private float _coolDownTimeTripleShot = 5.0f;       //3shot cool down time

    //PowerUp - Speed Boost
    [SerializeField]
    private bool _isSpeedBoostActive = false;           //SpeedBoost active toggle
    [SerializeField]
    private float _coolDownSpeedBoost = 5.0f;           //SpeedBoost cooldown time

    //Speed Modifiers
    [SerializeField]
    private float _powerupSpeedX = 1.0f;                //Player-Speed when SpeedBoost active ~~MULTIPLIER~~
    [SerializeField]
    private float _shiftSpeedX = 1.0f;                  //Player-Speed when LShift or MouseButton[2] active ~~MULTIPLIER~~

    //PowerUp - Shield
    [SerializeField]
    private GameObject _shieldPrefab;                   //Set @Inspector
    [SerializeField]
    private bool _isShieldActive = false;               //Shield active toggle
    [SerializeField]
    private float _coolDownShieldPowerup = 5.0f;        //Shield cooldown time
    [SerializeField]
    private GameObject _shieldVisualsPrefab;            //Set @Inspector
    [SerializeField]
    private int _shieldEnergy = 0;                      //Energy of shield
    public GameObject shieldColor;                      //Color of shield 

    //Engines for damage
    [SerializeField]
    private GameObject _rightEngine;        //Set @Inspector
    [SerializeField]
    private GameObject _leftEngine;         //Set @Inspector
    [SerializeField]
    private GameObject[] _bothEngines = new GameObject[1];  //Array of engines for random   //Set @Inspector

    //Mines - Secondary Weapon
    [SerializeField]
    private GameObject _mineFirePrefab;     //Set @Inspector

    private Animator _animPlayer;

    [SerializeField]
    AudioClip _sfxLaser;
    [SerializeField]
    AudioClip _sfxMine;
    [SerializeField]
    AudioClip _sfxNoAmmo;

    //START==============================================================================
    //Move player to start pos
    //Get:Set <SpawnManger> <UIManager> <AudioSource> <SoundFX>
    //Deactive engine damage visuals
    //Set engines to array
    void Start()
    {
        //transform.position = new Vector3(0f, -2.12f, 0f);                             //Player spawn pos, Override-@inpepctor
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();  //<SpawnManager> ref
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();               //<UIManager> ref
        _audioSource = GetComponent<AudioSource>();                                     //<AudioSource> ref
        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();   //<SoundFX> ref
        _spr_shipTurn = GetComponent<SpriteRenderer>().sprite = _spr_ShipTurnLeft[0];

        shieldColor = new GameObject();
        shieldColor = GameObject.Find("Shield");
        _colorBoostAndThruster_On = new Color(0.3f, 0f, 1.0f, 0.86f);                   //Purple blend
        _colorThruster_On = new Color(1.0f, 0f, 1.0f, 0.86f);                           //Red blend
        _colorSpeedBoost_On = new Color(0.5f, 1.0f, 0f, 0.86f);                         //Green blend


        _thrusterFuel = 1.0f;
        _animPlayer = GetComponent<Animator>();
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
    //START=============================================================START=END===========================
    //UPDATE================================================================================================
    //Calculate player movement
    //Shoot laser on spacebar
    void Update()
    {
        CalculateMovement();

        //RELOAD AMMO
        LaserAmmoReload();

        //INPUT: SPACE BAR DOWN
        //SHOOT LASER
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SFX No Shoot: check on canShoot and isReloadingAmmo
            SFX_LaserReloadAmmoCheck();
            //Shoot Laser or Active PowerUp Weapon
            FireLaser();
        }

        //INPUT: LEFT CONTROL DOWN
        //SHOOT SPECIAL WEAPON I
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetMouseButtonDown(1))
        {
            //ToDo: Add special weapon
            Debug.Log("Aquire a Special Weapon");
        }

        //INPUT: LEFT SHIFT ANY PRESSED STATE
        //THRUSTER ACTIVE
        //Colorize thrusters on button press
        //On key press add thruster speed multiplier
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3"))
        {
            if (!_isRefueling)
            {
                ThrusterActivated();
            }
        }

        //INPUT: LEFT SHIFT UP
        //THRUSTER + SPEED BOOST TOGGLE
        //to default color on button Release
        //Always true if these buttons not pushed
        //No thruster activated (normal or speed boost active only)
        if (Input.GetKeyUp(KeyCode.LeftShift) && Input.GetButtonUp("Fire3"))
        {
            //Normal Speed and Thruster Color
            ThrusterDeactivated();

            //Speed Boost active only, boost thruster color
            if (_isSpeedBoostActive) { GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = _colorSpeedBoost_On; Debug.Log("Speed Boost Only"); }
        }

        //SYSTEMS CHECK AND UI REFRESH============================================================================================
        //Refueling        
        Refueling_ShipSystemCheck();

        //UPDATE UI: Ammo
        _uiManager.Update_AmmoCount(_ammoLaser);

        //UPDATE UI: Fuel
        _uiManager.Update_ThrusterFuel(_thrusterFuel);

        _uiManager.UpdateStatusText(_ammoLaser, _thrusterFuel, _mineSupply, _ammoLaserMax, _isRefueling, _isReloadingAmmo);
        //SYSTEMS END
    }
    //SYSTEMS CHECK AND UI REFRESH===========================================SYTEMS END==================================
    //UPDATE=============================================UPDATE=END======================================================

    //AMMO: RELOAD LASER AMMO==========================================================================================
    public void LaserAmmoReload()
    {
        if ((_ammoLaser == 0 || _fireRate == 0.0f) && !_isReloadingAmmo)
        {
            Debug.Log("OUT OF AMMO!");
            _isReloadingAmmo = true;
            InvokeRepeating("Reload_AmmoLaser", 0f, 0.75f);
        }
    }

    //SFX: isReloadingAmmo CHECK Early Shoot or Ammo Reloading Play soundFX==============================================
    public void SFX_LaserReloadAmmoCheck()
    {
        if (Time.time > _canFire)
        {
            if (_isReloadingAmmo)   //Play sound if player attempts to shoot with no ammo
            {
                _audioSource.clip = _sfxNoAmmo;
                _audioSource.Play();
            }

        }
        else
        {                      //Player shoot attempt too early
            _audioSource.clip = _sfxNoAmmo;
            _audioSource.Play();
        }
    }

    //THRUSTER: Activated
    public void ThrusterActivated()
    {
        //Burn Thrusters
        //Overwrite vars if no thruster fuel left
        if (_thrusterFuel > 0 && _isRefueling == false)  //Has Fuel & Not Refueling
        {
            //Thruster Activated
            GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = _colorThruster_On;
            //Debug.Log("Thruster");
            _shiftSpeedX = 1.75f;       //Set thruster speed modifier
            BurnThrusters();                    //Decrease fuel
                                                //Thruster Actived plus Speed Boost
            if (_isSpeedBoostActive) { GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = _colorBoostAndThruster_On; Debug.Log("Thruster + Boost"); }
        }
        else
        {
            _isRefueling = true;                        //Once true then loop is false
            Debug.Log("OUT OF FUEL!!! REFUELING...");
            ThrusterDeactivated();
        }
    }
    //UPDATE=============================================UPDATE=END======================================================


    //Normal Speed and Thruster Color
    public void ThrusterDeactivated()
    {        
        if (!_isSpeedBoostActive) { GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = Color.white; /*Debug.Log("Thruster released.");*/ }
        _shiftSpeedX = 1.0f;        //Set thruster speed modifier to default
    }

    //THRUSTER FUEL BURN
    public void BurnThrusters()
    {
        _thrusterFuel -= 0.002f;
        //Thruster Actived plus Speed Boost
        if (_isSpeedBoostActive) { GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = _colorBoostAndThruster_On; /*Debug.Log("Thruster + Boost");*/ }
    }

    //REFUEL CHECK
    public void Refueling_ShipSystemCheck()
    {
        //Refueling
        //Is true if refueling
        if (_isRefueling)
        {
            _thrusterFuel += 0.0025f;                           //Refuel
            if (_thrusterFuel >= 1) { _isRefueling = false; }   //Full, refueling done
        }
    }


    //MOVEMENT================================================================================================================
    void CalculateMovement()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");    //Set x-input
        //if (horizontalInput > 8.0f) { horizontalInput = 7.99f; }
        //if (horizontalInput < -8.0f) { horizontalInput = -7.99f; }
        //Sprite change by Axis input (-1)to(1)
        if (horizontalInput < 0) { TurnLeft(horizontalInput); }
        else if (horizontalInput > 0) { TurnRight(horizontalInput); }
        else { TurnNone(); }

        gameObject.GetComponent<SpriteRenderer>().sprite = _spr_shipTurn;

        //Debug.Log("horzInput::: " + horizontalInput);
        float verticalInput = Input.GetAxis("Vertical");        //Set y-input
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //*Debug.Log("Ship is moveable!! Direction: " + direction);

        float _speedModified = _speed * _powerupSpeedX * _shiftSpeedX;
        //*Debug.Log("Speed: " + _speed + " PowerupSpeedX: " + _powerupSpeedX + " ShiftSpeed" + _shiftSpeedX);
        //*Debug.Log("Current Speed: " + _speedModified);
        transform.Translate(direction * _speedModified * Time.deltaTime);   //Multipliers default to 1.0f

        //Move Horizontal & Verticle with y-clamp
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0f);   //Clamps the y-cord (Prevent: ship from moving up pass clamp y-cords, up&down)
        
         //Move Horizontal & Verticle with y-boundries
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0f);
        }        

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

    //ANIMATION MOVEMENT: TURN LEFT
    public void TurnLeft(float turnAmt)
    {
        //Set sprite img to turnDegree index
        int turnDegree = -(int)Mathf.Floor(turnAmt * 10f);
        if (turnDegree >= _spr_ShipTurnLeft.Length) { turnDegree = _spr_ShipTurnLeft.Length - 1; }
        //Debug.Log("TurnDegree = " + turnDegree + "LeftLength" + (_spr_ShipTurnLeft.Length-1));
        _spr_shipTurn = _spr_ShipTurnLeft[turnDegree];
    }

    //ANIMATION MOVEMENT: TURN RIGHT
    public void TurnRight(float turnAmt)
    {
        //Set sprite img to turnDegree index
        int turnDegree = (int)Mathf.Floor(turnAmt * 10f);
        if (turnDegree >= _spr_ShipTurnRight.Length) { turnDegree = _spr_ShipTurnRight.Length - 1; }
        //Debug.Log("TurnDegree = " + turnDegree + "RightLength" + (_spr_ShipTurnRight.Length-1));
        _spr_shipTurn = _spr_ShipTurnRight[turnDegree];
    }

    //ANIMATION MOVEMENT: NO TURN
    public void TurnNone()
    {
        //
        //Debug.Log("Ship Not Turning!!!!!!");
    }
    //MOVEMENT=======================================================MOVEMENT=END================================
    //SHOOT======================================================================================================
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;           //Set new time for fire rate
        //Debug.Log("Can Fire Time: " + _canFire);
        //Debug.Log("MathCheck: FireRate = (" + _fireRate + ") <> GameTime(inSec) = " + Time.time + " <> Is FireRate + Time = " + _canFire);
        
        //Shoot triple-shot if active, otherwise shoot default laser
        if (_isTripleShotActive)    
        {
            //Debug.Log("Triple Laser Shot by Player");
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _ammoLaser--;
            //play laser audio clip
            _audioSource.clip = _sfxLaser;
            _audioSource.Play();
        }
        else if (_isSpaceMineActive == true)
        {
            //Spawn Space Mine
            //Debug.Log("Space Mine Deployed by Player");
            if (_mineSupply > 0)
            {
                var _spaceMine = Instantiate(_mineFirePrefab, transform.position, Quaternion.identity);
                _mineSupply--;
                _audioSource.clip = _sfxMine;
                _audioSource.Play();
                //change audio clip, play audio clip
            }
        }
        else
        {
            //Debug.Log("Laser Shot by Player");
            if (_ammoLaser > 0 && _fireRate > 0.0f)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.0f, 0f), Quaternion.identity);
                _ammoLaser--;
                //play laser audio clip
                _audioSource.clip = _sfxLaser;
                _audioSource.Play();
            }
            
            if (_ammoLaser <= 0)
            {
                //InvokeRepeating("AmmoLaser_Reload", 1.0f, 1.0f);
                Debug.Log("RELOADING AMMO PLEASE WAIT!!!");
            }
        }
    }

    //LASER AMMO RELOAD
    public void Reload_AmmoLaser()
    {
        StartCoroutine("AmmoLaser_Reload");
    }

    //RELOAD LASER AMMO COROUTINE
    IEnumerator AmmoLaser_Reload()
    {
        if (_ammoLaser < _ammoLaserMax)
        {
            _isReloadingAmmo = true;
            _fireRate = 0.0f;
            _ammoLaser++;
            //Debug.Log("RELOADING AMMO (" + _ammoLaser + ")");
            yield return new WaitForSeconds(30.75f);
        }
        else
        {
            Debug.Log("Reloaded!! Lasers Active");
            _isReloadingAmmo = false;
            //StopCoroutine("AmmoLaser_Reload");
            CancelInvoke();
            _fireRate = 0.5f;            
        }

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
            
            if (_shieldEnergy <= 1)
            {
                _isShieldActive = false;                //then Set isActive to false
                _shieldEnergy = 0;
                _shieldVisualsPrefab.SetActive(false);  //deactivate shield visual prefab
                return;                                 //EXIT Damage()
            }
            else if (_shieldEnergy > 1)
            {
                _shieldEnergy--;                        //Damage shield
                UpdateShieldColor();                    //Update shield loss color
                return;                                 //EXIT Damage()
            }
        }
        //DAMAGE
        _lives -= 1;                                //If no shield, then subtract a life

        //Random Engines
        if (_lives == 2)                            //If player is damaged...
        {
            int random = Random.Range(0, 2);        //Set random engine (0 or 1) #Always -1 max for int
           // Debug.Log("Random Engine: " + random);  
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

    //POWERUP: AMMO
    public void AmmoPowerUp()
    {
        _ammoLaserMax++;
        _ammoLaser = _ammoLaser + (_ammoLaserMax - _ammoLaser);    //Get and Add differnce to fill ammo to max
        //Debug.Log("Max Ammo is " + _ammoLaserMax);
    }

    //POWERUP: HEALTH
    public void HealthPowerUp()
    {
        if (_lives < 3)
        {
            _lives = 3;
            _bothEngines[0].SetActive(false);
            _bothEngines[1].SetActive(false);
            _uiManager.UpdateLives(_lives);
            //Debug.Log("Health!!! Ship Repaired");
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

    //POWERUP: SPACE MINE ACTIVE
    public void SpaceMineActive()
    {
        _mineSupply = _mineAmmoMax;            //Add max mines to supply 
        //Debug.Log("Mine Supply is " + _mineSupply + " and Max Mines is " + _mineAmmoMax);

        _isSpaceMineActive = true;             //Set active
        StopCoroutine(SpaceMineCoolDown());
        StartCoroutine(SpaceMineCoolDown());   //Begin cooldown
    }

    //COOLDOWN: SPACE MINE 
    IEnumerator SpaceMineCoolDown()
    {
        
        yield return new WaitForSeconds(5.0f);
        _mineSupply = 0;                        //Zero Mines
        _isSpaceMineActive = false;             //Set inactive after cooldown timer
    }

    //POWERUP: SPEED BOOST ACTIVE
    //CB: <Powerup> @OnTriggerEnter2D
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;             //Set Active
        GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0f, 0.86f);
        _powerupSpeedX = 2.0f;
        StartCoroutine(SpeedBoostCoolDown());   //Begin cooldown
        //set speed boost is active
        //start coroutine for cool down
    }

    //COOLDOWN: SPEED BOOST
    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(_coolDownSpeedBoost);
        GameObject.Find("Thruster").GetComponent<SpriteRenderer>().color = Color.white;
        _powerupSpeedX = 1.0f;
        _isSpeedBoostActive = false;            //Set not active //Runs after yield time

    }
    //SHOOT==================================================================SHOOT=END==========================
    //POWERUPS==================================================================================================
    //POWERUP SHIELD
    //CB: <Powerup> @OnTriggerEnter2D
    public void ShieldPowerupActive()
    {
        _isShieldActive = true;                 //SET toggle check active
        _shieldEnergy += 1;
        if(_shieldEnergy > 4) { _shieldEnergy = 4; }
        _shieldVisualsPrefab.SetActive(true);   //SET shield prefab active to display shield
        UpdateShieldColor();
        //enable visualizer
        //start coroutine
    }

    //POWERUP: SHIELD COLOR
    void UpdateShieldColor()
    {
        switch (_shieldEnergy)
        {
            case 4: _shieldVisualsPrefab.GetComponent<SpriteRenderer>().color = Color.magenta; break;
            case 1: _shieldVisualsPrefab.GetComponent<SpriteRenderer>().color = Color.red; break;
            case 2: _shieldVisualsPrefab.GetComponent<SpriteRenderer>().color = Color.green; break;

            case 3: 
            default: _shieldVisualsPrefab.GetComponent<SpriteRenderer>().color = Color.white; break; 
        }
    }
    //POWERUPS================================================================POWERUPS=END======================
    //SCORE=====================================================================================================
    //SCORE: ADD POINTS(int ~points set by call)
    public void AddScore(int points)
    {
        _score += points;                   //SET score
        _uiManager.UpdateScore(_score);     //Update UI score display
    }
    //SCORE=====================================================================SCORE= END======================
}
