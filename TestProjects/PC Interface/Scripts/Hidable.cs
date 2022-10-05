using UnityEngine;
using System.Collections;
using SelectionSystem;
using CameraSystem;

namespace NPCSystem
{
	public class Hidable : AbstractAnyHandler {

		public GameObject hiddenObject;
		public GameObject ghost;
		public BunnyThirdPerson bunny;
		public bool touchingCover = false;
		public HidingSpot hidingSpot;
		public HidingSpot _hidingSpot;
		public string requireGroup = "";
		public string coverGroup = "";
		
		public bool hidden = false;
		public int count = 0;
		public bool diving = false;
		public LayerMask layerMask = 0;
		
		public Transform targetB {
			
			get{return CameraHolder.instance.targetB;}
		}
		
		protected override void OnEnable()
		
		{
			base.OnEnable();
			bunny = hiddenObject.GetComponent<BunnyThirdPerson>();
			
			ghost.SetActive(false);
		}
		
		protected void Move(){
			
			RaycastHit[] hits;
			Vector3 start = Camera.main.transform.position;
			Vector3 end = targetB.position;
			Vector3 direction = end - start;
			
			
			
			hits = Physics.RaycastAll(start, direction.normalized, direction.magnitude + 0.1f, layerMask);
			
			HidingSpot hs;
			
			foreach (RaycastHit hit in hits)
			{
				
				hs = hit.collider.GetComponent<HidingSpot>();
			
				if (NewHidingspot(hs))
				{
					transform.position = hit.point;
					//ghost.transform.position = end;
					break;
					
				}
				
					
			}
		}
		protected override void OnPress()
		{
			if (hidden && Input.GetButtonDown("mouse 1"))
			{
				Move();
			}
			else if (Input.GetButtonDown("use")) 	
			{
				bunny.rigidbody.velocity = Vector3.zero;
				Hide();
			}
			else if (hidden && Input.GetButtonDown("shift")) 	
			{
				diving = true;
				Hide();
				
				diving = false;
			}
		}
		
		protected override void OnHold()
		{
			if (hidden && Input.GetButton("mouse 1"))
			{
				Move();
				
				
			}
			
		}
		
		
		protected void Hide()
		{
			if ( touchingCover )
			{
				if (!hidden)
				{
					hidden = true;
					
					hiddenObject.SetActive(false);
					
					
					if (_hidingSpot != null) 
						_hidingSpot.Hide();
					
					
					ghost.SetActive(true);
					//ghost.transform.position = transform.position;
					hidingSpot = _hidingSpot;
					requireGroup = coverGroup;
					
					
				}
				else if (coverGroup == requireGroup)
				{
					hidden = false;
					hiddenObject.transform.position = ghost.transform.position;
					transform.localPosition = Vector3.zero;
					hiddenObject.SetActive(true);
					if (hidingSpot != null) 
					{
						hidingSpot.Show();
						
					}
					ghost.SetActive(false);
					
					if (diving)
					{
						 
						bunny.jumping = false;
						bunny.touchingGround = true;
						bunny.bdiving = true;
					}
					StopCoroutine("NoReenter");
					StartCoroutine("NoReenter");
				}
			}
			
		}
		
		protected bool noEnter = false;
		protected IEnumerator NoReenter()
		{
			noEnter = true;
			yield return new WaitForSeconds(1f);
			noEnter = false;
		}
		protected bool NewHidingspot(HidingSpot hs)
		{
			if (hs == null) return false;
			bool success = false;
			
			if (requireGroup == hs.group)
			{
				_hidingSpot = hs;
				
				coverGroup = _hidingSpot.group;
				success = true;
				if (hidden) 
				{
					hidingSpot.Show();
					_hidingSpot.Hide();
					
				}
				
				hidingSpot = _hidingSpot;
				
			}
			
		
			return success;
		}
		protected void OnTriggerEnter(Collider col)
		{
			
			HidingSpot hs = col.GetComponent<HidingSpot>();
			
			if (hs != null) 
			{
				// here if this never triggers there'd be a glitch
				_hidingSpot = hs;
				touchingCover = true;
				coverGroup = _hidingSpot.group;
				
				if (requireGroup == coverGroup)
				{
					
					if (hidden) 
					{
						hidingSpot.Show();
						_hidingSpot.Hide();
						
					}
					
					hidingSpot = _hidingSpot;
					
				}
				count ++;
				
				if (!noEnter && !hidden && bunny.jumping) Hide();
			}
		}
		protected void OnTriggerExit(Collider col)
		{
			
			HidingSpot hidingSpot = col.GetComponent<HidingSpot>();
			
			if (hidingSpot != null) 
			{
				count --;
				if (count <= 0)
				{
					touchingCover = false;
					
				}
			}
			
			
		}
	}
}