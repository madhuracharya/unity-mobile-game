using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	public void nextScene()
	{
		Debug.Log("Loading next scene!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	public void restartScene()
	{
		Debug.Log("restarting currentt scene!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
