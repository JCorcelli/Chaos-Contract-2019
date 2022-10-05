using UnityEngine;
using System.Collections;



namespace Animations
{
	public class UseDoor : UpdateBehaviour {

		// This activates a door based on a separate entity's animator
		
		public Transform target;
		protected IDoor door;
		
		protected bool open = false;
		protected override void OnEnable() {
			base.OnEnable();
			door = target.GetComponent<IDoor>(); 
		}
		
		public bool isOpen { get { return IsOpen(); } private set {} }  
		public bool IsOpen() {
			
			return door.IsOpen();
		}
		public void Open() {
			
			door.Open();
			
		}
		
		public void Close() {
			
			door.Close();
			
		}
		public void Toggle () {
			
			door.Toggle();
			
		}
		
		public void OpenShut() {
			
			door.OpenShut();
			
		}
		
		
		
	}
}