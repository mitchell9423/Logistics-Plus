using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGridUI : MonoBehaviour
{
    public static LocationGridUI Instance { get; private set; }
    List<Vector3> oreLocations;

    public System.Action<Vector3> AddLocation;

	private void Awake()
	{
        if (Instance == null) Instance = this;
	}

	void Start()
    {
        oreLocations = GameManager.gameData.oreLocations;
        gameObject.SetActive(false);
    }
}
