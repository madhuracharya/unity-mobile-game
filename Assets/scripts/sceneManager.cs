using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
	private int levelSceneIndex= 3;
	private int levelSelectorScene= 2;
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

	public void openLevelSelector()
	{
		Debug.Log("Loading Level Selector!");
		StartCoroutine(waitForTransition(levelSelectorScene));
	}

	public void nextLevel()
	{
		Debug.Log("Loading next level!");
		PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);
		StartCoroutine(waitForTransition(levelSceneIndex));
	}

	public void loadLevel(int level)
	{
		Debug.Log("Loading level: " + (level + 1));
		PlayerPrefs.SetInt("currentLevel", level);
		StartCoroutine(waitForTransition(levelSceneIndex));
	}

	public void loadScene(int sceneIndx)
	{
		StartCoroutine(waitForTransition(sceneIndx + 3));
	}

	public void restartScene()
	{
		Debug.Log("restarting currentt scene!");
		StartCoroutine(waitForTransition(SceneManager.GetActiveScene().buildIndex));
	}

	public void saveJSON(ingredient[] ingList, int numbOfLevels)
	{
		JSONManager.SaveIntoJson(new levelData(ingList, numbOfLevels));
	}

	public List<recipe> loadJSON(ingredient[] ingList, int levelNmber)
	{
		levelData prof= JSONManager.LoadFromJson();
		if(prof != null)
		{
			return JSONManager.getLevelData(ingList, prof, levelNmber);
		}
		else
		{
			Debug.Log("failed to load level data");  
			return new List<recipe>();
		}
	}
}
