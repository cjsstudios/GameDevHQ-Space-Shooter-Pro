using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;    //Enemy prefab //Set @Inspector
    [SerializeField]
    private GameObject _enemyContainer; //Holds enemies under empty parent //Set @Inspector 

    [SerializeField]
    private GameObject[] _powerups;     //Array of powerups //Set @Inspector

    private bool _stopSpawning = false;             //Enemy spawn toggle

    //Start Spawning
    public void StartSpawning()
    {
        //StartCoroutine("SpawnRoutine");
        StartCoroutine(SpawnEnemyRoutine());        //Spawn enemy
        StartCoroutine(SpawnPowerupRoutine());      //Spawn powerup
    }

    //SPAWN ENEMY
    //CB: StartSpawning()
    //Loop until player is out of lives
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);  //Spawn Delay

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);                              //Spawn enemy random pos off screen-top
            UnityEngine.GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);   //Instantiate & create ref of new enemy
            newEnemy.transform.parent = _enemyContainer.transform;                                          //Move enemy to parent (container)
            yield return new WaitForSeconds(5.0f);
        }
        Debug.Log("Enemy Spawn Stopped!");                                                                  //Player has died, spawning stopped
        Destroy(GameObject.FindGameObjectWithTag("Enemy"), 2.0f);   //MOVE ME
    }

    //SPAWN RANDOM POWERUP
    //CB: StartSpawning()
    //Loop until player is out of lives
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);  //Spawn Delay

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);          //Spawn powerup random pos off screen-top
            int randomPowerUp = Random.Range(0, _powerups.Length);                       //Set random powerup (0-4) (5 power ups)
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);     //Create powerup
            yield return new WaitForSeconds(Random.Range(3, 8));                        //Spawn new powerup at random time
        }
        Debug.Log("Powerup Spawn Stopped!");                                            //Player has died, spawning stopped
    }

    //Player is dead, lives(0)
    //Stop spawning enemies and powerups
    public void OnPlayerDeath()
    {
        _stopSpawning = true;               //Set stopSpawning toggle
        Debug.Log("Player is out of lives");
    }
}
