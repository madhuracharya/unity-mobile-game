using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternManager : MonoBehaviour
{
	[SerializeField] private GameObject despawnAnimation;

	private spawnManager spawner;
	private string name;
	private GameObject slotParent;
	private UiManager UiManager;
	private bool despawning= false;

	void Start()
	{
		UiManager= GameObject.Find("Canvas").GetComponent<UiManager>();
		GameObject sp= GameObject.Find("spawnManager");
		spawner= sp != null ? sp.GetComponent<spawnManager>() : null;
		slotParent = GameObject.Find("slotParent");
		name= gameObject.name.Replace("(Clone)", "");
		Camera.main.GetComponent<eventSystem>().onRecipeBoardUpdate+= validateIngredients;

		IEnumerator delay()
		{	
			yield return new WaitForSeconds(1f);
			validateIngredients();
		}
		StartCoroutine(delay());
	}

	void Update()
	{
		if(despawning == false && transform.childCount <= 0)
		{
			next();
		}
	}

	public void despawnAndDestroy()
	{
		if(despawnAnimation == null) return;
		Camera cam= Camera.main;
		foreach(Transform child in transform)
		{
			GameObject temp= Instantiate(despawnAnimation, child.position, child.rotation);
			temp.transform.SetParent(cam.transform);
			Destroy(temp, 1);
		}
		Destroy(gameObject);
	}

	private void validateIngredients()
	{
		if(despawning == true) return;
		else
		{
			switch (name)
			{
				case "linearMove":
				{
					return;
				}
				default:
				{
					int activeSlots= 0;
					List<ingredient> actList= UiManager.getActiveIngredients();

					foreach (ingredient actIng in actList)
					{
						foreach(Transform spawnPoint in transform)
						{
							Transform ing= spawnPoint.GetChild(0);
							if(ing != null && ing.GetComponent<Alias>().alias == actIng.transform.GetComponent<Alias>().alias)
							{
								activeSlots++;
							}
						}
					}

					if(activeSlots <= 0) next();
					return;
				}
			}
		}
	}

	private void next()
	{
		IEnumerator despawn()
		{	
			yield return new WaitForSeconds(1f);
			if(spawner != null)
				spawner.spawnNewPattern();
			despawnAndDestroy();
		}

		Debug.Log("Spawning next!");
		despawning= true;
		StartCoroutine(despawn());
	}
}
