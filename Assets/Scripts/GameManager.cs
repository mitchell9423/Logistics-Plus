using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameData gameData;
    [SerializeField] public AppVideoSettings videoSettings;

	private void Awake()
	{
        if (Instance == null) Instance = this;
        LoadGameData();
    }

    void LoadGameData()
    {
        gameData = JSONManager.Instance.LoadData<GameData>();
        if (gameData == null) gameData = new GameData();
    }

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
