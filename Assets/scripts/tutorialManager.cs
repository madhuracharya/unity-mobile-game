using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class tutorialManager : MonoBehaviour
{
	[SerializeField] private GameObject blade;
	[SerializeField] private Rigidbody2D spawnRb;
	[SerializeField] private GameObject backDrop;
	[SerializeField] private GameObject helpingHand;
	[SerializeField] private GameObject ingOrange;
	[SerializeField] private GameObject ing2;
	[SerializeField] private GameObject ing3;
	[SerializeField] private GameObject invalidIng;
	[SerializeField] private GameObject text1;
	[SerializeField] private GameObject text2;
	[SerializeField] private GameObject text3;
	[SerializeField] private GameObject mask;
	[SerializeField] private GameObject OK;
	[SerializeField] private Transform slotParent;
	[SerializeField] private GameObject ingredientSlot;
	[SerializeField] private GameObject recipeBoard;
	[SerializeField] private recipe currentRecipe; 
	[SerializeField] private GameObject scoreBoard; 

	private eventSystem eventSystem;
	private bool recipeReady= false;
	private float firerate= 0.25f;
	private float canThrow= -1f;
	private Vector3 touchPos;
	private bool focus= true;
	private int stage= 0;
	private bool objectsReady= false;

	void Start()
	{
		eventSystem= Camera.main.GetComponent<eventSystem>();
		resetRecipeBoard();
		eventSystem.onRecipeReady+= () => {
			if(objectsReady == true) return;

			objectsReady= true;
			ingOrange.SetActive(true);
			ing2.SetActive(true);
			ing3.SetActive(true);
			invalidIng.SetActive(true);

			IEnumerator waitfor1second()
			{	
				yield return new WaitForSeconds(1f);
				backDrop.SetActive(true);
				helpingHand.SetActive(true);
				LeanTween.move(helpingHand, ingOrange.transform.position, 1.5f).setOnComplete(() => {
					text1.SetActive(true);
					ingOrange.transform.GetChild(0).gameObject.SetActive(true);
				});
			}
			StartCoroutine(waitfor1second());
		};

		eventSystem.onIngredientTrigger+= () => {
			if(ingOrange == null && ing2 != null && ing3 != null && invalidIng != null) 
			{
				updateRecipeBoard("orange");

				focus= true;
				Destroy(text1);
				text2.SetActive(true);
				mask.SetActive(true);
				Destroy(helpingHand);
				OK.SetActive(true);

				OK.GetComponent<Button>().onClick.AddListener(() => {
					focus= false;
					Destroy(text2);
					mask.SetActive(false);
					OK.SetActive(false);
					backDrop.SetActive(false);
				});
			}
			else if(ingOrange == null && invalidIng == null && (ing2 == null && ing3 == null) && text3 == null)
			{
				LeanTween.moveY(scoreBoard.GetComponent<RectTransform>(), 0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => {
					
				});
			}
			else if(ingOrange == null && invalidIng == null && (ing2 == null && ing3 == null) && text3 != null) 
			{
				focus= true;
				text3.SetActive(true);
				backDrop.SetActive(true);
				mask.SetActive(true);
				OK.SetActive(true);

				OK.GetComponent<Button>().onClick.AddListener(() => {
					focus= true;
					Destroy(text3);
					Destroy(mask);
					Destroy(OK);
					LeanTween.moveY(scoreBoard.GetComponent<RectTransform>(), 0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => {
						
					});
				});
			}
			else if(ingOrange == null && invalidIng == null && (ing2 != null || ing3 != null) && text3 != null) 
			{
				focus= true;
				text3.SetActive(true);
				backDrop.SetActive(true);
				mask.SetActive(true);
				OK.SetActive(true);

				OK.GetComponent<Button>().onClick.AddListener(() => {
					focus= false;
					Destroy(text3);
					Destroy(mask);
					Destroy(OK);
				});
			}

			if(ing2 == null) 
			{
				updateRecipeBoard("greenApple");
			}
			if(ing3 == null) 
			{
				updateRecipeBoard("pineapple");
			}
		};

		/*IEnumerator waitfor1second()
		{	
			yield return new WaitForSeconds(1f);
			backDrop.SetActive(true);
			helpingHand.SetActive(true);
			LeanTween.move(helpingHand, ingOrange.transform.position, .5f).setOnComplete(() => {
				text1.SetActive(true);
			});
		}
		StartCoroutine(waitfor1second());*/
	}

	void Update()
	{
		if(Input.touchCount > 0 && Time.time > canThrow)
		{	
			Touch touch= Input.GetTouch(0);
			touchPos= Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			touchPos.z= 0;

			if(touch.phase == TouchPhase.Ended)
			{
				if(focus == false)
				{
					throwBlade();
				}
				else if(ingOrange != null && verifyTouchPoint((Vector2)touchPos, (Vector2)ingOrange.transform.position) == true)
				{
					throwBlade();
				}
				canThrow= Time.time + firerate;
			}
		}
	}

	private void resetRecipeBoard()
	{
		RectTransform recipeBoardRect= recipeBoard.GetComponent<RectTransform>();
		float rectPos= recipeBoard.transform.position.x;

		LeanTween.moveX(recipeBoardRect, -recipeBoardRect.rect.width, .5f).setOnComplete(() => {
			if(objectsReady == false) recipeBoard.transform.GetChild(1).gameObject.SetActive(true);
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

		foreach (Transform slot in slotParent.transform)
		{
			if(ingredientName == slot.GetComponent<Alias>().alias && slot.transform.GetChild(2).gameObject.activeSelf == false)
			{
				slot.transform.GetChild(1).gameObject.SetActive(false);

				RectTransform rect= slot.transform.GetChild(0).GetComponent<RectTransform>();
				Vector3 initialScale= rect.localScale;

				slot.transform.GetChild(2).gameObject.SetActive(true);

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

	private void throwBlade()
	{
		Vector3 direction= (touchPos - (Vector3)spawnRb.position).normalized;
		float rotation= Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		spawnRb.rotation= rotation;
		Instantiate(blade, spawnRb.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
	}

	private bool verifyTouchPoint(Vector2 b1, Vector2 b2)
	{
		float dx= Mathf.Abs(b1.x - b2.x);
		float dy= Mathf.Abs(b1.y - b2.y);
		//Debug.Log(((dx * dx) + (dy * dy)) + " : " + b1.x + ", " + b1.y + (((dx * dx) + (dy * dy)) <= 2));
		return (((dx * dx) + (dy * dy)) <= 2);
	}
}





