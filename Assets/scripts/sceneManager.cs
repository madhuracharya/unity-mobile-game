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

	IEnumerator waitForTransition(string sceneName)
	{
		animator.SetTrigger("transition_start");
		yield return new WaitForSeconds(1f);
		if(sceneName == "Self") SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		else SceneManager.LoadScene(sceneName);
	}

	public void play()
	{
		if(PlayerPrefs.GetInt("tutorialComplete") == 0)
			StartCoroutine(waitForTransition("Tutorial"));
		else
		{
			StartCoroutine(waitForTransition("gameLevel"));
		}
	}

	public void openLevelSelector()
	{
		Debug.Log("Loading Level Selector!");
		StartCoroutine(waitForTransition("level_selector"));
	}

	public void openTitleScreen()
	{
		Debug.Log("Loading title screen");
		StartCoroutine(waitForTransition("StartScreen"));
	}

	public void nextLevel()
	{
		Debug.Log("Loading next level!");
		StartCoroutine(waitForTransition("gameLevel"));
	}

	public void loadLevel(int level)
	{
		Debug.Log("Loading level: " + (level + 1));
		PlayerPrefs.SetInt("currentLevel", level);
		StartCoroutine(waitForTransition("gameLevel"));
	}

	public void restartScene()
	{
		Debug.Log("restarting currentt scene!");
		StartCoroutine(waitForTransition("Self"));
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
