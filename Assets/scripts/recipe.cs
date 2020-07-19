using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct recipeItem
{
	public GameObject ingredient;
	public int quantity;
	public string ingredientName;
}

[CreateAssetMenu(fileName= "Recipe", menuName= "Recipe")]
public class recipe : ScriptableObject
{
	public List<recipeItem> theRecipe= new List<recipeItem>();

	public recipe(ingredient[] ingredientList)
	{
		if(ingredientList.Length == 0) return;

		List<int> sampleSpace= new List<int>();
		for(int i= 0; i < ingredientList.Length; i++)
		{
			sampleSpace.Add(i);
		}

		for(int i= 0; i < UnityEngine.Random.Range(2, ingredientList.Length); i++)
		{
			recipeItem itm= new recipeItem();
			int rand= UnityEngine.Random.Range(0, 100);
			rand= rand % sampleSpace.Count;
			int indx= sampleSpace[rand];

			ingredient ing= ingredientList[indx];
			string alias= ing.GetComponent<Alias>().alias;
			itm.ingredient= ing.gameObject;
			itm.quantity= UnityEngine.Random.Range(1, 3);
			itm.ingredientName= alias;
			theRecipe.Add(itm);
			sampleSpace.RemoveAt(rand);
		}
	}

}

