using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    private SoundFX _explosionSoundFX;  //<SoundFX> ref
    private static int _minesInSpace;   //Count of mines that drifted off screen
    public Vector3 mineLaunchDir;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SPAWNING MINE");
        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();               //<SoundFX> ref
        if (_explosionSoundFX == null) { Debug.LogError("_explosionSoundFX is NULL"); }             //<SoundFX> null-check
        mineLaunchDir = new Vector3();
        int rndNum = Random.Range(0, 3);
        switch (rndNum)
        {
            case 0: mineLaunchDir = Vector3.up; Debug.Log("UP mineLaunchDir Chosen");
                break;
            case 1: mineLaunchDir = new Vector3(-0.15f, 0.5f, 0f); Debug.Log("LEFT mineLaunchDir Chosen");
                break;
            case 2: mineLaunchDir = new Vector3(0.15f, 0.5f, 0f); Debug.Log("RIGHT mineLaunchDir Chosen");
                break;
            default: mineLaunchDir = Vector3.up; Debug.Log("Default mineLaunchDir Chosen");
                break;
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(mineLaunchDir * 3 * Time.deltaTime);
        if (transform.position.y > 8.0f)                                //If laser off screen-top, destroy parent, then self
        {
            //if object has parent
            //destroy parent
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            _minesInSpace++;
            Destroy(this.gameObject);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Mine hit Enemy!!!");
            Invoke("DestroyMineFire", 0.25f);
        }
    }
    */
    private void GetRandomSpawnDir()
    {

    }
    private void OnDestroy()
    {
        _explosionSoundFX.PlayExplosionSFX();           //Play soundFx
        Destroy(GetComponent<CircleCollider2D>());      //Destroy collider on mine
        Destroy(this.gameObject, 2.8f);                 //Destroy self  
    }

    void DestroyMineFire()
    {

    }
}
