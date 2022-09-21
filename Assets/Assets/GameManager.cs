using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarController;

public class GameManager : MonoBehaviour
{
    public Transform spawnpoint;
    public GameObject car;
    public GameObject spawnedCar;
    public float penalty;

    public CarController CC;
    public GameObject needle;
    private float startPos = 220f, endPos = -40;
    private float desiredPos;
    public float vehicleSpeed;

    // Start is called before the first frame update
    void Start()
    {
      spawnedCar = Instantiate(car, spawnpoint.position, spawnpoint.rotation);
        spawnedCar.name = "ROCMcar";
       CC = spawnedCar.GetComponent<CarController>();
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
