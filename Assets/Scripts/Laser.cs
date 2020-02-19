using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    /// <summary>
    /// Attached to Laser prefab & Enemy Laser prefab
    /// </summary>
    [SerializeField]
    private float _speedLaser = 8.0f;   //Laser speed @Inspector

    Player _player;                     //<Player> ref
    CameraShake _shakeCamera;

    //START
    //Set <Player> ref then null-check
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) { Debug.LogError("_player is NULL"); }
        _shakeCamera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        Debug.Log("Created laser named: " +  gameObject.tag);
    }

    //On Collision
    //OnTriggerEnter2D(Collider2D ~obj laser collides with)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameObject.tag == "LaserEnemy")    //If other is Player & this laser is enemy laser...
        {
            Debug.Log("Enemy laser hit player!");
            _player.Damage();                                           //Damage Player
            _shakeCamera.ShakeCamera();
 
            if (this.transform.parent != null)                          //If enemy laser has parent...
            {
                Destroy(this.transform.parent.gameObject);              //Destroy parent
            }
            Destroy(this.gameObject);                                   //Destroy self
        }
    }

    //UPDATE
    //Control laser behavior based on enemy laser or player laser
    //Script is attaced to both
    void Update()
    {
        //Debug.Log("This Laser (" + gameObject.name + ")");
        if (gameObject.tag == "Laser")                                      //If player ship laser...
        {
            //Debug.Log("Player Laser CREATED");
            transform.Translate(Vector3.up * _speedLaser * Time.deltaTime); //Move laser up

            if (transform.position.y > 8.0f)                                //If laser off screen-top, destroy parent, then self
            {
                //if object has parent
                //destroy parent
                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }

        if (gameObject.tag == "LaserEnemy")
        {
            //Debug.Log("Enemy Laser CREATED");
            transform.Translate(Vector3.down * _speedLaser * Time.deltaTime);//Move laser down
            if (transform.position.y < -7.0f)                               //If enemy laser off screen-bottom, destroy parent, then self
            {
                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
