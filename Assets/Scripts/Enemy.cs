using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //VARS
    [SerializeField]
    private float _speedEnemy = 4.0f;   //Enemy movement speed
    [SerializeField]
    private float _spawnY = 8.0f;       //Spawn location

    //Ref
    private Animator _enemyAnimator;    //handle to animator component

    private Player _player;             //<Player> ref
    private Mine _mineFire;             //<Mine>ref

    private SoundFX _explosionSoundFX;  //<SoundFX> ref

    [SerializeField]
    private GameObject _laserEnemyPrefab;//Laser [prefab] obj ref

    [SerializeField]
    private float _fireRate = 0.75f;     //Rate of fire  *OverRide @Inspector

    [SerializeField]
    private float _canShoot = -1.0f;    //Var to hold new time to cross-check fire rate

    // Start is called before the first frame update
    void Start()
    {
        transform.position = (new Vector3(Random.Range(-9.0f, 9.0f), _spawnY, 0f));                 //RandomX spawn location

        _player = GameObject.Find("Player").GetComponent<Player>();                                 //<Player> ref
        if (_player == null) { Debug.LogError("_player is NULL"); }                                 //<Player> null-check
        
        _enemyAnimator = GetComponent<Animator>();                                                  //<Animator> ref
        if (_enemyAnimator == null) { Debug.LogError("_enemyAnimator is NULL"); }                   //<Animator> null-check

        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();               //<SoundFX> ref
        if (_explosionSoundFX == null) { Debug.LogError("_explosionSoundFX is NULL"); }             //<SoundFX> null-check
    }

    //Update
    //Movement Enemy
    //Enemy shoot-check
    void Update()
    {
        transform.Translate(Vector3.down * _speedEnemy * Time.deltaTime);   //Move Down
        if (transform.position.y < -6.0f)                                   //If off bottom screen...
        {
            float randomX = Random.Range(-9.0f, 9.0f);                      //...then get a randomX...
            transform.position = (new Vector3(randomX, _spawnY, 0.0f));     //...then move to random pos at top
        }
        
        //Enemy fire rate check
        if (Time.time > _canShoot)                                          
        {
            EnemyShoot();
        }

    }

    //SHOOT ENEMY LASER
    private void EnemyShoot()
    {
        _fireRate = Random.Range(3.0f, 7.0f);   //Set random fire rate
        _canShoot = Time.time + _fireRate;      //Set (_canShoot) to time plus (_fireRate) = 1 less than fire rete (?)

        //Debug.Log("Enemy can shoot time: (" + _canShoot + ") <> Fire Rate: ( " + _fireRate + ")");
        
        if (_speedEnemy > 0)  //Prevents dead enemy from shooting that is still in animation
        {
            //Instantiate Enemy Laser
            if (_player)                            //If [Player] exist...then spawn laser  **Prevent: crash when player dies and spawn laser tries to create <Player> ref
            {
                Debug.Log("Enemy Laser Shot");
                GameObject enemyLaser = Instantiate(_laserEnemyPrefab, new Vector3(transform.position.x, transform.position.y - 0.73f, transform.position.z), Quaternion.identity);
            }
        }
        //enemyLaser.transform.Translate(Vector3.down * 8.0f * Time.deltaTime);
        
    }

    //On Collision
    //OnTriggerEnter2D(Collider2D ~other object in collision)
    //When Enemy hits Player, damage player, destroy self
    //When Enemy hits Laser, destroy both obj, add score
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(other.CompareTag("Player"))  //Enemy hits player
        if(other.tag == "Player")
        {
            Debug.Log("Enemy Hit Player!");
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            //trigger anim
            //Destroy(this.gameObject);
            DestroyEnemy();
        }

        //if(other.CompareTag("Laser"))   //Laser hits enemy
        if (other.tag == "Laser")
        {
            Debug.Log("Laser Hit Enemy!");
            if(other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                //transform.position = (new Vector3(Random.Range(-9.0f, 9.0f), _spawnY, 0f));   //**NOT IN USE Respawn enemy instead of destroying
                DestroyEnemy();
            }
        }

        //if(other.CompareTag("Mine"))  //Mine hits enemy
        if (other.tag == "Mine") 
        {
            Debug.Log("Enemy hit mine!!");
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(15);
            }
            DestroyEnemy();
            //Invoke("DestroyEnemy", 1.0f);
        }
    }

    //Play Explosion Animation
    //Stop Movement *Prevent: enemy from multiple hits on player while explosion is moving, but still fails due to player moving up&down
    //              *Outcome: explosion from flying into player after being shot, but will damage player if player runs into explosion
    //              *Solution: Remove enemy collider
    //Play Exposion soundFX
    //Destroy after animation
    public void DestroyEnemy()
    {
        Debug.Log("Enemy Destroyed!");
        
        this._enemyAnimator.SetTrigger("OnEnemyDeath"); //Play anim
        _speedEnemy = 0;                                //Stop movement
        _explosionSoundFX.PlayExplosionSFX();           //Play soundFx
        Destroy(GetComponent<Collider2D>());            //Destroy collider on enemy
        Destroy(this.gameObject, 2.8f);                 //Destroy self
    }
}
