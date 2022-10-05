using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StealthSystem;

namespace Dungeon
{
	public class RemoveWall : SeeTarget {

	
		protected string targetName = "OneWayWall";
		
		public VisibleRoom[] rooms = new VisibleRoom[2];
		protected GameObject[] walls = new GameObject[2];
		protected bool roomEnabled = false;
		protected int roomCount = 0;
		
		
		
		protected void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
		
				VisibleRoom room = col.gameObject.GetComponentInParent<VisibleRoom>();
		
				if (room != null)
				{
					
					if (roomCount >= rooms.Length) 
					{
						Debug.LogError("too many rooms", gameObject);
						return;;
					}
					
					
					col.gameObject.SetActive(false);
					
					walls[roomCount] = col.gameObject;
					
					
					rooms[roomCount]=room;
					
					
					room.AddLOSCheck(this);
					
					roomCount ++;
				}
				
				
			}
		}
		
		public void OnDestroy() {

			foreach (VisibleRoom v in rooms)
			{
				if (v != null)
					v.RemoveLOSCheck(this);
			}
			
			if (roomEnabled)
			{
				roomEnabled = false;
				
				foreach (VisibleRoom r in rooms)
				{
					
					if (r!= null) r.Calc(-1);
				}
			}
			foreach (GameObject v in walls)
			{
				if (v != null)
					v.SetActive(true);
			}
			
				
		}
		
		
		protected override void OnUpdate () {
			if (rooms[0] == null) return;
			base.OnUpdate();
			if (visible)
			{
				if (roomEnabled) return;
				roomEnabled = true;
				foreach (VisibleRoom r in rooms)
				{
					if (r!= null) r.Calc(1);
				}
			}
			else
			{
				
				if (!roomEnabled) return;
				roomEnabled = false;
				foreach (VisibleRoom r in rooms)
				{
					
					if (r!= null) r.Calc(-1);
				}
			}
		}
	}
}