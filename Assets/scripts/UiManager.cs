using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static CarController;
using static RememberCar;

public class UiManager : MonoBehaviour
{
    public CarController cc;
    public GameObject[] CarArray;
    public int carInArray;
    public Transform CarShowcase;
    // remember Car Script
    public RememberCar RCS;
    // remember Car
    public GameObject RC;



    // Start is called before the first frame update
    void Start()
    {
        /**  CarArray[0].SetActive(true);
          CarArray[1].SetActive(false);
          CarArray[2].SetActive(false);
          CarArray[3].SetActive(false);
          CarArray[4].SetActive(false);
        */

        RC = GameObject.Find("RememberCar");
        RCS = RC.GetComponent<RememberCar>();
        carInArray = RCS.CarUsed;
        GameObject childObject = Instantiate(CarArray[carInArray], CarShowcase.transform.position, CarShowcase.transform.rotation) as GameObject;
        childObject.transform.parent = CarShowcase.transform;
        
    }


    // Update is called once per frames
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            try {
                Destroy(CarShowcase.transform.GetChild(1).gameObject);
                carInArray++;
                GameObject childObject = Instantiate(CarArray[carInArray], CarShowcase.transform.position, CarShowcase.transform.rotation);
                childObject.transform.parent = CarShowcase.transform;

            }
            catch (IndexOutOfRangeException)
            {
                carInArray = 0;
                GameObject childObject = Instantiate(CarArray[carInArray], CarShowcase.transform.position, CarShowcase.transform.rotation);
                childObject.transform.parent = CarShowcase.transform;
            }  


        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            try
            {
                Destroy(CarShowcase.transform.GetChild(1).gameObject);
                carInArray--;
                GameObject childObject = Instantiate(CarArray[carInArray], CarShowcase.transform.position, CarShowcase.transform.rotation);
                childObject.transform.parent = CarShowcase.transform;

            }
            catch (IndexOutOfRangeException)
            {
                carInArray = 4;
                GameObject childObject = Instantiate(CarArray[carInArray], CarShowcase.transform.position, CarShowcase.transform.rotation);
                childObject.transform.parent = CarShowcase.transform;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RCS.CarUsed = carInArray;
            SceneManager.UnloadSceneAsync("CarSwitchScene");
            //gameManager.spawnedCar.SetActive(true);
            SceneManager.LoadSceneAsync("Citymap", LoadSceneMode.Additive);
        }

    }
}



