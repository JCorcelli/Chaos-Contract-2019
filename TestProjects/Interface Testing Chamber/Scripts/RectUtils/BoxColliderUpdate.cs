using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	[RequireComponent (typeof (BoxCollider2D))]
	public class BoxColliderUpdate : UpdateBehaviour {
		
		
		protected BoxCollider2D col;
		protected RectTransform tRect;
		
		protected void Awake() {
			col = GetComponent<BoxCollider2D>();
			tRect = GetComponent<RectTransform>();
			
			if (col == null) Debug.Log("no box collider", gameObject);
			if (tRect == null) Debug.Log("not RectTransform", gameObject);
			
		}
		protected override void OnLateUpdate() {
			
			col.size = tRect.rect.size;
		}
	}
}