using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform spawnpoint;
    public GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(car, spawnpoint.position, spawnpoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
