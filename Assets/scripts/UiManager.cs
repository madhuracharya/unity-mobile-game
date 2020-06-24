using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	[SerializeField] private GameObject slotParent;
	[SerializeField] private GameObject ingredientSlot;
	[SerializeField] private ingredient[] ingredientList;

	public List<recipe> recipeList= new List<recipe>();
	public recipe currentRecipe;

	// Start is called before the first frame update
	void Start()
	{
		for(int i= 0; i < 5; i++)
		{
			recipeList.Add(new recipe(ingredientList));
		}
		currentRecipe= recipeList[0];
		recipeList.RemoveAt(0);
		resetRecipeBoard();
	}

	private void resetRecipeBoard()
	{
		foreach(Transform slot in slotParent.transform)
		{
			Destroy(slot.gameObject);
		}

		foreach(recipeItem rec in currentRecipe.theRecipe)
		{
			GameObject slot= Instantiate(ingredientSlot, slotParent.transform);
			Image img= slot.transform.GetChild(0).GetComponent<Image>();
			img.sprite= rec.ingredient.GetComponent<SpriteRenderer>().sprite;
			img.enabled= true;

			TMPro.TextMeshProUGUI quantity= slot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
			quantity.text= "x" + rec.quantity.ToString();
			quantity.enabled= true;

			slot.GetComponent<Alias>().alias= rec.ingredientName;
		}
	}

	public void updateRecipeBoard(GameObject ingredient)
	{
		//int totalSlots= slotParent.transform.childCount;
		int uncheckedSlots= 0;

		foreach(Transform slt in slotParent.transform)
		{
			if(slt.GetChild(2).gameObject.activeSelf == false)
			{
				uncheckedSlots++;
			}
		}

		if(uncheckedSlots > 0)
		{
			foreach (Transform slot in slotParent.transform)
			{
				if(ingredient.GetComponent<Alias>().alias == slot.GetComponent<Alias>().alias && slot.transform.GetChild(2).gameObject.activeSelf == false)
				{
					TMPro.TextMeshProUGUI quantity= slot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
					int numb;
					bool isParsable = int.TryParse(quantity.text.Replace("x", ""), out numb);
					if(isParsable)
					{
						if((numb - 1) <= 0)
						{
							//Destroy(slot.gameObject);
							slot.transform.GetChild(2).gameObject.SetActive(true);
							slot.transform.GetChild(1).gameObject.SetActive(false);
							uncheckedSlots+= -1;
						}
						else
						{
							quantity.text= "x" + (numb - 1).ToString();
						}
					}
				}
			}
		}
		
		if(uncheckedSlots <= 0 && recipeList.Count > 0)
		{
			currentRecipe= recipeList[0];
			recipeList.RemoveAt(0);  
			resetRecipeBoard();
		}
		
	}

	public bool checkIngredientInRecipe(ingredient ing)
	{
		if(ing != null)
		{
			foreach (Transform slot in slotParent.transform)
			{
				if(ing.GetComponent<Alias>().alias == slot.GetComponent<Alias>().alias)
				{
					return slot.GetChild(2).gameObject.activeSelf;
				}
			}
		}
		return false;
	}

	public List<ingredient> getActiveIngredients()
	{
		List<ingredient> ingList= new List<ingredient>();
		foreach (Transform slot in slotParent.transform)
		{
			if(slot.GetChild(2).gameObject.activeSelf == false)
			{
				int indx= System.Array.FindIndex(ingredientList,  ig => ig.GetComponent<Alias>().alias == slot.GetComponent<Alias>().alias);
				if(indx > -1)
				{
					ingList.Add(ingredientList[indx]);
				}
			}
		}
		return ingList;
	}
}

