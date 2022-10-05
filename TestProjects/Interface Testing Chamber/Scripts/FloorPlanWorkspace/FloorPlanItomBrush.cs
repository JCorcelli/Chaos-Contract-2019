using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanItomBrush : ConnectHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected Collider2D col;
		protected const FloorPlanHub.Enum message_target = FloorPlanHub.Enum.Itom;
		public FloorPlanHub.Channel channel = FloorPlanHub.Channel.Default;
		public Color setColor = Color.white;
		
		
		protected override void OnDisable( ){
			base.OnDisable();

		}
			
		protected override void OnEnable( ){
			base.OnEnable();
			
			col = GetComponent<Collider2D>();
			
			
			if (col == null) 
			{
				Debug.Log("not collider2d", gameObject);
				
				Object.Destroy(this);
				return;
			}
			
			
			
			
		}
		protected void CheckCollision (GameObject ob ){
			// triggers for every itom that reconnects
			Collider2D other = ob.GetComponent<Collider2D>();
			if (other == null) return;
			
			bool hit = false;
			
			hit = col.IsTouching(other);
			//Debug.Log(hit);
			
			if (hit) ToolUsed(ob);
		}
		
		protected void ToolUsed(GameObject modified) {
			Image im = modified.GetComponent<Image>();
			im.color = setColor;
			
			/// maybe later this'll mean something else
		}
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
		}
		protected override void OnConnect(Object other) {
			
			if (!pinging ) return;
			
			GameObject ob = (GameObject)other;
			
			
			FloorDesigner.IMessage iping = (IMessage)other;
			
			if (iping == null) return;
			
			
			if (iping.GetMessage() == (int)message_target)
			{
				
				CheckCollision(ob);
				
			}
			
			
		}
		
		protected bool pinging = false;
		
		public bool useOnRelease = true;
		protected override void OnUpdate() {
			base.OnUpdate();
			pinging = true;
			
			
			if (useOnRelease)
			{
				
				if (Input.GetButtonUp("mouse 1") || Input.GetButtonUp("mouse 2")) 
					hub.Send(0,0); // pings, and enacts
			}
			else if (Input.GetButton("mouse 1") || Input.GetButton("mouse 2"))
				hub.Send(0,0); // pings every frame. maybe I should make a physics based brush.
			pinging = false;
			
			//if (!Input.anyKey) gameObject.SetActive(false);
		}
		
		

	}
}