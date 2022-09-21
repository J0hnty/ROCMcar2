using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedMeter : MonoBehaviour
{
    public bool trackingSpeed;
    public GameManager gameManager;
    public GameObject GMO;
    public bool penaltyGiven;
    // Start is called before the first frame update
    void Start()
    {
        GMO = GameObject.Find("GameManager");
        gameManager = GMO.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingSpeed)
        {
            if (gameManager.CC.currentSpeed >= 120f)
            {
                if(!penaltyGiven)
                gameManager.penalty += 1f;
                penaltyGiven = true;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trackingSpeed= true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trackingSpeed = false;
            penaltyGiven=false;

        }
    }
}

