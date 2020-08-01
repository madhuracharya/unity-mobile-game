using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class soundManager : MonoBehaviour
{
	[SerializeField] private AudioClip ingredientCollectSound;
	[SerializeField] private AudioClip recipeCompleteSound;
	[SerializeField] private AudioClip levelCompleteSound;
	[SerializeField] private AudioClip star1Sound;
	[SerializeField] private AudioClip star2Sound;
	[SerializeField] private AudioClip star3Sound;
	[SerializeField] private AudioClip UIButtonClick;
	[SerializeField] private AudioClip scoreTicker;

	private AudioSource audio;

	void Start()
	{
		audio= GetComponent<AudioSource>();
	}

	public void playIngredientCollectSound()
	{
		if(ingredientCollectSound == null) return;
		AudioSource.PlayClipAtPoint(ingredientCollectSound, new Vector3(0, 0, 0), 0.4f);
	}

	public void playRecipeCompleteSound()
	{
		if(recipeCompleteSound == null) return;
		AudioSource.PlayClipAtPoint(recipeCompleteSound, new Vector3(0, 0, 0), 1f);
	}

	public void playLevelCompleteSound()
	{
		if(levelCompleteSound == null) return;
		AudioSource.PlayClipAtPoint(levelCompleteSound, new Vector3(0, 0, 0), 1f);
	}

	public void playStar1Sound()
	{
		if(star1Sound == null) return;
		AudioSource.PlayClipAtPoint(star1Sound, new Vector3(0, 0, 0), 1f);
	}

	public void playStar2Sound()
	{
		if(star2Sound == null) return;
		AudioSource.PlayClipAtPoint(star2Sound, new Vector3(0, 0, 0), 1f);
	}

	public void playStar3Sound()
	{
		if(star3Sound == null) return;
		AudioSource.PlayClipAtPoint(star3Sound, new Vector3(0, 0, 0), 1f);
	}

	public void startScoreTicker()
	{
		if(scoreTicker == null && audio != null) return;
		audio.clip= scoreTicker;
		audio.Play();
	}

	public void playUIButtonClick()
	{
		if(UIButtonClick == null) return;
		AudioSource.PlayClipAtPoint(UIButtonClick, new Vector3(0, 0, 0), 1f);
	}

	public void stopScoreTicker()
	{
		if(scoreTicker == null && audio != null) return;
		audio.Stop();
		audio.clip= null;
	}
}

/*source.clip= recipeCompleteSound;
source.volume= 1f;
source.Play();*/
