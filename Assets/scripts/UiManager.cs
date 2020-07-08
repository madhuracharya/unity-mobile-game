using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	private GameObject slotParent;
	[SerializeField] private GameObject ingredientSlot;
	[SerializeField] private ingredient[] ingredientList;

	private GameObject recipeBoard;
	private TMPro.TextMeshProUGUI recipeCount;

	public List<recipe> recipeList;
	public recipe currentRecipe; 
	public int currentRecipeIndex= 0;
	private eventSystem eventSystem;
	private bool recipeReady= false;
	public int invalidIngredientCount= 0;
	public int totalIngredients= 0;

	// Start is called before the first frame update
	void Start()
	{
		eventSystem= Camera.main.GetComponent<eventSystem>();
		slotParent= GameObject.Find("slotParent");
		recipeBoard= GameObject.Find("recipeBoard");
		recipeCount= GameObject.Find("recipeCount").GetComponent<TMPro.TextMeshProUGUI>();

		if(recipeList.Count == 0)
		{
			recipeList= new List<recipe>();
			for(int i= 0; i < 5; i++)
			{
				recipeList.Add(new recipe(ingredientList));
			}
		}
		
		currentRecipe= recipeList[currentRecipeIndex];
		resetRecipeBoard();
	}

	private void resetRecipeBoard()
	{
		Vector2 rectPos= recipeBoard.transform.position;

		LeanTween.moveX(recipeBoard, rectPos.x - 100, .5f).setOnComplete(() => {
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
			recipeCount.text= currentRecipeIndex + 1 + " / " + recipeList.Count;
			LeanTween.moveX(recipeBoard, rectPos.x, .5f).setOnComplete(() => {
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
						slot.transform.GetChild(1).gameObject.SetActive(false);

						RectTransform rect= slot.transform.GetChild(0).GetComponent<RectTransform>();
						Vector3 initialScale= rect.localScale;

						if((numb - 1) <= 0)
						{
							slot.transform.GetChild(2).gameObject.SetActive(true);
							uncheckedSlots+= -1;
						}
						else
						{
							quantity.text= "x" + (numb - 1).ToString();
							slot.transform.GetChild(1).gameObject.SetActive(true);
						}

						if(uncheckedSlots <= 0)
						{
							if(currentRecipeIndex < recipeList.Count - 1)
							{
								currentRecipeIndex++;
								currentRecipe= recipeList[currentRecipeIndex];
								//recipeList.RemoveAt(0);  
								resetRecipeBoard();
							}
							else
							{
								LeanTween.moveX(recipeBoard, recipeBoard.transform.position.x - 100, .5f).setOnComplete(() => {
									if(eventSystem.onRecipeListEmpty != null)
									{
										eventSystem.callOnRecipeListEmpty();
									}
									showScoreBoard();
								});
							}
						}

						LeanTween.scale(rect, initialScale * 0.85f, .05f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() => {
							LeanTween.scale(rect, initialScale, .05f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() => {
								if(eventSystem.onRecipeBoardUpdate != null)
								{
									eventSystem.callOnRecipeBoardUpdate();
								}
								return;
							});
						});
					}
				}
			}
		}
	}

	public void showScoreBoard()
	{
		GameObject.Find("avatar").SetActive(false);
		GameObject scoreBoard= GameObject.Find("scoreBoard");
		GameObject scoreUI= scoreBoard.transform.GetChild(1).gameObject;
		GameObject backdrop= scoreBoard.transform.GetChild(0).gameObject;

		scoreUI.SetActive(true);

		TMPro.TextMeshProUGUI acc= GameObject.Find("recipeAccuracy").GetComponent<TMPro.TextMeshProUGUI>();
		if(acc != null)
		{
			backdrop.SetActive(true);
			LeanTween.moveY(scoreUI.GetComponent<RectTransform>(), 0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => {
				float par= 0;

				if(invalidIngredientCount == 0 || totalIngredients == 0)
				{
					par= 100;
				}
				else
				{
					par= (((float)totalIngredients - (float)invalidIngredientCount) / totalIngredients) * 100;
					par= Mathf.Floor(par * 10) / 10;
				}

				IEnumerator scoreTicker()
				{	
					for(int i= 0; i < Mathf.Floor(par); i++)
					{
						acc.text= i + "%";
						yield return new WaitForSeconds(0.005f);
					}
					acc.text= par + "%";
					
					if(par > 25) scoreUI.transform.GetChild(2).gameObject.SetActive(true);
					yield return new WaitForSeconds(0.5f);
					if(par > 50) scoreUI.transform.GetChild(3).gameObject.SetActive(true);
					yield return new WaitForSeconds(0.5f);
					if(par > 85) scoreUI.transform.GetChild(4).gameObject.SetActive(true);
					yield return new WaitForSeconds(0.5f);
					scoreUI.transform.GetChild(1).gameObject.SetActive(true);
				}
				StartCoroutine(scoreTicker());
			});
		}
	}	

	public void incrementInvalidIngredientCount()
	{
		invalidIngredientCount+= 1;
	}

	public void incrementTotalIngredients()
	{
		totalIngredients+= 1;
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

