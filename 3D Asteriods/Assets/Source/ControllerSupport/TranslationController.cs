﻿/// Created by Stanley Mugo on Nov 18, 2017
/// 
/// Translate objects in 3d space like the unity editor.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationController : MonoBehaviour
{
	public GameObject X;
	public GameObject Y;
	public GameObject Z;

	private Vector3 dimentions = new Vector3(0.05f, 0.2f, 0.03f); // float centerRadius, float axisLength, float axisThickness
	private GameObject selectedAxis = null;
	private Vector3 lastMousePos;
	public GameObject target;
	private delegate void MouseChange(bool isDown, GameObject selected);
	private bool secondDeselect = false;
    private Color lastColor;

	/// <summary>
	/// Set the colors of the axis and add their handles.
	/// </summary>
	private void Start()
	{
		// Initialize colors for axis cylinders
		GameObject[] gObjList = { X, Y, Z };
		foreach (GameObject gObj in gObjList)
		{
			CallbackSelectable addHanderl = gObj.AddComponent<CallbackSelectable>();
			addHanderl.onSelect = AxisSelected;
			addHanderl.onDrag = AxisMoved;
			addHanderl.onDeselect = AxisDeselected;
			addHanderl.onForceDeselect = () => { SetTarget(null); };
		}

		// Enable or disable
		//SetTarget(target);
		//dimentions = gameObject.transform.localScale;
	}

	/// <summary>
	/// Set the object to move. Hide this axis if nothing is selected.
	/// </summary>
	/// <param name="t">Object to translate.</param>
	public void SetTarget(GameObject t)
	{
		if (t == null)
		{
			gameObject.transform.SetParent(null);
			enabled = false;
		}
		else
		{
			enabled = true;

			transform.SetParent(t.transform.parent);

			float distance = (t.transform.position - Camera.main.transform.position).magnitude;
			float frustumHeight = distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);

			transform.localPosition = t.transform.localPosition;
			transform.localRotation = t.transform.localRotation;

			float scaleRelativeToFrustrum = frustumHeight * 0.07f;
			float scaleRelativeToSelected = Mathf.Max(t.transform.localScale.x,
							t.transform.localScale.y,
							t.transform.localScale.z);
			
			float maxScaleUp = Mathf.Max(scaleRelativeToFrustrum, scaleRelativeToSelected);
			
			Vector3 newDim = dimentions.normalized * maxScaleUp;

			ResizeAxis(newDim.x, newDim.y, newDim.z);
		}

		target = t;
		gameObject.SetActive(enabled);
	}

	/// <summary>
	/// Change the size of the axis.
	/// </summary>
	/// <param name="centerRadius">Size of sphere connecting all the axis</param>
	/// <param name="axisLength">Length of all axis</param>
	/// <param name="axisThickness">Thickness of all axis</param>
	public void ResizeAxis(float centerRadius, float axisLength, float axisThickness)
	{
		dimentions.x = centerRadius;
		dimentions.y = axisLength;
		dimentions.z = axisThickness;

		transform.localScale = Vector3.one * centerRadius;
		foreach (Transform child in transform)
		{
			if (float.IsNaN(axisThickness))
				Debug.Log("axisThickness IsNaN");

			if (float.IsNaN(axisLength))
				Debug.Log("axisLength IsNaN");

			if (float.IsNaN(centerRadius))
				Debug.Log("centerRadius IsNaN " + centerRadius);

			if (0 == centerRadius)
				Debug.Log("divide by zero error centerRadius" + centerRadius);

			child.localScale = (new Vector3(1, 0, 1) *
				axisThickness + Vector3.up * axisLength) *
				1 / centerRadius;

			child.localPosition = child.localPosition.normalized *
				(axisLength / centerRadius);
		}
	}

	/// <summary>
	/// Will deselect the current object if empty space is selected.
	/// </summary>
	/// <param name="newSelection"></param>
	/// <returns></returns>
	private bool AxisDeselected(Selectable newSelection)
	{
        if (lastColor != null)
            selectedAxis.GetComponent<Renderer>().material.SetColor("_Color", lastColor);
        if (secondDeselect) // Deselect this if something else is clicked on
		{
			if (newSelection == null)
			{
				// If empty space is selected
				SetTarget(null); // Deselect the object being moved and hide me
			}
		}

		secondDeselect = true;
		return true; // Don't deselect on mouse up
	}

	/// <summary>
	/// Called when the mouse is first pressed down. Sets up the initial mouse
	/// position.
	/// </summary>
	/// <param name="position">Mouse click position in world space</param>
	/// <param name="selected">Selected axis</param>
	/// <param name="reselected">If clicked on when already selected.</param>
	/// <returns></returns>
	private bool AxisSelected(Vector3 position, GameObject selected, Selectable lastSelected)
	{
        lastColor = selected.GetComponent<Renderer>().material.GetColor("_Color");
        selected.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 0, .5f));
        secondDeselect = false;
		lastMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		selectedAxis = selected;
		return target != null; // Don't select if there is no target
	}

	/// <summary>
	/// Called when the mouse is being pressed and this object is selected.
	/// </summary>
	/// <param name="selectedPos">Selected mouse position in world space (Zero if unknown)</param>
	/// <param name="selectedGameObj">Game object which the mouse is over (Null if none)</param>
	/// <returns></returns>
	private bool AxisMoved(Vector3 selectedPos, GameObject selectedGameObj)
	{
		if (selectedAxis == null ||target == null) return false;
		
		Vector3 curentMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		Vector3 mouseDelata = curentMousePos - lastMousePos;

		float distance = (target.transform.localPosition - Camera.main.transform.localPosition).magnitude;
		float frustumHeight = distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
		float frustumWidth = frustumHeight * Camera.main.aspect;

		float scaleUp = 1;
		Vector3 offset3d = Vector3.zero;
		offset3d += Camera.main.transform.right.normalized * mouseDelata.x * frustumWidth * scaleUp;
		offset3d += Camera.main.transform.up.normalized * mouseDelata.y * frustumHeight * scaleUp;

		Debug.DrawLine(target.transform.position, target.transform.position + offset3d, Color.blue, 3);

		//offset3d += Camera.main.transform.right * mouseDelata.x * scaleUp;
		//offset3d += Camera.main.transform.up * mouseDelata.y * scaleUp;
		offset3d = Vector3.Project(offset3d, selectedAxis.transform.up);

		transform.localPosition = target.transform.localPosition += offset3d;
		//Debug.DrawLine(target.transform.position, target.transform.position + offset3d, Color.red, 3);
		lastMousePos = curentMousePos;
		
		return true;
	}
}
