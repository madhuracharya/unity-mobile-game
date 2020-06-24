using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> spawnPatternList;
	private UiManager UiManager;
	private List<recipe> recipeList;

	void Start()
	{
		UiManager= GameObject.Find("Canvas").GetComponent<UiManager>();
		spawnNewPattern();
	}

	IEnumerator spawn()
	{
		Debug.Log("Spawning new pattern!");
		yield return new WaitForSeconds(1f);
		int rand= Random.Range(0,  spawnPatternList.Count * 3);
		rand= rand % spawnPatternList.Count;
		Instantiate(spawnPatternList[rand], transform);
	}

	public void spawnNewPattern()
	{
		recipeList= UiManager.recipeList;
		List<ingredient> actList= UiManager.getActiveIngredients();

		if(recipeList.Count > 0 && actList.Count > 0)
		{
			StartCoroutine(spawn());
		}
	}
}
