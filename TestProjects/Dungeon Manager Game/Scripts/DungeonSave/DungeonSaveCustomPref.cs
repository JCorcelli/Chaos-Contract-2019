using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Dungeon.Save
{
	
	public partial class DungeonSave : MonoBehaviour {
		public void LoadItemPreferences () {
			
			// this will revert preferences without altering the level
			
			
			// I expect preferences are saved on demand, so the use of this is limited to some special alternate preference case.
			
			
			if (loaded == null)
				Load();
			if (current == null ) 
			{
				LoadFile();
				return;
			}
			
			
			current.savedPreferences = loaded.savedPreferences;
			
			loaded = null;
			SaveTime();
			
			
		}
		public void SaveItemPreferences () {
			// save preferences without affecting the level
			
			if (current == null ) 
			{
				current = new SaveData();
			}
			
			if (loaded == null) Load();
			
			SaveData saveThis = loaded;
			
			loaded.savedPreferences = current.savedPreferences;
			
			Save(saveThis); // aka merged 
			loaded = null;
			SaveTime();
			
			
		}
		
		public void SavePreferenceMarkers () {
			// save preferences without affecting the level
			
			if (current == null ) 
			{
				current = new SaveData();
			}
			
			if (loaded == null) Load();
			
			SaveData saveThis = loaded;
			
			loaded.savedPreferences.customSaveMarkers = current.savedPreferences.customSaveMarkers;
			
			Save(saveThis); // aka merged 
			loaded = null;
			SaveTime();
			
			
		}
		
		
	}
}