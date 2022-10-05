using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

namespace BehaviorTree
{
	public class BTNode {
		public delegate IEnumerator<float> NodeDelegate();
		// Use this for initialization
		
		public BTNode parent;
		
		
		public bool active = false;
		public bool activeInHierarchy => active; // this and all its parents must be active
		protected virtual void OnEnable(){}
		protected virtual void OnDisable(){}
		
		public virtual void SetActive(bool b){
			active = b;
			if (active) OnEnable();
			else OnDisable();
			
		}
		
		protected void StartCoroutine(NodeDelegate method){
			Timing.RunCoroutine(method());
		}
		protected void StopCoroutine(string method){
			Timing.KillCoroutines(method);
		}
		protected virtual void Awake () {
			
			if (parent == null) return; // this sets the parent node, or there's no parent node.
			
			
			
		}
		
		
		protected void Success() {Succeed();}
		protected void Succeed() {
			
			if (parent == null) return; // this sets the parent node, or there's no parent node.
			parent._OnSuccess(this);
		}
		
		protected void Failure() {Fail();}
		protected void Fail() {
			
			if (parent == null) return; // this sets the parent node, or there's no parent node.
			
			parent._OnFailure(this);
			
		}
		
		// these send the child (this) so a multi-child wrapper can identify which child sent the message.
		public void _OnSuccess(BTNode node){
			if (!activeInHierarchy) return;
			OnSuccess(node);
		}
		public void _OnFailure(BTNode node){
			if (!activeInHierarchy) return;
			OnFailure(node);
		}
		
		
		// no alteration necessary, only for optimization
		protected virtual void OnSuccess(BTNode b){OnSuccess();}
		protected virtual void OnFailure(BTNode b){OnFailure();}
		
		// most common for decorator/wrapper nodes
		protected virtual void OnSuccess() {
			// receive success
			if (parent == null) return;
			Succeed();
		}
		protected virtual void OnFailure() {
			// receive failure
			if (parent == null) return;
			Failure();
		}
		
		public List<BTNode> children = new List<BTNode>();
		protected void EnableChildren(){
			foreach (BTNode child in children)
				child.SetActive(true);
		}
		protected void DisableChildren(){
			foreach (BTNode child in children)
				child.SetActive(false);
		}
		
		protected void EnableChild(int n = 0)
		{
			children[n].SetActive(true);
		}
		protected void DisableChild(int n = 0)
		{
			children[n].SetActive(false);
		}
		
		
		
	}
}