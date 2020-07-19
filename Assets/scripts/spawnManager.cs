using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
	[SerializeField] private List<GameObject> spawnPatternList;
	private UiManager UiManager;
	//private List<recipe> recipeList;

	void Start()
	{
		UiManager= GameObject.Find("Canvas") != null ? GameObject.Find("Canvas").GetComponent<UiManager>() : null;
		Camera.main.GetComponent<eventSystem>().onRecipeReady+= spawnNewPattern;
		//spawnNewPattern();
	}

	/*IEnumerator spawn()
	{
		yield return new WaitForSeconds(1f);
		int rand= Random.Range(0,  spawnPatternList.Count * 3);
		rand= rand % spawnPatternList.Count;
		Instantiate(spawnPatternList[rand], transform);
	}*/

	public void spawnNewPattern()
	{
		//recipeList= UiManager != null ? UiManager.recipeList : new List<recipe>();
		List<ingredient> actList= UiManager != null ? UiManager.getActiveIngredients() : new List<ingredient>();
		if(actList.Count > 0)
		{
			int rand= Random.Range(0,  spawnPatternList.Count * 3);
			rand= rand % spawnPatternList.Count;
			Instantiate(spawnPatternList[rand], transform);
		}
	}
}
