using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic_PlayerShip_Intro : MonoBehaviour
{
    public GameObject _player;
    public GameObject _ui;

    public void EndOfIntroScene()
    {
        _player.SetActive(true);
        //Debug.Log("Player is Active!!!");
        _player.transform.position = transform.position;
        //Debug.Log("Player Pos Set From: " + transform.position + " To: " + _player.transform.position );
        _player.transform.localScale = transform.localScale;
        //Debug.Log("Player Scale Set From: " + transform.localScale + " To: " + _player.transform.localScale);
        //Debug.Log("Activate UI");
        _ui.SetActive(true);
        //Debug.Log("Destroying Cinematic");
        Destroy(this.gameObject);
    }
}
