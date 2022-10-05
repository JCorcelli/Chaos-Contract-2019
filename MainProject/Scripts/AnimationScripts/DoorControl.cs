using UnityEngine;
using System.Collections;

namespace Animations
{
	public interface IDoor {
		void Open();
		
		void Close();
		
		void Toggle();
		void OpenShut();
		
		bool IsOpen();
	}
	public class DoorControl : UpdateBehaviour, IDoor {

		// Use this for initialization
		
		public bool isOpen = false;
		private Animator anim;
		
		void Awake () { anim = GetComponent<Animator>(); }
		public void Toggle () {if (isOpen) Open(); else Close();}
		
		
		public bool IsOpen() { return isOpen; }
		public void Open() {
			
			
			anim.SetTrigger("Open");
			isOpen = true;
		}
		public void OpenShut() {
			
			
			anim.SetTrigger("OpenShut");
			isOpen = false;
		}
		public void Close() {
			
			anim.SetTrigger("Close");
			isOpen = false;
			
		}
		
		
		
	}
}