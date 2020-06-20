using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingredientGhost : MonoBehaviour
{
	public string alias;
	public Transform lookAt;
	private GameObject recipeBoard;
	private UiManager ui;
	public GameObject ingredient;
	private bool flag= true;

	void Start()
	{
		recipeBoard= GameObject.Find("Canvas");
		ui= recipeBoard.GetComponent<UiManager>();
	}

	void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, lookAt.position, 50f);

		if(transform.position.x <= lookAt.position.x + 1 && flag == true)
		{
			flag= false;
			ui.updateRecipeBoard(ingredient);
			Destroy(gameObject);
			Destroy(ingredient);
		}
	}
}


