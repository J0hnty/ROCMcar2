using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class StopDetection : MonoBehaviour
{
    public bool isInStopZone;
    public GameManager gameManager;
    public GameObject GMO;
    public bool didntStopYet;
    // Start is called before the first frame update
    void Start()
    {
        GMO = GameObject.Find("GameManager");
        gameManager = GMO.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInStopZone)
        {
            if (gameManager.CC.currentSpeed <= 10f)
            {
                didntStopYet = false;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
           isInStopZone = true;
            didntStopYet = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            isInStopZone = false;
            if (didntStopYet)
            {
                gameManager.penalty += 1f;
            }
        }
    }
}

