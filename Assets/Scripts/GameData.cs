using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	[SerializeField] public List<int> objectList;
	[SerializeField] public List<Vector3> oreLocations;
	[SerializeField] public int limit;
	public int Limit { get { return limit; } set { limit = value; } }
	public GameData()
	{
		Verify();
	}
	void Verify()
	{
		if (objectList == null) objectList = new List<int>();
		if (oreLocations == null) oreLocations = new List<Vector3>();
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
