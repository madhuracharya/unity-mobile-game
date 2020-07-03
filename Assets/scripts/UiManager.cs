using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	[SerializeField] private GameObject slotParent;
	[SerializeField] private GameObject ingredientSlot;
	[SerializeField] private ingredient[] ingredientList;
	[SerializeField] private recipe customRecipe;

	public List<recipe> recipeList= new List<recipe>();
	public recipe currentRecipe;
	public int currentRecipeIndex= 0;
	private eventSystem eventSystem;
	private bool recipeReady= false;
	private List<string> ingredientNameQuee= new List<string>();
	private bool queeTrigger= false;

	// Start is called before the first frame update
	void Start()
	{
		eventSystem= Camera.main.GetComponent<eventSystem>();
		Camera.main.GetComponent<eventSystem>().onRecipeBoardUpdate+= () => {
			if (ingredientNameQuee.Count > 0) updateRecipeBoard(ingredientNameQuee[0]);
			else queeTrigger= false;
		};

		if(customRecipe != null)
		{
			recipeList.Add(customRecipe);
		}
		else
		{
			for(int i= 0; i < 5; i++)
			{
				recipeList.Add(new recipe(ingredientList));
			}
		}
		
		currentRecipe= recipeList[currentRecipeIndex];
		resetRecipeBoard();
	}

	public void addIngredientNameToQuee(string ingName)
	{
		ingredientNameQuee.Add(ingName);
		if(queeTrigger == false)
		{
			queeTrigger= true;
			updateRecipeBoard(ingredientNameQuee[0]);
		}
	}

	private void resetRecipeBoard()
	{
		GameObject rect= GameObject.Find("recipeBoard");
		Vector2 rectPos= rect.transform.position;

		LeanTween.moveX(rect, rectPos.x - 100, .5f).setOnComplete(() => {
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
			LeanTween.moveX(rect, rectPos.x, .5f).setOnComplete(() => {
				if(eventSystem.onRecipeChange != null) eventSystem.callOnRecipeChange();
				if(eventSystem.onRecipeReady != null && recipeReady == false)
				{
					eventSystem.callOnRecipeReady();
					recipeReady= true;
				}
			});
		});
	}

	public void updateRecipeBoard(string ingredientName)
	{
		if(ingredientName == null) return;

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
				if(ingredientName == slot.GetComponent<Alias>().alias && slot.transform.GetChild(2).gameObject.activeSelf == false)
				{
					TMPro.TextMeshProUGUI quantity= slot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
					int numb;
					bool isParsable = int.TryParse(quantity.text.Replace("x", ""), out numb);
					if(isParsable)
					{
						RectTransform rect= slot.transform.GetChild(0).GetComponent<RectTransform>();
						Vector3 initialScale= rect.localScale;
						slot.transform.GetChild(1).gameObject.SetActive(false);
						LeanTween.scale(rect, initialScale * 0.7f, .1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() => {
							LeanTween.scale(rect, initialScale, .1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(changeQuantity);
						});
						
						void changeQuantity()
						{
							if((numb - 1) <= 0)
							{
								//Destroy(slot.gameObject);
								slot.transform.GetChild(2).gameObject.SetActive(true);
								//slot.transform.GetChild(1).gameObject.SetActive(false);
								uncheckedSlots+= -1;
							}
							else
							{
								quantity.text= "x" + (numb - 1).ToString();
								slot.transform.GetChild(1).gameObject.SetActive(true);
							}

							if(uncheckedSlots <= 0 && currentRecipeIndex < recipeList.Count)
							{
								currentRecipeIndex++;
								currentRecipe= recipeList[currentRecipeIndex];
								//recipeList.RemoveAt(0);  
								resetRecipeBoard();
							}

							if(eventSystem.onRecipeBoardUpdate != null)
							{
								eventSystem.callOnRecipeBoardUpdate();
							}
							ingredientNameQuee.RemoveAt(0);
							return;
						}
					}
				}
			}
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

