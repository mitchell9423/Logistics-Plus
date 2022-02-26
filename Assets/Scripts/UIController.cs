using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
	[SerializeField] Button locationGridBtn;
	[SerializeField] GameObject locationGrid;

	private void Awake()
	{
		locationGridBtn.onClick.AddListener(delegate { ToggleLocationGrid();  });
	}

	private void ToggleLocationGrid()
	{
		locationGrid.SetActive(!locationGrid.activeSelf);
	}
}
