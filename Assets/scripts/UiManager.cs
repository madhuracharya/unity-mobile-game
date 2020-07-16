using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	[SerializeField] private GameObject slotParent;
	[SerializeField] private GameObject ingredientSlot;
	[SerializeField] private IngredientList ingList;
	[SerializeField] private GameObject recipeBoard;
	[SerializeField] private TMPro.TextMeshProUGUI recipeCount;
	[SerializeField] private GameObject scoreBoard;

	public List<recipe> recipeList;
	public recipe currentRecipe; 
	public int currentRecipeIndex= 0;
	private eventSystem eventSystem;
	private bool recipeReady= false;
	public int invalidIngredientCount= 0;
	public int totalIngredients= 0;
	private ingredient[] ingredientList;
	public profile player;
	public int levelNmber;

	// Start is called before the first frame update
	void Start()
	{
		player= loadData();

		eventSystem= Camera.main.GetComponent<eventSystem>();
		if(ingList != null) ingredientList= ingList.ingredientList;

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
		//showScoreBoard();
	}

	private void resetRecipeBoard()
	{
		RectTransform recipeBoardRect= recipeBoard.GetComponent<RectTransform>();
		float rectPos= recipeBoard.transform.position.x;

		LeanTween.moveX(recipeBoardRect, -recipeBoardRect.rect.width, .5f).setOnComplete(() => {
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
			recipeCount.text= currentRecipeIndex + 1 + "/" + recipeList.Count;
			LeanTween.moveX(recipeBoardRect, -rectPos, .5f).setOnComplete(() => {
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
								LeanTween.moveX(recipeBoard.GetComponent<RectTransform>(), -recipeBoard.GetComponent<RectTransform>().rect.width * 2f, .5f).setOnComplete(() => {
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
		GameObject scoreUI= scoreBoard.transform.GetChild(1).gameObject;
		GameObject backdrop= scoreBoard.transform.GetChild(0).gameObject;
		float treshold1= 50;
		float treshold2= 66;
		float treshold3= 85;

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
					bool flag1= false, flag2= false, flag3= false;

					for(int i= 0; i < Mathf.Floor(par); i++)
					{
						acc.text= i + "%";

						if(i > treshold1 && flag1 == false)
						{
							Transform star1= scoreUI.transform.GetChild(2);
							star1.GetComponent<Image>().color = new Color(0.99f,0.52f,0f,1f);
							star1.GetChild(0).gameObject.SetActive(true);
							flag1= true;
						}
						if(i > treshold2 && flag2 == false) 
						{
							Transform star2= scoreUI.transform.GetChild(3);
							star2.GetComponent<Image>().color = new Color(0.99f,0.52f,0f,1f);
							star2.GetChild(0).gameObject.SetActive(true);
							flag2= true;
						}
						if(i > treshold3 && flag3 == false) 
						{
							Transform star3= scoreUI.transform.GetChild(4);
							star3.GetComponent<Image>().color = new Color(0.99f,0.52f,0f,1f);
							star3.GetChild(0).gameObject.SetActive(true);
							flag3= true;
						}

						yield return new WaitForSeconds(0.005f);
					}
					acc.text= par + "%";

					scoreUI.transform.GetChild(1).gameObject.SetActive(true);
					if(par > treshold1) scoreUI.transform.GetChild(5).gameObject.SetActive(true);
					else scoreUI.transform.GetChild(1).position= new Vector3(0, scoreUI.transform.GetChild(1).position.y, 0);
				
					player.updateLevel(levelNmber, par);
					player.setCurrentLevel(levelNmber + 1);
					saveData();
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
				int indx= System.Array.FindIndex(ingredientList,  ig => ig != null ? ig.GetComponent<Alias>().alias == slot.GetComponent<Alias>().alias : false);
				if(indx > -1)
				{
					ingList.Add(ingredientList[indx]);
				}
			}
		}
		return ingList;
	}

	public void saveData()
	{
		saveSystem.Save(player);
	}

	public profile loadData()
	{
		profile prof= saveSystem.Load();
		if(prof != null)
		{
			Debug.Log("Player data Loaded!"); 
			return prof;
		}
		else
		{
			Debug.Log("failed to load profile data");  
			return new profile();
		}
	}
}

