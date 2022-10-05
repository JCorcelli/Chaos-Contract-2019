using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
 
//Tween position object class
[System.Serializable]
public class TweenPositionObject : BaseTweenObject
{
	public Vector3 tweenValue;
 
	private Vector3 _startValue;
	public Vector3 startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
 
	public TweenPositionObject ()
	{
		this.tweenType = TweenType.TweenPosition;
	}	
}
 
//Tween rotation object class
[System.Serializable]
public class TweenRotationObject : BaseTweenObject
{
	public Vector3 tweenValue;
 
	private Vector3 _startValue;
	public Vector3 startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
 
	public TweenRotationObject ()
	{
		this.tweenType = TweenType.TweenRotation;
	}	
}
 
//Tween rotation object class
[System.Serializable]
public class TweenAlphaObject : BaseTweenObject
{
	public float tweenValue;
 
	private float _startValue;
	public float startValue
	{
		set{_startValue = value;}
		get{return _startValue;}
	}
 
	public TweenAlphaObject ()
	{
		this.tweenType = TweenType.TweenAlpha;
	}	
}
 
 
 
//Engine class
public class UnityTween : MonoBehaviour {
 
 
	public TweenPositionObject[] positions =  new TweenPositionObject[0];
	public TweenRotationObject[] rotations =  new TweenRotationObject[0];
	public TweenAlphaObject[] alphas =  new TweenAlphaObject[0];
 
	public bool loopArray;
 
	private ArrayList tweens;
 
 
	void Start () {
 
		this.tweens = new ArrayList();
		this.AddTweens();
	}
 
	private void AddTweens ()
	{
		foreach(TweenPositionObject tween in positions)
		{
			TweenPosition(tween);
		}
		foreach(TweenRotationObject tween in rotations)
		{
			TweenRotation(tween);
		}
		foreach(TweenAlphaObject tween in alphas)
		{
			TweenAlpha(tween);
		}
	}
	//Add Tween in arrayList
	//Tween position
	public void TweenPosition (TweenPositionObject obj)
	{
		TweenPositionObject tween = new TweenPositionObject();
 
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
 
		this.tweens.Add(tween);
	}
	//Tween rotation
	public void TweenRotation (TweenRotationObject obj)
	{
		TweenRotationObject tween = new TweenRotationObject();
 
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
 
		this.tweens.Add(tween);
	}
	//Tween alpha
	public void TweenAlpha (TweenAlphaObject obj)
	{
		TweenAlphaObject tween = new TweenAlphaObject();
 
		tween.startTime = Time.time;
		tween.CopyTween(obj);
		tween.tweenValue = obj.tweenValue;
		tween.Init();
 
		this.tweens.Add(tween);
	}
 
	//Clear Tweens with the same type
	private void ClearTweensSameType (BaseTweenObject obj)
	{
		foreach (BaseTweenObject tween in tweens)
		{
			if(tween.id != obj.id && tween.tweenType == obj.tweenType)
				tween.ended = true;
		}
	}
 
	//Unity Updates
	void Update ()
	{
		this.DetectDelay();
		this.UpdateTween();
	}
	//Detect when time goes past delay
	private void DetectDelay ()
	{
		foreach (BaseTweenObject tween in tweens)
		{
			if(Time.time > tween.startTime + tween.delay && !tween.canStart)
			{
				if(tween.tweenType == TweenType.TweenPosition)
				{
					TweenPositionObject tweenPos = tween as TweenPositionObject;
					tweenPos.startValue = this.transform.localPosition;
				}
				else if(tween.tweenType == TweenType.TweenRotation)
				{
					TweenRotationObject tweenRot = tween as TweenRotationObject;
					tweenRot.startValue = this.transform.localRotation.eulerAngles;
				}
				else if(tween.tweenType == TweenType.TweenAlpha)
				{
					TweenAlphaObject tweenAlpha = tween as TweenAlphaObject;
					if(GetComponent<Image>() != null)
						tweenAlpha.startValue = GetComponent<Image>().color.a;
					else
						tweenAlpha.startValue = this.GetComponent<Renderer>().material.color.a;
				}
 
				this.ClearTweensSameType(tween);
 
				tween.canStart = true;
			}
		}
	}
	//Update tween by type
	private void UpdateTween ()
	{
 
		int tweenCompleted = 0;
		foreach (BaseTweenObject tween in tweens)
		{
			if(tween.canStart && !tween.ended)
			{
				if(tween.tweenType == TweenType.TweenPosition)
					UpdatePosition(tween as TweenPositionObject);
				else if(tween.tweenType == TweenType.TweenRotation)
					UpdateRotation(tween as TweenRotationObject);
				else if(tween.tweenType == TweenType.TweenAlpha)
					UpdateAlpha(tween as TweenAlphaObject);	
 
			}
			if(tween.ended)
				tweenCompleted++;
 
			if(tweenCompleted == tweens.Count && loopArray)
				this.MakeLoop ();
 
 
		}
	}
 
	private void MakeLoop ()
	{
		foreach (BaseTweenObject tween in tweens)
		{
			tween.ended = false;
			tween.canStart = false;
			tween.startTime = Time.time;			
		}
	}
 
 
 
	//Update Position
	private void UpdatePosition(TweenPositionObject tween)
	{
		Vector3 begin = tween.startValue;
		Vector3 finish =  tween.tweenValue; 
		Vector3 change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
 
		if(duration == 0)
		{
			this.EndTween(tween);
			this.transform.localPosition = finish;
			return;
		}
 
 
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
 
		this.transform.localPosition = Equations.ChangeVector(currentTime, begin, change ,duration, tween.ease);
	}
	//Update Rotation
	private void UpdateRotation(TweenRotationObject tween)
	{
		Vector3 begin = tween.startValue;
		Vector3 finish =  tween.tweenValue; 
		Vector3 change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
 
		if(duration == 0)
		{
			this.EndTween(tween);
			this.transform.localPosition = finish;
			return;
		}
 
 
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
 
		this.transform.localRotation = Quaternion.Euler(Equations.ChangeVector(currentTime, begin, change ,duration, tween.ease));
	}
	//Update Alpha
	private void UpdateAlpha(TweenAlphaObject tween)
	{
		float begin = tween.startValue;
		float finish =  tween.tweenValue; 
		float change =  finish - begin;
		float duration = tween.totalTime; 
		float currentTime = Time.time - (tween.startTime + tween.delay);	
 
		float alpha = Equations.ChangeFloat(currentTime, begin, change ,duration, tween.ease);
		float redColor;
		float redGreen;
		float redBlue;
 
		if(GetComponent<Image>() != null)
		{
			redColor = GetComponent<Image>().color.r;
			redGreen = GetComponent<Image>().color.g;
			redBlue = GetComponent<Image>().color.b;
 
			GetComponent<Image>().color = new Color(redColor,redGreen,redBlue,alpha);
 
			if(duration == 0)
			{
				this.EndTween(tween);
				GetComponent<Image>().color = new Color(redColor,redGreen,redBlue,finish);
				return;
			}
		}
		else
		{
			redColor = this.GetComponent<Renderer>().material.color.r;
			redGreen = this.GetComponent<Renderer>().material.color.g;
			redBlue = this.GetComponent<Renderer>().material.color.b;
 
			this.GetComponent<Renderer>().material.color = new Color(redColor,redGreen,redBlue,alpha);
 
			if(duration == 0)
			{
				this.EndTween(tween);
				this.GetComponent<Renderer>().material.color = new Color(redColor,redGreen,redBlue,finish);
				return;
			}
		}
 
		if(Time.time  > tween.startTime + tween.delay + duration)
			this.EndTween(tween);
	}	
 
	private void EndTween (BaseTweenObject tween)
	{
		tween.ended = true;
	}
}