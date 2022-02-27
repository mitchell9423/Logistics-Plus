using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WorkOrderUI : MonoBehaviour
{
    [SerializeField] Transform workorderContainer;
    [SerializeField] Button calculateBtn;
    [SerializeField] RectTransform displayBackground;
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] Button removeBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] InputField cargoLimit;

    List<InputField> inputFields = new List<InputField>();
    List<int> manifest;

	private void Awake()
    {
        removeBtn.onClick.AddListener(delegate { RemoveOrders(); });
        clearBtn.onClick.AddListener(delegate { ClearOrders(); });
        calculateBtn.onClick.AddListener(delegate { Calculate(); });
    }

	void Start()
    {
        if (cargoLimit.text != GameManager.gameData.Limit.ToString()) cargoLimit.text = GameManager.gameData.Limit.ToString();
        cargoLimit.onEndEdit.AddListener(delegate { UpdateLimit(); });
        InitializeWorkOrders();
	}

	void InitializeWorkOrders()
    {
        if (GameManager.gameData.objectList.Count > 0)
        {
            for (int i = 0; i < GameManager.gameData.objectList.Count; i++)
            {
                CreateInputField().SetTextWithoutNotify(GameManager.gameData.objectList[i].ToString());
			}
        }

        AddBlankInputField();

		for (int i = 0; i < inputFields.Count - 1; i++)
		{
			StartCoroutine(AddUpdateListener(inputFields[i]));
		}
    }

    void AddBlankInputField()
    {
        InputField inputField = CreateInputField();
        StartCoroutine(AddUpdateListener(inputField));
        StartCoroutine(AddNewFieldListener(inputField));
    }

    IEnumerator DeselectInput()
	{
        yield return null;
        clearBtn.Select();
    }

    InputField CreateInputField()
    {
        GameObject obj = Instantiate(Resources.Load("InputField"), workorderContainer) as GameObject;
        obj.transform.SetSiblingIndex(obj.transform.parent.childCount - 4);
        InputField inputField = obj.GetComponent<InputField>();
        inputField.Select();
        inputFields.Add(inputField);
        return inputField;
    }

    void AddNewInputField(InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        inputField.onEndEdit.RemoveAllListeners();
        StartCoroutine(AddUpdateListener(inputField));
        AddBlankInputField();
    }

    IEnumerator AddUpdateListener(InputField inputField)
    {
        yield return null;
        inputField.onEndEdit.AddListener(delegate { UpdateWorkOrderList(); });
    }

    IEnumerator AddNewFieldListener(InputField inputField)
    {
        yield return null;
        inputField.onEndEdit.AddListener(delegate { AddNewInputField(inputField); });
    }

    void UpdateWorkOrderList()
    {
        GameManager.gameData.objectList.Clear();

        for (int i = 0; i < inputFields.Count; i++)
        {
            int value;
            if (int.TryParse(inputFields[i].text, out value) && value > 0)
            {
                GameManager.gameData.objectList.Add(value);
            }
        }

        JSONManager.Instance.SaveData(GameManager.gameData);
    }

    void UpdateLimit()
	{
        GameManager.gameData.UpdateCargoLimit(cargoLimit.text);
    }

    void RemoveOrders()
    {
        if (manifest.Count <= 0) return;

        foreach (int num in manifest)
        {
            InputField found = inputFields.Find((field) => field.text == num.ToString());
            inputFields.Remove(found);
            Destroy(found.gameObject);
        }

        manifest.Clear();
        UpdateWorkOrderList();
    }

    void ClearOrders()
    {
        for (int i = 0; i < inputFields.Count; i++)
        {
            Destroy(inputFields[i].gameObject);
        }

        inputFields.Clear();
        GameManager.gameData.Clear();
        manifest.Clear();
        display.text = $"No valid orders.";
        removeBtn.gameObject.SetActive(false);
        JSONManager.Instance.SaveData(GameManager.gameData);

        AddBlankInputField();
    }

    void Calculate()
	{
        var sum = 0;
        display.text = "";

        int limit;

		if (int.TryParse(cargoLimit.text, out limit))
        {
            manifest = GreatestSum.BestSum(GameManager.gameData.objectList, limit * 100);

            if (manifest.Count > 0)
            {
                foreach (var num in manifest)
                {
                    display.text += num + "\n";
                    sum += num;
                }

                displayBackground.sizeDelta = new Vector2(0, (30 * manifest.Count) + 110);
				display.text += $"\n{sum / 100} SCUs";
				display.text += $"\n";
                display.text += $"\n{sum} units";
                removeBtn.gameObject.SetActive(true);
            }
            else
            {
                display.text = $"No valid orders.";
            }
        }
    }

    void CreateGraph()
	{
        Graph graph = new Graph(GameManager.gameData.objectList);
        //GreatestSum.ComputeGraph(graph);
        display.text = graph.PrintAdjacencyList();
	}
}
