using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPatternVertical : MonoBehaviour
{
	[SerializeField] private GameObject[] ingredients;

	private float minDelay= 1f;
	private float maxDelay= 3f;
	private float timeLeft = 30.0f;
	private bool stopSpawning= false;

	// Start is called before the first frame update
	void Start()
	{
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
			GameObject ingredient= ingredients[Random.Range(0, ingredients.Length)];

			GameObject sp= new GameObject();
			sp.tag= "spawnPoint";
			sp.transform.position= transform.position;
			sp.transform.rotation= transform.rotation;
			sp.transform.parent= transform;

			GameObject ing= Instantiate(ingredient, sp.transform);
			yield return new WaitForSeconds(.5f);
			ing.GetComponent<Rigidbody2D>().AddForce(transform.up * 3f, ForceMode2D.Impulse);

			Destroy(sp, 7);
		}
	}
}