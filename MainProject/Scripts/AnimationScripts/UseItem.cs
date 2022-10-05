using UnityEngine;
using System.Collections;



namespace Animations
{
	public class UseItem : UpdateBehaviour {

		// This activates an item
		
		public Transform target;
		protected IItem item;
		
		protected bool open = false;
		protected override void OnEnable() {
			base.OnEnable();
			item = target.GetComponent<IItem>(); 
		}
		
		public bool isTaken { get { return IsTaken(); } private set {} }  
		public bool IsTaken() {
			
			return item.IsTaken();
		}
		public void Grab() {
			
			item.Grab();
			
		}
		
		public void Spawn() {
			
			item.Spawn();
			
		}
		public void Toggle () {
			
			item.Toggle();
			
		}
		
		public void GrabRelease() {
			
			item.GrabRelease();
			
		}
		
		
		
	}
}