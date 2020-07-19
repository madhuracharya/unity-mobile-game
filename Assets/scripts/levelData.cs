using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ingredientPrimitives
{
	public string ingredient;
	public int quantity;

	public ingredientPrimitives(ingredient ing)
	{
		this.ingredient= ing.GetComponent<Alias>().alias;
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
		ingredient[] tempIngList= new ingredient[ingList.Length];
		System.Array.Copy(ingList, tempIngList, ingList.Length); 

		this.recipe= new List<ingredientPrimitives>();
		for(int i= 0; i < count; i++)
		{	
			int rand= Random.Range(0,  tempIngList.Length * 3);
			rand= rand % tempIngList.Length;
			this.recipe.Add(new ingredientPrimitives(tempIngList[rand]));
			RemoveAt(ref tempIngList, rand);
		}
	}

	void RemoveAt(ref ingredient[] arr, int index)
	{
		for (int a = index; a < arr.Length - 1; a++)
		{
			arr[a] = arr[a + 1];
		}
		System.Array.Resize(ref arr, arr.Length - 1);
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

	public levelData(ingredient[] ingList, int numbOfLevels= 100)
	{
		for(int i= 0; i < numbOfLevels; i++)
		{
			this.levels.Add(new levelPrimitive(ingList));
		}
	}
}
