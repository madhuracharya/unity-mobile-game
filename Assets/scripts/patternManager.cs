using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patternManager : MonoBehaviour
{
	spawnManager spawner;
	string name;
	GameObject slotParent;

	void Start()
	{
		spawner= GameObject.Find("spawnManager").GetComponent<spawnManager>();
		slotParent = GameObject.Find("slotParent");
		name= gameObject.name.Replace("(Clone)", "");
	}

	void Update()
	{
		switch (name)
		{
			case "linearMove":
			{
				break;
			}
			default:
			{
				int activeSlots= 0;

				foreach (Transform slot in slotParent.transform)
				{
					foreach(Transform spawnPoint in transform)
					{
						Transform ing= spawnPoint.GetChild(0);
						if(ing != null && ing.GetComponent<Alias>().alias == slot.GetComponent<Alias>().alias && slot.GetChild(2).gameObject.activeSelf == false)
						{
							activeSlots++;
						}
					}
				}

				if(activeSlots <= 0)
				{
					next();
				}

				break;
			}
		}

		if(transform.childCount <= 0)
		{
			next();
		}
	}

	private void next()
	{
		Destroy(gameObject);
		spawner.spawnNewPattern();
	}
}
