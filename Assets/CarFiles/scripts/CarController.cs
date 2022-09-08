using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    public float currentSpeed;
    public float maxSpeed;
    public int gearShift = 1;
    public bool gearShiftInput;
    public bool isBreaking;
    public Rigidbody rb;
    public GameObject car;
    public Transform spawnpoint;
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
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        if (!isRespawning)
        {
            isBreaking = Input.GetKey(KeyCode.Space);

        }
        StartCoroutine(waiter());
        IEnumerator waiter()
        {
            //Wait for 4 seconds
            if (gearShiftInput = Input.GetKeyDown(KeyCode.E))
            {
                gearShift++;
                yield return new WaitForSeconds(3);
            }
            else if (gearShiftInput = Input.GetKeyDown(KeyCode.Q))
            {
                gearShift--;
                yield return new WaitForSeconds(3);
            }

        }
        /*if (gearShiftInput = Input.GetKeyDown(KeyCode.E))
        {
            gearShift++;

        } else if (gearShiftInput = Input.GetKeyDown(KeyCode.Q))
        {
            gearShift--;

        }*/
    }

    private void HandleMotor()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.magnitude * 3.6f;
        

        switch (gearShift) {
            case 0:
                motorForce = 0;

                break;
            case 1:
                motorForce = 100;


                break;
            case 2:
                motorForce = 200;


                break;
            case 3:
                motorForce = 300;


                break;
            case 4:
                motorForce = 400;


                break;
            case 5:
                motorForce = 500;


                break;
            default:
                if (gearShift<0)
                {
                    gearShift = 0;
                }
                if (gearShift > 5)
                {
                    gearShift = 5;
                }
                break;
        }

        FRWCollider.motorTorque = verticalInput * motorForce;
        FLWCollider.motorTorque = verticalInput * motorForce;

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
    }

