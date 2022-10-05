
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;


namespace FloorDesigner.Tools
{
	
	
	[RequireComponent (typeof (CircleCollider2D))]
    public class ItomSnappingCircle : FloorPlanWorkspaceHubConnect
    {
		// I should really determine different types of itoms
		
		protected CircleCollider2D col;
		
		protected RectTransform target;
		protected Transform currentSnap;
		protected Transform newSnap;
		
		protected List<Transform> touchList = new List<Transform>();
		public float snapDistance = 20f; // or the radius?
		
		public GameObject toolObject;
		
		public float delay = .5f;
		
		protected UISelectMask sMask;
		
		
		public bool bSnapRooms = false;
		public bool bSnapWalls = false;
		// public bool bMisc = false;
		
		protected const int message_walls = (int)FloorPlanWorkspaceHub.Enum.SnapWalls;
		protected const int message_rooms = (int)FloorPlanWorkspaceHub.Enum.SnapRooms;
		
		
		
		protected FloorDesigner.IBool rooms;
		
		protected FloorDesigner.IBool walls;
		
		protected int itomMask = 0;
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			if (!sMask.isHovered) return;
			transform.position = Input.mousePosition;
			
			
			UpdateSnap();
			
		}
		
		protected void UpdateSnap() {
			// not snapping, basically wherever I point
			if (itomMask == 0)
			{
				currentSnap = transform;
				
				target.position = currentSnap.position ;
				return;
			}
			
			newSnap = GetClosest();
			if (newSnap == null) 
			{
				currentSnap = null;
				return;
			}
			if ( newSnap != currentSnap)
			{
				Show();
				//target.parent = other.parent;
				currentSnap = newSnap;
				target.position = currentSnap.position ;
			
			}
		}
		
		protected override void OnEnable() {
			
			base.OnEnable();
			StartReconnect();
			hub.Ping();
			col = GetComponent<CircleCollider2D>();
			
			sMask = GetComponentInParent<UISelectMask>();
			
			target = toolObject.GetComponent<RectTransform>();
			
			UpdateMask();
			
			
		}
			
		protected void UpdateMask(){
			itomMask = 0;
			if (bSnapRooms) 
			
				itomMask |= (int)FloorPlanItom.ItomType.Room;
			if (bSnapWalls)
				itomMask |= (int)FloorPlanItom.ItomType.Wall;
		}
		protected override void OnDisable() {
			base.OnDisable();
			
			touchList.Clear();
		}
			
		protected void OnTriggerExit2D(Collider2D other) {
			if (touchList.Contains(other.transform))
				touchList.Remove(other.transform);
		}
		protected void OnTriggerEnter2D(Collider2D other) {
			
			if (!sMask.isHovered) return;
			if (!connected) hub.Ping();
			
			FloorPlanItom itom = other.gameObject.GetComponent<FloorPlanItom>();
			
			if (itom == null) return;
			else if ( itom.IsMasked((int)itomMask))
			{
				if (!touchList.Contains(other.transform))
					touchList.Add(other.transform);
			}
		}
		
		protected Transform GetClosest() {
			
			if (touchList.Count == 1) 
			{
				if (touchList[0] != null)
					return touchList[0];
				else
					touchList.RemoveAt(0);
			}
			
			if (touchList.Count == 0) return null;
			
			Vector3 pos = transform.position;
			Transform closest = touchList[0];
			
			float newDist = Vector3.Distance(pos, touchList[0].position);
			float dist = newDist;
			Transform t;
			for ( int i = 1; i < touchList.Count ; i ++) 
			{
				t = touchList[i];
				if (t == null) continue; // I really need to clean lists better.
				newDist = Vector3.Distance(pos, t.position);
				if (newDist < dist)
				{
					dist = newDist;
					closest = t;
				}
			}
			
			return closest;
		}
		
		protected void Show(){
			StopCoroutine("QuickShow");
			StartCoroutine("QuickShow");
		}
		
		protected IEnumerator QuickShow(){
			
			toolObject.SetActive(true);
			
			yield return new WaitForSeconds(delay);
			toolObject.SetActive(false);
			
		}
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			if (senderEnum != (int)channel) return;
			if (msgEnum == 0) Connect();
		
			if (msgEnum == message_rooms)
			{
				
				if (rooms == null) return;
				bSnapRooms = rooms.GetBool();
			}
			else if (msgEnum == message_walls)
			{
				if (walls == null) return;
				bSnapWalls = walls.GetBool();
			}
			//else if misc. message_tool? idk
			else
				return;
			
				
			UpdateMask();
				
			
				
		}
		
		protected override void OnConnect(Object other) {
			base.OnConnect(other);
			
			GameObject ob = (GameObject)other;
			
			FloorDesigner.IMessage button = ob.GetComponent<IMessage>();
			
			if (button == null) return;
			
			
			if (button.GetMessage() == message_rooms)
			{
				rooms = ob.GetComponent<IBool>();
				bSnapRooms = rooms.GetBool();
			}
			else if (button.GetMessage() == message_walls)
			{
				walls = ob.GetComponent<IBool>();
				bSnapWalls = walls.GetBool();
			}
			
			//misc
			if (rooms != null && walls != null ) 
				connected = true;
				
			UpdateMask();
		}
		
	}
}
