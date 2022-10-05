using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

using Dungeon;

namespace Dungeon.Save
{
	public class DungeonUIFormExitSLoad : GenericListOpenButton {
		// needs to generate the list. easily done by taking variables from the GenericListNode
		
		public Transform formPrefab;
		public Transform previewPrefab;
		protected List<string> deleted = new List<string>();

		public string prefabName = "ExitS"; // helps build the generic item list
		
		[System.Flags]
		public enum Mark {
			NONE = 0 << 0,         // 1 << 0,
			FAVORITE = 1 << 1,     // 1 << 1,
			DELETE = 1 << 2        // 1 << 2,
			                       // 
			                       // 1 << 3,
			 
		}
		
		protected override void OnEnable( ){
			base.OnEnable();
			DungeonItems.onSave += OnItemSave;
		}
		protected override void OnDisable( ){
			base.OnDisable();
			DungeonItems.onSave -= OnItemSave;
			
		}
		protected void OnItemSave() {
			if (list == null) return;
			
			Spawnable sp = DungeonItems.preferences.saved;
			if (sp == null) return;
			
			if (sp.GetName() == prefabName)
			{
				int i = list.GetList().IndexOf(sp.GetSavedName());
				
				if (i < 0)
					list.Add(sp.GetSavedName());
				else
				{
					// find what I saved...
					list.SetSelected(i);
				}
			}
		}
		protected override void Build() {
			// something like this on enable
			if (list == null) return;
			if (!DungeonItems.preferences.customSaves.ContainsKey(prefabName)) return;
			
			List<string> elements = new List<string>(DungeonItems.preferences.customSaves[prefabName].Keys) ;
			
			// if hidden
			//if (elements.Contains(s))
			//	elements.Remove(s);
			
			
			list.Build(elements);
			stringSelection = "";
			intSelection = -1;
			
			// if colored
			
			list.ClearMarkers();
			
			
			SpS n = new SpS();
			foreach (string e in elements)
			{
				n = NewSpS(e);
				if (!DungeonItems.preferences.HasMarker(n)) continue;
				
				
				list.SetMarker(GetColorMark(e), e); // setmarker changes colors during a build
				
			}
			
			
			
		}
		
		protected void Remark(string s)
		{
			// called by single changes usually
			list.Recolor(GetColorMark(s),s);
			DungeonSave.instance.SavePreferenceMarkers();
			
		}
		protected Color GetColorMark(string e)
		{
			
			Color c = Color.white;
			
			SpS n = NewSpS(e);
			if (!DungeonItems.preferences.HasMarker(n)) return c;
			
			
			if (DungeonItems.preferences.IsMarked(n, (int)Mark.DELETE))
			{
				c = Color.black;
				if (!deleted.Contains( e)) deleted.Add( e);
			}
			else if (DungeonItems.preferences.IsMarked(n, (int)Mark.FAVORITE)) 
				c = Color.green;
			
			
			
			
			return c;
			
			
		}
		
		// this happens the instant someone mouses down
		// this is the only time I can compare previous and current selection
		
		protected override void OnSelect(int i) { }
		
		protected override void OnSelectString(string s) {
			
			DungeonPreviewItem prev = DungeonPreviewItem.instance;
			
			if (prev == null) return; // do nothing
			/* 
			if Preview doesn't exist
				StartPreview
			
			replace previewNode
			
			
			make it look like a boring Carbon copy
			
			-----------------
			# Preview window:
			-----------------
			#	item 		Exit Spawner
			#	saved as	Coolio
			#	goes to		department store
			-----------------
			*/
			
			
			SpS exit = NewSpS(stringSelection);
			
			Spawnable item = DungeonItems.preferences.Load(exit); 
			
			
			prev.Run(item.AsPreview());
			
			
			
		}
		
		protected override void OnUpdate() {
			base.OnUpdate();
			
			
			if (message.ToLower() == "undo" && deleted.Count > 0)
				
			{
				// this can't recover extensive item data changes
				OnUndo();
			}
			else if (message.ToLower() == "save")
				
			{
				
				DeleteFinal();
				DungeonSave.instance.SaveItemPreferences(); // markers and valid items change. But it won't get a callback. 
			}
			else if (message.ToLower() == "close")
				
			{
				
				Close(); // 
			}
			
			if (list == null || intSelection < 0) return;
			
			
			if (message.ToLower() == "delete")
				
			{
				// selection -1 unless I update on late
				
				DeleteProc();
			}
			else if (message.ToLower() == "favorite")
				
			{
				// selection -1 unless I update on late
				
				Favorite();
			}
			
			// really. add an ok button, and a way to check if a button is pressed. please.
			else if (message.ToLower() == "open")
			{
				SetPreferences(); 
				
				// ChangeParentForm();
				GenerateForm();
			}
			
		}
		

		protected void OnUndo() {
			List<string> saved = list.GetList();
			
			string s = deleted[deleted.Count -1];
			deleted.RemoveAt(deleted.Count -1);
			
			DungeonItems.preferences.RemoveMarker(NewSpS(s), (int)Mark.DELETE);
			
			if (!saved.Contains(s))
			{
				// hidden, or completely deleted
				list.Add(s);
			
			}
			Remark( s);
			
			
		}
				
		protected void Favorite() {
			SpS item = NewSpS(stringSelection);
			
			if (DungeonItems.preferences.IsMarked(item, (int)Mark.FAVORITE))
			{
				
				DungeonItems.preferences.RemoveMarker(item, (int)Mark.FAVORITE);
				
			}
			else
			{
				DungeonItems.preferences.AddMarker(item, (int)Mark.FAVORITE);
				
			}
			Remark(stringSelection);
		}
		
		protected SpS NewSpS(string n){
			
			SpS item = new SpS();
			item.prefabName = prefabName;
			item.SetSavedName(n);
			return item;
			
		}
		protected void DeleteProc() {
			
			if (deleted.Contains(stringSelection))
			{
				deleted.Remove(stringSelection);
				DungeonItems.preferences.RemoveMarker(NewSpS(stringSelection), (int)Mark.DELETE);
				
				
				// if hidden
				//list.Add(intSelection);
			}
			else
			{
				deleted.Add(stringSelection);
				DungeonItems.preferences.AddMarker(NewSpS(stringSelection), (int)Mark.DELETE);
				
				
				// if hidden
				//list.Delete(intSelection);
				// else // recolor
				
			}
			Remark(stringSelection);
			
			
		}
		
		
		protected void DeleteFinal() {
			
			// permanent deletion
			foreach (string s in deleted)
			{
				SpS exit = NewSpS(s);
			
				DungeonItems.preferences.Remove(exit);
				DungeonItems.preferences.DeleteMarkerEntry(exit);
				
				
			}
			deleted.Clear();
			Build();
				
		}
		
		
		
		protected void SetPreferences() {
			// set recent and backup
			
			
			SpS exit = NewSpS(stringSelection);
			
			Spawnable item = DungeonItems.preferences.Load(exit) ; 
			
			DungeonItems.preferences.SetRecent(item); 
			
			DungeonItems.preferences.SaveBak(item); // changes form
			// necessary? // DungeonItems.instance.Save();
		}
		
		protected void GenerateForm() {
			// a free-floating form. When selecting means make one appear.
			
			Transform t = Object.Instantiate(formPrefab) as Transform;
			
			if (!t.gameObject.activeSelf)
				t.gameObject.SetActive(true);
			
		}
		
			
		protected void ChangeParentForm() {
			// this changes the values on the form. For when there's a load button on a form.
			FormHub hub = GetComponentInParent<FormHub>();

			hub.Load(); // refreshes form
			
		}
		
		

	}
}