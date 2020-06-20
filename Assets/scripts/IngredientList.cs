using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "ingredientList", menuName= "ingredientList")]
public class IngredientList : ScriptableObject
{
	public ingredient[] ingredientList;
}

