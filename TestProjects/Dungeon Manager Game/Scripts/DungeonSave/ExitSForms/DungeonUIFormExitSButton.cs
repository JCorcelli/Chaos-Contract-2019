using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Dungeon.Save
{
	
		
	public class DungeonUIFormExitSButton:  SelectNodeAbstract {
		//perhaps split saveAll off considering it is completely different
		
		public enum ExitSEnum { 
			SaveAs  	=0 ,
			ToLevel   	=1 ,
			OtherExit   =2 ,
			ThisExit   	=3 ,
			Display     =4
		}
			
		
		protected Color defaultColor = Color.white;
		
		public Image image;
		public InputField text;
		public FormHub hub;
		
		public string savedName = "Default";
		
		public string stringVar = "Unknown";
		public string savedVar = "Unknown";
		
		protected virtual void Start() {
			
			if (image == null)
				image = GetComponent<Image>();
			
			
			
			Retrieve();
			
			prevStringVar = stringVar;
			
			if (text != null)
				text.text = stringVar;
		}
		protected override void OnEnable() {
			base.OnEnable();
			
			if (image == null)
				image = GetComponent<Image>();
			
			
			if (hub == null)
				hub = GetComponentInParent<FormHub>();
			
			if (hub == null) Debug.LogError("hub needed", gameObject);
			
			hub.onLoad += OnLoad;
			hub.onSave += OnSave;
			hub.onBackup += Backup;
		}
		protected override void OnDisable() {
			base.OnDisable();
			
			
			if (hub != null)
			{
				hub.onLoad -= OnLoad;
				hub.onSave -= OnSave;
				hub.onBackup -= Backup;
			}
		}
		
		protected override void OnUpdate() {
			base.OnUpdate();
			SetStringFromText();
		}
		
		public string prevStringVar = "";
		protected void SetStringFromText() {
			if (text.text == "" ) return;
			stringVar = text.text;
			
			if (prevStringVar == stringVar) return;
			
			prevStringVar = stringVar;
			
			// each click will save, to some degree, well then...
			
			
			
			Backup(); // always save current progress to RAM, it's better than nothing. Saving every second will take smaller file sizes.
			
			if (hub.isSelected)
			{
				ExitS d = new ExitS();
				d = DungeonItems.preferences.LoadBak(d) as ExitS;
				DungeonItems.preferences.selected = d; // may be replaced by something smarter
			}
			
			
			Recolor();
			
		}
		
		public ExitSEnum selectMethod = (ExitSEnum)0;
		protected override void OnClick() {
			
			
			// each click will guarantee a save
			
			SetItem();
			
			
			DungeonItems.instance.Save();
		}
		public void Saved(){
			// for convenience/safety, auto-save every single change to a <previous save>. This is the user's save.
			savedVar = stringVar;
			if (image != null) image.color = defaultColor;
			
			
		}
		public void Recolor(){
			
			// assuming it was set up properly the item is saved once. But for convenience I should probably auto-save every single change to a <previous save>.
			if (image == null ) return;
			
			bool changed = false;
			if (savedVar != stringVar) changed = true;
			
			if (changed && ((int)selectMethod == 0 ))
			{
				ExitS named = new ExitS();
				named.SetSavedName(stringVar);
				
				if (DungeonItems.preferences.Has(named))
					image.color = Color.red;
				else
					image.color = defaultColor;
				return;	
			}
			
			
			if (changed)
				image.color = Color.red;
			else
				image.color = defaultColor;
		}
		
		
		
		
		protected void Backup(){
			ExitS bak = new ExitS();
			bak = DungeonItems.preferences.LoadBak(bak) as ExitS;
			
			
			int changed = (int)selectMethod;
			if (changed == 0) bak.savedName  = stringVar;
			
			else if (changed == 1) bak.level = stringVar;
			else if (changed == 2) bak.exit  = stringVar;
			else if (changed == 3) bak.exit2 = stringVar;
			else if (changed == 4) bak.title = stringVar;
			
			DungeonItems.preferences.SaveBak(bak);
		}
		
		
		protected void OnLoad() {
			Retrieve();
			prevStringVar = stringVar;
			if (text != null)
				text.text = stringVar;
			
		}
		protected void OnSave() {
			ExitS d = new ExitS();
			
			d = DungeonItems.preferences.LoadBak(d) as ExitS;
			
			savedName = d.GetSavedName();
			
			if ((int)selectMethod == 0 ) 
			{
				text.text = savedVar = stringVar = savedName;
				return; // avoid inf. recursion
			}
			Saved();
			
			
			SetItem();
		}
		public void Retrieve() {
			// sync the variables with the actual dungeon item
			
			ExitS bak = new ExitS();
			bak = DungeonItems.preferences.LoadBak(bak) as ExitS;
			
			ExitS copy = bak.Clone();
			
			savedName = copy.savedName;
			if ((int)selectMethod == 0 ) stringVar = copy.savedName;
			
			else if ((int)selectMethod == 1) stringVar = copy.level;
			else if ((int)selectMethod == 2) stringVar = copy.exit;
			else if ((int)selectMethod == 3) stringVar = copy.exit2;
			else if ((int)selectMethod == 4) stringVar = copy.title;
			
			
			ExitS s = new ExitS();
			s = DungeonItems.preferences.GetRecent(s) as ExitS;
			// ie the last time I saved something of this type
			
			
			
			copy = s.Clone();
			
			
			
			if ((int)selectMethod == 0 ) savedVar = copy.savedName;
			else if ((int)selectMethod == 1) savedVar = copy.level;
			else if ((int)selectMethod == 2) savedVar = copy.exit;
			else if ((int)selectMethod == 3) savedVar = copy.exit2;
			else if ((int)selectMethod == 4) savedVar = copy.title;
			
			Recolor();
		}
		
		public void SetItem(){
			int changed = (int)selectMethod;
			
			
			Saved();
			ExitS d = new ExitS();
			
			d.SetSavedName(savedName);
			
			d = DungeonItems.preferences.Load(d) as ExitS; 
			
			
			if (changed == 0) 
			{
				d.savedName  = stringVar;
				savedName = stringVar;
			}
			
			if (changed == 1) d.level = stringVar;
			else if (changed == 2) d.exit  = stringVar;
			else if (changed == 3) d.exit2 = stringVar;
			else if (changed == 4) d.title = stringVar;
			
			
			
			DungeonItems.preferences.Save(d);
			
		
			DungeonItems.preferences.SaveBak(d); // changes form
			
			
			if (changed == 0) 
			{
				// it makes  sense to change the name, then everything else
				SaveAll();
			}
			
		}
		
		public void SaveAll()
		{
			hub.Save();
			
			// I could keep this optional just in case.
			
			
			
			return;
		}
			
		
		
		
	}
}