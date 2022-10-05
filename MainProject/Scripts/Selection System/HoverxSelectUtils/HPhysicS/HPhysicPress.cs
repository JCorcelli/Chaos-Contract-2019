using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace SelectionSystem
{
	public delegate void HPhysicDelegate();
	public class HPhysicPress : UpdateBehaviour {
		
		public static HPhysicPress instance;
		public static HPhysicDelegate press;
		public static HPhysicDelegate touch;
		
		public static GameObject touching;
		
		public static bool _available = true;
		
		public static bool available{
			get
				{return _available;}
			set
			{
				if (_available == value) return;
				else
				{
					_available = value;
					
					PhysicsRaycaster r = Camera.main.GetComponent<PhysicsRaycaster>();
					
					if (r != null) 
					{
						if (_available) 
							r.enabled = true;
						else r.enabled = false;
					}
				}
			
			}
		}
		
		
		
		new protected Collider2D collider;
		
		
		public Collider2D GetCollider(){return collider;}
		protected void Awake()
		{
			if (instance == null) instance = this;
			// otherwise I might leave this to toggle somehow
			
			else
			{
				Debug.Log("Multiple HPhysicPress aren't supported",gameObject);
				GameObject.Destroy(this);
				
			}
			collider = GetComponent<Collider2D>();
			
		}
		
		protected override void OnEnable(){
			base.OnEnable();
			
		}
		protected override void OnDisable(){
			base.OnDisable();
			
			available = true;
			
		}
		
		
		
		public void Touch() {
			if (touch != null) touch();
		}
		
		
		protected override void OnUpdate () {
			// supposedly things I hover over will subscribe to my press delegate.
			//if (SelectGlobal.uiSelect) return;
			
			if (press != null)
			{
				if (Input.GetButtonDown("mouse 1"))
				{
					press();
				}
			
			}
			
			
		}
		
		
		protected override void OnLateUpdate () {
			// other object turns off
			if (!available && !touching.activeInHierarchy) available = true;
			
			transform.position = Input.mousePosition;
		}
		
	}
}