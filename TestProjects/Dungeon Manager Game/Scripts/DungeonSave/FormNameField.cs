
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Dungeon.Save
{
	
	public class FormNameField : UpdateBehaviour 
	{
		public string prefabName = "ExitS";
		
		// maybe also set the title/header/footer on form types
		
		public Text text;
		public string savedName = "unknown";
		public FormHub hub;
		protected virtual void Start() {
			
			
			
			Retrieve();
			
			if (text != null)
				text.text = savedName;
		}
		protected override void OnEnable() {
			base.OnEnable();
			
			
			
			if (hub == null)
				hub = GetComponentInParent<FormHub>();
			
			if (hub == null) Debug.LogError("hub needed", gameObject);
			
			hub.onLoad += OnLoad;
			hub.onSave += OnSave;
		}
		protected override void OnDisable() {
			base.OnDisable();
			
			
			if (hub != null)
			{
				hub.onLoad -= OnLoad;
				hub.onSave -= OnSave;
			}
		}
		

		protected void OnLoad() {
			Retrieve();
			
			if (text != null)
				text.text = savedName;
		}
		protected void OnSave() {
			SpS d = new SpS();
			d.prefabName = prefabName;
			
			

			Spawnable a = DungeonItems.preferences.GetRecent(d);
			
			savedName = a.GetSavedName();
			
		
			if (text != null)
				text.text = savedName;
			return; // avoid inf. recursion
				
		}
		public void Retrieve() {
			SpS d = new SpS();
			d.prefabName = prefabName;
			
			Spawnable a = DungeonItems.preferences.GetRecent(d);
			
			
			savedName = a.GetSavedName();
			
			
			
		}
	}
}