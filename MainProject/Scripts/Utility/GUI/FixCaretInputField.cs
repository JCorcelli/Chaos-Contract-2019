// copied from separate author
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FixCaretInputField:MonoBehaviour, ISelectHandler
{
	/// by me ////// I CAN"T TELL IF IT"S ANY GOOD FUCK!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	protected bool alreadyFixed = false;
	
	public void OnSelect(BaseEventData eventData)
	{
		if (alreadyFixed) return;
		alreadyFixed = true;
		Text text = transform.Find("Text").GetComponent<Text>();
		float sf = GetComponentInParent<Canvas>().scaleFactor;

		int upp = (text.fontSize - 14);
		Debug.Log(text.fontSize);

		string nm = gameObject.name+" Input Caret";
		RectTransform caretRT = (RectTransform)transform.Find(nm);

		Vector2 fuckUnity = caretRT.anchoredPosition;

		Debug.Log("here's the ap .. " +fuckUnity);

		fuckUnity.y = fuckUnity.y + upp *sf;

		caretRT.anchoredPosition = fuckUnity;

		Debug.Log("    here's a somewhat better ap .. " +fuckUnity);
	 }
 }