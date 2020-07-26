using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelSelector : MonoBehaviour
{
	[SerializeField] private GameObject levelContainer;
	[SerializeField] private GameObject levelButton;
	[SerializeField] private Transform swiper;

	public int totalLevels= 50;
	private int levelCount;
	private int padding= 20;
	private Rect actualButtonDimentions;
	public profile player;

	void Start()
	{
		Camera camera = Camera.main;

		float camHeight = camera.orthographicSize * 2f;
		float camWidth = camera.aspect * camHeight;

		player= loadData();

		Rect pannelDimentions= levelContainer.GetComponent<RectTransform>().rect;
		actualButtonDimentions= levelButton.GetComponent<RectTransform>().rect;
		Rect buttonDimentions= new Rect(0, 0, levelButton.GetComponent<RectTransform>().rect.width + padding, levelButton.GetComponent<RectTransform>().rect.height + padding);
		int maxLevelsPerRow= Mathf.FloorToInt(pannelDimentions.width / buttonDimentions.width);
		int maxLevelsPerCollumn= Mathf.FloorToInt(pannelDimentions.height / buttonDimentions.height);
		int levelsPerPage= maxLevelsPerRow * maxLevelsPerCollumn;
		int totalPages= Mathf.CeilToInt((float)totalLevels /  levelsPerPage);

		Debug.Log("Total pages: " + totalPages + ", levels per page: " + levelsPerPage);

		createPages(totalPages, levelsPerPage, pannelDimentions, buttonDimentions);

		int currentIndex= 0;
		float previousScore= 100;

		foreach(Transform page in transform.GetChild(0))
		{
			foreach(Transform itm in page)
			{
				if(previousScore >= 50)
				{
					itm.GetChild(1).gameObject.SetActive(true);
					itm.GetChild(2).gameObject.SetActive(true);
					itm.GetChild(3).gameObject.SetActive(true);
					itm.GetComponent<Button>().interactable= true;
					previousScore= itm.GetComponent<Alias>().score;
				}
				currentIndex++;
			}
		}
		Destroy(levelContainer);
	}

	void createPages(int totalPages, int levelsPerPage, Rect pannelDimentions, Rect buttonDimentions)
	{
		GameObject pannelClone= Instantiate(levelContainer) as GameObject;

		for(int i= 0; i < totalPages; i++)
		{
			GameObject pannel= Instantiate(pannelClone) as GameObject;
			pannel.transform.SetParent(transform, false);
			pannel.transform.SetParent(swiper);
			//pannel.transform.SetParent(levelContainer.transform);
			pannel.name= "Page_" + i;
			
			GridLayoutGroup grid= pannel.AddComponent<GridLayoutGroup>();
			grid.cellSize= new Vector2(actualButtonDimentions.width, actualButtonDimentions.height); 
			grid.childAlignment= TextAnchor.MiddleCenter;
			grid.spacing= new Vector2(padding, padding);

			pannel.GetComponent<RectTransform>().localPosition= new Vector2(pannelDimentions.width * (i), 0);

			int levelnumb= i == (totalPages - 1) ? (totalLevels - levelCount) : levelsPerPage;

			loadButtons(levelnumb, pannel);
		}

		Destroy(pannelClone);
	}

	void loadButtons(int numOfIcons, GameObject parent)
	{
		for(int i= 0; i < numOfIcons; i++)
		{
			int buttonLvl= levelCount;
			GameObject level= Instantiate(levelButton) as GameObject;
			level.transform.SetParent(transform, false);
			level.transform.SetParent(parent.transform);
			level.name= "Level_" + levelCount;
			level.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= "Level " + (levelCount + 1);
			level.GetComponent<Button>().onClick.AddListener( () => {
				sceneManager sceneManager= Camera.main.GetComponent<sceneManager>();
				sceneManager.loadLevel(buttonLvl);
			});

			if(player.levelData.Count > i )
			{
				updateLevelUI(player.levelData[levelCount], level);
			}

			levelCount++;
		}
	}

	void updateLevelUI(float score, GameObject levelUI)
	{
		if(levelUI == null) return;

		Image star1= levelUI.transform.GetChild(1).GetComponent<Image>();
		Image star2= levelUI.transform.GetChild(2).GetComponent<Image>();
		Image star3= levelUI.transform.GetChild(3).GetComponent<Image>();

		if(score > 50)
		{
			star1.gameObject.SetActive(true);
			star2.gameObject.SetActive(true);
			star3.gameObject.SetActive(true);
			levelUI.transform.GetChild(4).gameObject.SetActive(false);
			levelUI.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text= score + "%";
			levelUI.GetComponent<Alias>().score= score;

			star1.color = new Color(0.7f,0.4f,0f,1f);

			if(score > 66)
			{
				star2.color = new Color(0.7f,0.4f,0f,1f);

				if(score > 85)
				{
					star3.color = new Color(0.7f,0.4f,0f,1f);
				}
			}
		}
		else
		{
			levelUI.GetComponent<Button>().interactable= false;
		}
	}

	public void saveData()
	{
		saveSystem.Save(player);
	}

	public profile loadData()
	{
		profile prof= saveSystem.Load();
		if(prof != null)
		{
			Debug.Log(prof.currentlevel + " : " + prof.levelData);
			return prof;
		}
		else
		{
			Debug.Log("failed to load profile data.Creating new!");  
			return new profile();
		}
	}

}




