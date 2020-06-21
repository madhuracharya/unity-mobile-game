using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternManager : MonoBehaviour
{

	void Update()
	{
		if(transform.childCount <= 0)
		{
			Destroy(gameObject);
			GameObject.Find("spawnManager").GetComponent<spawnManager>().spawnNewPattern();
		}
	}
}
