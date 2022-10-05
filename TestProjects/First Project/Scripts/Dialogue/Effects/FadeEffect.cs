using UnityEngine;
using System.Collections;

public class FadeEffect : MonoBehaviour {

	[SerializeField] private UnityEngine.UI.Image screen;
	[SerializeField] private Color solid;
	[SerializeField] private Color transparent;
	
	
	public void FadeIn()
	{
		StartCoroutine("_FadeIn");
	}
	
	public void FadeOut()
	{
		StartCoroutine("_FadeOut");
	}
	
	
	// turns screen solid
	public void Blanket()
	{
		screen.color = solid;
	}
	
	public void Clear()
	{
		screen.color = transparent;
	}
	
	
	// turns screen solid
	private IEnumerator _FadeOut() 
	{
		float frac;
		float timer = 0f;
		float timeLimit = 3f;
		while (timer < timeLimit)
		{
			frac =  timer / timeLimit;
			screen.color =  Color.Lerp(transparent, solid, frac) ; // would matter if it wasn't 0 - 1
			yield return null;
			timer += Time.deltaTime;
		}
	}
	
	// screen disappears
	private IEnumerator _FadeIn()
	{
		float frac;
		float timer = 0f;
		float timeLimit = 3f;
		while (timer < timeLimit)
		{
			frac =  timer / timeLimit;
			screen.color =  Color.Lerp(solid, transparent, frac) ; // would matter if it wasn't 0 - 1
			yield return null;
			timer += Time.deltaTime;
		}
	}
}
