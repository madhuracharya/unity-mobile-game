using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JSONManager
{
	private static string path= Application.persistentDataPath + "/SND/" + "SND.json";

	public static void SaveIntoJson(levelData levelData)
	{
		string data = JsonUtility.ToJson(levelData);
		File.WriteAllText(path, data);
		Debug.Log("Data saved!");
	}

	public static levelData LoadFromJson()
	{
		levelData data = JsonUtility.FromJson<levelData>(File.ReadAllText(path));
		return data;
	}
}


	/*private class ingredient
	{
		public string ingredient;
		public int quantity;
	}	

	private class recipe
	{
		public List<ingredient> recipe;
	}

	private class level
	{
		public List<recipe> recipeList
	} */