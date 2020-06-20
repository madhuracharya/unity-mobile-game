using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredientSpawner : MonoBehaviour
{
	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private GameObject[] ingredients;

	private float minDelay= .5f;
	private float maxDelay= 2f;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(SpawnIngredients());
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public IEnumerator SpawnIngredients()
	{
		for(int i= 0; i <= 100; i++)
		{
			float delay= Random.Range(minDelay, maxDelay);
			yield return new WaitForSeconds(delay);

			int index= Random.Range(0, spawnPoints.Length);
			GameObject ingredient= ingredients[Random.Range(0, ingredients.Length)];
			GameObject ing= Instantiate(ingredient, spawnPoints[index].position, spawnPoints[index].rotation);
			Destroy(ing, 7);
		}
	}
}
