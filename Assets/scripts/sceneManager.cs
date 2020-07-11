using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
	public Animator animator;
	public List<GameObject> objectsList;

	// Start is called before the first frame update
	void Start()
	{
		IEnumerator waitForOneSecond()
		{
			yield return new WaitForSeconds(1f);
			foreach(GameObject itm in objectsList)
			{
				if(itm != null) itm.SetActive(true);
			}
		}

		StartCoroutine(waitForOneSecond());
	}

	IEnumerator waitForTransition(int sceneIndex)
	{
		animator.SetTrigger("transition_start");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(sceneIndex);
	}

	public void nextScene()
	{
		Debug.Log("Loading next scene!");
		StartCoroutine(waitForTransition(SceneManager.GetActiveScene().buildIndex + 1));
	}
	public void restartScene()
	{
		Debug.Log("restarting currentt scene!");
		StartCoroutine(waitForTransition(SceneManager.GetActiveScene().buildIndex));
	}
}
