using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Dungeon;

namespace Dungeon.Save
{
	public class FormDecorRecolor : UpdateBehaviour {
		
		
		public string prefabName = "ExitS"; // helps build the generic item list
		public string savedName = "no name"; // helps build the generic item list
		public FormHub hub;
		
		protected Image image;
		public Color defaultColor = Color.black	;
		public Color selectedColor = Color.yellow	;
		
		protected void Start( ){
			image = GetComponent<Image>();
			if (image == null) Debug.Log("no image", gameObject);
			
		}
		protected override void OnEnable( ){
			base.OnEnable();
			if (hub == null)
				hub = GetComponentInParent<FormHub>();
			
			if (hub == null) Debug.LogError("hub needed", gameObject);
			
			if (hub.isSelected ) OnSelect();
			hub.onSelect += OnSelect;
			hub.onDeselect += OnDeselect;
			
			
		}
		protected override void OnDisable( ){
			base.OnDisable();
			
			
			if (hub != null)
			{
			
				hub.onSelect -= OnSelect;
				hub.onDeselect -= OnDeselect;
			}
			
		}
		
		protected void OnSelect() {
			
			// called on item save, and called at the start
			// if you simply change values... it's not saved, afterall.
			if (image == null) return;
			
			image.color = selectedColor;
			
			
		}
		
		
		protected void OnDeselect() {
			
			if (image == null) return;
			
			
			image.color = defaultColor;
		}
		
		
	}
}