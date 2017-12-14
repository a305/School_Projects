/// Created by Stanley Mugo Nov 19, 2017
/// 
/// Makes the attatched object able to be moved like in the unity editor.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableSceneNode : Scalable
{
	/// <summary>
	/// On mouse first clicked, select self as what should be getting its position
	/// moved around.
	/// </summary>
	/// <returns>false to say that this object shouldn't be selected.</returns>
	public override bool OnSelect(Vector3 mousePos, GameObject gObj, Selectable lastSelected)
	{
		if (gameObject.GetComponent<NodePrimitive>() == null)
			return false;

		GameObject gSceneNodeParent = null;
		for (GameObject g = gameObject.transform.parent.gameObject; g != null; g = g.transform.parent.gameObject)
			if (g.GetComponent<SceneNode>() != null)
			{
				gSceneNodeParent = g;
				break;
			}

		if (gSceneNodeParent == null)
			return false;

		base.OnSelect(mousePos, gObj, lastSelected);
		scaleCtrl.SetTarget(gSceneNodeParent);
		return true; // Set as selected so OnMouseUp, deselect will be called
	}
}
