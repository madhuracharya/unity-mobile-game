using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ingredientPrimitives
{
	public string ingredient;
	public int quantity;

	public ingredientPrimitives()
	{
		this.ingredient= "orange";
		this.quantity= 2;
	}
}

[System.Serializable]
public class recipePrimitive
{
	public List<ingredientPrimitives> recipe;

	public recipePrimitive()
	{
		this.recipe= new List<ingredientPrimitives> {
			new ingredientPrimitives(),
			new ingredientPrimitives()
		};
	}
}

[System.Serializable]
public class levelPrimitive
{
	public List<recipePrimitive> level;

	public levelPrimitive()
	{
		this.level= new List<recipePrimitive> {
			new recipePrimitive(),
			new recipePrimitive()
		};
	}
}

[System.Serializable]
public class levelData
{
	public List<levelPrimitive> levels= new List<levelPrimitive>();

	public levelData()
	{
		this.levels= new List<levelPrimitive> {
			new levelPrimitive(),
			new levelPrimitive()
		};
	}
}


/*[System.Serializable]
public class levelData
{
	public List<List<List<ingredientPrimitives>>> levels= new List<List<List<ingredientPrimitives>>>();

	public levelData()
	{
		List<ingredientPrimitives> recipe= new List<ingredientPrimitives>{
			new ingredientPrimitives(),
			new ingredientPrimitives()
		};

		List<List<ingredientPrimitives>> level= new List<List<ingredientPrimitives>>{
			recipe,
			recipe
		};

		this.levels.Add(level);
		this.levels.Add(level);
	}
}*/

/*[System.Serializable]
public class levelData
{
	public List<ingredientPrimitives> levels;
}*/
