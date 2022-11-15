using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using static CarController;

public class audioManager : MonoBehaviour
{
    public float audioPitch;
    public float audioPitchFormula;
    public float standardAudioPitch = 0.7f;
    public float minAudioPitch = 0.01f;
    public float maxAudioPitch = 2;
    AudioSource gearShiftSource;
    AudioSource audioSourceExtra;
    public float pitchMultiplier;
    public float ccVerticalInput;
    public bool isBreaking;
    public GameObject gasPedal;
    AudioSource gasPedalSource;

    void Start()
    {
        gearShiftSource = GetComponent<AudioSource>();
        gasPedal = GameObject.Find("gasPedal");
        gasPedalSource = gasPedal.GetComponent<AudioSource>();
        Debug.Log(gearShiftSource);
    }

    void Update()
    {
        pitchMultiplier = CarController.cc.pitchMultiplier;
        ccVerticalInput = CarController.cc.verticalInput;
        isBreaking = CarController.cc.isBreaking;
        audioPitchFormula = pitchMultiplier / audioPitch;
        //gasPedalSource.pitch = 0.7f;
        if (audioPitch <= minAudioPitch)
            audioPitch = minAudioPitch;

        if (isBreaking == true)
        {
            gearShiftSource.pitch = 0.7f;
        }
        else
        {
            if (audioPitch <= maxAudioPitch && audioPitch >= minAudioPitch)
            {
                //zorg er voor dat de audiopitch minder snel omhoog gaat als je versnelling hoger is.

                audioPitch = standardAudioPitch + audioPitchFormula;
                gearShiftSource.pitch = audioPitch;
            }
            if (ccVerticalInput >= 0)
            {
                gasPedalSource.pitch = ccVerticalInput;
                gasPedalSource.volume = ccVerticalInput;
            }
        }
    }
}


