/// Created by Stanley Mugo Nov 19, 2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Controller : MonoBehaviour
{
	//public MeshUIController meshUIController;
	public TranslationController posCtrl;
	public ScaleController scaleCtrl;
	public SelectTRS selectTRS;
	public TheWorld myWorld;

	/// <summary>
	/// Initialize stuff
	/// </summary>
	private void Start()
	{
		Debug.Assert(myWorld != null);
		Debug.Assert(posCtrl != null);
		Debug.Assert(scaleCtrl != null);
		Debug.Assert(selectTRS != null);
		//Debug.Assert(meshUIController != null);

		Translatable.posCtrl = posCtrl;
		Scalable.scaleCtrl = scaleCtrl;
	}
	
	/// <summary>
	/// Handle mouse events
	/// </summary>
	private void Update ()
	{
		ProcessMouseEvents();
		ProcessKeyboardEvents();
	}
}
