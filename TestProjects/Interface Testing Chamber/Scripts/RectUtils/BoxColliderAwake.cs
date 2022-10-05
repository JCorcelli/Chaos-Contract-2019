using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	[RequireComponent (typeof (BoxCollider2D))]
	public class BoxColliderAwake : UpdateBehaviour {
		
		
		
		
		protected void Awake() {
			BoxCollider2D col = GetComponent<BoxCollider2D>();
			RectTransform tRect = GetComponent<RectTransform>();
			
			if (col == null) Debug.Log("no box collider", gameObject);
			if (tRect == null) Debug.Log("not RectTransform", gameObject);
			
			col.size = tRect.rect.size;
		}
		
	}
}