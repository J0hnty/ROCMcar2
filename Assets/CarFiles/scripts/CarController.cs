using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    public float horizontalInput;
    public float verticalInput;
    private float horizontalForce;
    private float currentSteerAngle;
    private float currentBreakForce;
    public bool isBreaking;
    public float currentSpeed;
    public Transform spawnpoint;
    public float maxSpeed;
    public int gearShift = 1;
    public GameObject car;
    public Rigidbody rb;
    public bool isRespawning;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider FLWCollider;
    [SerializeField] private WheelCollider FRWCollider;
    [SerializeField] private WheelCollider RLWCollider;
    [SerializeField] private WheelCollider RRWCollider;
    
    [SerializeField] private Transform FLWTransform;
    [SerializeField] private Transform FRWTransform;
    [SerializeField] private Transform RLWTransform;
    [SerializeField] private Transform RRWTransform;




    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
        if (Input.GetKey(KeyCode.T))
        {
            Teleport();
        }




    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        if (!isRespawning)
        {
            isBreaking = Input.GetKey(KeyCode.Space);
        }

    }

    private void HandleMotor()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.magnitude * 3.6f;
       



        if (currentSpeed >= 0f && currentSpeed < maxSpeed)
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
           // RLWCollider.motorTorque = verticalInput * motorForce;
           // RRWCollider.motorTorque = verticalInput * motorForce;
            gearShift = 1;
        }
        else if (currentSpeed >= 20f && currentSpeed < maxSpeed)
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
         //   RLWCollider.motorTorque = verticalInput * motorForce;
          //  RRWCollider.motorTorque = verticalInput * motorForce;
            gearShift = 2;
        }
        else if (currentSpeed >= 40f && currentSpeed < maxSpeed)
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
          //  RLWCollider.motorTorque = verticalInput * motorForce;
           // RRWCollider.motorTorque = verticalInput * motorForce;
            gearShift = 3;
        }
        else if (currentSpeed >= 60f && currentSpeed < maxSpeed)
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
          //  RLWCollider.motorTorque = verticalInput * motorForce;
          // RRWCollider.motorTorque = verticalInput * motorForce;
            gearShift = 4;
        }
        
        else if (currentSpeed >= 80f && currentSpeed < maxSpeed)
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
           // RLWCollider.motorTorque = verticalInput * motorForce;
           // RRWCollider.motorTorque = verticalInput * motorForce;
            gearShift = 5;
        }
        else if (currentSpeed >= maxSpeed)
        {
            FLWCollider.motorTorque = 0f;
            FRWCollider.motorTorque = 0f;
           // RLWCollider.motorTorque = 0f;
          //  RRWCollider.motorTorque = 0f;
        }
        else
        {
            FRWCollider.motorTorque = verticalInput * motorForce;
            FLWCollider.motorTorque = verticalInput * motorForce;
          //  RLWCollider.motorTorque = verticalInput * motorForce;
           // RRWCollider.motorTorque = verticalInput * motorForce;
        }
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
        else
        {
            UnApplyBreaking();
        }
    }

    private void ApplyBreaking()
    {
        FRWCollider.brakeTorque = currentBreakForce;
        FLWCollider.brakeTorque = currentBreakForce;
        RRWCollider.brakeTorque = currentBreakForce;
        RLWCollider.brakeTorque = currentBreakForce;
    }
    private void UnApplyBreaking()
    {
        FRWCollider.brakeTorque = 0f;
        FLWCollider.brakeTorque = 0f;
        RRWCollider.brakeTorque = 0f;
        RLWCollider.brakeTorque = 0f;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        FLWCollider.steerAngle = currentSteerAngle;
        FRWCollider.steerAngle = currentSteerAngle;

    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWCollider, FLWTransform);
        UpdateSingleWheel(FRWCollider, FRWTransform);
        UpdateSingleWheel(RLWCollider, RLWTransform);
        UpdateSingleWheel(RRWCollider, RRWTransform);
    }

    private void UpdateSingleWheel(WheelCollider WCollider, Transform WTransform)
    {
        Vector3 pos;
        Quaternion rot;
        WCollider.GetWorldPose(out pos, out rot);
        WTransform.rotation = rot;
        WTransform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("WorldEnd"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {

            rb = GetComponent<Rigidbody>();
            rb.Sleep();
            isRespawning = true;
            isBreaking = true;


            transform.position = spawnpoint.position;
            transform.rotation = spawnpoint.rotation;
            StartCoroutine(waiter());
            IEnumerator waiter()
            {
                //Wait for 4 seconds
                yield return new WaitForSeconds(3);
                isBreaking = false;
                isRespawning = false;

            }

     }

    private void Teleport()
    {



        transform.position = spawnpoint.position;
        transform.rotation = spawnpoint.rotation;


    }



}

