using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //CB: New_Game_Button @MainMenu Scene
    public void LoadGame()
    {
        SceneManager.LoadScene(1);  //Load Scene 1 = "Game"
    }
}
