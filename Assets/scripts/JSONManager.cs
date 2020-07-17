using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JSONManager
{
	private static string path= Application.persistentDataPath + "/SND/" + "SND.json";

	public static void SaveIntoJson(levelData levelData)
	{
		string data = JsonUtility.ToJson(levelData);
		File.WriteAllText(path, data);
		Debug.Log("Data saved!");
	}

	public static levelData LoadFromJson()
	{
		levelData data = JsonUtility.FromJson<levelData>(File.ReadAllText(path));
		return data;
	}

	public static List<recipe> getLevelDate(ingredient[] ingList, levelData levelData, int currentLevel)
	{
		List<levelPrimitive> levels= levelData.levels;

		if(levels.Count >= currentLevel)
		{
			List<recipePrimitive> recipeList= levels[currentLevel].level;
			if(recipeList.Count > 0)
			{
				List<recipe> actualRecipeList= new List<recipe>();
				foreach(recipePrimitive rep in recipeList)
				{
					recipe actualRecipe= new recipe(new ingredient[]{});
					List<ingredientPrimitives> primIngList= rep.recipe;

					foreach(ingredientPrimitives ing in primIngList)
					{
						recipeItem obj= new recipeItem();
						int indx= System.Array.FindIndex(ingList, x => x.GetComponent<Alias>().alias == ing.ingredient);
						if(indx > -1)
						{
							obj.ingredient= ingList[indx].gameObject;
							obj.quantity= ing.quantity;
							obj.ingredientName= ing.ingredient;
						}
						else
						{
							ingredient temp= ingList[Random.Range(0, ingList.Length)]; 
							obj.ingredient= temp.gameObject;
							obj.quantity= Random.Range(1, 5);
							obj.ingredientName= temp.GetComponent<Alias>().alias;
						}
						
						actualRecipe.theRecipe.Add(obj);
					}
					actualRecipeList.Add(actualRecipe);
				}
				return actualRecipeList;
			}
			else
			{
				List<recipe> actualRecipeList= new List<recipe>();
				for(int i= 0; i < Random.Range(1, 5); i ++)
				{
					actualRecipeList.Add(new recipe(ingList));
				}
				return actualRecipeList;
			}
		}
		else
		{
			return new List<recipe>();
		}
		
	}	
}