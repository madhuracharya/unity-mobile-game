using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
	[SerializeField] private IngredientList ingList;

	void Start()
	{
		ingredient[] ingredientList= ingList.ingredientList;
		int rand= Random.Range(0,  ingredientList.Length * 3);
		rand= rand % ingredientList.Length;
		ingredient ing= Instantiate(ingredientList[rand], transform.position, transform.rotation);
		ing.gameObject.transform.parent= transform;
	}
}
