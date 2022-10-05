using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using StealthSystem;
using SelectionSystem.IHSCx;
using SelectionSystem;

using Dungeon.Save;

namespace Dungeon
{
	public class WallBuildHotspot : SelectHub{
		// all the makings of a 3D "workstation"
	
		protected string targetName = "ToolCollider"; // refers to the on trigger function, which may or may not limit the activation distance
		
		public GameObject hoverCue; // set active when any button press will activate the function
		
		protected bool used = false;
		
		protected VisibleRoom thisRoom;
		protected VisibleRoom otherRoom;
		
		protected RemoveWall removal;
		
		
		
		protected override void OnDisable() {
			base.OnDisable();
			
			// player left room
			
			if (hoverCue != null) hoverCue.SetActive(false);
			
			
			
			hovercount = 0;
			hovered = false;
		}
		protected override void OnEnable() {
			base.OnEnable();
			
			if (!hovered && hoverCue != null) hoverCue.SetActive(false);
			
			if (thisRoom == null)
				thisRoom = transform.GetComponentInParent<VisibleRoom>();
			
			GetRemoval();
			
				
				
			
		}
		
		
		protected Transform newRoomTarget;
		
		// the room that's loaded
		[System.NonSerialized]
		public static Transform roomPrefab;
		
		// an accompanying object that deconstructs the wall, and provides LOS for dungeons
		[System.NonSerialized]
		public static Transform removalPrefab;
		
		protected void Awake() {
			if (roomPrefab == null) 
				roomPrefab = UnityEngine.Resources.Load("DungeonBuildMode/RoomModule", typeof (Transform)) as Transform;
			
			if (removalPrefab == null) 
			
				removalPrefab = UnityEngine.Resources.Load("DungeonBuildMode/RemoveWall", typeof (Transform)) as Transform;
			
			
			newRoomTarget = transform.Find("NewRoomTarget");
			hoverCue = transform.Find("HoverCue").gameObject;
			if (!hovered && hoverCue != null) hoverCue.SetActive(false);
				
				
				
			
			thisRoom = transform.GetComponentInParent<VisibleRoom>();
			
			GetRemoval();
			
			
			if (thisRoom == null) Debug.LogError("no room in parent", gameObject);
			
			// no reason to check otherRoom since it could be disabled or completely destroyed
		}
		
		
		protected int hovercount = 0;
		
		protected void OnTriggerExit( Collider col ) {
			//if (targetName == col.name) OnExit();
		}
		protected void OnTriggerEnter(Collider col ) {
			//if (targetName == col.name) OnEnter();
		}
		public override void OnExit( ) {
			
			
			hovercount --;
			
			if (hovercount > 0) return;
			
			if (hovercount < 0) hovercount = 0;
		
			hovered = false;
			if (hoverCue != null) hoverCue.SetActive(false);
				
				
		}
		public override void OnEnter( ) {
			
			if (thisRoom == null ) return;
			// check if tool hovered
		
			hovercount ++;
			hovered = true;
			if (hovercount > 1) return;
		
			if (hoverCue != null) hoverCue.SetActive(true);
			
			
			
		}
		
		protected override void OnUpdate () {
			base.OnUpdate();
			
			if (hovered && Input.anyKeyDown) UseTool();
		}
		protected override void OnLateUpdate () {
			base.OnLateUpdate();
			used = false;
		}
		
		public override void OnPress() {
			if (pressed) return;
			pressed = true;
			
			// UseTool();
		}
		
		
		protected void UseTool () {
			if (SelectGlobal.uiSelect) return;
			
			SelectGlobal.uiSelect = true;
			used = true;
			// in the right spot?
			if (thisRoom == null) return;
			
			//show active hotspot, or highlight the wall, or something
			
			if (Input.GetButton("mouse 1"))
			{
				RemoveWall();
			}
			else if (Input.GetButton("mouse 2"))
			{
				AddWall();
			}
			else if (Input.GetButton("mouse 3"))
			{
				RemoveRoom();
			}
		}
		
		
		protected void RemoveWall () { // adds room, if there isn't one
			
			if (removal != null) return;
			
			if (otherRoom == null)
				GetOtherRoom ();
			
			IDungeonSaveStatic d;
			if (otherRoom == null)
			{
				// check if there's already a room
				
				Transform room = Instantiate(roomPrefab);
				room.position = newRoomTarget.position;
				otherRoom = room.GetComponent<VisibleRoom>();
				
				d = room.GetComponent<IDungeonSaveStatic>();
				d.Save();
			}
				
			
			// adds a remove wall at position
			Transform rm = Instantiate(removalPrefab);
			removal = rm.GetComponent<RemoveWall>();
			rm.position = transform.position + Vector3.up* .01f;
			// next tick it should remove walls
			d = rm.GetComponent<IDungeonSaveStatic>();
			d.Save();
		}
		protected void AddWall () {
			if (removal != null)
				Object.Destroy(removal.gameObject);
		}
		
		
		protected void GetOtherRoom () {
			 
			 
			float r = .1f;
			
			float d;
			
			
			foreach (VisibleRoom rm in VisibleRoom.rooms)
			{
				if (rm == thisRoom) continue;
				
				d = Vector3.Distance(newRoomTarget.position, rm.transform.position);
				
				if (r > d)
				{
					otherRoom = rm;
					return;
				}
				
			}
			
			 
		}
		protected void GetRemoval () {
			if (removal != null) return;
				
			// remove the remove wall, check visibleroom
			
			float r = .1f;
			
			float d;
			foreach (RemoveWall rm in thisRoom.wallxList)
			{
				d = Vector3.Distance(transform.position, rm.transform.position);
				
				if (r > d)
				{
					removal = rm;
					return;
				}
				
			}
			
			
		}
		protected void RemoveRoom () { // remove the room, this could access a remove wall master of some kind, check visibleroom
			if (otherRoom == null) return;
			
			Object.Destroy(otherRoom.gameObject);
		}
		
	}
}