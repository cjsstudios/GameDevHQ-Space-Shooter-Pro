using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    //private Animator _astroidAnim;

    private SpawnManager _spawnManager;
    private SoundFX _explosionSoundFX;

    //Start
    //Get ref to <SpawnManager> & <SoundFX>
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) { Debug.LogError("_spawnManager is NULL"); }

        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();
        if (_explosionSoundFX == null) { Debug.LogError("_explosionSoundFX is NULL"); }
    }

    //Update
    //Rotate astroid for visual fx
    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    //When player shoots astroid start spawning enemies and powerups
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity); //Blow up Astroid
            Destroy(other.gameObject);                                              //Destroy Laser
            _spawnManager.StartSpawning();                                          //Start Enemy & Powerup Spawning
            Debug.Log("Spawning");
            _explosionSoundFX.PlayExplosionSFX();                                   //Play Explosion sound
            Destroy(this.gameObject, 0.25f);                                        //Destroy self after time for animation/sound to finish
        }
    }
}
