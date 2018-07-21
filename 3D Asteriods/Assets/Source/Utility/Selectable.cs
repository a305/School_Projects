using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
	public abstract bool OnSelect(Vector3 mousePos, GameObject gObj, Selectable lastSelected);
	public abstract bool OnDeselect(Selectable newSelection);
	public abstract bool OnDrag(Vector3 mousePos, GameObject gObj);
	public abstract void OnForcedDeselect();
}