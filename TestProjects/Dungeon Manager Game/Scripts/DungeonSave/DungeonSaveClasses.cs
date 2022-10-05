using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Dungeon.Save
{
	public interface Spawnable { 
		// for everything that's saved, and possibly ever will matter
		
		void Spawn();
		void Spawn(Vector3 pos);
		string GetName();
		string GetSavedName();
		void SetSavedName(string s);
		Spawnable IClone();
		
		void Save();
		string[] AsPreview();
	}
	
}
namespace Dungeon.Save
{
	
	
	// each class placed in a list or dictionary needs to be System.Serializable .
	// It (probably) makes sense to keep serializable classes for each thing that needs to be saved
	
	[System.Serializable]
	public class SaveV3 {
		public float x=0f;
		public float y=0f;
		public float z=0f;
		
		public SaveV3 (){}
		public SaveV3 (Vector3 v) {
			
			x = v.x;
			y = v.y;
			z = v.z;
		}
		
		public Vector3 ToVector3() {
			
			
			return new Vector3(x,y,z);
		}
	}
	
	[System.Serializable]
	public class SaveData {
		
		// At game start this class currently contains all the modified object information in the game. 
		// That might be too much to load.
		
		public Dictionary<string, SpawnedItems> sceneData = new Dictionary<string, SpawnedItems> (); // multiple item lists, one per scene
		
		// prefab defaults, given 
		
		public CustomPreferences savedPreferences = new CustomPreferences() ; // contains all preferences
		
		public string version;
		public bool success; // since I can't really check any faster
		
		public void SpawnLevel(string name) {
			success = sceneData.ContainsKey(name);
			
			if (success == false) return;
			
			sceneData[name].SpawnAll();
			
				
			
		}
		
		public SpawnedItems GetLevel(string name) {
			// for easy add/removal, remember this is by reference
			
			success = sceneData.ContainsKey(name);
			
			if (success == false) 
			{
				SpawnedItems sp = new SpawnedItems();
				sceneData[name] = sp; // This makes sense, otherwise, i need to initialize it with the keys
				return sp;
			}
			
			SpawnedItems data = sceneData[name];
			
			return data;
		}
		
		
		
		
		// unload scene? isSpawned should reset?
	}
	
	[System.Serializable]
	public class SpawnedItems {
		// if IClone isn't used, these are by reference, meaning it'll never add the same thing twice, even if invoked twice.
		// this means any changes are in fact auto-saved here even if I don't invoke save.
		
		// example of an item spawner class
		public List<Spawnable> obData = new List<Spawnable>();
		
		// public List[FakeOb2] ob2data; 
		
		
		public void Add(Spawnable a){
			if (obData.Contains(a)) return;
			obData.Add(a);
		}
		public void Remove(Spawnable a){
			if (!obData.Contains(a)) return;
			obData.Remove(a);
		}
		
		public void Replace(SpawnedItems newList){
			obData = newList.obData;
		}
		
		public void SpawnAll() {
			// we know exactly what prefab every list is for.
			//int count = 0;
			foreach (Spawnable f in obData)
			{
				f.Spawn();
				
			//	count++;
			}
			//Debug.Log(count+" items spawned");
		}
		
	}
	
	[System.Serializable]
	public class CustomPreferences {
		// With IClone (added for this) all saves are unique. 
		// All returned values NEED TO BE CLONED. Avoid the mess.
		
		// maintains the last save file, and item saved
		public string prefabName = "";
		public string savedName = "";
		public Spawnable selected;
		public Spawnable saved;
		
		public Dictionary<string, Spawnable> recent = new Dictionary<string, Spawnable>();
		
		public Dictionary<string, Spawnable> autoSave = new Dictionary<string, Spawnable>(); // a class with multiple item lists
		
		public Dictionary<string, Dictionary<string, Spawnable>> customSaves = new Dictionary<string, Dictionary<string, Spawnable>>() ; // a class with multiple item lists
		
		
		public Dictionary<string, Dictionary<string, int>> customSaveMarkers = new Dictionary<string, Dictionary<string, int>>() ; // a class with multiple item lists
		
		
		
		
		public Spawnable GetRecent(Spawnable a){
			if (recent == null) recent = new Dictionary<string, Spawnable>();
			
			string prefabName = a.GetName();
			
			if (!recent.ContainsKey(prefabName))
			{
				recent[prefabName] = a.IClone();
				
			}
			else
				a = recent[prefabName].IClone();
			
			return a;
		}
		
		public bool Has(Spawnable a) {
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			
			if (customSaves == null ) return false;
			
			if (!customSaves.ContainsKey(prefabName)) return false;
			
			return customSaves[prefabName].ContainsKey(savedName);
			
			
		}
		public void SetRecent(Spawnable a){
			if (recent == null) recent = new Dictionary<string, Spawnable>();
			prefabName = a.GetName();
			savedName = a.GetSavedName();
			
			saved = a.IClone();
			recent[prefabName] = a.IClone();
			
		}
		
		
		public void Save(Spawnable a){
			if (customSaves == null ) customSaves = new Dictionary<string, Dictionary<string, Spawnable>>();
			// save a listed item name
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			
			
			// need to initialize the prefab dict
			if (!customSaves.ContainsKey(prefabName))
				customSaves[prefabName] = new Dictionary<string, Spawnable>();
			
			customSaves[prefabName][savedName] = a.IClone();
			SetRecent(a);
		}
		
		public void SaveBak(Spawnable a){
			// an unlited autosave
			if (autoSave == null) autoSave = new Dictionary<string, Spawnable>();
			
			autoSave[a.GetName()] = a.IClone();
		}
		
		
		public Spawnable LoadBak(Spawnable a){
			if (autoSave == null) autoSave = new Dictionary<string, Spawnable>();
			
			if (autoSave.ContainsKey(a.GetName()))
				a = autoSave[a.GetName()].IClone();
			return a;
		}
		
		public Spawnable Load(Spawnable a){
			if (customSaves == null ) customSaves = new Dictionary<string, Dictionary<string, Spawnable>>();
			// listed name
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			
			// !! so far so good?
			
			
			// possible it doesn't exist?
			if (!customSaves.ContainsKey(prefabName))
			{
				return a;
				
				
			}
			
			else if (!customSaves[prefabName].ContainsKey(savedName))
			{
				return a;
			}
			else
			{
				a = customSaves[prefabName][savedName].IClone();
				
			}
			
			
			return a;
		}
		
		
		public void Add(Spawnable a){ 
			if (customSaves == null ) customSaves = new Dictionary<string, Dictionary<string, Spawnable>>();
			// avoid replacing
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			if (!customSaves.ContainsKey(prefabName)) Save(a);
			else if (!customSaves[prefabName].ContainsKey(savedName)) Save(a);
			
		}
		public void Remove(Spawnable a){
			if (customSaves == null ) customSaves = new Dictionary<string, Dictionary<string, Spawnable>>();
			// avoid erroring
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			if (!customSaves.ContainsKey(prefabName)) return;
			else if (!customSaves[prefabName].ContainsKey(savedName)) return;
			customSaves[prefabName].Remove(savedName);
		}
		
		public void AddMarker(Spawnable a, int marker)
		{
			if (customSaveMarkers == null ) customSaveMarkers = new Dictionary<string, Dictionary<string, int>>();
			
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			
			if (!customSaveMarkers.ContainsKey(prefabName))
				customSaveMarkers[prefabName] = new Dictionary<string, int>();
			
			
			if (customSaveMarkers[prefabName].ContainsKey(savedName))
				customSaveMarkers[prefabName][savedName] |=  marker;
			else
				customSaveMarkers[prefabName][savedName] =  marker;
			
			
			
			
			
		}
		public void RemoveMarker(Spawnable a, int marker)
		{
			if (customSaveMarkers == null ) customSaveMarkers = new Dictionary<string, Dictionary<string, int>>();
			
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			
			if (!customSaveMarkers.ContainsKey(prefabName))
				customSaveMarkers[prefabName] = new Dictionary<string, int>();
			
			if (customSaveMarkers[prefabName].ContainsKey(savedName))
				customSaveMarkers[prefabName][savedName] &=  ~marker;
			
			
			
			
		}
		
		public bool HasMarker(Spawnable a)
		{
			if (customSaveMarkers == null ) return false;
			
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			if (!customSaveMarkers.ContainsKey(prefabName)) return false;
			if (!customSaveMarkers[prefabName].ContainsKey(savedName)) return false;
			
			return true;
			
		}
		public bool IsMarked(Spawnable a, int comp)
		{
			int m = GetMarker(a);
			
			
			return ((m & comp) == comp); 
			
		}
		
		public int GetMarker(Spawnable a)
		{
			if (customSaveMarkers == null ) return 0;
			
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			if (!customSaveMarkers.ContainsKey(prefabName)) return 0;
			if (!customSaveMarkers[prefabName].ContainsKey(savedName)) return 0;
			
			return customSaveMarkers[prefabName][savedName];
			
		}
		
		public void DeleteMarkerEntry(Spawnable a)
		{
			if (customSaveMarkers == null ) return ;
			
			string prefabName = a.GetName();
			string savedName = a.GetSavedName();
			if (!customSaveMarkers.ContainsKey(prefabName)) return ;
			if (!customSaveMarkers[prefabName].ContainsKey(savedName)) return ;
			
			customSaves[prefabName].Remove(savedName);
			
		}
		
	
		
	}
	
	
	
	
		
	
}