using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTRS : MonoBehaviour
{
	public Button btnT, btnS;
	public TranslationController tCtrl;
	public ScaleController sCtrl;

	// Use this for initialization
	void Start () {
        Debug.Assert(btnT != null);
		Debug.Assert(btnS != null);

		Debug.Assert(tCtrl != null);
		Debug.Assert(sCtrl != null);

		btnT.onClick.AddListener(() =>
		{
			SetTRSMode(TheWorld.AxisModes.TRANSLATING);
		});

		btnS.onClick.AddListener(() =>
		{
			SetTRSMode(TheWorld.AxisModes.SCALING);
		});
	}

	public void SetTRSMode(TheWorld.AxisModes mode)
	{
		if (mode != TheWorld.axisMode)
		{
			// Save the old target
			GameObject target = null;
			switch (TheWorld.axisMode)
			{
				case TheWorld.AxisModes.TRANSLATING:
					target = tCtrl.target;
					tCtrl.SetTarget(null);
					btnT.interactable = true;
                    break;
				case TheWorld.AxisModes.SCALING:
					target = sCtrl.target;
					sCtrl.SetTarget(null);
					btnS.interactable = true;
					break;
			}

			// Reselet what was being selected using the new axis mode
			switch (mode)
			{
				case TheWorld.AxisModes.TRANSLATING:
					tCtrl.SetTarget(target);
					btnT.interactable = false;
                    break;
				case TheWorld.AxisModes.SCALING:
					sCtrl.SetTarget(target);
					btnS.interactable = false;
					break;
			}

			TheWorld.axisMode = mode;
		}
	}
}
