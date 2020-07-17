using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
	[SerializeField] private IngredientList ingList;
	public List<recipe> theRecipe;
	public levelData lvlData;

	public void testSave()
	{
		lvlData= new levelData(ingList.ingredientList, 10);

		JSONManager.SaveIntoJson(lvlData);
	}

	public void TestLoad()
	{
		levelData prof= JSONManager.LoadFromJson();
		if(prof != null)
		{
			Debug.Log("loaded level data");  
			lvlData= prof;

			theRecipe= JSONManager.getLevelDate(ingList.ingredientList, prof, 2);
		}
		else
		{
			Debug.Log("failed to load level data");  
		}
	}
}
