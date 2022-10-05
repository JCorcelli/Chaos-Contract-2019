using UnityEngine;
using System.Collections;

namespace Anim.Jerri
{
	public abstract class JerriAnimAbstract : UpdateBehaviour {

		public GameObject body;
		protected Animator _anim;
		public Animator anim {set { _anim = value; } get { return _anim; }}
		protected virtual void Awake () {
			_anim = body.GetComponent<Animator>();
		}
		
		public virtual void OnCall() {}
		
		public int substate = 0;
		public void SetSubstate(int p) {
			anim.SetInteger("SubState", p);
			substate = p;
			OnCall();
		}
		
		public int state = 0;
		public void SetState(int p) {
			anim.SetInteger("State", p);
			state = p;
			OnCall();
		}
		
		public void Stand() {
			anim.SetInteger("State", 0);
			state = 0;
			OnCall();
		}
			
		public void InBed() {
			anim.SetInteger("State", 1);
			state = 1;
			OnCall();
		}
		
		public void Kneel() {
			anim.SetInteger("State", 2);
			state = 2;
			OnCall();
		}
		
		public void Crawl() {
			anim.SetInteger("State", 3);
			state = 3;
			OnCall();
		}
		
		
		public float speed = 1f;
		public void SetSpeed(float p ) {
			speed = p;
			OnCall();
		}
		
		public float reachWeight = 0f;
		public void Reach(){
			SetReachWeight(1);
		}
		public void NoReach(){
			SetReachWeight(0);
		}
		public void SetReachWeight(float p){
			reachWeight = p;
			OnCall();
		}
		
		public float headWeight = 0f;
		public void Look(){
			SetHeadWeight(1);
		}
		public void NoLook(){
			SetHeadWeight(0);
		}
		public void SetHeadWeight(float p){
			
			headWeight = p;
			OnCall();
		}
		
		public float 	reachX = 0f;
		public float 	reachY = 0f;
		public void SetReach(float x, float y){
			reachX = x;
			reachY = y;
			OnCall();
		}
		
		public float 	reachDist = 100f;
		public void SetReachDist(float x){
			reachDist = x;
			anim.SetFloat("ReachDist", reachDist, 0f, Time.deltaTime);
			OnCall();
		}
		
		
		
		
		public float 	carry = 0f;
		
		public void SetCarry(float p){
			carry = p;
			OnCall();
		}
		
		public string play_name = "";
		public int 	play_layer = -1;
		public float cross_transitionDuration = 0f;
		
		public void Play(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity){
			// used to enter or escape a very simple animation state
			anim.Play( name, layer, normalizedTime);
			play_name = name;
			play_layer = layer;
			cross_transitionDuration = -1f;
			OnCall();
			
			
		}
		
		public bool crossfading = false;
		public float fadeTime = -1f;
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer = -1, float fixedTime = 0.0f){
			anim.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
			play_name = stateName;
			play_layer = layer;
			cross_transitionDuration = transitionDuration;
			crossfading  = true;
			fadeTime = fixedTime;
			OnCall();
		}
		
		public void CrossFade(string stateName, float transitionDuration, int layer = -1, float normalizedTime = float.NegativeInfinity){
			anim.CrossFade(stateName, transitionDuration, layer, normalizedTime);
			play_name = stateName;
			play_layer = layer;
			cross_transitionDuration = transitionDuration;
			crossfading = false;
			OnCall();
		}
		
		protected override void OnUpdate() {
			// can't use the built-in fade
			if (fadeTime > 0f)
			{
				fadeTime -= Time.deltaTime;
			}
			else
				crossfading = false;
			
			anim.SetFloat("ReachX", reachX, speed, Time.deltaTime);
			anim.SetFloat("ReachY", reachY, speed, Time.deltaTime);
			anim.SetFloat("Carry", carry, speed, Time.deltaTime);
			
			// fade overrides
			
			float weight;
			float newWeight;
			weight = anim.GetLayerWeight(1); //reachWeight
			if (Mathf.Abs(weight - reachWeight) > 0.01f )
			{
				newWeight = Mathf.Lerp(weight, reachWeight, Time.deltaTime);
				anim.SetLayerWeight(1, newWeight);
				
			}
			
			weight = anim.GetLayerWeight(2); //headWeight
			if (Mathf.Abs(weight - headWeight) > 0.01f )
			{
				newWeight = Mathf.Lerp(weight, headWeight, Time.deltaTime);
				anim.SetLayerWeight(2, newWeight);

			}
			
		}
		
		
	}
}