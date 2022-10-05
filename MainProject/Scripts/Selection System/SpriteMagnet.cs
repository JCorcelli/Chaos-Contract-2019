using UnityEngine;
using System.Collections;
using CameraSystem;

namespace SelectionSystem
{
	public class SpriteMagnet : UpdateBehaviour {
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
		public Color colorInReverse = new Color(1,1,1,.6f);
		
		public int fullScale =  100;
		public int edgeScale =  50;
		
		protected int prevFullScale =  100;
		protected int prevEdgeScale =  50;
		
		
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
			canvas = GetComponentInParent<Canvas>();
			scaleFactor = transform.lossyScale.y;
			SetMargins();
			
			originalRotation = imageTransform.localEulerAngles;
			originalScale = imageTransform.localScale;
		}
		
		protected Canvas canvas;
		protected float scaleFactor = 1f;
		protected void SetMargins() {
			// screenedges
			
			scaleFactor = transform.lossyScale.y;
			
			float sHeight = Screen.height;
			float sWidth = Screen.width;
			
			currentScreenHeight = Screen.height;
			
			screenScaler = (float)(Screen.width / Screen.height);
			pad = edgeScale * .5f / screenScaler * scaleFactor; // half edgescale
			//bottom
			bottomEdgea = new Vector3(pad, pad, 0);
			bottomEdgeb = new Vector3(sWidth - pad*2, 0, 0);
			
			
			// top
			topEdgea = new Vector3(pad, sHeight - pad, 0);
			topEdgeb = new Vector3(sWidth - pad*2, 0, 0);
			
			
			// left
			leftEdgea = new Vector3(pad, pad, 0);
			leftEdgeb = new Vector3(0, sHeight - pad*2, 0);
			
			// right
			rightEdgea = new Vector3(sWidth - pad, pad, 0);
			rightEdgeb = new Vector3(0, sHeight - pad*2, 0);
			
		}
		
		public float clipNear = 1f;
		public float clipFar = 20f;
		public float clipVanish = 30f;
		
		public int b_freezeRotate = 7;
		public int b_freezeScale = 7;
		public Vector3 originalRotation;
		public Vector3 originalScale;
		
		
		protected void FreezeImageVectors () {
			if (b_freezeRotate > 0)
			{
				Vector3 rotation = imageTransform.localEulerAngles;
				if (b_freezeRotate >= 100) rotation.x = originalRotation.x; 
				if (b_freezeRotate % 100 >= 10) rotation.y = originalRotation.y; 
				if (b_freezeRotate % 10 == 1) rotation.z = originalRotation.z; 
				
				imageTransform.localEulerAngles = rotation;
			}
				
			if (b_freezeScale > 0)
			{
				Vector3 scale = imageTransform.localScale;
				if (b_freezeScale >= 100) 	scale.x = originalScale.x;
				if (b_freezeScale % 100 >= 2) 	scale.y = originalScale.y;
				if (b_freezeScale % 10 == 1) 	scale.z = originalScale.z;
				imageTransform.localScale = scale;
			}
		}
		
		protected override void OnLateUpdate () {
			// Determine if 3D sprite is visible
			
			Canvas ncanvas = GetComponentInParent<Canvas>();
			
			if (edgeScale != prevEdgeScale || fullScale != prevFullScale || ncanvas != canvas || currentScreenHeight != Screen.height)
			{
				// something changed!
				canvas = ncanvas;
				
				SetMargins();
				FreezeImageVectors();	
			}
			// save changes outside of canvas / screen alteration
			originalRotation = imageTransform.localEulerAngles;
			originalScale = imageTransform.localScale;
			
				
			
			
			
			
			if ( rcheck.GetSafe()) 
			{
				SpriteInRegion(); // alternatives to showing sprite
			}
			else if (seeBackside && IsBehind())
			{
				
				SpriteInRegionReverse(); // alternatives to showing sprite
			}
			else
			{
				
				
				physicalRenderer.SetActive(false);
				image.enabled = true;
				ShowCompassOnEdge();
			}
			
		}
		public bool seeBackside = true;
		public GameObject physicalRenderer;
		
		protected bool IsBehind(){
			return Vector3.Dot(
			(spriteTransform.position - camTransform.position).normalized, camTransform.forward) < 0f;
		}
		protected void SpriteInRegionReverse() {
				physicalRenderer.SetActive( false);
				CompassInViewReverse();
			
		}
		
		protected void CompassInViewReverse(){
			// image scaled to view (distance?)
			
			Vector3 pos = Camera.main.WorldToScreenPoint(spriteTransform.position + camTransform.forward * (Vector3.Distance(camTransform.position, spriteTransform.position) + 1f));

			imageTransform.position = pos;
			imageTransform.sizeDelta = Vector2.one * fullScale * screenScaler;
			image.color = colorInReverse;
		}
		
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
			imageTransform.position = Camera.main.WorldToScreenPoint(spriteTransform.position) ;
			imageTransform.sizeDelta = Vector2.one * fullScale * screenScaler;
			image.color = colorInView;
		}
		protected void ShowCompassOnEdge() {
			// the image scaled to edge
			imageTransform.sizeDelta = Vector2.one * edgeScale * screenScaler; 
			image.color = colorAtEdge;
			
			// we draw a line going to the alert position in screen space
			
			Vector3 focalpoint = camTransform.position + camTransform.forward * 100f;
			p = Camera.main.WorldToScreenPoint(focalpoint) ;
			p2 = Camera.main.WorldToScreenPoint(Vector3.MoveTowards(focalpoint, spriteTransform.position, 90f)) ;
			
			r = (p2 - p).normalized*1e4f;
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
		public bool visibleBehind = true;
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
				
				Vector3 pos = p + t*r;
				if (visibleBehind)
					pos.z = 0;
				imageTransform.position = pos;
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