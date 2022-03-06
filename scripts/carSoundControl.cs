using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSoundControl : MonoBehaviour
{

    AudioSource audioSource;

    public AudioClip idle;
    Rigidbody rb;

    bool isMoving = false;
    //bool isBraking = false;

    public float minVolume = 0.1f;

    public WheelCollider wheelColFR;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        audioSource.volume = minVolume;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = idle;
        audioSource.Play();

    }

    // Update is called once per frame
    void Update()
    {

        if(wheelColFR.motorTorque != 0)
        {
            isMoving = true;
        }else{
            isMoving = false;
        }

        //if(wheelColFR.brakeTorque != 0)
        //{
        //    isBraking = true;
        //}else{
        //    isBraking = false;
        //}

        if(isMoving == true)
        {
            audioSource.volume = 0.3f;
        }

    }
}
