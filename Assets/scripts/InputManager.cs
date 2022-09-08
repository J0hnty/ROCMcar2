using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarController;
public class InputManager : MonoBehaviour
{
    public CarController RR;

    public GameObject needle;
    private float startPos = 220f, endPos = -40;
    private float desiredPos;

    public float vehicleSpeed;

    private void FixedUpdate()
    {

        vehicleSpeed = RR.currentSpeed;
        UpdateNeedle();
    }

    public void UpdateNeedle()
    {
        desiredPos = startPos - endPos;
        float temp = vehicleSpeed / 180;
        needle.transform.eulerAngles =  new Vector3(0,0,(startPos - temp * desiredPos));

    }
}
