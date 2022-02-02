using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkOrderUI : MonoBehaviour
{
    [SerializeField] Transform workorderContainer;
    [SerializeField] RectTransform displayBackground;
    [SerializeField] TextMeshProUGUI display;
    [SerializeField] Button removeBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] InputField cargoLimit;
    [SerializeField] ObjectList workOrders;

    List<InputField> inputFields = new List<InputField>();
    int[] workOrderManifest;

	private void Awake()
    {
        removeBtn.onClick.AddListener(delegate { RemoveOrders(); });
        clearBtn.onClick.AddListener(delegate { ClearOrders(); });
        cargoLimit.onEndEdit.AddListener(delegate { UpdateLimit(); });
    }

	void Start()
    {
        if (cargoLimit.text != workOrders.Limit.ToString()) cargoLimit.text = workOrders.Limit.ToString();
        InitializeWorkOrders();
	}

    void InitializeWorkOrders()
    {
        if (workOrders.objectList.Count > 0)
        {
            for (int i = 0; i < workOrders.objectList.Count; i++)
            {
                CreateInputField(null);
                inputFields[inputFields.Count - 1].SetTextWithoutNotify(workOrders.objectList[i].ToString());
            }
            StartCoroutine(DeselectInput());
        }
        else
        {
            CreateInputField(null);
        }
    }

    IEnumerator DeselectInput()
	{
        yield return null;
        clearBtn.Select();
    }

    void CreateInputField(string txt = "")
	{
        if (txt == null || txt != "")
        {
            GameObject obj = Instantiate(Resources.Load("InputField"), workorderContainer) as GameObject;
            obj.transform.SetSiblingIndex(obj.transform.parent.childCount - 4);
            InputField inputField = obj.GetComponent<InputField>();
            AddInputFieldToList(inputField);
            inputField.Select();
        }
    }

    void AddInputFieldListeners(InputField inputField)
    {
        inputField.onEndEdit.AddListener(delegate { UpdateWorkOrderList(); });
    }

    void AddInputFieldToList(InputField inputField)
    {
        AddInputFieldListeners(inputField);
        if (inputFields.Count > 0) inputFields[inputFields.Count - 1].onEndEdit.RemoveListener(delegate { CreateInputField(inputField.text); });
        inputField.onEndEdit.AddListener(delegate { CreateInputField(inputField.text); });
        inputFields.Add(inputField);
    }

    void UpdateWorkOrderList()
    {
        workOrders.Clear();
        foreach (InputField field in inputFields)
        {
            int value;
            if (int.TryParse(field.text, out value) && value > 0)
            {
                workOrders.objectList.Add(value);
            }
        }
        Calculate();
    }

    void UpdateLimit()
	{
        if (workOrders.UpdateCargoLimit(cargoLimit.text)) Calculate();
    }

    void RemoveOrders()
	{
		for (int i = 0; i < workOrderManifest.Length; i++)
        {
            InputField found = inputFields.Find((field) => field.text == workOrderManifest[i].ToString());
            inputFields.Remove(found);
            Destroy(found.gameObject);
        }

        display.text = "";
        removeBtn.gameObject.SetActive(false);
        Calculate();
    }

    void ClearOrders()
	{
		for (int i = 0; i < inputFields.Count; i++)
        {
            Destroy(inputFields[i].gameObject);
        }

        inputFields.Clear();
        workOrders.Clear();
        workOrderManifest = null;
        display.text = "";
        removeBtn.gameObject.SetActive(false);
        CreateInputField(null);
    }

    void Calculate()
	{
        var sum = 0;
        display.text = "";

        int limit = 10;
		if (int.TryParse(cargoLimit.text, out limit))
			workOrderManifest = GreatestSum.BestSum(limit * 100, workOrders.objectList.ToArray());

        if (workOrderManifest.Length > 0)
        {
            foreach (var num in workOrderManifest)
            {
                display.text += num + "\n";
                sum += num;
            }

            displayBackground.sizeDelta = new Vector2(0, (30 * workOrderManifest.Length) + 60);
            display.text += $"\n{sum / 100} SCUs";
            removeBtn.gameObject.SetActive(true);
        }
        else
		{
            display.text = $"No valid orders.";
        }

    }
}
