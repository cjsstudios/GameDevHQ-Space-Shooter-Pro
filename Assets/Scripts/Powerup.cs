using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    /// <summary>
    /// To create a new powerup:
    /// 1)Create prefab, pick a powerup add to scene then unpack 
    /// 2)Select obj and change name
    /// 3)Change (Powerup ID) in inspector to powerups total count -1)
    /// 4)Assign sprite, add last one twice *doesn't show last img*
    /// 5)Change (Controller) in <Animator> component to 'None'
    /// 6)Create Animation, Save, DragNDrop images
    /// 7)Create a prefab with obj with default transform settings
    /// 8)Remove instance from hierarchy
    /// 9)
    /// 10)Add to <SpawnManager> Array @inspector, Drag prefab in Powerups-Array
    /// 11)Create Active & Cooldown methods for powerup in <Player></Player>
    /// 12)Create prefab ref GameObject var in <Player> then add to component
    /// 13)Add prefab name to <Powerup> this script @OnTriggerEnter2D->switch [case = powerup count - 1]
    /// 14)Add player.PowerUpActive @ switch call in <Powerup> @switch to call active when picked up
    /// 15)Add to <Player> on [Player] dragNdrop prefab
    /// </summary>
    [SerializeField]
    private float _speedTripleShot = 3;     //Movement Speed (Applies to all powerups)
    //ID for Powerups
    [SerializeField]
    private int _powerupID;                 //Array of powerups //0 = Triple Shot,  1= Speed,  2 = Shields

    
    //private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClipPowerup;    //<AudioClip> ref

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

                    case 3:
                        Debug.Log("Ammo Laser Collected");
                        player.AmmoPowerUp();
                        break;

                    case 4: Debug.Log("Health Collected");
                        player.HealthPowerUp();
                        break;

                    case 5: Debug.Log("Space Mine Collected");
                        player.SpaceMineActive();
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
