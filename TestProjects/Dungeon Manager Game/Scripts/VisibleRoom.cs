using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dungeon
{
	public class VisibleRoom : MonoBehaviour {

		// Use this for initialization
		
		public int count = 0;
		
		public static List<VisibleRoom> rooms = new List<VisibleRoom>();
		
		protected List<MeshRenderer> renderers = new List<MeshRenderer>(); // I should probably have things look up the hierarchy to this instead of the reverse.
		
		public List<RemoveWall> wallxList = new List<RemoveWall>();
		
		protected GameObject floor;
		
		
		protected void Awake() {Init();}
		protected void Init() {
			
			foreach (Transform child in transform)
			{
				MeshRenderer rend = child.GetComponent<MeshRenderer>();
				if (rend != null)
				{
					renderers.Add(rend);
					rend.enabled = false;
				}
				
				
				if (child.name == "2x2Floor")
				{
					floor = child.gameObject;
					SetFloor(12);
				}
				
			}
			
			rooms.Add(this);
		}
		
		
		
		protected void SetRenderers(bool b){
			
			foreach (MeshRenderer rend in renderers)
			{
				rend.enabled = b;
			}
		}
		protected void SetFloor(int s){
			
			floor.layer = s;
		}
		
		public void OnDestroy() {
			
			RemoveWall[] clone = wallxList.ToArray();
			
			foreach (RemoveWall rm in clone)
			{
				Object.Destroy(rm.gameObject); // this action alters the wallxlist
			}
				
			rooms.Remove(this);
		}
		
		public void AddLOSCheck ( RemoveWall rm ) {
			if (wallxList.Contains(rm)) return;
			wallxList.Add(rm);
		}
		public void RemoveLOSCheck ( RemoveWall rm ) {
			if (!wallxList.Contains(rm)) return;
			wallxList.Remove(rm);
		}
			
		public void Calc (int amt) {
			
			count += amt;
			if (count <= 0)
			{
				count = 0;
				SetRenderers(false);
				SetFloor(12);
			}
			
			if (count > 0)
			{
				SetRenderers(true);
				SetFloor(0);
			}
		}
		
	}
}