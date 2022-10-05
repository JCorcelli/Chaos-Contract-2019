using UnityEngine;
using System.Collections;


namespace Utility.GUI
{
	public class MenuScripted : UpdateBehaviour,IMenu {

		public bool mainMenu = false;
		
		protected Animator _anim;
		protected CanvasGroup _canvasGroup;
		public bool running = false;
		public bool IsOpen()
		{ 
			return _anim.GetBool("IsOpen");
		}
		
		
		
		public void IsOpen(bool value)
		{ 
			if (!value)
			{
				_anim.enabled = true;
				_canvasGroup.interactable = value;
				_canvasGroup.blocksRaycasts = value;
				
			}
			running = true;
			_anim.SetBool("IsOpen", value); 
			
			
		}
			
		protected virtual void Awake () {
			
			_anim = GetComponent<Animator>();
			_canvasGroup = GetComponent<CanvasGroup>();
			
		}
		
		// these values let me work on off screen prototypes without considering their position in game
		// public Vector2 pivotSetting;
		// public Vector2 position;
		protected virtual void Start () {
			if (!mainMenu) 
			{
				IsOpen(false);
				
			}
			else
				IsOpen(true);
			
			// Auto-sets position at beginning, so I can freely edit
			var rect = GetComponent<RectTransform>();
			
			
			// these values let me work on off screen prototypes
			//Vector2 size = rect.
			//rect.offsetMax = rect.offsetMin = new Vector2(0,0);
			
			rect.pivot = new Vector2(0,0); // pivot to bottom left?
			rect.position = new Vector2(0,0);
			
		}
		
		
		
		public bool started = false;
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			if (!running) return;
			
			
			
			if (!started ) 
			{
				started = true;
				return;
			}
			
			
			
			if (!_anim.IsInTransition(0))
			{
				
				running = false;
				started = false;
				
				if (IsOpen()) 
				{
					_canvasGroup.interactable = true;
					_canvasGroup.blocksRaycasts = true;
					_anim.enabled = false; // I let the script take over
				}
			}
		}
		
		
	}
}