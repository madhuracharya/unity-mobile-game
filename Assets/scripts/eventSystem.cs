using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventSystem : MonoBehaviour
{
	public delegate void eventDeligate();
	public eventDeligate onIngredientTrigger;	


	public void callOnIngredientTrigger()
	{
		IEnumerator trigger()
		{	
			yield return new WaitForSeconds(0);
			if(onIngredientTrigger != null)
			{
				onIngredientTrigger();
			}
		}
		StartCoroutine(trigger());
	}


}
