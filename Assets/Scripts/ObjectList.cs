using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New_ObjectList", menuName = "ScriptableObjects/ObjectList", order = 1)]
public class ObjectList : ScriptableObject
{
	[SerializeField] public List<int> objectList;
	[SerializeField] private int limit;
	public int Limit { get { return limit; } set { limit = value; } }
	void Verify()
	{
		if (objectList == null) objectList = new List<int>();
	}
	public void Add(int value)
	{
		Verify();
		if (value > 0) objectList.Add(value);
	}
	public void Clear()
	{
		objectList.Clear();
	}
	public bool UpdateCargoLimit(string limit)
	{
		int intLimit;
		if (int.TryParse(limit, out intLimit))
		{
			if (intLimit >= 0)
			{
				Limit = intLimit;
				return true;
			}
		}

		return false;
	}
}
