using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using static CarController;
using static RememberCar;

public class GameManager : MonoBehaviour
{
    public Transform spawnpoint;
    public GameObject[] car;
    public GameObject spawnedCar;
    public bool isSpawned;
    public float penalty;

    // remember Car Script
    public RememberCar RCS;
    // remember Car
    public GameObject RC;


    public TextMeshProUGUI warningUI;
    public CarController CC;
    public GameObject needle;
    private float startPos = 220f, endPos = -40;
    private float desiredPos;
    public float vehicleSpeed;

    public GameObject mirrorScreen;

    // Start is called before the first frame update
    void Start()
    {
        RC = GameObject.Find("RememberCar");
        RCS = RC.GetComponent<RememberCar>();

        spawnedCar = Instantiate(car[RCS.CarUsed], spawnpoint.position, spawnpoint.rotation);
        spawnedCar.name = "ROCMcar";
        CC = spawnedCar.GetComponent<CarController>();
        warningUI.text = "";
    }

    private void FixedUpdate()
    {
        
        vehicleSpeed = CC.currentSpeed;
        UpdateNeedle();
    }

    public void UpdateNeedle()
    {
        desiredPos = startPos - endPos;
        float temp = vehicleSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPos - temp * desiredPos));

    }

}
