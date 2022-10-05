using UnityEngine;
using System.Collections;
using CameraSystem;

namespace SelectionSystem
{
	public class SpriteIndicator : UpdateBehaviour {
		/*
			Move an image to edge of screen, optionally changes it for ease of view, like a compass
		*/
		public Transform spriteTransform;
		
		public RectTransform imageTransform;
		

		
		protected Vector3 p;
		protected Vector3 p2;
		protected Vector3 q;
		
		protected Vector3	r;
		protected Vector3	s;
		
		protected float t;
		protected float u;
		
		
		
		
		protected float pad; // half the image size
		protected UnityEngine.UI.Image image;
		
		
		public Color colorAtEdge = new Color(1,1,1,1);
		public Color colorInView = new Color(1,1,1,.6f);
		public int fullScale =  100;
		public int edgeScale =  50;
		
		protected float screenScaler;
		protected int currentScreenHeight;
		
		protected Vector3 bottomEdgea;
		protected Vector3 bottomEdgeb;
		
		protected Vector3 topEdgea;
		protected Vector3 topEdgeb;
		
		protected Vector3 leftEdgea;
		protected Vector3 leftEdgeb;
		
		protected Vector3 rightEdgea;
		protected Vector3 rightEdgeb;
		
		
		protected RegionCheckUtil rcheck = new RegionCheckUtil();
		public RectTransform safeRegion;
		protected Transform camTransform;
		protected void Start(){
			
			rcheck.region = safeRegion;
			rcheck.cam = Camera.main;
			rcheck.target = spriteTransform;
			
			camTransform = Camera.main.transform;
			image = imageTransform.gameObject.GetComponent<UnityEngine.UI.Image>();
			currentScreenHeight = Screen.height;
			SetMargins();
		}
		
		protected void SetMargins() {
			// screenedges
			
			currentScreenHeight = Screen.height;
			screenScaler = (float)(Screen.width / Screen.height);
			pad = edgeScale * .5f / screenScaler; // half edgescale
			//bottom
			bottomEdgea = new Vector3(pad, pad, 0);
			bottomEdgeb = new Vector3(Screen.width - pad*2, 0, 0);
			
			
			// top
			topEdgea = new Vector3(pad, Screen.height - pad, 0);
			topEdgeb = new Vector3(Screen.width - pad*2, 0, 0);
			
			
			// left
			leftEdgea = new Vector3(pad, pad, 0);
			leftEdgeb = new Vector3(0, Screen.height - pad*2, 0);
			
			// right
			rightEdgea = new Vector3(Screen.width - pad, pad, 0);
			rightEdgeb = new Vector3(0, Screen.height - pad*2, 0);
			
		}
		
		public float clipNear = 1f;
		public float clipFar = 20f;
		public float clipVanish = 30f;
		
		protected override void OnLateUpdate () {
			// Determine if 3D sprite is visible
			
			
			if (currentScreenHeight != Screen.height) SetMargins();
			
			if ( rcheck.GetSafe()) 
			{
				SpriteInRegion(); // alternatives to showing sprite
			}
			else
			{
				
			
				physicalRenderer.SetActive(false);
				image.enabled = true;
				ShowCompassOnEdge();
			}
		}
		
		public GameObject physicalRenderer;
		protected void SpriteInRegion() {
			float distance = Vector3.Distance(spriteTransform.position, camTransform.position);
			if (distance < clipNear)
			{
				physicalRenderer.SetActive( false);
				CompassInView();
			}
			else
				physicalRenderer.SetActive( true);
				
			/*
			else if (distance < clipFar ) 
				physicalRenderer.SetActive( true);
			else if (distance < clipVanish) 
			{
				physicalRenderer.SetActive( false);
			}
			else
				physicalRenderer.SetActive( false);
			*/
				
				
			image.enabled = !physicalRenderer.activeSelf;
		}
		
		protected void CompassInView(){
			// image scaled to view (distance?)
			imageTransform.anchoredPosition = Camera.main.WorldToScreenPoint(spriteTransform.position);
			imageTransform.sizeDelta = Vector2.one * fullScale * screenScaler;
			image.color = colorInView;
		}
		protected void ShowCompassOnEdge() {
			// the image scaled to edge
			imageTransform.sizeDelta = Vector2.one * edgeScale * screenScaler; 
			image.color = colorAtEdge;
			
			// we draw a line going to the alert position in screen space
			
			Vector3 focalpoint = camTransform.position + camTransform.forward * 100;
			p = Camera.main.WorldToScreenPoint(focalpoint);
			p2 = Camera.main.WorldToScreenPoint(Vector3.MoveTowards(focalpoint, spriteTransform.position, 90f));
			r = (p2 - p).normalized*1e10f;
			// Debug.Log((p2 - p).normalized,gameObject);
			
			q = bottomEdgea;
			s = bottomEdgeb;
			
			// fix / help: I might be able to cut this down by checking direction first
			
			if (SetCompassPosition()) return;
			
			// top
			q = topEdgea;
			s = topEdgeb;
			
			if (SetCompassPosition()) return;
			
			// left
			q = leftEdgea;
			s = leftEdgeb;
		
			if (SetCompassPosition()) return;
			
			// right
			q = rightEdgea;
			s = rightEdgeb;
			
			if (SetCompassPosition()) return;
			
		}	
		protected bool SetCompassPosition() {
			
			// check if parallel
			/*
			if (Cross(r, s) .IsZero() && Cross((q-p), r) .IsZero())
			{
				// colinear, point on screen
				imageTransform.anchoredPosition = Camera.main.WorldToScreenPoint(spriteTransform.position);  
				return true;
			}
			else if (Cross(r, s) .IsZero() && Cross((q - p), r) != 0)
				// parallel non-intersecting, fail
				return false;
			*/
			
			t = Cross((q - p), s) / Cross(r, s);
			u = Cross((q - p), r) / Cross(r, s);
			
			// check if intersecting
			if (!Cross(r, s).IsZero() && (0 <=u && u <=1) && (0 <=t && t <= 1))
			{
				
				imageTransform.anchoredPosition = p + t*r;
				return true;
			}
			// non-intersecting, fail
			return false;
			
			//#### imageTransform.anchoredPosition = finalPosition;
				
				

			// optimize by stopping at line intersection
			
			
		}

		public float Cross(Vector3 v, Vector3 w)
			{
				return v.x * w.y - v.y * w.x;
			}
	}
}