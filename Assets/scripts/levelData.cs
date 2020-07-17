using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ingredientPrimitives
{
	public string ingredient;
	public int quantity;

	public ingredientPrimitives(ingredient[] ingList)
	{
		int rand= Random.Range(0,  ingList.Length * 3);
		rand= rand % ingList.Length;

		this.ingredient= ingList[rand].GetComponent<Alias>().alias;
		this.quantity= Random.Range(1,  5);
	}
}

[System.Serializable]
public class recipePrimitive
{
	public List<ingredientPrimitives> recipe;

	public recipePrimitive(ingredient[] ingList)
	{
		int count= Random.Range(1,  5);

		this.recipe= new List<ingredientPrimitives>();
		for(int i= 0; i < count; i++)
		{
			this.recipe.Add(new ingredientPrimitives(ingList));
		}
	}
}

[System.Serializable]
public class levelPrimitive
{
	public List<recipePrimitive> level;

	public levelPrimitive(ingredient[] ingList)
	{
		int count= Random.Range(1,  5);

		this.level= new List<recipePrimitive>();
		for(int i= 0; i < count; i++)
		{
			this.level.Add(new recipePrimitive(ingList));
		}
	}
}

[System.Serializable]
public class levelData
{
	public List<levelPrimitive> levels= new List<levelPrimitive>();

	public levelData(ingredient[] ingList, int numbOfLevels)
	{
		for(int i= 0; i < numbOfLevels; i++)
		{
			this.levels.Add(new levelPrimitive(ingList));
		}
	}
}
