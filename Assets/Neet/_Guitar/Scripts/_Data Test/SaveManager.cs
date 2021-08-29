using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.FileManagement;
using System;
using System.Linq;

public class SaveManager : MonoBehaviour
{
	[System.Serializable]
	public class SaveFile
	{
		public string fileName;
		public string path;
		public List<object> data;
	}


	private static SaveManager _instance;
	private SaveFile _saveFile;


	public static SaveManager instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<SaveManager>();
			return _instance;
		}
	}
	public SaveFile saveFile
	{
		get
		{
			if (_saveFile == null)
				_saveFile = Load();

			if (_saveFile.data == null)
				_saveFile.data = new List<object>();
			return _saveFile;
		}
	}
	public static string path
	{
		get
		{
			return FileManager.PersistentDataPath + "data.sav";
		}
	}

	public SaveFile Load()
	{
		if (saveFile == null)
			FileManager.DeserializeBinary<SaveFile>(out _saveFile, path);


		return saveFile;
	}
	public void Save()
	{
		FileManager.SerializeBinary(saveFile, saveFile.path);
	}

	public void AddData(object o)
	{
		saveFile.data.Add(o);
	}
	public void Remove(object o)
	{
		try
		{

			var toRemove = saveFile.data.Where(obj => obj == o);
			print("Deleting " + toRemove.Count().ToString() + "...");
			foreach (var item in toRemove)
				saveFile.data.Remove(o);
		}
		catch (Exception e)
		{
			Debug.LogError("Error removing data\n" + e.Message);
		}
	}

	public void OpenFileLocation()
	{

	}

	public T GetData<T>()
	{
		var found = GetAll<T>();
		if (found.Count >= 1)
			return found[0];
		else
			Debug.LogWarning("No data found, returning default");
		return default(T);
	}
	public List<T> GetAll<T>()
	{
		return (List<T>)saveFile.data.Where(obj => obj.GetType() == typeof(T));
	}

	public string GetObjects()
	{
		string s = "";
		s += saveFile.data.Count.ToString() + " objects serialized:\n";

		foreach (object obj in saveFile.data)
		{
			try
			{
				s += obj.GetType().ToString();
			}
			catch
			{
				s += "????";
			}
			s += "\n";
		}
		return s;
	}
}
