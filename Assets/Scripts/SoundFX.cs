using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    private AudioSource _audioSourceExplosion;  //<AudioSource> ref 
    [SerializeField]
    private AudioClip _audioClipExplosion;      //<AudioClip> ref explosion

    //START
    private void Start()
    {
        _audioSourceExplosion = GetComponent<AudioSource>();                                        //Set <AudioSource> ref
        if (_audioSourceExplosion == null) { Debug.LogError("_audioSourceExplosion is NULL"); }     //<AudioSource> null-check
    }

    //SOUND FX: EXPLOSION
    public void PlayExplosionSFX()
    {
        if (_audioClipExplosion != null && _audioSourceExplosion != null)   //If <AudioClip> AND <AudioSource> not null
        {
            Debug.Log("Playing Explosion");
            _audioSourceExplosion.Play();       //Play soundfX
        }
    }
}
