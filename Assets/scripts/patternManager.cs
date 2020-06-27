using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternManager : MonoBehaviour
{
	private spawnManager spawner;
	private string name;
	private GameObject slotParent;
	private UiManager UiManager;
	private bool despawning= false;

	void Start()
	{
		UiManager= GameObject.Find("Canvas").GetComponent<UiManager>();
		spawner= GameObject.Find("spawnManager").GetComponent<spawnManager>();
		slotParent = GameObject.Find("slotParent");
		name= gameObject.name.Replace("(Clone)", "");
		Camera.main.GetComponent<eventSystem>().onIngredientTrigger+= checkValidIngredients;
	}

	void Update()
	{
		if(despawning == false && transform.childCount <= 0)
		{
			next();
		}
	}

	void checkValidIngredients()
	{
		if(despawning == true)
		{
			return;
		}
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

					if(activeSlots <= 0)
					{
						next();
					}
					return;
				}
			}
		}
	}

	private void next()
	{
		Debug.Log("Spawning next!");
		despawning= true;
		StartCoroutine(despawn());
	}

	IEnumerator despawn()
	{	
		yield return new WaitForSeconds(1f);
		spawner.spawnNewPattern();
		Destroy(gameObject);
	}
}
