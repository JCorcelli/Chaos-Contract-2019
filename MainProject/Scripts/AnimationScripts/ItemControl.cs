using UnityEngine;
using System.Collections;

namespace Animations
{
	public interface IItem {
		void Spawn();
		
		void Grab();
		
		void Toggle();
		void GrabRelease();
		
		bool IsTaken();
	}
	public class ItemControl : UpdateBehaviour, IItem {

		// Use this for initialization
		
		public bool isTaken = false;
		private Animator anim;
		
		void Awake () { anim = GetComponent<Animator>(); }
		public void Toggle () {if (isTaken) Grab(); else Spawn();}
		
		
		public bool IsTaken() { return isTaken; }
		public void Grab() {
			
			
			anim.SetTrigger("Grab");
			isTaken = true;
		}
		public void GrabRelease() {
			
			
			anim.SetTrigger("Item.GR");
			isTaken = false;
		}
		public void Spawn() {
			
			anim.SetTrigger("Spawn");
			isTaken = false;
			
		}
		
		
		
	}
}