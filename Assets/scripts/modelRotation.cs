using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelRotation : MonoBehaviour
{ 
    public Vector3 objectRotation;
    void Update()
    {
        transform.Rotate(objectRotation);
    }
}
