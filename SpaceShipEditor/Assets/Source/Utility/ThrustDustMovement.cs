using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustDustMovement : MonoBehaviour
{
	private float timerStartTime;
	public Vector3 velocity;
	private float timeout;
	
	public void setTimeout(float timeInSecs)
	{
		timeout = timeInSecs;
		timerStartTime = Time.time;
		gameObject.SetActive(true);
	}

	private void Update () {
		transform.localPosition += velocity;
		if (Time.time - timerStartTime > timeout)
			gameObject.SetActive(false);
	}
}
