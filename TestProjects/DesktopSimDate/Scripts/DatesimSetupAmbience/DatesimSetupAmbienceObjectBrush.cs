using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Utility.UI;
using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceObjectBrush : DatesimSetupAmbienceObject {
		// This is a list of locations, I should be able to auto-generate them
		
		
		protected ImageBrushAnimation brushAnim;
		
		protected RectTransform clone;
		protected GameObject cloneObject;
		protected Image cloneImage;
		protected Image image;
		public override void MakeReal(){
			
			
			// supposed to use ambience brush maybe?
			
			ar = gameObject.AddComponent<DatesimAmbienceRectUtil>();
			brushAnim = GetComponent<ImageBrushAnimation>();

			image = GetComponent<Image>();
			
			gameObject.AddComponent<ErasedByEraser>();
			DragHold2DAmbience dr = gameObject.AddComponent<DragHold2DAmbience>();
			
			/// basel
			
			enabled = true;
			// layer = 0;
			// animator.enabled = true;
			if (collider == null) 
				collider = GetComponent<Collider2D>();
			isAlive = true;
			
			
			
			/// end base
			
			collider.enabled = true;
			
			
			brushAnim.enabled = true;
			
			
			
			InitClone();
			
			dr.screenBound = false;
			dr.interiorBound = true;
			dr.pressed = true;
			dr.OnInstance();
			
			
			// resizes brush?
			StartCoroutine("Pick");
			
		}
		
		protected void InitClone()
		{
			cloneObject = new GameObject("trail", typeof(RectTransform));
			
			
			cloneObject.SetActive(false);
			
			clone = cloneObject.transform as RectTransform;
			clone.SetParent(rectTransform, false);
			
			cloneObject.layer = gameObject.layer;
			clone.sizeDelta = rectTransform.sizeDelta;
			clone.position = rectTransform.position;
			cloneObject.AddComponent<CircleCollider2D>();
			cloneImage = cloneObject.AddComponent<Image>();
			cloneImage.color = image.color;
			cloneImage.preserveAspect = true;
			
			
		}
		protected bool running = false;
		
		protected override void OnChange(){
			base.OnChange();
			if (!isAlive) return;
			
			if (ambienceHub.totalAmbience >=ambienceHub.maxAmbience )
				holdingButton = false;
		}
		
		protected Vector3 lastCursorPos;
		protected Vector3 trailPos;
		protected float distanceDelta = 0f;
		public int next = 0;
		protected float _increment = .8f;
		protected float increment = 1.5f;
		protected float _jitter = .5f;
		protected float jitter = 1.5f;
		
		protected bool _picking = false;
		protected bool holdingButton = true;
		public IEnumerator Pick(){
			
			if (_picking) yield break;
			_picking = true;
			
			if (image == null) yield return null;
			
			
			next = 
			 (int)Mathf.Repeat(ambienceHub.nextBrush, brushAnim.list.Length );
			 
			image.sprite = brushAnim.list[next];
			
			ar.Rescale();
			trailPos = Input.mousePosition;
			NextPick();
			while (holdingButton)
			{
			ar.Rescale();
			increment = (rectTransform.rect.width + rectTransform.rect.height) / 2f * _increment ;
				
			while (distanceDelta< increment)
			{
				lastCursorPos = rectTransform.position;
			
				
				yield return null;
				
				holdingButton = Input.GetButton("mouse 1");
				if (!holdingButton) break;
				
				
				
				distanceDelta += Vector3.Distance(lastCursorPos, rectTransform.position);
			}
			if (!holdingButton) break;
			
			trailPos = lastCursorPos;
			while (distanceDelta > increment)
			{
				lastCursorPos = rectTransform.position;
				
				distanceDelta -= increment;
				trailPos = Vector3.MoveTowards(trailPos, rectTransform.position, increment); // this assumes I'll draw a straight line?
				
				NextPick();
				// place pick()
				
				
				
				distanceDelta += Vector3.Distance(lastCursorPos, rectTransform.position);
			}
			
			}
			FinalStep();
			
		}
		
		
		public override void MakeProxy(){
			
			collider.enabled = false;
			GameObject.Destroy(gameObject.GetComponent<ErasedByEraser>());
			GameObject.Destroy(brushAnim);
			
			
			
		}
		
		protected bool _final = false;
		public void FinalStep(){
			if ( _final) return;
			_final = true;
			
			if (ambienceHub.totalAmbience <ambienceHub.maxAmbience )
			{
				trailPos = rectTransform.position;
				NextPick();
			}
			Kill(); // I guess it doesn't need anything else.
			
		}
		public void NextPick(){
			
			if (cloneObject == null) {distanceDelta = 0; return;}
			cloneImage.sprite = brushAnim.list[next];
			
			RectTransform t = GameObject.Instantiate(clone) as RectTransform;
			
			
			t.SetParent(rectTransform.parent, false);
			
			if (ambienceHub.totalAmbience >= ambienceHub.maxAmbience - 1 )
				jitter = 0f;
			else
				jitter = (rectTransform.rect.width + rectTransform.rect.height) / 2f * _jitter ;
			
			t.position = trailPos + (Vector3)Random.insideUnitCircle * jitter;
			
			
			t.gameObject.SetActive(true);
			t.gameObject.AddComponent<DatesimSetupAmbienceObjectTrail>().MakeReal();
			DatesimAmbienceRectUtil nar = t.gameObject.GetComponent<DatesimAmbienceRectUtil>();
			
			nar.SetPosition(t.position);
			nar.Fix();
			
			ambienceHub.nextBrush = next = (int)Mathf.Repeat(next + 1, brushAnim.list.Length );
			// not time based
			image.sprite = brushAnim.list[next];
		}
			
		

	}
}