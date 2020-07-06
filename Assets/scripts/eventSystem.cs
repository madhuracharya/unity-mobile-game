using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventSystem : MonoBehaviour
{
	public delegate void eventDeligate();
	public eventDeligate onIngredientTrigger;	
	public eventDeligate onRecipeBoardUpdate;
	public eventDeligate onRecipeChange;
	public eventDeligate onRecipeReady;
	public eventDeligate onRecipeListEmpty;


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

	public void callOnRecipeBoardUpdate()
	{
		IEnumerator trigger()
		{	
			yield return new WaitForSeconds(0);
			if(onRecipeBoardUpdate != null)
			{
				onRecipeBoardUpdate();
			}
		}
		StartCoroutine(trigger());
	}

	public void callOnRecipeChange()
	{
		IEnumerator trigger()
		{	
			yield return new WaitForSeconds(0);
			if(onRecipeChange != null)
			{
				onRecipeChange();
			}
		}
		StartCoroutine(trigger());
	}

	public void callOnRecipeReady()
	{
		IEnumerator trigger()
		{	
			yield return new WaitForSeconds(0);
			if(onRecipeReady != null)
			{
				onRecipeReady();
			}
		}
		StartCoroutine(trigger());
	}

	public void callOnRecipeListEmpty()
	{
		IEnumerator trigger()
		{	
			yield return new WaitForSeconds(0);
			if(onRecipeListEmpty != null)
			{
				onRecipeListEmpty();
			}
		}
		StartCoroutine(trigger());
	}

}
