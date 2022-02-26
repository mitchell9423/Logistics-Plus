using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LocationGrid : MonoBehaviour
{
	[SerializeField] int scale;
	[SerializeField] float lineThickness;
	[SerializeField] LineRenderer[] axisLines;

	[SerializeField] Transform camAnchor;

	[SerializeField] Transform origin;

	private void Start()
	{
		LocationGridUI.Instance.AddLocation = AddLocation;
	}

	private void OnEnable()
	{
		DrawAxis(axisLines[0], Color.red, Vector3.right);
		DrawAxis(axisLines[1], Color.green, Vector3.up);
		DrawAxis(axisLines[2], Color.blue, Vector3.forward);
	}

	void DrawAxis(LineRenderer line, Color color, Vector3 direction)
	{
		Vector3[] positions = new Vector3[] { origin.position, direction * scale };
		line.SetPositions(positions);
		line.startWidth = lineThickness;
		line.endWidth = lineThickness;
		if (Application.isPlaying) line.material.color = color;
		else line.sharedMaterial.SetColor("_EmissionColor", color);
	}

	void AddLocation(Vector3 location)
	{

	}
}
