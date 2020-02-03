using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speedTripleShot = 3;     //Movement Speed (Applies to all powerups)
    //ID for Powerups
    [SerializeField]
    private int _powerupID;                 //Array of powerups //0 = Triple Shot,  1= Speed,  2 = Shields

    
    //private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClipPowerup;    //<AudioClip> ref

    /*
     **NOT IN USE
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null) { Debug.LogError("_audioSource Powerup is NULL"); }
    }
    */

    //UPDATE
    //Move powerup down until off screen-bottom, then destroy self
    void Update()
    {

        transform.Translate(Vector3.down * _speedTripleShot * Time.deltaTime);
        if(transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    //On Collision
    //OnTriggerEnter2D(Collider2D ~other object in collision)
    //If powerup hit player then set active, then destroy self
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")                                              //if collision with player...
        {
            //communicate player
            Debug.Log("Powerup Hit Player!");
            Player player = other.transform.GetComponent<Player>();             //Set <Player> ref to call <Player>()
            AudioSource.PlayClipAtPoint(_audioClipPowerup, transform.position); //Play sound at time of collision, at position
            if (player != null)                                                 //Player null-check
            {
                switch (_powerupID)                                             //Use powerupID to determine which powerup to call
                {
                    case 0:
                        Debug.Log("Triple Shot Collected");
                        player.TripleShotActive();
                        break;

                    case 1:
                        Debug.Log("Speed Collected");
                        player.SpeedBoostActive();
                        //call speed boost active
                        break;

                    case 2:
                        Debug.Log("Shield Collected");
                        player.ShieldPowerupActive();
                        //call shield active
                        break;

                    default:
                        Debug.Log("Unknown Powerup");
                        break;
                }
            }
            
            //other.transform.GetComponent<Player>().TripleShotActive();  //then Set _isTripleShotActive = true
            Destroy(this.gameObject);   //Destroy self after pickup
        }
    }
}
