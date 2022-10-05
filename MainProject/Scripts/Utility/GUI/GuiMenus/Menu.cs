using UnityEngine;
using System.Collections;


namespace Utility.GUI
{
	public class Menu : MonoBehaviour,IMenu {

		// Use this for initialization
		
		
		public bool mainMenu = false;
		protected Animator _anim;
		protected CanvasGroup _canvasGroup;
		
		public bool IsOpen()
		{ 
			return _anim.GetBool("IsOpen");
		}
		
		public void IsOpen(bool value)
		{ 
			_anim.SetBool("IsOpen", value); 
			_canvasGroup.interactable = value;
			
		}
			
		protected virtual void Awake () {
			
			_anim = GetComponent<Animator>();
			_canvasGroup = GetComponent<CanvasGroup>();
			
			// Auto-sets position at beginning, so I can freely edit
			var rect = GetComponent<RectTransform>();
			rect.offsetMax = rect.offsetMin = new Vector2(0,0);
			
			if ( mainMenu)
				GetComponentInParent<MenuHUB>().ShowMenu( this);
			else if (IsOpen()) 
			{
				IsOpen(false);
				
			}
		}
		
		
	}
}