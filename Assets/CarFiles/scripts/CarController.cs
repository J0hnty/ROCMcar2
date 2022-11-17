using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GameManager;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";

    public float horizontalInput;
    public float verticalInput;

    private float currentSteerAngle;
    private float currentBreakForce;
    public bool isBreaking;
    public float currentSpeed;
    public Transform spawnpoint;
    public int gearShift = 0;
    public int oldGearShift = 0;
    public float gearShiftInput;
    public GameObject car;
    public Rigidbody rb;
    public bool isRespawning;
    //car stats
    public float maxSpeed = 0;
    public float maxGear = 0;
    public float motorForceMultiplier = 0;
    //=========
    public float engineRPM;
    public float maxRPM;
    public bool test; 
    

    public float pitchMultiplier = 0;


    public static CarController cc;

    public GameManager gameManager;
    public GameObject GMO;

    //Menu 
    public bool carSwitchMenu = false;

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

    private void Start()
    {
        //!!!
        cc = this;

        GMO = GameObject.Find("GameManager");
        gameManager = GMO.GetComponent<GameManager>();
    }

    // hier worden alle gebruikte functie aan geroepen
    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        GearShift();
        HandleSteering();
        UpdateWheels();
        if (Input.GetKey(KeyCode.R))
            Respawn();
        if (Input.GetKey(KeyCode.T))
            Teleport();
    }


    //hier worden alle inputs gevraagt
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        if (!isRespawning && !carSwitchMenu)
            isBreaking = Input.GetKey(KeyCode.Space);
    }
    private void GearChangeCheck(int gearShiftHold)
    {
        if (currentSpeed <= 5 && gearShift >= 3 && Input.GetKey(KeyCode.W))
        {
            gameManager.warningUI.text = "gearshift too high to drive away";
            motorForce = 0;
            StartCoroutine(warningTimer());
            IEnumerator warningTimer()
            {
                //Wait for 5 seconds
                yield return new WaitForSeconds(5);
                gameManager.warningUI.text = "";
            }
        }

        if (gearShiftHold - oldGearShift == 1)
        {
            oldGearShift++;
            Debug.Log("opschakelen");
            ChangeMotorForce();
        }
        else if (
         gearShiftHold - oldGearShift == -1 ||
         gearShiftHold - oldGearShift == -2 ||
         gearShiftHold - oldGearShift == -3 ||
         gearShiftHold - oldGearShift == -4 ||
         gearShiftHold - oldGearShift == -5)
        {
            oldGearShift--;
            Debug.Log("terug schakelen");
            ChangeMotorForce();
        }
        else if (gearShiftHold - oldGearShift >= 2)
        {
            Debug.Log("slechte schakel");
            gearShift = 0;
            oldGearShift = 0;
            ChangeMotorForce();
        }
    }
    private void ChangeMotorForce()
    {
        Debug.Log("motor force");
        // Debug.Log(gearShift);
        //deze switch zorgt er voor dat de motorForce meegaat met de versnelling.
        switch (gearShift)
        {
            case 0:
                motorForce = 0;
                pitchMultiplier = 0f;
                Debug.Log("gear 0");
                break;
            case 1:
                motorForce = 200;
                pitchMultiplier = 0.1f;
                Debug.Log("gear 1");
                break;
            case 2:
                motorForce = 300;
                pitchMultiplier = 0.2f;
                Debug.Log("gear 2");
                break;
            case 3:
                motorForce = 400;
                pitchMultiplier = 0.3f;
                Debug.Log("gear 3");
                break;
            case 4:
                motorForce = 550;
                pitchMultiplier = 0.4f;
                Debug.Log("gear 4");
                break;
            case 5:
                motorForce = 600;
                pitchMultiplier = 0.5f;
                Debug.Log("gear 5");
                break;
            case 6:
                motorForce = 700;
                pitchMultiplier = 0.6f;
                Debug.Log("gear 6");
                break;
            default:
                if (gearShift < 0 || gearShift > 5)
                gearShift = 0;
                break;
        }
    }
    // deze functie zorgt er voor dat de gearshift wordt gegeven.
    private void GearShift()
    {
        GearChangeCheck(gearShift);
        //GearInputs();
        // deze if statements moeten er voor zorgen dat-
        // de auto in een versnelling gaat.
        if (Input.GetKey(KeyCode.Alpha1))
        {
            gearShift = 1;
            GearChangeCheck(gearShift);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            gearShift = 2;
            if (currentSpeed < 15)
                gearShift = 0;
            GearChangeCheck(gearShift);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            gearShift = 3;
            if (currentSpeed < 30)
                gearShift = 0;
            GearChangeCheck(gearShift);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            gearShift = 4;
            if (currentSpeed < 70)
                gearShift = 0;
            GearChangeCheck(gearShift);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            gearShift = 5;
            if (currentSpeed < 100)
                gearShift = 0;
            GearChangeCheck(gearShift);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            gearShift = 6;
            if (currentSpeed < 150)
                gearShift = 0;
            GearChangeCheck(gearShift);
        }
    }
    // deze functie zorgt voor alles wat met de motor te maken heeft.
    private void HandleMotor()
    {
        // hier komt de rigidbody 'auto' binnen
        // en wordt current speed omgezet van meter/seconde naar km/u
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.magnitude * 3.6f;

        // deze formules zorgen er voor dat de auto rijd
        FRWCollider.motorTorque = verticalInput * motorForce;
        FLWCollider.motorTorque = verticalInput * motorForce;

        // dit zorgt er voor dat je gaat remmen
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
    // deze functie zorgt er voor dat je kunt remmen.
    private void ApplyBreaking()
    {
        FRWCollider.brakeTorque = currentBreakForce;
        FLWCollider.brakeTorque = currentBreakForce;
        RRWCollider.brakeTorque = currentBreakForce;
        RLWCollider.brakeTorque = currentBreakForce;
    }
    // deze functie zorgt er voor dat je kunt stopen remmen.
    private void UnApplyBreaking()
    {
        FRWCollider.brakeTorque = 0f;
        FLWCollider.brakeTorque = 0f;
        RRWCollider.brakeTorque = 0f;
        RLWCollider.brakeTorque = 0f;
    }
    // deze functie zorgt er voor dat je kan sturen
    // door middel van formules.
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        FLWCollider.steerAngle = currentSteerAngle;
        FRWCollider.steerAngle = currentSteerAngle;

    }
    // deze functie zorgt er voor dat de wielen
    // mee gaan met de inputs van de user.
    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWCollider, FLWTransform);
        UpdateSingleWheel(FRWCollider, FRWTransform);
        UpdateSingleWheel(RLWCollider, RLWTransform);
        UpdateSingleWheel(RRWCollider, RRWTransform);
    }
    // deze functie zorgt er voor dat het wiel wat meegegeven wordt
    // mee gaat met de input van de user.
    private void UpdateSingleWheel(WheelCollider WCollider, Transform WTransform)
    {
        Vector3 pos;
        Quaternion rot;
        WCollider.GetWorldPose(out pos, out rot);
        WTransform.rotation = rot;
        WTransform.position = pos;
    }
    // deze functie zorgt er voor dat als voorbeeld:
    // jij van de map valt dat je weer in de map terecht komt.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Building") || collision.collider.CompareTag("Fences"))
        {
            FindObjectOfType<backGroundManager>().PlaySound("Toeter");
        }

        if (collision.collider.CompareTag("WorldEnd"))
        {
            FindObjectOfType<backGroundManager>().PlaySound("Toeter");
            Respawn();
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarSwitch"))
        {
            carSwitchMenu = true;
            rb.Sleep();
            isBreaking = true;
            
        }
    }
        // zorgt ervoor dat je respawnd zonder snelheid en dergelijken.
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
                //Wait for 3 seconds
                yield return new WaitForSeconds(3);
                isBreaking = false;
                isRespawning = false;
            }


    }
    // teleport je zonder aanpassingen aan te brengen.
    private void Teleport()
    {

        transform.position = spawnpoint.position;
        transform.rotation = spawnpoint.rotation;


    }
}

