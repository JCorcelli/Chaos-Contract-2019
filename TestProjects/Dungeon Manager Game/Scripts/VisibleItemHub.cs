using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dungeon
{
	public class VisibleItemHub : MonoBehaviour {

		// Use this for initialization
		
		public int count = 0;
		
		protected List<MeshRenderer> renderers = new List<MeshRenderer>(); // I should probably have things look up the hierarchy to this instead of the reverse.
		
		public List<VisibleItemNode> nodeList = new List<VisibleItemNode>();
		
		protected GameObject floor;
		
		
		protected void Awake() {Init();}
		protected void Init() {
			MeshRenderer rend;
			foreach (Transform child in transform)
			{
				rend = child.GetComponent<MeshRenderer>();
				if (rend != null)
				{
					renderers.Add(rend);
					rend.enabled = false;
				}
				
				
				
			}
		
			rend = GetComponent<MeshRenderer>();
			if (rend != null)
			{
				renderers.Add(rend);
				rend.enabled = false;
			}
		}
		
		
		
		protected void SetRenderers(bool b, int s){
			
			foreach (MeshRenderer rend in renderers)
			{
				if (rend == null) continue;
				rend.enabled = b;
				rend.gameObject.layer = s;
			}
		}
		
		
		public void OnDestroy() {
			// this is basically if the objects are connected, but not in heirarchy
			
			VisibleItemNode[] clone = nodeList.ToArray();
			
			foreach (VisibleItemNode rm in clone)
			{
				Object.Destroy(rm.gameObject); // this action alters the nodeList
			}
			
		}
		
		public void AddLOSCheck ( VisibleItemNode rm ) {
			if (nodeList.Contains(rm)) return;
			nodeList.Add(rm);
		}
		public void RemoveLOSCheck ( VisibleItemNode rm ) {
			if (!nodeList.Contains(rm)) return;
			nodeList.Remove(rm);
		}
			
		public void Calc (int amt) {
			
			count += amt;
			if (count <= 0)
			{
				count = 0;
				SetRenderers(false, 10);
				
			}
			
			if (count > 0)
			{
				SetRenderers(true, 0); // needs to be something they were originally though
				
			}
		}
		
	}
}