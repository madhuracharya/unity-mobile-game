using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class saveSystem
{
	private static string path= Application.persistentDataPath + "/SND/" + "SND_save" + ".bin";

	public static bool Save(profile saveData)
	{
		BinaryFormatter formatter= getBinaryFormatter();

		if(!Directory.Exists(Application.persistentDataPath + "/SND"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/SND");
		}

		FileStream file= File.Create(path);
		formatter.Serialize(file, saveData);
		file.Close();

		Debug.Log("saving at: " + path);

		return true;
	}	

	public static profile Load()
	{
		if(!File.Exists(path))
		{
			Debug.Log(path + " :Directory doesnt exist");
			return null;
		}
		else
		{
			BinaryFormatter formatter= getBinaryFormatter();
			FileStream file= File.Open(path, FileMode.Open);

			try
			{
				profile data= formatter.Deserialize(file) as profile;
				file.Close();
				return data;
			}
			catch
			{
				Debug.LogErrorFormat("Failed to load file at {0}", path);
				file.Close();
				return null;
			}
		}
	}

	public static BinaryFormatter getBinaryFormatter()
	{
		BinaryFormatter formatter= new BinaryFormatter();
		return formatter;
	}
}
