using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredientSlices : MonoBehaviour
{
	public string alias;
	public Transform lookAt;
	private UiManager ui;
	public string ingredientName;
	private float duration= 0.4f;

	void Start()
	{
		ui= GameObject.Find("Canvas").GetComponent<UiManager>();

		if(lookAt != null)
		{
			if(alias == "sliceFront")
			{
				LeanTween.move(gameObject, lookAt.position, duration).setEase(LeanTweenType.easeInQuad).setOnComplete(harakiri);
			}
			else
			{
				LeanTween.moveY(gameObject, lookAt.position.y, duration).setEase(LeanTweenType.easeInQuad);
				LeanTween.moveX(gameObject, lookAt.position.x, duration).setEase(LeanTweenType.easeInBack);
				LeanTween.scale(gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale* 0f, duration / 2).setDelay(duration / 2).setOnComplete(Cleanup);
			}
		}
		else
		{
			if(alias == "sliceFront")
			{
				LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, duration).setEase(LeanTweenType.easeInCirc).setOnComplete(harakiri);
				LeanTween.moveY(gameObject, transform.position.y + 2, duration).setEase(LeanTweenType.easeInQuad);
				LeanTween.moveX(gameObject, transform.position.x - 3, duration).setEase(LeanTweenType.easeInBack);
			}
			else
			{
				LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, duration).setEase(LeanTweenType.easeInCirc).setOnComplete(harakiri);
				LeanTween.moveY(gameObject, transform.position.y - 2, duration).setEase(LeanTweenType.easeInQuad);
				LeanTween.moveX(gameObject, transform.position.x + 3, duration).setEase(LeanTweenType.easeInBack);
			}
			ui.incrementInvalidIngredientCount();
		}
		ui.incrementTotalIngredients();

		if(gameObject != null) Destroy(gameObject, 2);
	}

	private void harakiri()
	{
		if(gameObject != null) Destroy(gameObject);
		//if(ingredient != null) Destroy(ingredient);
	}

	private void Cleanup()
	{
		if(gameObject != null) Destroy(gameObject);
		if(ingredientName != null) 
		{
			ui.updateRecipeBoard(ingredientName);
			//Destroy(ingredient);
		}
	}
}