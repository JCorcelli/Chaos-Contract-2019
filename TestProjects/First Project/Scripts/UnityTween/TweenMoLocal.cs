using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TweenMoLocal : UpdateBehaviour {

	public TweenPositionObject[] positions =  new TweenPositionObject[0];
	public TweenRotationObject[] rotations =  new TweenRotationObject[0];
	public TweenAlphaObject[] alphas =  new TweenAlphaObject[0];
	
 
	private List<BaseTweenObject> tweens;
	
 
	public int[] activeTweens = new int[]{-1, -1, -1};
	public const int TWEENTYPE_POS	= 0 ;
	public const int TWEENTYPE_ROT	= 1 ;
	public const int TWEENTYPE_A	= 2 ;
	
	void Start () {
 
		this.tweens = new List<BaseTweenObject>();
		
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
 
	
	
	protected override void OnUpdate ()
	{
		base.OnUpdate();
		
		this.UpdateTween();
	}
	
	public void BeginTween(int i)
	{
		if (i >= 0 && i < tweens.Count)
		{
			BaseTweenObject tween = tweens[i];
			
			int activeTween = -1;
			if(tween.tweenType == TweenType.TweenPosition)
			{
				TweenPositionObject tweenPos = tween as TweenPositionObject;
				tweenPos.startValue = this.transform.localPosition;
				
				activeTween = activeTweens[TWEENTYPE_POS];
				if (activeTween > -1)
					this.EndTween(tweens[activeTween]);
				
				activeTweens[TWEENTYPE_POS] = i;
			}
			else if(tween.tweenType == TweenType.TweenRotation)
			{
				TweenRotationObject tweenRot = tween as TweenRotationObject;
				tweenRot.startValue = this.transform.localRotation.eulerAngles;
				
				activeTween = activeTweens[TWEENTYPE_ROT];
				if (activeTween > -1)
					this.EndTween(tweens[activeTween]);
				
				activeTweens[TWEENTYPE_ROT] = i;
			}
			else if(tween.tweenType == TweenType.TweenAlpha)
			{
				TweenAlphaObject tweenAlpha = tween as TweenAlphaObject;
				if(GetComponent<Image>() != null)
					tweenAlpha.startValue = GetComponent<Image>().color.a;
				else
					tweenAlpha.startValue = this.GetComponent<Renderer>().material.color.a;
				
				activeTween = activeTweens[TWEENTYPE_A];
				if (activeTween > -1)
					this.EndTween(tweens[activeTween]);
				
				activeTweens[TWEENTYPE_A] = i;
			}


			tween.startTime = Time.time;	
			tween.canStart = true;
			tween.ended = false;
				
		}
	}
	//Update tween by type
	private void UpdateTween ()
	{
		foreach (int i in activeTweens)
		{
			if (i == -1) continue;
			BaseTweenObject tween = tweens[i];
			if(tween.tweenType == TweenType.TweenPosition)
				UpdatePosition(tween as TweenPositionObject);
			else if(tween.tweenType == TweenType.TweenRotation)
				UpdateRotation(tween as TweenRotationObject);
			else if(tween.tweenType == TweenType.TweenAlpha)
				UpdateAlpha(tween as TweenAlphaObject);	
	 
			if(tween.ended) activeTweens[i] = -1;
 
		}
	}
 
 
 
	public void Reset ()
	{
		foreach (BaseTweenObject tween in tweens)
		{
			tween.ended = false;
			tween.canStart = false;
			
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
