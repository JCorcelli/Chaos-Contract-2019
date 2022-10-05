using UnityEngine;
using System.Collections;
using Utility.GUI;
using SelectionSystem;
using Dungeon;

namespace Dungeon.Save
{
	
	
	public class DoneButtonAnim: OneButtonDelayed {
		
		
		public bool active = false;
		//protected MenuHUBScripted hub;
		//protected MenuScripted form;
		
		protected FormNameField nameField;
		protected FormHub hub;
		
		protected virtual void Start() {
			
			nameField = GetComponentInParent<FormNameField>();
			//hub = GetComponentInParent<MenuHUBScripted>();
			//form = GetComponentInParent<MenuScripted>();
			
			if (hub == null)
				hub = GetComponentInParent<FormHub>();
			
			if (hub == null) Debug.LogError("hub needed", gameObject);
			
		}
		
		protected override void OnCall(){
			hub.Select();
			// how to call select without saving... the F it's impossible without a backup all function 
			
			// BACKUP ALL
			
			//Animator anim = form.GetComponent<Animator>();
			SpS d = new SpS();
			d.prefabName = nameField.prefabName;
			
			Spawnable bak = DungeonItems.preferences.LoadBak(d); // doesn't 
			
			bak.SetSavedName(nameField.savedName);
			
			
			DungeonItems.preferences.SaveBak(bak); // doesn't affect name field. Keep it like that?
			
			hub.Backup();
			
			DungeonItems.preferences.selected = DungeonItems.preferences.LoadBak(d); // may be replaced by something smarter
			
			Spawnable loaded = DungeonItems.preferences.Load(bak); // doesn't affect name field. Keep it like that?
			DungeonItems.preferences.SetRecent(loaded); // doesn't affect name field. Keep it like that?
			
			DungeonItems.instance.Save();
			
		}
		 
	}
}