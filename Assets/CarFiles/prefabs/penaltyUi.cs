using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using TMPro;

public class penaltyUi : MonoBehaviour
{

    public TextMeshProUGUI penaltyText;
    public GameManager gameManager;
    public GameObject GMO;

    // Start is called before the first frame update
    void Start()
    {
        GMO = GameObject.Find("GameManager");
        gameManager = GMO.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        penaltyText.text = gameManager.penalty.ToString() + "Penaltys";
    }
}
