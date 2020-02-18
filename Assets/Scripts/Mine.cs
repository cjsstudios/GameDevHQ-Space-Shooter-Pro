using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    private SoundFX _explosionSoundFX;  //<SoundFX> ref

    // Start is called before the first frame update
    void Start()
    {
        _explosionSoundFX = GameObject.Find("Explosion_SFX").GetComponent<SoundFX>();               //<SoundFX> ref
        if (_explosionSoundFX == null) { Debug.LogError("_explosionSoundFX is NULL"); }             //<SoundFX> null-check
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 3 * Time.deltaTime);
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
