using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTNode : MonoBehaviour {

		// Use this for initialization
		
		protected BTNode _parentNode;
		protected Transform _transform;
		
		new public Transform transform{get{if (_transform == null) _transform = GetComponent<Transform>();
		return _transform;}set{_transform = value;}}
		
		
		protected BTNode parentNode{get{if (_parentNode == null) _parentNode = transform.parent.GetComponentInParent<BTNode>() as BTNode;
		return _parentNode;}set{_parentNode = value;}}
		
		
		protected virtual void Awake () {
			
			if (parentNode == null) return; // this sets the parent node, or there's no parent node.
			
			
			
		}
		
		
		protected void Success() {Succeed();}
		protected void Succeed() {
			
			if (parentNode == null) return; // this sets the parent node, or there's no parent node.
			parentNode._OnSuccess(this);
		}
		
		protected void Failure() {Fail();}
		protected void Fail() {
			
			if (parentNode == null) return; // this sets the parent node, or there's no parent node.
			
			parentNode._OnFailure(this);
			
		}
		
		// these send the child (this) so a multi-child wrapper can identify which child sent the message.
		public void _OnSuccess(BTNode node){
			if (!gameObject.activeInHierarchy) return;
			OnSuccess(node);
		}
		public void _OnFailure(BTNode node){
			if (!gameObject.activeInHierarchy) return;
			OnFailure(node);
		}
		
		
		// no alteration necessary, only for optimization
		protected virtual void OnSuccess(BTNode b){OnSuccess();}
		protected virtual void OnFailure(BTNode b){OnFailure();}
		
		// most common for decorator/wrapper nodes
		protected virtual void OnSuccess() {
			// receive success
			if (_parentNode == null) return;
			Succeed();
		}
		protected virtual void OnFailure() {
			// receive failure
			if (_parentNode == null) return;
			Failure();
		}
		
		protected void EnableChildren(){
			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
		}
		protected void DisableChildren(){
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
		}
		
		protected void EnableChild(int n = 0)
		{
			transform.GetChild(n).gameObject.SetActive(true);
		}
		protected void DisableChild(int n = 0)
		{
			transform.GetChild(n).gameObject.SetActive(false);
		}
		
		
		
	}
}