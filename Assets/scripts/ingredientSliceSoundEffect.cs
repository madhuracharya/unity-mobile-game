using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class ingredientSliceSoundEffect : MonoBehaviour
{
	[SerializeField] private List<AudioClip> audioSources;

	void Start()
	{
		AudioSource source= GetComponent<AudioSource>();
		if(audioSources.Count > 0)
		{
			source.clip= audioSources[Random.Range(0, audioSources.Count)];
			source.Play();
		}
	}
}
