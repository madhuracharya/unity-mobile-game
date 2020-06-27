using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredientSlices : MonoBehaviour
{
	public string alias;
	public Transform lookAt;
	private UiManager ui;
	public GameObject ingredient;
	private float duration= .4f;

	void Start()
	{
		ui= GameObject.Find("Canvas").GetComponent<UiManager>();
		if(lookAt != null && alias != null)
		{
			if(alias == "sliceFront")
			{
				LeanTween.move(gameObject, lookAt.position, duration).setEase(LeanTweenType.easeInQuad).setOnComplete(harakiri);
			}
			else
			{
				LeanTween.moveY(gameObject, lookAt.position.y, duration).setEase(LeanTweenType.easeInQuad);
				LeanTween.moveX(gameObject, lookAt.position.x, duration).setEase(LeanTweenType.easeInBack);
				LeanTween.scale(gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale* 0.5f, duration).setDelay(duration).setOnComplete(Cleanup);
			}
		}
	}

	private void harakiri()
	{
		Destroy(gameObject);
	}

	private void Cleanup()
	{
		ui.updateRecipeBoard(ingredient);
		Destroy(gameObject);
		Destroy(ingredient);
	}
}