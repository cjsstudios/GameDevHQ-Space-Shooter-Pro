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
    //0-Triple Shot  1-Speed  2-Shield  3-Ammo Reload(+1)  4-Health  5-Space Mine
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);  //Spawn Delay

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7, 0);          //Spawn powerup random pos off screen-top
            int randomPowerUp = 0;                                                    //Set random powerup to 100 as default **Modified to Spawn%
            //Powerup Spawn Frequency
            int rnd = Random.Range(1, 101);
            if (rnd < 100) { randomPowerUp = 1; }       //Speed  25% Spawn Chance
            if (rnd < 76) { randomPowerUp = 2; }        //Shield 20% Spawn Chance
            if (rnd < 56) { randomPowerUp = 3; }        //Ammo 20% Spawn Chance
            if (rnd < 36) { randomPowerUp = 4; }        //Health 15% Spawn Chance
            if (rnd < 21) { randomPowerUp = 5; }        //Mines 5% Spawn Chance
            if (rnd < 16) { randomPowerUp = 0; }        //3-Shot 15% Spawn Chance
            Debug.Log("Random Powerup Spawn is " + rnd.ToString());
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);     //Create powerup
            yield return new WaitForSeconds(Random.Range(5, 10));                        //Spawn new powerup at random time
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
