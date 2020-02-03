using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;   //Toggle

    //CB: <UIManager> @GameOverFXRoutine()
    public void GameOver()
    {
        _isGameOver = true;
    }

    //UPDATE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true) //Restart game if (isGameOver) toggle true
        {
            SceneManager.LoadScene(1);                          //Load Scene 1 = "Game"
        }

        if (Input.GetKeyDown(KeyCode.Escape))                   //Quit Application at any time
        {
            Application.Quit();
        }
    }
}
