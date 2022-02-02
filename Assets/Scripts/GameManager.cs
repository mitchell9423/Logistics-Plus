using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public AppVideoSettings videoSettings;

    void Start()
    {
        //videoSettings.ChangeDisplay();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
		{
            Application.Quit();
		}
    }
}
