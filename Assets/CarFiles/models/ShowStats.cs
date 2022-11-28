using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class ShowStats : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI MaxSpeedUI, AccelUI, BreakingUI, TractionUI, MassUI;
    public string MaxSpeedNumber, AccelNumber, BreakingNumber, TractionText, MassNumber;

    public GameObject MaxSpeed, Accel, Breaking, Traction, Mass;

    void Start()
    {
        MaxSpeed = GameObject.Find("MaxSpeedNumber");
        MaxSpeedUI = MaxSpeed.GetComponent<TextMeshProUGUI>();
        Debug.Log(MaxSpeedUI.text);

        Accel = GameObject.Find("AccelNumber");
        AccelUI = Accel.GetComponent<TextMeshProUGUI>();
        Debug.Log(AccelUI.text);

        Breaking = GameObject.Find("BreakingNumber");
        BreakingUI = Breaking.GetComponent<TextMeshProUGUI>();
        Debug.Log(BreakingUI.text);

        Traction = GameObject.Find("TractionText");
        TractionUI = Traction.GetComponent<TextMeshProUGUI>();
        Debug.Log(TractionUI.text);

        Mass = GameObject.Find("MassNumber");
        MassUI = Mass.GetComponent<TextMeshProUGUI>();
        Debug.Log(MassUI.text);

        MaxSpeedUI.text = MaxSpeedNumber + " km/h";
        AccelUI.text = "0 to 100 km/h in " + AccelNumber + "/sec"; ;
        BreakingUI.text = "100 to 0 km/h in " + BreakingNumber + "/sec";
        TractionUI.text = TractionText;
        MassUI.text = MassNumber + " kg";
        
    }



    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
