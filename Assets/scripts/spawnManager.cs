using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> spawnPatternList;

	void Start()
	{
		spawnNewPattern();
	}

	IEnumerator spawn()
	{
		yield return new WaitForSeconds(1f);
		int rand= Random.Range(0,  spawnPatternList.Count * 3);
		rand= rand % spawnPatternList.Count;
		Instantiate(spawnPatternList[rand], transform);
	}

	public void spawnNewPattern()
	{
		StartCoroutine(spawn());
	}
}
