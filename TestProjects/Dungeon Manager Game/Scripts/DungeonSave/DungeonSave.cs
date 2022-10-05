using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;


namespace Dungeon.Save
{
	
	public partial class DungeonSave : MonoBehaviour {

		
	
		// pretty much the entire game, twice, in one file, starting to wonder if that's a bit much
		public SaveData current ;
		public SaveData loaded ;
		
		public static DungeonSave instance;
		public string version = "0";
		
		public float saveTime;
		public bool autoSave = true;
		// public float delayTime = 2; added time before autosaving
		
		protected void Awake () {
			if (instance != null)
			{
				Destroy(this);
			
				return;
			}
			
			instance = this;
		
			LoadFile();
			
			// and then something spawns... ?
			
		}
		
		// new level would wipe the information?
		public void SaveLevel () {
			// should save the information for the level only, data is modified
			if (current == null ) 
			{
				current = new SaveData();
			}
			
			if (loaded == null) Load();
			
			SaveData saveThis = loaded;
			
			// stuff passed by reference here
			SpawnedItems savedState = loaded.GetLevel(DungeonVars.level);
			
			SpawnedItems currentLevel = current.GetLevel(DungeonVars.level);
			
			
			savedState.Replace(currentLevel); // old is now new
			
			Save(saveThis); // aka merged 
			loaded = null;
			SaveTime();
		}
		public void LoadLevel () {
			// opposite of save level, but only affects this level
			if (loaded == null)
				Load();
			if (current == null ) 
			{
				LoadFile();
				return;
			}
			// stuff passed by reference here
			SpawnedItems savedState = loaded.GetLevel(DungeonVars.level);
			
			SpawnedItems currentLevel = current.GetLevel(DungeonVars.level);
			
			currentLevel.Replace(savedState); //  new is now old
			loaded = null;
			SaveTime();
			
			
		}
		public void EraseLevel () { // new level, clear level
			
			SpawnedItems currentLevel = current.GetLevel(DungeonVars.level);
			currentLevel.Replace (new SpawnedItems());
			
		}
		
		protected void SaveTime() {
			saveTime = Time.time;
		}
		public void LoadFile() {
			// resets all progress
			Load(); 
			
			current = loaded;
			
			loaded = null;
			SaveTime();
		}
		public void SaveFile() {
			// overwrites file
			if (current == null) 
			{
				Debug.Log("nothing to save", gameObject); 
				return;
				
			}
			Save(current);
			loaded = null; // old information, current is always loaded, but not vice-versa
			SaveTime();
			
		}
		
		
		protected string saveFile = "/ChaosSaveTest.dat";
		protected string backupFile = "/ChaosSaveTest.bak";
		protected void Save (SaveData sd) {
			// saves literally everything, overwrites everything

			BinaryFormatter binary  = new BinaryFormatter();
			FileStream file         = File.Create(Application.persistentDataPath +saveFile); //the creates a file in the unity app data path

			sd.version = version;
			binary.Serialize(file, sd);
			file.Close();

		}
		protected void Load()
		{
			// reload / load everything, but erases the work that's been done.
			// Debug.Log(Application.persistentDataPath + saveFile);
			
			if (!File.Exists(Application.persistentDataPath + saveFile)) 
			{
				Debug.Log("no file information exists, loaded will be blank file.", gameObject);
				
				
				loaded = new SaveData();
				return;

			}
			
			BinaryFormatter binary  = new BinaryFormatter();
			FileStream file         = File.Open(Application.persistentDataPath + saveFile, FileMode.Open);
			SaveData dat = (SaveData)binary.Deserialize(file);
			file.Close();
			
			if (dat.version != version)
			{
				
				// autoSave = false; // avoid deleting the File
				// maybe creat a backup file instead
				FileStream bfile  = File.Create(Application.persistentDataPath +backupFile); //the creates a file in the unity app data path

				binary.Serialize(bfile, dat);
				bfile.Close();
				Debug.Log("Different version. Data is backed up but info may be lost.");
			
			}
			
			loaded = dat;
			
			
		 
		}
	
	}
	
	
	
}