using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class soundManager : MonoBehaviour
{
	[SerializeField] private AudioClip ingredientCollectSound;
	[SerializeField] private AudioClip recipeCompleteSound;

	public void playIngredientCollectSound()
	{
		if(ingredientCollectSound == null) return;
		AudioSource.PlayClipAtPoint(ingredientCollectSound, new Vector3(0, 0, 0), 0.4f);
	}

	public void playRecipeCompleteSound()
	{Debug.Log("Works");
		if(recipeCompleteSound == null) return;
		AudioSource.PlayClipAtPoint(recipeCompleteSound, new Vector3(0, 0, 0), 1f);
	}
}

/*source.clip= recipeCompleteSound;
source.volume= 1f;
source.Play();*/
