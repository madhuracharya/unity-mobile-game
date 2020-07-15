using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class profile
{
	public List<float> levelData;
	public int currentlevel; 

	public profile()
	{
		this.currentlevel= 0;
		this.levelData= new List<float>();

		for(int i= 0; i < 50; i++)
		{
			levelData.Add(0f);
		}
	}

	public profile updateLevel(int level, float score )
	{
		if(levelData.Count > level)
		{
			levelData[level]= score;
		}
		else
		{
			levelData.Add(score);
		}
		
		return this;
	}

	public profile setCurrentLevel(int lvl)
	{
		currentlevel= lvl;
		return this;
	}

	public profile updateProfile(List<float> lvlDta, int curr)
	{
		levelData= lvlDta;
		currentlevel= curr;

		return this;
	}
}

