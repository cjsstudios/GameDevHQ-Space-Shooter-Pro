using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private UnityEngine.GameObject _explosionPrefab;

    //private Animator _astroidAnim;

    private SpawnManager _spawnManager;
    private SoundFX _explosionSoundFX;

    //Start
    //Get ref to <SpawnManager> & <SoundFX>
    private void Start()
    {
        _spawnManager = UnityEngine.GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) { Debug.LogError("_spawnManager is NULL"); }

        _explosionSoundFX = UnityEngine.GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();
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
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Debug.Log("Spawning");
            _explosionSoundFX.PlayExplosionSFX();
            Destroy(this.gameObject, 0.25f);            
        }
    }
}
