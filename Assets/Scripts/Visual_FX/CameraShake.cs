using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _machCamera;

    private void Start()
    {
        _machCamera = gameObject.GetComponent<Animator>();
    }

    public void ShakeCamera()
    {
        _machCamera.SetTrigger("PlayerHit");
    }    
}
