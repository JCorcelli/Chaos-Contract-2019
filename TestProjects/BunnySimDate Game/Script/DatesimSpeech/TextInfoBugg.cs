using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SelectionSystem;
public class TextInfoBugg : SelectAbstract {

	// Use this for initialization
	void Start () {
	
		t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	protected Text t;
	protected override void OnPress () {
		if (t == null) return;
		
		if (t.cachedTextGenerator != null)
		{
			//Debug.Log(t.cachedTextGenerator.
			//GetPreferredWidth(t.text, t.GetGenerationSettings((transform as RectTransform).sizeDelta)));
			//rectExtents);
			Debug.Log(t.cachedTextGenerator.
			characters[0].cursorPos);
			
		}
	}
}
