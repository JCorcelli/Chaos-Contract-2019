using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace Dungeon.SaveExample
{
	// each class placed in a list or dictionary needs to be System.Serializable .
	// It (probably) makes sense to keep serializable classes for each thing that needs to be saved
	
	[System.Serializable]
	public class TempSaveData {
		
		// base menus, given
		
		public Dictionary<string, SpawnedItems> sceneData; // multiple item lists, one per scene
		
		// prefab defaults, given 
		
		public Dictionary<string, CustomPreferences> savedPreferences; // prefab properties multiple per prefab
		
		
		
	}
	
	[System.Serializable]
	public class SpawnedItems {
		// example of an item spawner class
		public List<FakeOb> ob1data;
		public List<FakeOb> ob2data;
		// public List[FakeOb2] ob2data; 
		
		protected void SpawnAll() {
			SpawnAllType1();
			SpawnAllType2();
		}
		
		protected void SpawnAllType1() {
			// we know exactly what prefab every list is for.
			foreach (FakeOb f in ob1data)
			{
				// i = instance
				// i.parent ?
				// i.property1 = f.property1
				
				Debug.Log("this was just a test: "+f.ToString());
				
			}
		}
		protected void SpawnAllType2() {}
		
	}
	
	[System.Serializable]
	public class CustomPreferences {
		
		public Dictionary<string, string[]> preferenceNames; // a class with multiple item lists
		
		public Dictionary<string, FakeOb> preferences; // a class with multiple item lists
		public Dictionary<string, FakeOb> preferences2; // a class with multiple item lists
		
		
		protected void NamesToMenu(string s) {
			//var list = new List<string>();
			//Dialogue.Generate(s), fit the menu to the item
			//Dialogue.Default
			foreach (string itemName in preferenceNames[s])
			{
				Debug.Log(itemName);
				// increase the menu dialogue
				// list.Add(itemName) ?
			}
			
			//I could set a delegate and skip guessing what's what
		}
		protected void PropertiesToBuffer(string prefabName, string saveName) {
			
			// here I don't know what kind of item it is
			// the calling script could know, and access this class' correct dictionary
			
			if (prefabName == "FakeOb")
			{
				//FakeOb item = preferences[saveName];
				
				// dialogue.PropertiesToMenu(item) //? fill it with the preferences
					
			}
			// set the menu dialogue
			
		}
		
	
		
	}
	
	
	[System.Serializable]
	public class SaveData {
		
		// example class
		public Dictionary<string, int[]> saved;
		public List<SaveOb> so;
		public List<SaveOb2> so2;
		
	}
	
	[System.Serializable]
	public class FakeOb {
		
		public string name = "Unnamed";
		
	}
	
	/*
	################################
	################################
	################################
	################################
	*/
	[System.Serializable]
	public class SaveOb {
		
		public Dictionary<string, int[]> saved;
		
	}
	
	[System.Serializable]
	public class SaveOb2 {
		
		public Dictionary<string, int[]> saved;
		
	}
	
	public class SaveDictToFile : MonoBehaviour {

		
		protected Dictionary<string, int[]> loaded ;
		protected Dictionary<string, int[]> saved ;
	
		// protected SaveData sd = new SaveData();
		protected TempSaveData sd = new TempSaveData();
		
		
		
		protected void Start () {
		
			// sd.saved= new Dictionary<string, int[]>();
			// saved = sd.saved;
			// 
			// saved.Add("one",new int[]{1,2,3,4,5});
			// saved.Add("two",new int[]{6,7,8,9,10});
			// saved.Add("three",new int[]{11,12,13,14,15});
			// sd.so = new List<SaveOb>(){new SaveOb()};
			// 
			// sd.so2 = new List<SaveOb2>(){new SaveOb2()};
			// sd.so2.Add(new SaveOb2());
			
			Save();
			Load();
		}
		
		
		protected void Save () {
			

			BinaryFormatter binary  = new BinaryFormatter();
			FileStream file         = File.Create(Application.persistentDataPath +"/ChaosSaveTest2.dat"); //the creates a file in the unity app data path

			binary.Serialize(file, sd);
			file.Close();

		}
		protected void Load()
		{
			Debug.Log(Application.persistentDataPath + "/ChaosSaveTest2.dat");
			
			if (!File.Exists(Application.persistentDataPath + "/ChaosSaveTest2.dat")) 
			{
				Debug.LogError("couldn't load");
				return;

			}
			BinaryFormatter binary  = new BinaryFormatter();
			FileStream file         = File.Open(Application.persistentDataPath + "/ChaosSaveTest2.dat", FileMode.Open);
			TempSaveData dat = (TempSaveData)binary.Deserialize(file);
			file.Close();
			Debug.Log(dat.ToString());
			
			
			
		 
		}
	}
		
	
}