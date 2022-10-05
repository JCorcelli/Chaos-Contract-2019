using UnityEngine;
using System.Collections;
 
public enum TweenType {
	TweenPosition = 0,
	TweenRotation = 1,
	TweenAlpha = 2
}
 
 
public class BaseTweenObject {
 
	public float totalTime = 0;
	public float delay = 0;
	public Ease ease = Ease.Linear;	
 
	private TweenType _tweenType;
	public TweenType tweenType
	{
		set{_tweenType = value;}
		get{return _tweenType;}
	}
 
	private float _startTime;
	public float startTime
	{
		set{_startTime = value;}
		get{return _startTime;}
	}
 
	private bool _ended = false;
	public bool ended
	{
		set{_ended = value;}
		get{return _ended;}
	}
 
	private bool _canStart = false;
	public bool canStart
	{
		set{_canStart = value;}
		get{return _canStart;}
	}
 
	private string _id = "";
	public string id
	{
		set{_id = value;}
		get{return _id;}
	}
 
	public BaseTweenObject ()
	{
 
	}
	public void Init()
	{
		this.id = "tween" + Time.time.ToString();
	}
 
	public void CopyTween (BaseTweenObject tween)
	{
		this.totalTime = tween.totalTime;
		this.delay = tween.delay;
		this.ease = tween.ease;
		this.tweenType = tween.tweenType;
	}
 
 
 
}