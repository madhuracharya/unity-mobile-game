using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPatternVertical : MonoBehaviour
{
	//[SerializeField] private GameObject[] ingredients;
	[SerializeField] private IngredientList ingList;
	private ingredient[] ingredientList;
	private UiManager uiManager;
	private List<recipeItem> therecipe;

	private float minDelay= 1f;
	private float maxDelay= 3f;
	private float timeLeft = 10.0f;
	private bool stopSpawning= false;

	// Start is called before the first frame update
	void Start()
	{
		uiManager= GameObject.Find("Canvas").GetComponent<UiManager>();
		therecipe= uiManager.currentRecipe.theRecipe;

		if(ingList != null) ingredientList= ingList.ingredientList;
		StartCoroutine(SpawnIngredients());
	}

	// Update is called once per frame
	void Update()
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft < 0)
		{
			stopSpawning= true;
		}

		if(stopSpawning == true && transform.childCount <= 0)
		{
			Destroy(gameObject);
		}
	}

	public IEnumerator SpawnIngredients()
	{
		for(int i= 0; i <= 100; i++)
		{
			if(stopSpawning == true)
			{
				yield break;
			}

			float delay= Random.Range(minDelay, maxDelay);
			yield return new WaitForSeconds(delay);

			if(Random.value <= 0.3)
			{
				int rand= Random.Range(0,  therecipe.Count * 3);
				rand= rand % therecipe.Count;
				GameObject ing= Instantiate(therecipe[rand].ingredient, transform.position, transform.rotation);
				ing.transform.parent= transform;
				ing.GetComponent<Alias>().inRecipe= true;
				ing.tag= "spawnPoint";

				yield return new WaitForSeconds(.5f);
				ing.GetComponent<Rigidbody2D>().AddForce(transform.up * 3f, ForceMode2D.Impulse);
				Destroy(ing, 7);
			}
			else
			{
				int rand= Random.Range(0,  ingredientList.Length * 3);
				rand= rand % ingredientList.Length;
				ingredient ing= Instantiate(ingredientList[rand], transform.position, transform.rotation);
				ing.gameObject.transform.parent= transform;
				ing.gameObject.tag= "spawnPoint";

				yield return new WaitForSeconds(.5f);
				ing.GetComponent<Rigidbody2D>().AddForce(transform.up * 3f, ForceMode2D.Impulse);
				Destroy(ing.gameObject, 7);
			}
		}
	}
}