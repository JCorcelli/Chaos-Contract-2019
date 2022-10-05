using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dungeon.Save;

namespace Dungeon //.Items
{
	
	//instance
	
	public delegate void DungeonItemsDelegate();
	
	public class DungeonItems : MonoBehaviour {
		
		// should have a list of all original resources, and maybe a note
		
		public static DungeonItemsDelegate onSave;
		
		public static CustomPreferences preferences; // backup == autosave, but a pointer to the current should persist
		public static DungeonItems instance; // current == savefile
		
		
		// prefab List <- complicated later, for now, it can be anywhere, it's one thing.
		
		// saved name list (exists in save classes)
		
		
		//*** DeBUG
		public static void Spawn(Vector3 pos) {
			
			
			Dungeon.Save.Spawnable item = preferences.selected;
			
			if (item != null)
			{
				// or maybe i deselect?
				
				item.Spawn(pos); // should instantiate and make it plunk down here.
				item.Save(); // adds item to list, doesn't save the game
			}
		}
		
		public bool active = true;
		protected void Awake() {
			if (instance != null) return;
			
			
			instance = this;
			
			
			// the current item is essentially the backup file, always
			InitLoadPreferences();
			
		}
		
		public void InitLoadPreferences() {
			// UHHHHHHH
			DungeonSave file = DungeonSave.instance;
			// load backup?
			if (file.current == null) Debug.LogError("The DngeonVars.current didn't load, wtf.", gameObject);
			
			preferences = file.current.savedPreferences;
		}
		
		
		
		
		
		//GetItem, needs to be converted
		
		public void Save() {
			// user initiated save. preference or inventory items, I suppose.
			DungeonSave file = DungeonSave.instance;
			
			file.SaveItemPreferences ();
			
			if (onSave != null) onSave();
		}
		
		
		
	}
}