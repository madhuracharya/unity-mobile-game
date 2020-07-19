using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameInitializer : MonoBehaviour
{
	[SerializeField] private IngredientList ingList;
	private int numbOfLevels= 100;

	void Start()
	{
		if(PlayerPrefs.GetInt("firstTime") == 0)
		{
			JSONManager.SaveIntoJson(new levelData(ingList.ingredientList, numbOfLevels));
			saveSystem.Save(new profile(numbOfLevels));
			PlayerPrefs.SetInt("firstTime", 1);
		}

		/*JSONManager.SaveIntoJson(new levelData(ingList.ingredientList, numbOfLevels));
		saveSystem.Save(new profile(numbOfLevels));
		PlayerPrefs.SetInt("firstTime", 1);*/
	}
}
