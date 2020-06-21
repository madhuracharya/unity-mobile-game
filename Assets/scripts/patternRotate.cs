using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternRotate : MonoBehaviour
{
	private bool delayComplete= false;

	void Start()
	{
		StartCoroutine(delayUpdate());
	}

	void Update()
	{
		if(delayComplete == true)
		{
			transform.Rotate(0, 0, 30f * Time.deltaTime);
		}
	}

	IEnumerator delayUpdate()
	{
		yield return new WaitForSeconds(1.5f);
		delayComplete= true;
	}
}
