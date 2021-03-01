using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameInitializer : MonoBehaviour
{
	[SerializeField] private IngredientList ingList;

	void Start()
	{
		JSONManager.initialize(ingList.ingredientList);
		saveSystem.initialize();

		if(PlayerPrefs.GetInt("firstTime") == 0)
		{
			PlayerPrefs.SetInt("firstTime", 1);
		}
	}

	void ResetData()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("Data reset complete");
	}
}
