using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RememberCar : MonoBehaviour
{
    public static RememberCar rememberCar;
    public int CarUsed = 0;

    bool gameStart;
    // Start is called before the first frame update
    void Awake()
    {
        if (!gameStart)
        {
            rememberCar = this;
            SceneManager.LoadSceneAsync("Citymap", LoadSceneMode.Additive);
            gameStart = true;
        }
    }

    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    IEnumerator Unload(int scene)
    {
        yield return null;
        SceneManager.UnloadScene(scene);
    }
} 
