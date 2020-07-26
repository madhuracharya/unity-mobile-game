using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
	[SerializeField] private IngredientList ingList;
	[SerializeField] private float probability= 0.5f;
	private UiManager uiManager;

	void Start()
	{
		IEnumerator spawn()
		{	
			yield return new WaitForSeconds(0.5f);

			uiManager= GameObject.Find("Canvas").GetComponent<UiManager>();
			List<recipeItem> therecipe= (uiManager != null && uiManager.currentRecipe != null) ?  uiManager.currentRecipe.theRecipe : new List<recipeItem>();
			Debug.Log(uiManager.currentRecipe);
			if(Random.value <= probability && therecipe.Count > 0)
			{
				int rand= Random.Range(0,  therecipe.Count * 3);
				rand= rand % therecipe.Count;
				GameObject ing= Instantiate(therecipe[rand].ingredient, transform.position, transform.rotation);
				ing.transform.parent= transform;
				ing.GetComponent<Alias>().inRecipe= true;
			}
			else
			{
				ingredient[] ingredientList= ingList.ingredientList;
				int rand= Random.Range(0,  ingredientList.Length * 3);
				rand= rand % ingredientList.Length;
				ingredient ing= Instantiate(ingredientList[rand], transform.position, transform.rotation);
				ing.gameObject.transform.parent= transform;

				if(uiManager.currentRecipe != null)
				{
					Alias alias= ing.GetComponent<Alias>();

					foreach(recipeItem itm in uiManager.currentRecipe.theRecipe)
					{
						if(alias.alias == itm.ingredientName)
						{
							alias.inRecipe= true;
							break;
						}
						else
						{
							alias.inRecipe= false;
						}
					}
				}
			}
		}
		StartCoroutine(spawn());
	}
}
