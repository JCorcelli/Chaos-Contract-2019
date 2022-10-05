using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event 
using SelectionSystem;


	public class UISelectBreak : MonoBehaviour, IPointerEnterHandler {

		// suppress my selectionmanager if it selects this
		// on select: do nothing
		
		public string buttonName = "mouse 1";
		public void  OnPointerEnter( PointerEventData eventData ) {
			if (Input.GetButton( buttonName)) 
			{
				if (!SelectGlobal.locked)
				{
					SelectGlobal.locked = true;
					StartCoroutine("GiveBack");
				}
			}
			
		}
		protected IEnumerator GiveBack(){
			yield return null;
			SelectGlobal.locked = false;
			
		}
	}
